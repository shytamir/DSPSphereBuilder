#Requires -Version 7.0
[CmdletBinding()]
param([string]$RepositoryRoot = (Split-Path -Parent $PSScriptRoot))
$files = [ordered]@{
    'README.md' = 'packaging/README.md'
    'icon.png' = 'packaging/icon.png'
    'LICENSE' = 'LICENSE'
    'BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll' = 'artifacts/plugin/Shim/DSPSphereBuilder.dll'
    'source/README.md' = 'packaging/SOURCE.md'
}
$sourceFiles = @('global.json', 'VERSION', 'LICENSE', 'checks/Directory.Build.props',
    'scripts/Build-Plugin.ps1', 'scripts/Get-BuildIdentity.ps1', 'scripts/Test-ReferenceMap.ps1',
    'scripts/write_plan.py', 'scripts/derive_patches.py', 'scripts/blueprint_geometry.py')
foreach ($directory in @('src', 'references', 'checks/Metadata', 'research/cosmin1490')) {
    $sourceFiles += Get-ChildItem -LiteralPath (Join-Path $RepositoryRoot $directory) -Recurse -File |
        Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' -and $_.Extension -in @('.cs', '.csproj', '.props', '.json', '.md', '.txt', '') } |
        ForEach-Object { [IO.Path]::GetRelativePath($RepositoryRoot, $_.FullName).Replace('\', '/') }
}
foreach ($path in $sourceFiles | Sort-Object -Unique) { $files["source/$path"] = $path }
return $files
