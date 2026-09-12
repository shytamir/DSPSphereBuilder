#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidateRange(1, [int]::MaxValue)][int]$BuildNumber,
    [Parameter(Mandatory)][ValidatePattern('^[0-9a-fA-F]{40}$')][string]$Commit,
    [ValidateRange(1, [int]::MaxValue)][int]$RunAttempt = 1,
    [string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot)
)
$ErrorActionPreference = 'Stop'
$parts = @{}
foreach ($line in Get-Content -LiteralPath (Join-Path $RepositoryRoot 'VERSION')) {
    if ([string]::IsNullOrWhiteSpace($line)) { continue }
    if ($line -cnotmatch '^(MAJOR|MINOR)=(0|[1-9][0-9]*)$') { throw 'VERSION requires MAJOR and MINOR as non-negative integers without leading zeros.' }
    if ($parts.ContainsKey($Matches[1])) { throw "Duplicate VERSION key: $($Matches[1])" }
    $parts[$Matches[1]] = [int]$Matches[2]
}
if ($parts.Count -ne 2) { throw 'VERSION requires both MAJOR and MINOR.' }
$version = '{0}.{1}.{2}' -f $parts.MAJOR, $parts.MINOR, $BuildNumber
$revision = $Commit.ToLowerInvariant()
[pscustomobject]@{
    package_version = $version
    assembly_version = '{0}.{1}.0.0' -f $parts.MAJOR, $parts.MINOR
    build_label = "$version.$($revision.Substring(0, 7))"
    source_commit = $revision
    build_number = $BuildNumber
    run_attempt = $RunAttempt
    package_file = "DSPSphereBuilder-$version.zip"
}
