#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$PackagePath,
    [Parameter(Mandatory)][ValidatePattern('^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)$')][string]$ExpectedVersion,
    [Parameter(Mandatory)][ValidatePattern('^[0-9a-fA-F]{40}$')][string]$ExpectedCommit,
    [ValidatePattern('^[0-9a-fA-F]{64}$')][string]$ExpectedPayloadHash
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
Add-Type -AssemblyName System.Drawing
$repo = Split-Path -Parent $PSScriptRoot
$required = @('manifest.json', 'README.md', 'icon.png', 'LICENSE',
    'BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll')
$zip = [IO.Compression.ZipFile]::OpenRead((Resolve-Path -LiteralPath $PackagePath).Path)
try {
    $names = @($zip.Entries.FullName)
    if ($names.Count -ne $required.Count -or @($required | Where-Object { $_ -cnotin $names }).Count -ne 0) {
        throw 'Package entries differ: missing required files or extra, nested, dependency, or probe content.'
    }
    $texts = @{}
    foreach ($name in $required | Where-Object { $_ -notmatch '\.(dll|png)$' }) {
        $reader = [IO.StreamReader]::new($zip.GetEntry($name).Open(), [Text.UTF8Encoding]::new($false, $true), $false)
        try { $texts[$name] = $reader.ReadToEnd() } finally { $reader.Dispose() }
        if ([string]::IsNullOrWhiteSpace($texts[$name])) { throw "Empty package text: $name" }
    }
    $manifest = $texts['manifest.json'] | ConvertFrom-Json -AsHashtable
    if ($manifest.name -cne 'DSPSphereBuilder' -or $manifest.version_number -cne $ExpectedVersion -or
        $manifest.website_url -cne 'https://github.com/shytamir/DSPSphereBuilder' -or
        $manifest.description -isnot [string] -or [string]::IsNullOrWhiteSpace($manifest.description) -or $manifest.description.Length -gt 250 -or
        $manifest.dependencies -isnot [array] -or $manifest.dependencies.Count -ne 1 -or $manifest.dependencies[0] -cne 'xiaoye97-BepInEx-5.4.17') {
        throw 'Manifest identity, version, description, or dependencies differ.'
    }
    $label = "$ExpectedVersion.$($ExpectedCommit.Substring(0, 7).ToLowerInvariant())"
    $sourceUrl = "https://github.com/shytamir/DSPSphereBuilder/archive/$($ExpectedCommit.ToLowerInvariant()).zip"
    foreach ($name in @('README.md', 'LICENSE')) {
        if (!$texts[$name].Contains($sourceUrl)) { throw "Missing revision-specific source access: $name" }
    }
    foreach ($path in @('LICENSE', 'research/cosmin1490/LICENSE')) {
        if (!$texts['LICENSE'].Contains([IO.File]::ReadAllText((Join-Path $repo $path)))) { throw "Changed license text: $path" }
    }
    $stream = $zip.GetEntry('icon.png').Open()
    try { $hash = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($stream)) } finally { $stream.Dispose() }
    if ($hash -cne (Get-FileHash -LiteralPath (Join-Path $repo 'packaging/icon.png')).Hash) { throw 'Changed package icon.' }
    $stream = $zip.GetEntry('icon.png').Open()
    $buffer = [IO.MemoryStream]::new()
    try {
        $stream.CopyTo($buffer); $buffer.Position = 0
        $icon = [Drawing.Image]::FromStream($buffer, $false, $true)
        try {
            if ($icon.RawFormat.Guid -ne [Drawing.Imaging.ImageFormat]::Png.Guid -or $icon.Width -ne 256 -or $icon.Height -ne 256) { throw 'Package icon must be a 256x256 PNG.' }
        } finally { $icon.Dispose() }
    } finally { $buffer.Dispose(); $stream.Dispose() }
    $inspection = Join-Path $repo 'artifacts/package-inspection'
    New-Item -ItemType Directory -Force $inspection | Out-Null
    $dll = Join-Path $inspection 'DSPSphereBuilder.dll'
    [IO.Compression.ZipFileExtensions]::ExtractToFile($zip.GetEntry('BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll'), $dll, $true)
    if ($ExpectedPayloadHash -and (Get-FileHash -LiteralPath $dll).Hash -ne $ExpectedPayloadHash) { throw 'Payload bytes differ from the build record.' }
    $parts = $ExpectedVersion.Split('.')
    & dotnet run --project (Join-Path $repo 'checks/Metadata/Metadata.csproj') -c Release -- $dll $ExpectedVersion $label "$($parts[0]).$($parts[1]).0.0"
    if ($LASTEXITCODE -ne 0) { throw 'Package DLL identity check failed.' }
}
finally { $zip.Dispose() }
Write-Host "PASS: executable Thunderstore ZIP $ExpectedVersion; $($required.Count) files, plugin identity, source access/licenses, UTF-8 and 256x256 PNG."
