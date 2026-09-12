#requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$DspManagedPath,
    [Parameter(Mandatory)][string]$BepInExCorePath,
    [string]$PythonCommand = 'python'
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
$gitRepo = $repo.Replace('\', '/')
Push-Location $repo
try {
    $revision = git -c "safe.directory=$gitRepo" rev-parse HEAD
    if ($LASTEXITCODE -ne 0) { throw 'Cannot identify source revision.' }
    $changes = git -c "safe.directory=$gitRepo" status --porcelain
    if ($LASTEXITCODE -ne 0) { throw 'Cannot inspect working tree.' }
    if ($changes) { $revision += '-dirty' }
    $output = Join-Path $repo 'artifacts/probe'
    $plan = Join-Path $output 'plan.json'
    & $PythonCommand -B scripts/write_probe_plan.py $plan
    if ($LASTEXITCODE -ne 0) { throw 'Probe plan generation failed.' }
    $buildArgs = @('build', 'probe/FeasibilityProbe.csproj', '--configuration', 'Release',
        '--output', (Join-Path $output 'bin'), "-p:BaseIntermediateOutputPath=$output/obj/",
        "-p:DspManagedPath=$DspManagedPath", "-p:BepInExCorePath=$BepInExCorePath",
        "-p:ProbePlanPath=$plan", "-p:ProbeRevision=$revision", '-p:NuGetAudit=false')
    & dotnet @buildArgs
    if ($LASTEXITCODE -ne 0) { throw 'Probe compilation failed.' }
    $checkArgs = @('run', '--project', 'tests/probe-json/ProbeJsonChecks.csproj', '--configuration', 'Release',
        "-p:BaseIntermediateOutputPath=$output/json-checks/obj/", "-p:OutputPath=$output/json-checks/bin/",
        "-p:ProbeDllPath=$output/bin/DSPSphereBuilder.Feasibility.dll", "-p:DspManagedPath=$DspManagedPath")
    & dotnet @checkArgs
    if ($LASTEXITCODE -ne 0) { throw 'Probe JSON checks failed.' }
    $package = Join-Path $output 'package/DSPSphereBuilder.Feasibility'
    New-Item -ItemType Directory -Force $package | Out-Null
    Copy-Item -LiteralPath (Join-Path $output 'bin/DSPSphereBuilder.Feasibility.dll') -Destination $package
    Copy-Item -LiteralPath 'probe/README.md' -Destination $package
    Copy-Item -LiteralPath 'LICENSE' -Destination $package
    Copy-Item -LiteralPath 'research/cosmin1490/60.txt' -Destination (Join-Path $package 'reference-60.txt')
    Copy-Item -LiteralPath 'research/cosmin1490/LICENSE' -Destination (Join-Path $package 'REFERENCE-LICENSE.txt')
    $label = $revision.Substring(0, 7)
    if ($changes) { $label += '-dirty' }
    $zip = Join-Path $output "DSPSphereBuilder-Feasibility-$label.zip"
    Compress-Archive -LiteralPath $package -DestinationPath $zip -Force
    Write-Output $zip
    Write-Output "Source: $revision"
}
finally { Pop-Location }
