#Requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][int]$BuildNumber,
    [Parameter(Mandatory)][string]$Commit,
    [int]$RunAttempt = 1,
    [string]$DspManagedPath,
    [string]$BepInExCorePath
)
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$repo = Split-Path -Parent $PSScriptRoot
Push-Location $repo
try {
    $identity = & ./scripts/Get-BuildIdentity.ps1 -BuildNumber $BuildNumber -Commit $Commit -RunAttempt $RunAttempt
    $changes = git -c "safe.directory=$($repo.Replace('\', '/'))" status --porcelain
    if ($LASTEXITCODE -ne 0) { throw 'Cannot identify working-tree state.' }
    $identity | Add-Member -NotePropertyName working_tree_dirty -NotePropertyValue ([bool]$changes)
    $output = Join-Path $repo 'artifacts/plugin'
    New-Item -ItemType Directory -Force $output | Out-Null
    $buildInfo = Join-Path $output 'BuildInfo.cs'
    @"
namespace DSPSphereBuilder
{
    internal static class BuildInfo
    {
        public const string Version = "$($identity.package_version)";
        public const string Label = "$($identity.build_label)";
    }
}
"@ | Set-Content -LiteralPath $buildInfo -Encoding utf8NoBOM
    $modes = @('Shim')
    if ($DspManagedPath -or $BepInExCorePath) {
        if (!$DspManagedPath -or !$BepInExCorePath) { throw 'Supply both native reference paths.' }
        $target = Join-Path $DspManagedPath 'Assembly-CSharp.dll'
        if ((Get-FileHash -LiteralPath $target).Hash -ne 'AE0BA95F75BD879A62AA4CE253B2AB78EAA4FB3C7C595F5E1FEE75EBE0E0EF85') { throw 'Local target changed; review the recorded baseline before building.' }
        $modes += 'Native'
    }
    foreach ($mode in $modes) {
        $arguments = @('build', 'src/DSPSphereBuilder.csproj', '-c', 'Release', '--nologo',
            '-o', (Join-Path $output $mode), "-p:ReferenceMode=$mode", "-p:BuildInfoPath=$buildInfo",
            "-p:Version=$($identity.package_version)", "-p:AssemblyVersion=$($identity.assembly_version)",
            "-p:FileVersion=$($identity.assembly_version)", "-p:InformationalVersion=$($identity.build_label)")
        if ($mode -eq 'Native') { $arguments += @("-p:DspManagedPath=$DspManagedPath", "-p:BepInExCorePath=$BepInExCorePath") }
        & dotnet @arguments
        if ($LASTEXITCODE -ne 0) { throw "$mode compilation failed." }
    }
    $identity | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $output 'BUILD-INFO.json') -Encoding utf8NoBOM
    $checks = @('run', '--project', 'checks/Metadata/Metadata.csproj', '-c', 'Release', '--',
        (Join-Path $output 'Shim/DSPSphereBuilder.dll'), $identity.package_version, $identity.build_label, $identity.assembly_version)
    if ($modes -contains 'Native') { $checks += (Join-Path $output 'Native/DSPSphereBuilder.dll') }
    & dotnet @checks
    if ($LASTEXITCODE -ne 0) { throw 'Compiled metadata check failed.' }
    if ($modes -contains 'Native') {
        & ./scripts/Test-ReferenceMap.ps1 -DspManagedPath $DspManagedPath -BepInExCorePath $BepInExCorePath -ShimDirectory (Join-Path $output 'Shim')
    }
}
finally { Pop-Location }
