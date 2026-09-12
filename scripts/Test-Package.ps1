#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [string]$PackagePath,

    [Parameter(Mandatory)]
    [ValidatePattern('^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)$')]
    [string]$ExpectedVersion
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
Add-Type -AssemblyName System.Drawing

$zip = [IO.Compression.ZipFile]::OpenRead((Resolve-Path -LiteralPath $PackagePath).Path)
try {
    $required = @('manifest.json', 'README.md', 'icon.png', 'LICENSE')
    $names = @($zip.Entries.FullName)
    if ($names.Count -ne $required.Count -or
        @($required | Where-Object { $_ -cnotin $names }).Count -ne 0) {
        throw 'Mock ZIP must contain exactly manifest.json, README.md, icon.png, and LICENSE at its root.'
    }

    $texts = @{}
    foreach ($name in @('manifest.json', 'README.md', 'LICENSE')) {
        $reader = [IO.StreamReader]::new(
            $zip.GetEntry($name).Open(), [Text.UTF8Encoding]::new($false, $true), $false
        )
        try { $texts[$name] = $reader.ReadToEnd() }
        finally { $reader.Dispose() }
        if ([string]::IsNullOrWhiteSpace($texts[$name])) { throw "Empty package text: $name" }
    }
    $manifest = $texts['manifest.json'] | ConvertFrom-Json -AsHashtable
    if ($manifest.name -cne 'DSPSphereBuilder' -or
        $manifest.version_number -cne $ExpectedVersion -or
        $manifest.website_url -cne 'https://github.com/shytamir/DSPSphereBuilder' -or
        $manifest.description -isnot [string] -or
        $manifest.description.Length -gt 250 -or
        $manifest.description -notmatch '\bMock\b' -or
        $manifest.dependencies -isnot [array] -or
        $manifest.dependencies.Count -ne 0) {
        throw 'Manifest does not satisfy the mock package identity, version, description, or dependency contract.'
    }
    if ($texts['README.md'] -notmatch '(?i)mock package') {
        throw 'Package README must identify this artifact as a mock package.'
    }

    # Decode the actual image; a PNG-shaped header alone is not sufficient.
    $stream = $zip.GetEntry('icon.png').Open()
    $buffer = [IO.MemoryStream]::new()
    try {
        $stream.CopyTo($buffer)
        $buffer.Position = 0
        $icon = [Drawing.Image]::FromStream($buffer, $false, $true)
        try {
            if ($icon.RawFormat.Guid -ne [Drawing.Imaging.ImageFormat]::Png.Guid -or
                $icon.Width -ne 256 -or $icon.Height -ne 256) {
                throw 'Package icon must be a 256x256 PNG.'
            }
        }
        finally { $icon.Dispose() }
    }
    finally { $buffer.Dispose(); $stream.Dispose() }
}
finally { $zip.Dispose() }

Write-Host "PASS: mock Thunderstore ZIP $ExpectedVersion (four root files, metadata, UTF-8, 256x256 PNG)."
