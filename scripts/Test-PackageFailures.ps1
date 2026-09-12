#Requires -Version 7.0
[CmdletBinding()]
param([Parameter(Mandatory)][string]$PackagePath, [Parameter(Mandatory)][string]$ExpectedVersion, [Parameter(Mandatory)][string]$ExpectedCommit)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$repo = Split-Path -Parent $PSScriptRoot
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
$cases = @('missing-dll', 'wrong-version', 'wrong-dll-version', 'wrong-guid', 'shim-payload', 'extra-dependency', 'extra-probe', 'nested-zip', 'wrapper', 'missing-source', 'wrong-dependency', 'bad-image')
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
            $old = [Text.Encoding]::UTF8.GetBytes('dsp.spherebuilder')
            $data = $files[$payload]; $found = $false
            for ($i = 0; $i -le $data.Length - $old.Length; $i++) {
                if ($data[$i] -ne $old[0]) { continue }
                $matches = $true
                for ($j = 1; $j -lt $old.Length; $j++) { if ($data[$i + $j] -ne $old[$j]) { $matches = $false; break } }
                if ($matches) { $data[$i + $old.Length - 1] = [byte][char]'X'; $found = $true; break }
            }
            if (!$found) { throw 'Cannot prepare changed GUID fixture.' }
        }
        'wrong-dll-version' {
            $version = '9.9.999'
            $manifest = [Text.Encoding]::UTF8.GetString($files['manifest.json']) | ConvertFrom-Json
            $manifest.version_number = $version
            $files['manifest.json'] = [Text.Encoding]::UTF8.GetBytes(($manifest | ConvertTo-Json))
            $files['source/REVISION.txt'] = [Text.Encoding]::UTF8.GetBytes("Commit: $ExpectedCommit`nBuild: $version.$($ExpectedCommit.Substring(0, 7))`nWorking tree dirty: False")
        }
        'shim-payload' { $files[$payload] = [IO.File]::ReadAllBytes((Join-Path $repo 'artifacts/plugin/Shim/Assembly-CSharp.dll')) }
        'extra-dependency' { $files['BepInEx/plugins/DSPSphereBuilder/UnityEngine.CoreModule.dll'] = $files[$payload] }
        'extra-probe' { $files['BepInEx/plugins/DSPSphereBuilder/FeasibilityProbe.dll'] = $files[$payload] }
        'nested-zip' { $files["DSPSphereBuilder-$ExpectedVersion.zip"] = [byte[]]@(80, 75) }
        'wrapper' { $wrapped = [ordered]@{}; foreach ($item in $files.GetEnumerator()) { $wrapped["package/$($item.Key)"] = $item.Value }; $files = $wrapped }
        'missing-source' { $files.Remove('source/research/cosmin1490/60.txt') }
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
