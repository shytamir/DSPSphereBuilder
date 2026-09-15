# Pentagon grid alignment evidence

[PROJECT.md](PROJECT.md) owns decisions, progress and acceptance. The
[hotfix roadmap](management/ROADMAP.md) defines scope and gates. This record
separates native-data measurements, offline checks and owner observations.

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
checks. Runtime alignment and real-save continuation still require owner evidence.
