# DSP Sphere Builder

Paint the reference C60 framework one pentagon at a time. Twelve clicks progress
from one polar cap through two rings to the opposite cap, ending with 60 nodes
and 90 frames. Each click adds the missing connections and one leading spoke.
Hexagons emerge between the pentagons; fill shells yourself using the native tool.

## Install

Requires Dyson Sphere Program and BepInEx 5.4.17 (Thunderstore dependency
`xiaoye97-BepInEx-5.4.17`). With BepInEx installed, extract this package's `BepInEx`
directory into the game directory, preserving its folders. For a mod-manager
profile, use its profile directory instead. The payload is
`BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll`.

Remove an older Sphere Builder DLL before replacing it. The feasibility probe
must not be active alongside this plugin. The plugin GUID is `dsp.spherebuilder`.
See the repository's [project record](https://github.com/shytamir/DSPSphereBuilder/blob/main/docs/PROJECT.md)
for validation and acceptance state.

## Use

Open the native Dyson sphere editor in a running game. Select exactly one empty
sphere layer at a native legal radius. At least 68 degrees of unlocked Dyson
sphere latitude is required. Click **Paint Next Patch** to start, then again
whenever ready; construction need not finish between clicks. The panel reports
progress or why painting is unavailable.

Continue after a save reload by selecting the layer again. Progress comes from
its existing framework. Completed designs add nothing. A layer that differs from
the staged design is refused; arbitrary blueprints and manual repairs are not
supported. An exact earlier prefix is recognized as that prefix.

Existing framework and construction are preserved. No shells are created or
filled, no research is unlocked, and no future patch is planned automatically.
On an unexpected error, painting stops for the plugin session and details go to
`BepInEx/LogOutput.log`. A partial patch can remain: there is no rollback or
automatic retry. Returning to the menu does not clear this stop.

## Credit and source

Geometry: [Cosmin1490's optimized 60-node sphere](https://www.dysonsphereblueprints.com/en/blueprints/dyson-sphere-best-cost-efficiency-optimized-sphere-design-60-nodes-15-cheaper-than-football),
with reference material from the author's
[Blueprint Generator](https://github.com/Cosmin1490/DysonSphereProgram-BlueprintGenerator).
"Perfect" means faithful geometry; this project has not independently proved
mathematical optimality. The source link below includes the pinned reference and
derivation. LICENSE retains the GPL-3.0 distribution terms and original Apache-2.0
source grant, with attribution and both license texts.
