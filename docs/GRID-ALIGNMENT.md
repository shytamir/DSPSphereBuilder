# Pentagon grid alignment evidence

[PROJECT.md](PROJECT.md) owns decisions, progress and acceptance. The
[hotfix roadmap](management/archive/ROADMAP-grid-alignment-hotfix.md) retains its
scope and gates. This record separates native-data measurements, offline checks
and owner observations.

## Native measurement — 2026-09-15

The target Assembly-CSharp.dll matched the [recorded target](PROJECT.md#target-game-reference).
Read-only ILSpy inspection traced `UIDysonDrawingGrid.InitLineSegs` through
`MeshData.LoadFromResource` and `MeshData.Load(MeshDataAsset)`. The relevant resource
was `Dyson Sphere/MeshDatas/dyson-grid-geo20`, not the tetrahedral `geo4` or
octahedral `geo8` grids. No game process or native method was invoked.

| Input | Identity |
| --- | --- |
| `resources.assets` | 448,414,040 bytes; SHA-256 `7232732470c9ad45f8f255dcaf5f5a0b9e8741ec736d851a2dfc5feae3429300` |
| `level0` | 10,259,952 bytes; SHA-256 `6d9c2abbcac25c06d0a0f15e284858e394e98bc04ae75618d5081b8130c35170` |
| Grid MeshDataAsset | `resources.assets`, path ID 135959, `dyson-grid-geo20` |
| Grid mesh byte array | 450,267 bytes; SHA-256 `eacc9ce5f3d2a0d7371c9f74457dee605e463d2bf375c546fe1406337aaf8c61` |
| Drawing-grid component | `level0`, path ID 52157, object `geometry20-grid`, `meshName = geo20` |

Reproduction method:

1. Read the installed resource files with [UnityPy](https://pypi.org/project/UnityPy/)
   1.25.2, installed only under ignored `artifacts/grid-alignment/tools`. Its
   standard MonoBehaviour reader supplies the base header; the stripped asset
   lacks the custom type tree. Read the byte-array field immediately after that
   aligned header, as declared by the inspected `MeshDataAsset` type. Do not
   execute game assemblies or save asset changes.
2. Decode the array using the inspected `MeshData.Load` layout: vertex count,
   unused integer, submesh count and sizes, seven attribute flags, XYZ float32
   vertices, optional attribute arrays, then each submesh's index count and
   integer triangle indices. Verify full consumption and valid indices. The
   resource yielded 6,500 vertices and 11,520 triangles.
3. Merge coincident seam vertices by Euclidean distance at most `1e-6` on the unit
   sphere, checking for ambiguous matches. This tolerance only joins resource
   seams for measurement; it is not a runtime recognition tolerance. Count
   distinct neighbors in the triangle graph. The result had 5,762 distinct
   positions: exactly twelve degree-five vertices and 5,750 degree-six vertices.
   The degree-five vertices identify the pentagon centers. The other two native
   grids had four degree-three and six degree-four vertices respectively.
4. Resolve the drawing component's script/GameObject references from `level0`
   using `globalgamemanagers.assets`. Its mesh name selects `geo20`. The stored
   grid transform quaternion was `(2.1855694143368964e-8, 0, 0, 1)`; its ancestor
   rotations were identity, with zero translations and unit scales. Native
   `UIDysonEditor` assigns the selected layer's `currentRotation` to the drawing
   group and supplies the same rotation to grid line rendering. Thus the grid
   and node positions share the layer-local frame; the tiny stored X rotation
   is below the numeric allowance and is not a new product tilt.
5. Derive the existing reference directions with `scripts/derive_patches.py`.
   Normalize each pentagon's mean direction; rotate by the formula below and
   compare to normalized native centers. Require a unique correspondence for
   all twelve. The [measured fixture](../checks/fixtures/native-grid-centers.json)
   retains only these twelve numeric observations and their resource identity,
   not a redistributable game mesh. Raw extraction and comparison scripts remain
   under ignored `artifacts/grid-alignment/`.

## Result and correction

Azimuth here means `atan2(z, x)`, not Unity's signed Euler yaw convention. The
old upper-ring centers were at multiples of 72°. The native upper ring was at
18°, 90°, 162°, 234° and 306°; both had latitude approximately +26.565052°.
The lower ring was offset by another 36° at the opposite latitude. Both cap
centers were already on the poles.

Increase azimuth by **18°** for every direction, keeping Y unchanged:

```text
c = cos(18°), s = sin(18°)
x' = c*x - s*z
y' = y
z' = s*x + c*z
```

This is a proper rigid rotation: it preserves distances, edges, topology and all
latitudes. The smallest correction was selected; equivalent 72° additions are
unnecessary. The maximum normalized center discrepancy was `5.5245773e-8` for
the mesh positions, or `5.6550915e-8` including the tiny stored grid quaternion.
Both fit the existing `4u/(1-u) = 2.384185934e-7` direction allowance (`u = 2^-24`).
The previous maximum center discrepancy was approximately `0.279838511`.
All twelve centers matched uniquely after correction. This proves numeric
alignment to the measured grid; it does not substitute for an in-game visual
check. It does not claim every design vertex lies on a native grid vertex.

SB-D030 records the choice to retain the original orientation for continuation
of existing layers. That preservation is necessary because the published graph
recognizer compares positions against a fixed direction table.

## Implementation and offline validation — 2026-09-15

The generator emitted two direction tables and one shared patch/face definition.
Grid-aligned positions are the default on empty layers. Recognition first checks
that orientation, then the published one if the graph does not match; each
subsequent patch uses the recognized orientation. Matching remains based on
native content with unchanged tolerances, without a saved flag or runtime mesh
access. The post-write check also requires the intended orientation.

The [published-direction fixture](../checks/fixtures/published-directions.json)
was extracted from `src/Plan.Data.cs` at the published DLL source commit
`b92fb00cde46fa795b2f80c69aa9e5fad76a8aff`. It independently fixes the previous
float coordinates rather than recalculating them with the changed generator.
The original `derive` default was retained for historical probes and capture
checks; the new orientation is an explicit derivation argument.

Checks and observed results:

- `Build-Package.ps1 -BuildNumber 1` with the real managed and loader references
  built both modes with zero warnings/errors. All 217 emitted references agreed;
  the five shim assemblies matched the native map. No new native surface,
  dependency, plugin identity or build/version rule was introduced. The existing
  five-file ZIP validator passed. This dirty build was a local rehearsal, not the
  candidate; its `1.0.1` number did not reserve a CI version.
- `dotnet run --project checks/Logic/Logic.csproj -c Release -- artifacts/compiled-plan.json`
  passed both orientations at all thirteen graph states (empty through complete),
  each continuation step with a fresh session, shell recognition, mixed/unsupported
  orientation rejection and post-write orientation checking. Existing preservation,
  selection, partial-failure, numeric-boundary and no-op checks also passed.
- `python -B scripts/check_plan.py artifacts/compiled-plan.json` checked all twelve
  deltas, 32 faces and final 60/90 topology. Published float bits matched exactly.
  Five radii (100, 9,700, 36,000, 100,000 and 1,000,000) preserved the numeric
  envelope; these were arithmetic samples, not a newly claimed game radius range.
  Maximum direction error was `6.72304398e-8`, relative radius error
  `7.07654325e-8`, and native center error `7.27119405e-8`. All were within the
  existing respective bounds, with all twelve centers matched uniquely.
- `python -B -m unittest discover -s scripts -p test_geometry.py` passed ten tests,
  including all pairwise distances for both rigid orientations.

Compilation initially encountered denied SDK-folder access in the sandbox; it
passed through the supported permission route. This was an environment access
failure, not a source failure. Native methods and the game were not run by these
checks. Runtime alignment and real-save continuation were subsequently reported
by the owner below; they were not established by these offline checks.

## Candidate package — 1.0.60

[CI run 34991849654](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34991849654),
build 60 / attempt 1, completed successfully. The
[direct hotfix package](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34991849654/artifacts/10405409805)
was the candidate used for the owner session. Later documentation builds did not
replace this identified download.

| Field | Verified value |
| --- | --- |
| Source revision | `145a03bb49e242327ba518f082989366bc822a23` |
| Build label | `1.0.60.145a03b` |
| Thunderstore / BepInEx version | `1.0.60` |
| Plugin GUID | `dsp.spherebuilder` |
| Assembly / file version | `1.0.0.0` |
| Package | `DSPSphereBuilder-1.0.60.zip`, 52,188 bytes |
| Package SHA-256 | `C777D8A436482FC127DC204BE7B2C7FCB0DB002C2E8E7A14C54FBCE483F52B8E` |
| DLL SHA-256 | `C93F1E938CBD0E9659D9F462CADBE3181721A52BD349D79AB0C14C131DFBC486` |

The independently downloaded bytes matched both the separate CI build record
(artifact 10406082045) and hosted package digest. `Test-Package.ps1` passed against
those exact source/version/DLL-hash inputs, including DLL metadata and all five
package entries. The ZIP had no nested archive, game/shim assemblies or extra
files. CI passed the logic/geometry checks and all fifteen malformed-package
cases. Native-reference compilation was established by the same runtime-source
local rehearsal described above; the workflow did not execute native methods.

The public source archive for that full revision was downloaded and its changed
runtime files, derivation, retained reference and license files compared exactly
with Git. The candidate, extracted DLL, CI build record, log and source archive
are retained under ignored `artifacts/hosted/34991849654/`. Raw artifact/log access
required the authenticated CLI permission route after sandbox authentication
failed; no credentials were copied into the repository.

The final diff review found the correction confined to generated directions,
orientation selection and the corresponding placement/postcondition. It added
no runtime mesh reader, save state, retry/recovery layer, dependency or native
declaration. Tests assert numerical/graph behavior and published float bits,
not UI prose or bans on exception constructs. Historical derivation defaults
and archive story bodies were preserved.

## Focused owner check

The following was the handoff procedure for 1.0.60, retained for reproduction.
The owner's report is recorded below; this is not a request for another session.

Use the identified candidate in the same modded profile used for normal play.
Replace the profile's existing Sphere Builder DLL while the game is closed;
avoid loading two copies. Use a disposable save or a copy of an existing save.
Keep an unfinished layer painted by 1.0.56 for the continuation check.

1. On an empty layer with sufficient latitude unlocked, select the native
   pentagon/icosahedral grid and paint all twelve patches. Inspect the polar caps
   and both latitude rings: the native grid pentagons should sit at the design
   pentagons' centers. Capture a screenshot and the selected-layer blueprint as
   `new-complete.txt`.
2. On the older unfinished layer, copy the selected-layer blueprint as
   `legacy-before.txt`. Save, return to the menu and reload, then paint one more
   patch. Confirm that the previous construction stayed in place and the new
   patch continued its original orientation. Copy that layer as `legacy-after.txt`.

Put those three text exports, the screenshot and the session's BepInEx log in the
usual local evidence folder. Report any unexpected behavior and whether you
accept the candidate. No extra star/radius matrix, shell-filling exercise,
full-process restart sequence or UI workshop is required. These exports permit
numeric checking of the new alignment and unchanged old node/frame records;
they do not capture live construction counters, which remain an observation.

## Owner validation report — 2026-09-15

The owner reported that validation of build **1.0.60** passed: one more patch on
an existing offset layer and all twelve patches on a new grid-aligned layer
worked, with no observed regressions. The owner then requested a minor-version
promotion using the version procedure. SB-D032 records acceptance and that
promotion authority; the two reported cases complete the focused owner gate.

This is an explicit owner runtime report. No new blueprint exports, screenshot
or log were independently inspected for this acceptance, and no additional
coverage is inferred beyond the report. The existing static geometry and
preservation evidence remains separate from that observation.

## Minor promotion — 1.1

The owner chose a minor promotion for the accepted hotfix. VERSION changed from
major 1 / minor 0 to major 1 / minor 1; the sequential CI number remains assigned
by the existing workflow. The package README's Changes section describes aligned
new spheres and continued original-orientation spheres in player-facing prose.
The root changelog retains the linked implementation under Unreleased until
publication, as required by [the procedure](VERSIONING.md#change-records).

Runtime source, native reference declarations, dependencies and the build
workflow were unchanged from the accepted 1.0.60 source. That owner's acceptance
therefore carries forward to the verified version-only promotion without another
gameplay session. The new DLL metadata and package bytes were checked separately
below.

Before promotion, the live
[Thunderstore listing](https://thunderstore.io/c/dyson-sphere-program/p/DSPSphereBuilder/DSPSphereBuilder/)
identified **1.0.56** as the latest published version. GitHub listed the published
release tag `1.0`, targeting `5a0707b49e075ad7f588730b7401db6195384dda`.
The 1.1 line is numerically newer. No tag, GitHub release or Thunderstore upload
was created during promotion.

### Verified promoted package — 1.1.63

[CI run 34995172878](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34995172878)
completed successfully on 2026-09-15. Its
[direct package](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34995172878/artifacts/10406814890)
and [separate build record](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34995172878/artifacts/10407840282)
were downloaded and independently checked.

| Field | Verified value |
| --- | --- |
| Source commit | `41f9dcfc3cbea8f8ac4278a996e3be5b51681eb5` |
| CI build / attempt / working tree | 63 / 1 / clean |
| Thunderstore / BepInEx version | `1.1.63` |
| Informational version / build label | `1.1.63.41f9dcf` |
| Assembly / file / PE file version | `1.1.0.0` |
| Assembly name / plugin GUID | `DSPSphereBuilder` / `dsp.spherebuilder` |
| Package | `DSPSphereBuilder-1.1.63.zip`, 52,254 bytes |
| Package SHA-256 | `618109C96D606B7ACA40822168BECF90942DB76298B875A73A918E2B5219ABAC` |
| DLL SHA-256 | `F2B2A452842F2CBCD6FD86BB95419706995C41A69FB7A5E48873A67DF6D80B6D` |

The downloaded ZIP had exactly the five required files, with no enclosing or
nested package ZIP. Its digest matched both GitHub's artifact digest and the
build record; the DLL digest also matched. The existing package validator passed
all identity fields above, UTF-8 text, the unchanged 256×256 PNG, complete licenses
and revision-specific source links. Metadata inspection examined 217 emitted
references without loading the plugin. The packaged Changes section described
both new alignment and continuation of existing spheres.

The public source archive was accessible; 51 runtime, reference, delivery,
version, packaging and license inputs matched the nominated Git commit byte for
byte. Runtime source, native declarations and build scripts/workflow were unchanged
from `145a03bb49e242327ba518f082989366bc822a23`. CI compilation reported zero
warnings/errors; production logic, both-orientation geometry and all 15 malformed
package checks passed. Native compilation and gameplay were not repeated for this
metadata/documentation-only delta; the preceding native build evidence and the
owner's 1.0.60 report apply to its unchanged runtime implementation.

The ZIP, DLL, build record, source archive and CI log were retained under ignored
`artifacts/hosted/34995172878/` outside CI artifact retention. Numeric comparison
confirmed `1.1.63` is newer than the observed published `1.0.56`. Current nomination,
acceptance and manual publication state remain in [PROJECT.md](PROJECT.md#current-phase).
