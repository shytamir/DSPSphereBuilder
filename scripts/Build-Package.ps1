#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidateRange(1, [int]::MaxValue)][int]$BuildNumber,
    [Parameter(Mandatory)][ValidatePattern('^[0-9a-fA-F]{40}$')][string]$Commit,
    [ValidateRange(1, [int]::MaxValue)][int]$RunAttempt = 1,
    [string]$DspManagedPath,
    [string]$BepInExCorePath
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$repo = Split-Path -Parent $PSScriptRoot
Push-Location $repo
try {
    & ./scripts/Build-Plugin.ps1 @PSBoundParameters
    $info = Get-Content artifacts/plugin/BUILD-INFO.json -Raw | ConvertFrom-Json -AsHashtable
    $version = $info.package_version
    $manifest = [ordered]@{
        name = 'DSPSphereBuilder'
        version_number = $version
        website_url = 'https://github.com/shytamir/DSPSphereBuilder'
        description = 'Paint the exact reference C60 sphere framework one connected pentagon patch at a time. Fill shells yourself.'
        dependencies = @('xiaoye97-BepInEx-5.4.17')
    }
    $packageFiles = & ./scripts/Get-PackageInputs.ps1
    foreach ($path in $packageFiles.Values) {
        if (!(Test-Path -LiteralPath $path -PathType Leaf)) { throw "Missing package input: $path" }
    }
    $directory = Join-Path $repo 'artifacts/packages'
    New-Item -ItemType Directory -Path $directory -Force | Out-Null
    $packagePath = Join-Path $directory $info.package_file
    $file = [IO.File]::Open($packagePath, [IO.FileMode]::Create)
    try {
        $zip = [IO.Compression.ZipArchive]::new($file, [IO.Compression.ZipArchiveMode]::Create)
        try {
            $texts = [ordered]@{
                'manifest.json' = ($manifest | ConvertTo-Json)
                'source/REVISION.txt' = "Commit: $($info.source_commit)`nBuild: $($info.build_label)`nWorking tree dirty: $($info.working_tree_dirty)"
            }
            foreach ($text in $texts.GetEnumerator()) {
                $writer = [IO.StreamWriter]::new($zip.CreateEntry($text.Key).Open(), [Text.UTF8Encoding]::new($false))
                try { $writer.WriteLine($text.Value) } finally { $writer.Dispose() }
            }
            foreach ($item in $packageFiles.GetEnumerator()) {
                [IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip, (Join-Path $repo $item.Value), $item.Key) | Out-Null
            }
        }
        finally { $zip.Dispose() }
    }
    finally { $file.Dispose() }
    $info.payload_sha256 = (Get-FileHash artifacts/plugin/Shim/DSPSphereBuilder.dll).Hash
    & ./scripts/Test-Package.ps1 -PackagePath $packagePath -ExpectedVersion $version -ExpectedCommit $Commit -ExpectedPayloadHash $info.payload_sha256
    $info.package_sha256 = (Get-FileHash -LiteralPath $packagePath).Hash
    $info | ConvertTo-Json | Set-Content artifacts/BUILD-INFO.json -Encoding utf8NoBOM
    if ($env:GITHUB_OUTPUT) {
        "package_version=$version" | Add-Content $env:GITHUB_OUTPUT
        "build_label=$($info.build_label)" | Add-Content $env:GITHUB_OUTPUT
    }
    Write-Host "Build $($info.build_label) — $packagePath"
}
finally { Pop-Location }
