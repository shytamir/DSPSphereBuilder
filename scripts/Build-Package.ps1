#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)]
    [ValidateRange(1, [int]::MaxValue)]
    [int]$BuildNumber,

    [Parameter(Mandatory)]
    [ValidatePattern('^[0-9a-fA-F]{40}$')]
    [string]$Commit,

    [ValidateRange(1, [int]::MaxValue)]
    [int]$RunAttempt = 1,

    [string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot)
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$RepositoryRoot = (Resolve-Path -LiteralPath $RepositoryRoot).Path

# VERSION is input, never rewritten by a build.
$versionParts = @{}
foreach ($line in Get-Content -LiteralPath (Join-Path $RepositoryRoot 'VERSION')) {
    if ([string]::IsNullOrWhiteSpace($line)) { continue }
    if ($line -cnotmatch '^(MAJOR|MINOR)=(0|[1-9][0-9]*)$') {
        throw 'VERSION must contain MAJOR and MINOR as non-negative decimal integers without leading zeros.'
    }
    $key = $Matches[1]
    if ($versionParts.ContainsKey($key)) { throw "Duplicate VERSION key: $key" }
    $versionParts[$key] = [int]$Matches[2]
}
if ($versionParts.Count -ne 2) { throw 'VERSION requires both MAJOR and MINOR.' }

$version = '{0}.{1}.{2}' -f $versionParts.MAJOR, $versionParts.MINOR, $BuildNumber
$sourceCommit = $Commit.ToLowerInvariant()
$buildLabel = "$version.$($sourceCommit.Substring(0, 7))"
$manifest = [ordered]@{
    name = 'DSPSphereBuilder'
    version_number = $version
    website_url = 'https://github.com/shytamir/DSPSphereBuilder'
    description = 'Mock packaging artifact for DSP Sphere Builder. Contains no playable mod.'
    dependencies = @()
}
$packageFiles = [ordered]@{
    'README.md' = 'packaging/README.md'
    'icon.png' = 'packaging/icon.png'
    'LICENSE' = 'LICENSE'
}
foreach ($relativePath in $packageFiles.Values) {
    if (-not (Test-Path -LiteralPath (Join-Path $RepositoryRoot $relativePath) -PathType Leaf)) {
        throw "Missing package input: $relativePath"
    }
}

$outputDirectory = Join-Path $RepositoryRoot 'artifacts/packages'
New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null
$packagePath = Join-Path $outputDirectory "DSPSphereBuilder-$version.zip"
# Rebuilding the same local version replaces only this generated ZIP.
$file = [IO.File]::Open($packagePath, [IO.FileMode]::Create)
try {
    $zip = [IO.Compression.ZipArchive]::new($file, [IO.Compression.ZipArchiveMode]::Create)
    try {
        $entry = $zip.CreateEntry('manifest.json')
        $writer = [IO.StreamWriter]::new($entry.Open(), [Text.UTF8Encoding]::new($false))
        try { $writer.WriteLine(($manifest | ConvertTo-Json)) }
        finally { $writer.Dispose() }
        foreach ($item in $packageFiles.GetEnumerator()) {
            [IO.Compression.ZipFileExtensions]::CreateEntryFromFile(
                $zip, (Join-Path $RepositoryRoot $item.Value), $item.Key
            ) | Out-Null
        }
    }
    finally { $zip.Dispose() }
}
finally { $file.Dispose() }

& (Join-Path $PSScriptRoot 'Test-Package.ps1') -PackagePath $packagePath -ExpectedVersion $version

$buildInfo = [ordered]@{
    package_version = $version
    build_label = $buildLabel
    source_commit = $sourceCommit
    build_number = $BuildNumber
    run_attempt = $RunAttempt
    package_file = [IO.Path]::GetFileName($packagePath)
}
$buildInfo | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $RepositoryRoot 'artifacts/BUILD-INFO.json') -Encoding utf8NoBOM
if ($env:GITHUB_OUTPUT) {
    "package_version=$version" | Add-Content -LiteralPath $env:GITHUB_OUTPUT -Encoding utf8NoBOM
    "build_label=$buildLabel" | Add-Content -LiteralPath $env:GITHUB_OUTPUT -Encoding utf8NoBOM
}
Write-Host "Build $buildLabel — $packagePath"
