#Requires -Version 7.0
[CmdletBinding()]
param()
$files = [ordered]@{
    'README.md' = 'packaging/README.md'
    'icon.png' = 'packaging/icon.png'
    'LICENSE' = 'LICENSE'
    'BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll' = 'artifacts/plugin/Shim/DSPSphereBuilder.dll'
}
return $files
