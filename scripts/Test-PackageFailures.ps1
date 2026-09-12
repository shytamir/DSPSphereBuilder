#Requires -Version 7.0
[CmdletBinding()]
param([Parameter(Mandatory)][string]$PackagePath, [Parameter(Mandatory)][string]$ExpectedVersion, [Parameter(Mandatory)][string]$ExpectedCommit)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$repo = Split-Path -Parent $PSScriptRoot
& "$PSScriptRoot/Test-Package.ps1" -PackagePath $PackagePath -ExpectedVersion $ExpectedVersion -ExpectedCommit $ExpectedCommit
$source = [IO.Compression.ZipFile]::OpenRead((Resolve-Path -LiteralPath $PackagePath).Path)
$entries = [ordered]@{}
try {
    foreach ($entry in $source.Entries) {
        $stream = $entry.Open(); $buffer = [IO.MemoryStream]::new()
        try { $stream.CopyTo($buffer); $entries[$entry.FullName] = $buffer.ToArray() }
        finally { $buffer.Dispose(); $stream.Dispose() }
    }
} finally { $source.Dispose() }
$payload = 'BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll'
$directory = Join-Path $repo 'artifacts/package-negative'
New-Item -ItemType Directory -Force $directory | Out-Null
function Set-Marker([byte[]]$Data, [byte[]]$Before, [byte[]]$After) {
    if ($Before.Length -ne $After.Length) { throw 'Fixture marker lengths differ.' }
    $offsets = @()
    for ($i = 0; $i -le $Data.Length - $Before.Length; $i++) {
        if ($Data[$i] -ne $Before[0]) { continue }
        $matches = $true
        for ($j = 1; $j -lt $Before.Length; $j++) { if ($Data[$i + $j] -ne $Before[$j]) { $matches = $false; break } }
        if ($matches) { $offsets += $i }
    }
    if ($offsets.Count -ne 1) { throw 'Fixture marker must identify one metadata value.' }
    [Array]::Copy($After, 0, $Data, $offsets[0], $After.Length)
}
$cases = @('missing-dll', 'wrong-version', 'wrong-dll-version', 'wrong-file-version', 'wrong-file-resource', 'wrong-guid', 'shim-payload', 'extra-dependency', 'extra-probe', 'nested-zip', 'wrapper', 'missing-source-link', 'missing-license', 'wrong-dependency', 'bad-image')
foreach ($case in $cases) {
    $files = [ordered]@{}
    foreach ($item in $entries.GetEnumerator()) { $files[$item.Key] = $item.Value.Clone() }
    $version = $ExpectedVersion
    switch ($case) {
        'missing-dll' { $files.Remove($payload) }
        'wrong-version' {
            $manifest = [Text.Encoding]::UTF8.GetString($files['manifest.json']) | ConvertFrom-Json
            $manifest.version_number = '9.9.999'; $files['manifest.json'] = [Text.Encoding]::UTF8.GetBytes(($manifest | ConvertTo-Json))
        }
        'wrong-guid' {
            Set-Marker $files[$payload] ([Text.Encoding]::UTF8.GetBytes('dsp.spherebuilder')) ([Text.Encoding]::UTF8.GetBytes('dsp.spherebuildeX'))
        }
        { $_ -in 'wrong-file-version', 'wrong-file-resource' } {
            $parts = $ExpectedVersion.Split('.')
            $encoding = if ($case -eq 'wrong-file-resource') { [Text.Encoding]::Unicode } else { [Text.Encoding]::UTF8 }
            $prefix = if ($case -eq 'wrong-file-resource') { "FileVersion`0`0" } else { '' }
            Set-Marker $files[$payload] ($encoding.GetBytes("$prefix$($parts[0]).$($parts[1]).0.0")) ($encoding.GetBytes("$prefix$($parts[0]).$($parts[1]).0.1"))
        }
        'wrong-dll-version' {
            $version = '9.9.999'
            $manifest = [Text.Encoding]::UTF8.GetString($files['manifest.json']) | ConvertFrom-Json
            $manifest.version_number = $version
            $files['manifest.json'] = [Text.Encoding]::UTF8.GetBytes(($manifest | ConvertTo-Json))
        }
        'shim-payload' { $files[$payload] = [IO.File]::ReadAllBytes((Join-Path $repo 'artifacts/plugin/Shim/Assembly-CSharp.dll')) }
        'extra-dependency' { $files['BepInEx/plugins/DSPSphereBuilder/UnityEngine.CoreModule.dll'] = $files[$payload] }
        'extra-probe' { $files['BepInEx/plugins/DSPSphereBuilder/FeasibilityProbe.dll'] = $files[$payload] }
        'nested-zip' { $files["DSPSphereBuilder-$ExpectedVersion.zip"] = [byte[]]@(80, 75) }
        'wrapper' { $wrapped = [ordered]@{}; foreach ($item in $files.GetEnumerator()) { $wrapped["package/$($item.Key)"] = $item.Value }; $files = $wrapped }
        'missing-source-link' {
            $readme = [Text.Encoding]::UTF8.GetString($files['README.md'])
            $readme = $readme.Replace("https://github.com/shytamir/DSPSphereBuilder/archive/$($ExpectedCommit.ToLowerInvariant()).zip", '')
            $files['README.md'] = [Text.Encoding]::UTF8.GetBytes($readme)
        }
        'missing-license' { $files.Remove('LICENSE') }
        'wrong-dependency' {
            $manifest = [Text.Encoding]::UTF8.GetString($files['manifest.json']) | ConvertFrom-Json
            $manifest.dependencies = @(); $files['manifest.json'] = [Text.Encoding]::UTF8.GetBytes(($manifest | ConvertTo-Json))
        }
        'bad-image' { $files['icon.png'] = [byte[]]@(137, 80, 78, 71) }
    }
    $path = Join-Path $directory "$case.zip"
    $stream = [IO.File]::Open($path, [IO.FileMode]::Create)
    try {
        $zip = [IO.Compression.ZipArchive]::new($stream, [IO.Compression.ZipArchiveMode]::Create)
        try {
            foreach ($item in $files.GetEnumerator()) {
                $output = $zip.CreateEntry($item.Key).Open()
                try { $output.Write($item.Value, 0, $item.Value.Length) } finally { $output.Dispose() }
            }
        } finally { $zip.Dispose() }
    } finally { $stream.Dispose() }
    $rejected = $false
    try { & "$PSScriptRoot/Test-Package.ps1" -PackagePath $path -ExpectedVersion $version -ExpectedCommit $ExpectedCommit *> (Join-Path $directory "$case.log") }
    catch { $rejected = $true }
    if (!$rejected) { throw "Malformed package accepted: $case" }
}
Write-Host "PASS: $($cases.Count) malformed package cases rejected."
exit 0
