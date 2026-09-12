#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$DspManagedPath,
    [Parameter(Mandatory)][string]$BepInExCorePath,
    [string]$ShimDirectory = 'artifacts/reference-shims',
    [switch]$UpdateMap
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
Add-Type -Path (Join-Path $BepInExCorePath 'Mono.Cecil.dll')
$mapPath = Join-Path $PSScriptRoot '../references/Map.json'
$assemblies = [ordered]@{}
$projectNames = @(Get-ChildItem -LiteralPath (Join-Path $PSScriptRoot '../references') -Recurse -Filter '*.csproj' | ForEach-Object { $_.BaseName + '.dll' })
$files = @(Get-ChildItem -LiteralPath $ShimDirectory -Filter '*.dll' | Where-Object { $_.Name -in $projectNames } | Sort-Object Name)
if ($files.Count -eq 0) { throw 'Build the reference shims first.' }
foreach ($file in $files) {
    $nativePath = Join-Path $(if ($file.BaseName -eq 'BepInEx') { $BepInExCorePath } else { $DspManagedPath }) $file.Name
    $shim = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($file.FullName)
    $native = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($nativePath)
    try {
        if ($shim.Name.FullName -cne $native.Name.FullName) { throw "Assembly identity mismatch: $($file.Name)" }
        $types = [ordered]@{}
        foreach ($type in $shim.MainModule.GetTypes() | Where-Object { $_.IsPublic -or $_.IsNestedPublic } | Sort-Object FullName) {
            $actual = $native.MainModule.GetType($type.FullName)
            if ($null -eq $actual -or $type.BaseType.FullName -cne $actual.BaseType.FullName -or $type.IsValueType -ne $actual.IsValueType) {
                throw "Missing or different native type: $($type.FullName)"
            }
            $members = [ordered]@{}
            foreach ($field in $type.Fields | Where-Object IsPublic | Sort-Object FullName) {
                $found = @($actual.Fields | Where-Object { $_.FullName -ceq $field.FullName -and $_.IsStatic -eq $field.IsStatic -and $_.IsPublic })
                if ($found.Count -ne 1) { throw "Field differs: $($field.FullName)" }
                if ($found[0].HasConstant -ne $field.HasConstant -or ($field.HasConstant -and $found[0].Constant -ne $field.Constant)) {
                    throw "Field constant differs: $($field.FullName)"
                }
                $members[$field.FullName] = $found[0].MetadataToken.ToUInt32().ToString('X8')
            }
            foreach ($method in $type.Methods | Where-Object { $_.IsPublic -or $_.IsFamily } | Sort-Object FullName) {
                $found = @($actual.Methods | Where-Object {
                    $_.FullName -ceq $method.FullName -and $_.IsStatic -eq $method.IsStatic -and
                    $_.IsPublic -eq $method.IsPublic -and $_.IsFamily -eq $method.IsFamily
                })
                if ($found.Count -ne 1) { throw "Method differs: $($method.FullName)" }
                $members[$method.FullName] = $found[0].MetadataToken.ToUInt32().ToString('X8')
            }
            $types[$type.FullName] = [ordered]@{
                token = $actual.MetadataToken.ToUInt32().ToString('X8')
                base = $actual.BaseType.FullName
                members = $members
            }
        }
        $assemblies[$file.Name] = [ordered]@{
            identity = $native.Name.FullName
            sha256 = (Get-FileHash -LiteralPath $nativePath -Algorithm SHA256).Hash
            mvid = $native.MainModule.Mvid.ToString()
            types = $types
        }
    }
    finally { $shim.Dispose(); $native.Dispose() }
}
$json = $assemblies | ConvertTo-Json -Depth 12
if ($UpdateMap) { $json | Set-Content -LiteralPath $mapPath -Encoding utf8NoBOM }
elseif ($json.Replace("`r`n", "`n").Trim() -cne (Get-Content -LiteralPath $mapPath -Raw).Replace("`r`n", "`n").Trim()) {
    throw 'Reference map differs. Inspect the changed native surface, update the map, and include it with the declarations in this commit.'
}
Write-Host "PASS: $($files.Count) shim assemblies match native type/member signatures and the recorded map."
