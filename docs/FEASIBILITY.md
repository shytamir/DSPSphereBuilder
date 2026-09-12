# Feasibility evidence

Current execution state, decisions, and gate results are in [PROJECT.md](PROJECT.md).
This record describes observed inputs and findings. References to native members
are inspection coordinates, not a copied implementation.

Research commands below run from the repository root. Python checks use the
standard library only; use an available Python 3 interpreter and `-B` to avoid
writing bytecode beside the scripts. Outputs belong under ignored `artifacts/`.

## SB-F1.1 — Target and probe environment

### Target identity

On 2026-09-12, read-only PE metadata and SHA-256 inspection reconfirmed the
[target assembly](PROJECT.md#target-game-reference): `Assembly-CSharp`, managed
version `0.0.0.0`, MVID `ece4a40e-5e73-43f4-a9f8-4e74970b5942`, 7,830,016 bytes.
The hash matched the planning baseline. No game assembly was loaded for execution.

`GameConfig`'s static initializer sets `_gameVersion` to `0.10.34`. Its separate
`build` field does not have a value in that initializer. Inspection of GameConfig,
Configs, and GameMain did not establish the complete displayed build number.
The exact target is therefore the recorded hash, with the build suffix explicitly
unknown; the blueprint's `28529` suffix is not substituted for it.

The assembly references `netstandard` version `2.1.0.0`, UnityEngine.CoreModule,
UnityEngine.UI `1.0.0.0`, and other Unity modules. A narrow future probe can target
`netstandard2.1` against the real local references. Additional modules should be
referenced only when the inspected operations actually consume them.

### Local tools and dependencies

| Input | Observed identity | SHA-256 |
| --- | --- | --- |
| BepInEx.dll | 5.4.17.0 | `DC1CB6B58B962BDA5AAA1D6B5F9AE14EC174F61836A1A1F96C1A040C7E8381F7` |
| UnityEngine.CoreModule.dll | 0.0.0.0 | `E2B5AE2FD12646D03FC3D04D1A37D522572A3B97022FE1B95BBF2A2F2B04853A` |
| UnityEngine.UI.dll | 1.0.0.0 | `54953EBD7C9B4B39279876B37109F0F503938847F2A7BE4A22D62E9B94C347EB` |

The colocated UnityPlayer reports product version `2022.3.62f3c1 (1623fc0bbb97)`
and file version `2022.3.62.1451004`. These identify the local engine, not a
game-build suffix. The installed loader assembly is available as a compile input;
its presence alone does not demonstrate that a live session loads it successfully.

PowerShell 7, .NET SDK `10.0.302`, the `NETStandard.Library.Ref` `2.1.0` pack,
and ILSpy command line `11.0.0.9375` are available. The existing ILSpy executable
was used read-only; no tool was installed and no other mod's source was adopted.
No compilation was claimed: this story creates no plugin project.

### Reproduction

Set `$managed` to the selected game's `DSPGAME_Data/Managed`, `$loader` to its
`BepInEx/core`, and `$ilspy` to the available ILSpy command-line executable.
From the repository root:

```powershell
./scripts/Read-AssemblyIdentity.ps1 -Path (Join-Path $managed 'Assembly-CSharp.dll') |
    ConvertTo-Json -Depth 4
./scripts/Read-AssemblyIdentity.ps1 -Path (Join-Path $loader 'BepInEx.dll'),
    (Join-Path $managed 'UnityEngine.CoreModule.dll'),
    (Join-Path $managed 'UnityEngine.UI.dll') | ConvertTo-Json -Depth 4
& $ilspy --disable-updatecheck -t GameConfig -r $managed (Join-Path $managed 'Assembly-CSharp.dll')
dotnet --list-sdks
```

The identity reader uses PE metadata; it does not load or execute inspected
assemblies. Decompiler output is kept under ignored `artifacts/inspection/`.
Future probe compilation will use the installed SDK and explicit reference paths;
the project and its actual build command belong to SB-F3.1.

### Live-test boundary

The owner agreed to run the live probe in a disposable test save when ready.
The test handoff must identify the probe source/artifact, the target hash, required
loader, instructions, expected observations, and cleanup. Before interpreting
results, compare the selected game's assembly identity and record the displayed
game-build number and loader actually used. The owner operates the live game;
no agent game launch, installation change, or save mutation has occurred.

The missing complete game-build label is not a substitute-runtime permission.
A hash mismatch requires a target-baseline decision before consuming observations.
No unavailable tool prevents the next static investigation. Live observations
remain unavailable until the owner runs the identified probe.

## SB-F1.2 — Native placement and lifecycle

### Operation map

The following members were statically inspected with ILSpy 11 against the same
target hash. This establishes available code paths, not observed live behavior.

| Concern | Native source | Finding |
| --- | --- | --- |
| Selected target | `UIGame.dysonEditor`, `UIDysonEditor.selection`, `DESelection.singleSelectedLayer` | Public editor/selection surfaces exist. A single layer is returned only when exactly one layer is selected. |
| Star change | `DESelection.SetViewStar` | Clears selections and resolves/creates the viewed star's Dyson sphere through GameMain.data. |
| Editor lifetime | `UIDysonEditor._OnOpen`, `_OnClose`, `_OnFree` | Open selects a star; close clears selection; free releases the selection object and native editor components. |
| Node creation | `DysonSphereLayer.NewDysonNode(int, Vector3)` | Allocates/reuses one pool slot, assigns layer-local position, initializes new node construction, registers render data, updates native auto-node selection, and rebuilds models. Returns its ID. |
| Frame creation | `DysonSphereLayer.NewDysonFrame(int, int, int, bool)` | Rejects missing/equal endpoints or an existing connecting frame with return value zero before allocation. Otherwise creates one frame, adds endpoint adjacency, recalculates native requests, and rebuilds models. |
| Blueprint replacement | `DysonSphereLayer.ImportFromBlueprint`, `ResetNew` | Resets pools and imports new records. It is not an additive operation and cannot preserve existing record identity/progress. |
| Shell ownership | `UIDysonBrush_Shell._OnUpdate`, `DysonSphereLayer.NewDysonShell` | Native brush discovers a bounded closed cycle before creating a shell. Shell creation is a distinct operation, unnecessary for adding the framework. |
| Layer identity | `DysonSphere.AddLayer`, `AddLayerOnId`, `RemoveLayer`, `QueryLayerId` | Removal frees the object and vacates its ID; a later layer can occupy that ID. A numeric layer ID alone is not durable ownership. |
| Save/load | `DysonSphereLayer.Export/Import`, `DysonNode.Export/Import`, `DysonFrame.Export/Import`, `DysonShell.Export/Import` | Native records serialize geometry, construction, and relationships; import reconstructs objects. References retained across load cannot be assumed valid. |

The node/frame brushes call creation from their UI update flow. A later probe
should perform one synchronous action on that same Unity UI thread, resolving
the current selected layer at the time of action. A child Unity UI control under
the editor is a candidate integration point; placement/input coexistence is
reserved for the live observation in SB-F3.3. No event hooks or controls were added.

### Placement checks are separate from allocation

`UIDysonBrush_Node._OnUpdate` combines `RecalcCollides`, its private
`CheckCondition`, and latitude comparison before calling `NewDysonNode`.
It also has a precise-position input path, which normalizes the supplied
direction and multiplies by the selected layer radius; grid snapping is not
the only native creation path.

`UIDysonBrush_Frame.CheckCondition` checks endpoint latitude, normalized chord
length (maximum `0.518f`), proximity to other nodes/frames, duplicate/crossing
frames, and shell interior conflicts. Latitude is rounded from the absolute
normalized Y component and compared with rounded `GameMain.history.dysonNodeLatitude`.
The constructor itself does not repeat these geometric/research checks.
SB-F2.3 must evaluate them; direct constructor success would not prove compliance.

Node positions are layer-local radius-scaled vectors. Rendering applies
`currentRotation`; the design must not bake the layer's animated rotation into
its saved node positions. `DysonFrame.euler` selects the interpolation form.
The reference's actual frame mode and numeric precision remain SB-F2 questions.

### Construction and failure boundary

`NewDysonNode` initializes only the allocated node with `sp = 0` and `spMax = 30`.
`NewDysonFrame` initializes only the new frame's `spA/spB` and computes its
`spMax` from native segment count. It appends the new frame to existing endpoint
adjacency and calls their `RecalcSpReq`; this legitimately changes requested
construction and aggregate totals without resetting invested node/frame points.

The preservation probe should compare existing node `sp`, frame `spA/spB`, raw
positions and adjacency, and shell `nodes/frames/nodecps`. Derived request totals,
auto-node choice, render indices, and aggregate maxima can change legitimately
when the graph expands. Runtime construction can also advance between observations.

These methods are not transactions: allocation and adjacency changes precede
renderer/auto-node calls. An exception after those mutations can leave a partial
delta. No rollback was found in the inspected creation methods. SB-F3.1 must test
the bounded response; retries must not blindly recreate already-added elements.
The ordinary frame rejection paths return zero before mutating the graph.

Native save data provides a candidate source for reconstructing the next patch,
but the mod's logical patch index is not a native field. Matching geometry after
save/load, manual edits, and recycled IDs remains unproven until SB-F3.2. No custom
persistence requirement is inferred from that gap.

### Reproduce and resolve remaining questions

Use the SB-F1.1 inspection command with the types `DESelection`, `UIDysonEditor`,
`UIDysonBrush_Node`, `UIDysonBrush_Frame`, `UIDysonBrush_Shell`, `DysonSphere`,
`DysonSphereLayer`, `DysonNode`, `DysonFrame`, and `DysonShell`. Their member names
above locate the findings independently of decompiler line numbers.

| Remaining question | Owning story and check |
| --- | --- |
| Exact reference graph, frame modes, and float fidelity | SB-F2.1: inspect source records and measure geometry |
| Legal polar route and complete per-click deltas | SB-F2.2: deterministic graph/coordinate checks |
| Radius, unlock, spacing, crossing, and shell-face limits | SB-F2.3: evaluate native predicates and bound live cases |
| Preservation and partial mutation in a real initialized game | SB-F3.1: identified before/after probe |
| Resumption and correct target after selection/load/edit changes | SB-F3.2: native-data reconstruction and transition observations |
| Normal editor action and player shell filling | SB-F3.3: full traversal and human observations |

The inspected API supports a candidate additive path. No fundamental missing
static capability was found. Neither the full design nor live preservation is
claimed validated by this finding.

## SB-F2.1 — Canonical reference geometry

### Source identity and reuse

Retrieved 2026-09-12 from the [published sphere](https://www.dysonsphereblueprints.com/en/blueprints/dyson-sphere-best-cost-efficiency-optimized-sphere-design-60-nodes-15-cheaper-than-football).
Its exact `DYBP` string, without surrounding whitespace, has SHA-256
`d6d9c52f4baf1167739a7dc421e0cb362ecd50af08686396ecd40919ef06f2c7`.
The decoded payload is 8,205 bytes, SHA-256
`b345c98c8778667d45df790dc2f76d2bf0f5c46b43a9454a944e801737d91f7c`.
Header version is `0.10.34.28529`, layer type `1`, latitude `81`; the page's
separate game-version label says `0.10.34.28524`. Neither identifies our target.
The author's optimality claim remains outside this investigation.

The [site terms](https://www.dysonsphereblueprints.com/terms), dated 2026-09-09,
retain contributor ownership and grant the site a content license; they do not
establish a general mod-distribution license. The published full string remains
an external comparison input, not a newly committed asset.

The author's [repository fixture](../research/cosmin1490/README.md), pinned to
`bf00f4b2334c93215f63e0291f9acb6003c9a663`, is retained under its upstream GPL-3.0
terms with the unmodified license. Its raw file/string SHA-256 is
`96bd7badd6d0bd971df477b74622c0296b483e7b60e77aa3074cdf0b239620e0`;
payload SHA-256 is `804bb4284bb432c5867982334fb564967c21c99c61c4d4b779e0981b7fd69a64`.
It has a different header (`0.10.29.21950`) and coordinate scale, but the comparison
below establishes matching geometry within encoding precision. SB-D004 records
this research-input choice. No generator code was copied and the mock package
does not include the fixture.

### Measured geometry and canonical mapping

The independent [reader](../scripts/blueprint_geometry.py) follows the inspected
single-layer record layout, consumes all payload bytes, and checks pool identities.
Gzip integrity is checked by decompression. This tool does not implement the game's
custom `MD5F` signature algorithm or claim an in-game import result.

Both full inputs have 60 nodes, 90 unique frames, and 32 shell boundary records.
Every node has degree three; the graph is connected. Each of twelve pentagons
owns five distinct vertices. Twenty hexagons complete a convex closed surface:
every frame belongs to exactly two boundaries and `60 - 90 + 32 = 2`.
All frames have `euler=false`, hence great-circle arcs rather than Euler paths.
Prototype records are node `0`, frame `1`, shell `0`; these are serialized values,
not yet verified creation-argument choices.

Canonical node IDs retain source IDs 1–60. Pentagon `Pk` owns IDs
`5k-4` through `5k`, with consecutive perimeter edges and the closing edge.
Edges are unordered endpoint pairs; frame pool IDs and shell enumeration order
are not canonical identities. Face IDs use lexicographically sorted cyclic
boundaries, ignoring starting vertex and winding. The verifier emits all mappings.

| Quantity, after normalizing positions | Published measurement |
| --- | --- |
| 60 pentagon perimeter chord lengths | 0.447837935801–0.447837955976 |
| 30 inter-pentagon spoke chord lengths | 0.324058597073–0.324058633943 |
| Greatest within-face plane-distance spread | 1.988675 × 10⁻⁸ |
| Greatest direction difference from repository fixture | 2.545625 × 10⁻⁸ |

Pentagons have equal perimeter chords and lie on small circles within the measured
precision; they are regular to that precision. Each hexagon alternates three
perimeter edges and three shorter spokes. Native frame angle is
`2 asin(chord / 2)` and arc length is that angle times layer radius. Straight
chord length and native spherical arc length are not interchangeable.

Repository-to-published node mapping is exactly `i → i`; all 90 endpoint pairs
and all 32 cyclic boundaries agree. The sample has ten nodes, ten frames, and no
shell records. Its IDs 1–10 map to published IDs
`11,12,13,14,15,19,24,32,53,58`; raw coordinates match those published nodes exactly.
Its five spokes reach five different pentagons. It remains a five-spoke example,
not the intended one-leading-spoke first click.

### Precision rule and reproduction

Positions are binary32 components. For rounding unit `u = 2^-24`, the vector
rounding error is bounded by `u` times its magnitude. Normalizing two independently
encoded vectors gives a conservative direction-difference bound
`4u/(1-u) = 2.384185934 × 10⁻⁷` (binary64 analysis arithmetic is negligible here).
The comparison uses Euclidean distance between normalized vectors, a bijective
node match, and exact graph connectivity. Edge-class spread and face planarity
checks allow twice that bound. All observed errors are below it; the files are
not bit-identical and are not described as exact real-number geometry.
This is an input-comparison rule, not a blanket runtime mutation tolerance.

```powershell
python -B scripts/verify_geometry.py --output artifacts/geometry.json
python -B scripts/test_geometry.py
# Optional independent published input, saved as plain DYBP text:
python -B scripts/verify_geometry.py --published artifacts/reference/published.txt --output artifacts/reference/verified.json
```

Both verification runs passed. Five focused tests passed: radial scaling retains
geometry; a removed frame, displaced node, non-boundary cycle, and truncated
payload are rejected. Shell records are used only to verify boundaries;
this does not authorize automatic shell creation or prove player filling.

## SB-F2.2 — Polar twelve-patch traversal

The [derivation](../scripts/derive_patches.py) chooses P1 as north and P4 as south.
North is the normalized mean of P1's five unit directions. The local +X axis is
node 1's direction projected perpendicular to north; +Z is `X × north`. Dotting
each source direction with those three axes is a rigid rotation with determinant
+1. It sets a fixed design orientation within the layer, independently of the
layer's animated orbital rotation. No vertex is moved separately to force a fit.

The rotation rows, rounded here for display only, are:

```text
[-0.688190966397, -0.425325392017,  0.587785253877]
[-0.525731097838,  0.850650817178,  0.000000000000]
[-0.500000006536, -0.309016986814, -0.809016993223]
```

Pentagon-center latitudes are ±90° for the caps and approximately ±26.565052°
for the rings (ring spread below 0.0000025° from encoded coordinates).
Vertices reach approximately ±67.607230°; the cap centers are not nodes.
SB-F2.3 determines the native unlock consequence.

Each ring is ordered by increasing `atan2(z,x)`, cyclically starting the upper
ring at its lowest numbered pentagon. The lower ring starts at the lowest
numbered lower-ring neighbor of the upper ring's final pentagon. This gives:
**P1 → P2 → P6 → P12 → P11 → P8 → P7 → P9 → P10 → P5 → P3 → P4**.
Both rings form native graph cycles. Every transition uses a real short spoke;
the equator crossing is P8–P7 through nodes 38–35.

### Complete deltas

Node IDs and Pk are defined in SB-F2.1. Every row adds the five perimeter frames
of its Pk; none already exists. In addition, add the listed closing connections
and leading spoke. Each row after the first reuses exactly its incoming spoke
(the preceding row's leading edge) and one existing endpoint. All other earlier
nodes and frames remain untouched. An edge `a–b` is unordered.

| Click / Pk | New nodes | Reused node | Other closing connections | Leading spoke | Total nodes / frames |
| --- | --- | --- | --- | --- | --- |
| 1 / P1 | 1–6 | — | — | 4–6 | 6 / 6 |
| 2 / P2 | 7,8,9,10,27 | 6 | — | 10–27 | 11 / 12 |
| 3 / P6 | 26,28,29,30,56 | 27 | 5–26 | 30–56 | 16 / 19 |
| 4 / P12 | 54,57,58,59,60 | 56 | 1–60 | 54–59 | 21 / 26 |
| 5 / P11 | 37,51,52,53,55 | 54 | 2–55 | 37–51 | 26 / 33 |
| 6 / P8 | 35,36,38,39,40 | 37 | 3–36; 7–40 | 35–38 | 31 / 41 |
| 7 / P7 | 31,32,33,34,42 | 35 | 31–52 | 34–42 | 36 / 48 |
| 8 / P9 | 41,43,44,45,49 | 42 | 8–45; 39–41 | 44–49 | 41 / 56 |
| 9 / P10 | 22,46,47,48,50 | 49 | 9–50; 28–46 | 22–47 | 46 / 64 |
| 10 / P5 | 12,21,23,24,25 | 22 | 21–29; 25–57 | 12–24 | 51 / 72 |
| 11 / P3 | 11,13,14,15,19 | 12 | 11–58; 14–32; 15–53 | 13–19 | 56 / 81 |
| 12 / P4 | 16,17,18,20 | 19 | 16–48; 17–43; 18–33; 20–23 | — | 60 / 90 |

Every click closes its own pentagon. The following table names each newly closed
hexagon by the three pentagons surrounding it; no other face closes on that click.
These are boundary closures, not shell creation.

| Click | Newly closed hexagons, by surrounding Pk numbers |
| --- | --- |
| 1–2 | — |
| 3 | (1,2,6) |
| 4 | (1,6,12) |
| 5 | (1,11,12) |
| 6 | (1,8,11); (1,2,8) |
| 7 | (7,8,11) |
| 8 | (2,8,9); (7,8,9) |
| 9 | (2,9,10); (2,6,10) |
| 10 | (5,6,10); (5,6,12) |
| 11 | (3,5,12); (3,11,12); (3,7,11) |
| 12 | (3,4,5); (3,4,7); (4,9,10); (4,5,10); (4,7,9) |

### Verification

```powershell
python -B scripts/derive_patches.py --output artifacts/patches.json
python -B scripts/test_geometry.py
```

The output records full rotated directions, exact endpoint-pair deltas, reuse,
and all cyclic boundaries. Checks passed for all twelve monotonically growing,
connected graphs, exactly one future endpoint/spoke before the last click, and
final equality to all 60 nodes, 90 frames, and 32 reference boundaries.
Seven focused tests passed, including unchanged deltas when input pool enumeration
is reversed and preservation of all 1,770 pairwise node distances under rotation
to fourteen decimal places. The route is reproducible and agrees with the concept;
no alternate route, adjustable orientation, or shell operation was introduced.

## SB-F2.3 — Native placement envelope

### Predicates and measured margins

The target's node/frame brushes normalize positions before their geometric
comparisons. The [envelope check](../scripts/check_envelope.py) evaluates those
geometric conditions over the entire graph, then checks every delta against all
face boundaries that could already have been filled. It is an independent numeric
analysis, not execution of the Unity editor or a replacement for its controls.

| Native condition | Target member / rule | Result for the rotated plan |
| --- | --- | --- |
| Node spacing | `UIDysonBrush_Node.RecalcCollides`: squared unit distance below 0.0051122503 collides | Minimum 0.105013969 |
| Node-to-frame and frame endpoint proximity | Both brushes' `PointToSegmentSqr` and `CheckCondition`: below 0.0027562499 refuses | Minimum nonincident distance squared 0.105013969 |
| Frame length | `UIDysonBrush_Frame.CheckCondition`: unit chord above 0.518 refuses | Maximum 0.447837957 |
| Frame crossing | Same member: normalized great-circle side products both below −10⁻⁹, excluding shared endpoints | Zero crossings among 120 nonincident candidate pairs |
| Shell cycle size | `UIDysonBrush_Shell._OnUpdate`: normalized centroid-to-corner squared distance must not exceed 0.26832402 × 0.6 | Pentagon maximum 0.150811786; hexagon maximum 0.156343699; limit approximately 0.160994412 |
| Shell candidate discovery | Same member: candidates within squared distance 0.16099441 of the cursor; nearby walls within 0.26832402 | At each face center, candidate nodes are exactly its five or six vertices |
| Other nodes in a shell | Same member: a closed cycle must contain no other node | All 32 convex face interiors are empty |
| Adding through an existing shell | Node/frame `CheckCondition`, `DysonShell.IsPointInShell` | No later node or new-frame midpoint lies inside any previously closed reference face |

The point-to-segment calculation projects onto the 3D chord, clamps to its ends,
then normalizes the result back onto the sphere, matching the inspected geometric
rule. Checking all nonincident node/frame pairs is stronger than the brushes'
nearby-node filtering. Crossing checks use their 0.13443033 neighborhood filter;
testing antipodal great-circle extensions as if they were nearby frames would be
incorrect. `DysonFrame.GetSegments` returns just the two endpoints for these
non-Euler frames. No Euler interpolation approximation is involved.

Shell interior checks use the equivalent convex spherical half-spaces established
in SB-F2.1. Native cursor tracking, cycle search, raycast parity, and shell mesh/cell
generation remain live observations in SB-F3.3. Their success is not inferred from
having parsed a shell record. At a face center, all boundary vertices are within
both native collection limits, with a minimum shell-size margin above 0.00465.

### Latitude and native layer bounds

The node brush and frame `CheckCondition` compare rounded absolute node latitude
to `Mathf.RoundToInt(GameMain.history.dysonNodeLatitude)`. The rotated maximum is
67.607230°, so the required rounded unlock is **68°**, already needed on click 1;
no later click requires more. The reference's original 81° import header does
not apply to this rotation. Great-circle edges may curve poleward; the inspected
unlock predicates test nodes/endpoints, not every point on an arc.

`GameHistoryData` starts `dysonNodeLatitude` at zero, saves/loads it, and adds
unlock-function 26's value when research grants it. Research names, level numbers,
and increments come from runtime prototype data and were not established from
this assembly. The probe must record the actual value and available research;
it must not guess a technology level from the 68° predicate.

`DysonSphere.Init` derives star-specific minimum and maximum layer radii from
`physicsRadius`, `dysonRadius`, and star type, then rounds to 100-unit steps.
The initial minimum floor is 4,000; giants apply a 0.6 multiplier before rounding.
`CheckLayerRadius` also excludes the first planet's orbit band and layers within
999.95 units. These are **layer creation** constraints: calling that method on an
existing selected layer would find the layer itself and wrongly refuse it.
The candidate operation uses the selected native layer and does not create,
resize, or reposition layers. Layer selection and current native bounds are read
from the viewed sphere, not a hard-coded star or radius list.

All inspected placement distances, shell collection limits, and latitude checks
operate on unit directions; positive uniform scaling therefore leaves their
mathematical result unchanged. Radius affects frame segmentation and construction
cost: `DysonFrame.segCount` rounds `arcAngle × radius / 600`, doubles the result,
and applies a minimum of two; `spMax` is ten times that count. This rules out a
fixed rocket-cost claim. No radius-dependent geometric restriction was found for
this graph beyond the game's layer bounds.

The check also quantizes scaled coordinates and normalization arithmetic to
binary32 at radii 2,400, 10,000, and 1,000,000. All predicates passed; maximum
chord stayed below 0.447838006, largest shell distance below 0.156343729, and
required rounded latitude remained 68. These are numerical scale checks, **not
claims that those radii are legal in a particular star**. The sampled values do
not prove native rendering or resource allocation at every radius. The scale
derivation supports a candidate range; actual star-specific endpoint observations
are required below. Generated finite coordinates should be checked before mutation.

### Candidate envelope and live cases

The candidate supports a native-created selected layer in the currently viewed
system, initially empty or later recognized as our own partial graph, at its
existing game-supported radius. It requires rounded unlocked latitude at least
68 and the fixed SB-F2.2 orientation. It preserves native layer bounds and does
not bypass research, placement limits, or shell control. Continuation recognition
is still SB-F3.2's question. Arbitrary existing designs are outside the concept.

Nodes use creation prototype 0 and geodesic frames creation prototype 0. The
editor declares one node prototype and three frame prototypes; renderer loading
determines actual counts, and native frame creation offsets by that node count.
The probe must record loaded counts before confirming the serialized frame ID 1
mapping. It should use a normal editor action in its current layer context.

| Live case | Required observation / owner story |
| --- | --- |
| Below the first usable unlock, then sufficient research | Clear refusal with no added records, followed by the six-node/six-frame first patch; log actual research values (SB-F3.3) |
| Ordinary legal radius, partially built then completed earlier structure | Correct next delta, preserved raw invested construction and existing records (SB-F3.1) |
| Smallest and largest radius accepted by one star's native layer controls | Full geometry and successful manual pentagon/hexagon filling; log actual limits and selected radii (SB-F3.3) |
| Another system, including a giant if the save permits | Correct selected sphere/layer; log star bounds and any skipped case without claiming coverage (SB-F3.2/3.3) |
| Fill a closed pentagon and hexagon before continuing | Existing shells preserved; later frames are accepted; final graph remains complete (SB-F3.1/3.3) |
| Missing/multiple layer selection or unrelated nonempty graph | Refusal without mutation; no automatic target creation or repair (SB-F3.2/3.3) |

Static inspection found no concept-breaking geometry constraint. This remains a
candidate support envelope until identified live observations confirm it. Any
runtime-only restriction must return for an owner scope decision; it cannot be
hidden behind a narrower advertised radius range.

```powershell
python -B scripts/check_envelope.py --output artifacts/envelope.json
python -B scripts/test_geometry.py
```

The unit-geometry and three quantized-scale runs passed. Ten focused tests passed,
including projection clamping, projection back onto the sphere, and invalid-radius
rejection. No game code was executed for these checks.

## SB-F3.1 — Additive probe preparation

The initial disposable probe applied only the first two checked deltas. Its
corrected [source](https://github.com/shytamir/DSPSphereBuilder/blob/4c49a0c666d1ef39a4ce24cfb129fbec42ee25e6/probe/Probe.cs)
and [operator procedure](https://github.com/shytamir/DSPSphereBuilder/blob/4c49a0c666d1ef39a4ce24cfb129fbec42ee25e6/probe/README.md)
identify the SB-F3.1 experiment; the current probe evolves in the subsequent story.
The selected native layer is resolved at each button click. A panel beneath the
editor's control panel uses native Unity UI, `UIBlockZone`, and the editor's GUI
rectangle list; actual layout and interaction await observation. No Harmony patch,
loader migration, shipping plugin scaffold, or mock-pipeline change was needed.

The source compiles as `netstandard2.1` against the identified local assemblies,
including Unity UI, text rendering, and JSON serialization modules. The compile
completed with zero warnings and zero errors using the installed .NET SDK.
The sandbox initially denied MSBuild access to installed SDK metadata; the same
build command succeeded in the desktop context. No SDK or package installation
was performed. No game method was executed by compiling the probe.

```powershell
./scripts/Build-Probe.ps1 -DspManagedPath $managed -BepInExCorePath $loader -PythonCommand python
```

The build emits an ignored ZIP containing the DLL, operator instructions, original
license, and the reference asset/license. Its embedded input contains only the
eleven nodes and two deltas needed here. The build records the source revision
and marks dirty source explicitly; final handoff uses a clean revision. The probe
checks and records the actual game assembly hash at startup and writes the loaded
game/build, Unity version, prototype counts, selected target, and raw snapshots.
The DLL does not bundle game or loader dependencies.

Snapshots record node positions, raw SP/order counts, endpoint SP, graph links,
shell associations, shell node CP, and colors. Direct object-reference comparisons
occur in process and their failures are exported; JSON IDs alone are not treated
as proof of object identity. Comparisons permit increased construction and do not
require native request/selection/render bookkeeping to stay unchanged. Counts,
canonical-to-native node mappings, and the final expected graph are checked.

Before any mutation, a snapshot is written. Known refusals capture actual
before/after state. Unexpected exceptions stop further painting; any captured
partial result is retained for inspection, without retry, deletion, or rollback.
The separate empty-layer rejection experiment makes one valid node call followed
by a same-endpoint frame call. It tests the known native return-zero branch after
an earlier successful mutation; it is not evidence that a valid planned patch
spontaneously reaches that branch, and it adds no fault-injection framework.

The probe's temporary in-memory tracking is sufficient to compare two consecutive
actions on the same layer. It is not an MVP resumption policy. SB-F3.2 will assess
native reconstruction only after the additive observations satisfy SB-F3.1.
At preparation time, partial/completed construction, preservation, shell handoff,
control behavior, and rejection had not been observed in the running game.
The owner-run records below distinguish subsequent observations from gaps.

### First owner run: serialization failure before placement

On 2026-09-12 the owner stopped at case A and supplied eight JSON files from the
installed probe. Source revision was `88074c48f9944a61e5b2208d5edae9e432a7e013`;
the installed DLL SHA-256 matched the handed-off DLL:
`7B31E10D24AC923BA826BDCDBE7442B5358E07D56A33957996BDF75162EA22D4`.
Reports confirmed the target assembly hash/MVID, Unity `2022.3.62f3c1`, one loaded
node prototype, and three frame prototypes. The reported game version was
`0.10.34` with `GameConfig.build = 0`; the full player-facing suffix remains unknown.

The no-selection and unrelated-nonempty-layer refusals reached the owner through
the probe UI. The first eligible Paint action produced a `NullReferenceException`
in `Probe.Run`, reported at IL offset `0x00205`. Inspection of the matching binary
places that block at `completed == plan.patches.Length` (the array length read is
at `0x00216`). The plan root/reference hash was loaded, but its patch array was
null. The native creation loops follow this block: this failure occurred before
any placement call. It does not establish a native additive-placement failure.

Every supplied report also omitted its nested snapshot fields, including
`20260912T021543.5271080-paint-before.json` and the matching `-paint-error.json`.
Their SHA-256 values are recorded below. Consequently these files cannot verify
selected radius, unlock value, graph counts, or preservation. Empty failure arrays
are not accepted as preservation evidence when the underlying snapshots are absent.
Original files were read only; an ignored local copy preserves the supplied run.

| Input | SHA-256 |
| --- | --- |
| Case A before report | `40C4600990B252C1458B58A499A52B1010209DBC34C7DAE898E3AE4FD1A80F35` |
| Case A error report | `99462E777178CDC085C82581DA2E8185B83CDBAD61BC8E66C8EFFCB0C952F5BC` |

The observed defect is the probe's `JsonUtility` path failing to retain custom
nested data in this runtime. Its internal Unity cause is not established. The
fix replaces plan reading and report writing with the standard managed
`DataContractJsonSerializer`; no external package or geometry change is needed.
A missing plan array now fails initialization rather than reaching a Paint action.

The build now runs [serialization regression checks](../tests/probe-json/Program.cs)
against the actual compiled DLL and embedded plan before packaging. They compare
every node coordinate and patch endpoint with an independent JSON reader, verify
populated node/frame/shell progress and associations, preserve empty snapshots and
identity mappings, and exclude native object references. Compilation and these
checks passed with zero compiler warnings/errors. They execute under .NET 10,
not the game's Unity/Mono runtime; they do not replace another owner run.

Retest with the replacement DLL after restarting the game. Repeat the quick
refusal cases so their before/after records can be verified, then resume the
procedure at case A on an empty layer. Cases A–C and additive preservation still
require live observations; no later story is advanced by this fix.

### Second owner run: additive results and case B gap

The owner supplied the corrected probe's evidence on 2026-09-12 and reported no
observable problems other than case B's fresh-layer refusal. Reports identify
source `4c49a0c666d1ef39a4ce24cfb129fbec42ee25e6`, the target hash/MVID above,
Unity `2022.3.62f3c1`, and populated nested snapshots. Original files were read
only; an ignored local copy was audited against the derived first-two-patch graph.
All selected cases below used star ID 60 with latitude unlock 90.

- **Case A:** `20260912T022658.9418313-paint.json` added 6 nodes / 6 frames
  to empty layer 2 at radius 9,700. `20260912T022826.6967923-paint.json`
  extended it to 11 / 12. The original six nodes held 76 invested SP before
  and after addition; frame SP was zero and no shells existed. Direct in-process
  preservation comparisons reported no failures. Independent record comparison
  confirmed retained positions, properties, progress, and prior connectivity.
  The leading endpoint was reused and both resulting graphs match the intended
  deltas. Maximum normalized direction discrepancy from the derived reference
  was 3.575 × 10⁻⁸ (rounded upward).
- **Refusals:** No selection was reported without a target or placement.
  The unrelated-layer refusal at `022640.2076894` preserved 56 nodes,
  103 frames, and 48 shells, including invested construction. The end-of-probe
  refusal at `022842.7449293` preserved 11 / 12 and two manually added shells.
  These establish refusal behavior, not shell preservation during addition.
- **Case B:** `20260912T025221.8173508-snapshot.json` shows layer 2 at radius
  9,700 with 6 nodes / 6 frames / 1 shell. All nodes and frames were complete:
  180 node SP and 800 frame SP. The pentagon shell had 14,480 total node CP.
  Subsequent Paint attempts refused the nonempty untracked layer; their
  snapshots and comparison results show it remained intact. The owner clarified
  that a partially formed layer had been deleted and a fresh one created for
  faster construction, and recalled a save reload between cases A and B. The
  supplied records do not establish the exact ordering of those transitions.
  Reload replaces native layer objects, so the probe's reference-based tracking
  cannot resume that loaded layer. Source inspection confirms deselection alone
  does not clear tracking; starting another empty layer replaces the tracked one.
- **Fresh-layer repeat:** `025238.9057476` and `025244.8357915` successfully
  painted both patches on layer 1 at radius 36,000, with correct topology and
  maximum direction discrepancy 3.585 × 10⁻⁸ (rounded upward). Before the second
  addition, all node/frame SP was zero and no shells existed. This is another
  successful additive result, but cannot substitute for case B's completed
  structure and preexisting shell. Returning to this tracked layer succeeded
  even after an intervening refusal on layer 2.
- **Case C:** `20260912T025256.2528989-rejection.json` records an empty layer 3
  at radius 25,700 receiving one node, then frame return ID 0 for the intentional
  same-endpoint request. `025257.6156564-snapshot.json` confirms 1 node / 0 frames
  remained. This establishes lack of rollback across those two native calls;
  it does not demonstrate failure of a valid patch.

Critical input identities (SHA-256; timestamps above refer to files from this run):

| Input | SHA-256 |
| --- | --- |
| Case A second Paint | `10C205A70BD97C6B9F6242951CBCD41CEFCD7F10FB59F51D087FFEB354E4D806` |
| Case B completed snapshot | `7EAFF570809155B3880A17AE9EC08BBD0E3B16BB6D517B1CFF45CCD3670A6443` |
| Fresh-layer second Paint | `F80F33D08DFC4FF467AEA31F93CA51B577803A552886674E4A1B885657900DE1` |
| Native rejection | `4480B66C368A9BF91AAB8561486A8242102A442C90DE14EBA982CA621D1692C0` |
| Post-rejection snapshot | `A8F60D60AD37BB838792B6A726748EA7FD16C903331615E0A4216E8BB0B3084B` |

The remaining additive evidence is a successful second Paint against completed
first-patch structure with a preexisting shell. Repeat only case B using the
same DLL and one uninterrupted tracked layer, as clarified in the
[operator procedure](../probe/README.md). Save/reload continuation remains a
separate investigation, not a capability established by this two-patch probe.
See [PROJECT.md](PROJECT.md) for story and gate state.

### Completed-shell retest

The owner reported no observed aberrations and supplied another run from source
`4c49a0c666d1ef39a4ce24cfb129fbec42ee25e6`. Reports again match the target
hash/MVID and Unity version. `20260912T030339.0524890-paint.json` starts empty
layer 2 at radius 9,700; `032721.1733071-snapshot.json` records its completed
first patch and one shell. In `20260912T032723.6295965-paint.json`, the successful
second addition changes 6 nodes / 6 frames / 1 shell to 11 / 12 / 1.

All six original nodes retain their completed 180 SP total, and all six original
frames retain 800 SP. The shell retains its boundary, associations, and 14,480 CP.
The in-process identity/preservation comparison reports no failures. Independent
raw-record comparison confirms retained properties and invested construction;
canonical mapping and topology match the second delta, including endpoint reuse.
Maximum normalized direction discrepancy remains below 3.575 × 10⁻⁸.
Later snapshot/refusal records show both manually filled pentagons and no further
probe additions. Original evidence was read only and copied under ignored artifacts.

| Input | SHA-256 |
| --- | --- |
| First-patch completed snapshot | `0FFE0B3D17ACE2E9A70C669E7E136F8F55E9D31915AB572BC2194DA2AFCF6F70` |
| Second Paint with existing shell | `DE5DC053140785E09A4C815C5146991B5862129D236F3E43ABE144C7B833FB13` |
| End-of-probe refusal | `9535D17454EFF00AA81F6088AE9D5946B0F19B5592F33C8FF7E2746E7F86C921` |

Together with the earlier partial-construction, prerequisite-refusal, and native
rejection observations, this covers the two-patch additive experiment. Native
creation is not atomic: stop on a failed addition, retain evidence, and do not
blindly retry, roll back, or repair. Whether the surviving graph establishes a
safe next step is the subsequent continuation investigation. No spontaneous
failure of a valid delta, save/reload resumption, or complete traversal is claimed.

## SB-F3.2 — Continuation probe preparation

### Native reconstruction inputs

Targeted reinspection of the same target assembly confirms:

- `DysonNode.Export/Import` writes and reads each position component as a binary
  single, without normalization during ordinary save import. Layer radius is
  likewise a single in `DysonSphereLayer.Export/Import`.
- `DysonFrame.Export/Import` retains endpoint IDs and Euler mode; import resolves
  fresh node references and reconstructs adjacency. Construction fields and
  native shells remain owned by the native save.
- `DysonSphereLayer.Import` creates new node/frame objects. `DysonSphere.RemoveLayer`
  clears the layer slot and frees its contents; `AddLayer`/`AddLayerOnId` creates
  a new object and can occupy a previously used ID.
- `UIDysonEditor._OnClose` clears selection; `_OnFree/_OnInit` releases/recreates
  it. `DESelection.SetViewStar` clears selection and resolves the viewed sphere.
  A cached object reference or numeric layer ID cannot identify a continued design.

These are source findings, not new live transition observations. They support
trying geometry/topology reconstruction on the selected layer before adding
stored mod state. Reproduce with the SB-F1.1 decompiler command and these members;
the retained output remains ignored and no native implementation is copied.

### Matching experiment and numeric boundary

[GraphRecognition.Match](../probe/GraphRecognition.cs) reads a fresh snapshot on
each action. It maps positions to unique canonical nodes, verifies the exact
unordered frame set and non-Euler mode, and recognizes only a complete union of
the first `k` deltas. Existing shell boundaries must match reference faces whose
frames are present. Construction progress, colors, record IDs, and pool order
do not determine `k`. There is no layer registry, ownership marker, custom save
data, repair, or replacement import. Runtime object references are retained only
inside each action's preservation snapshots.

Bitwise coordinate comparison was tried first. Re-reading the owner's successful
radius-9,700 snapshot under .NET 10 reproduces canonical node 8's Y/Z one float
step away from the game result: Y `1336.9548` versus `1336.955`, Z `-3743.6982`
versus `-3743.6985` (short round-trip decimal forms). All graph data remains the
same. The exact source of the runtime arithmetic/parsing difference is not
established; bit equality across these runtimes is therefore not a sound check.

Recognition uses Euclidean position distance at most `radius × γ10`, where
`u = 2^-24` and `γ10 = 10u / (1 - 10u) ≈ 5.960468 × 10^-7`. This is a conservative
float-arithmetic allowance for comparing two normalized/scaled evaluations of
the same encoded direction: three-term squared magnitude has relative error
bounded by `γ3`; square root, component division, and radius multiplication give
approximately `4.5u` per evaluation, covered jointly by `γ10`. This bound assumes
normal finite coordinates at game radii; it is not a measured game placement
limit. At radius 9,700 it allows about 0.00579 position units. It is far below the
reference's shortest normalized node separation (about 0.324); every match must
still be unique. Placement still uses the unchanged normalized reference and
never moves an existing node. Topology remains exact. SB-F2.1's separate
source-blueprint comparison bound is unchanged.

This identifies graph content, not historical ownership. An identical prefix
recreated by another route is indistinguishable; an edit leaving an exact earlier
prefix is likewise indistinguishable from that earlier state. An unmatched
partial delta refuses rather than being completed as a repair. The proposed
policy and its acceptance boundary are recorded in SB-D007 in
[PROJECT.md](PROJECT.md#sb-d007--probe-continuation-from-the-native-graph).

### Local checks and live handoff

The probe embeds all twelve already-derived deltas and their 32 face boundaries
because SB-F3.2 requires observing reload at completion. It still exposes only
one patch per Paint click and creates no shells. Reports add before/after
recognition, the recovered mapping, and a per-plugin-session identifier; no
progress is read from earlier report files. Graph states serialize as Mismatch
`0`, Empty `1`, Prefix `2`, and Complete `3`. Human transition notes remain
necessary to distinguish editor closure, selection switching, and save loading.

The existing local check executable now verifies all prefixes after serialization
with permuted/recycled IDs and changed star/layer IDs. It accepts reference
shells and float rounding, and rejects missing nodes/frames, displaced coordinates,
extra/duplicate edges, duplicate canonical positions, Euler frames, a partial next
patch, unrelated geometry, wrong radius, invalid radii, and invalid shell cycles.
Report serialization covers recognition and session identity. No assertion
depends on UI/error-message wording, and no game allocation or save API is called.

Both actual second-Paint snapshots from SB-F3.1 (radii 9,700 and 36,000) also
recognize as prefix 2, with mappings matching the creation evidence. The initial
historical-record reader failed because the old reports lack newly added fields;
the check now reads their unchanged Snapshot payload rather than treating them
as current reports. There is no production evidence migration path.

Reproduce compilation, embedded-plan/JSON checks, and recognition checks with
`scripts/Build-Probe.ps1` as above. To additionally check retained owner inputs:

```powershell
dotnet artifacts/probe/json-checks/bin/ProbeJsonChecks.dll $completedShellPaint $radius36000Paint
```

Compilation and local checks pass against the identified real reference
assemblies. The checks execute under .NET 10, not the game runtime. The
[continuation procedure](../probe/README.md#run-the-continuation-story-sb-f32)
covers intermediate and completed transitions, restart, edits, unrelated geometry,
and layer recreation. Its expected results are not observations. Native save/load
fidelity and live recognition remain unverified until that owner run; state and
the next gate are tracked only in PROJECT.md. Full traversal records from this
run may inform SB-F3.3, without claiming its shell and envelope checks in advance.

### Owner continuation run

The 2026-09-12 run identifies source
`9dd00e2909cc7e07f6db329cf3a4c6e694f5075b`, the unchanged target hash/MVID,
and Unity `2022.3.62f3c1`. All reports use plugin session
`f4dbe0732cb4402bbca2665a7924c4d9`, star 60, and unlocked latitude 90. The owner
reported no aberrations, clarified that star switching involved returning without
painting in the other star, and confirmed that quit/reload meant returning to
the main menu. It occurred after the manual edits. No full application restart
is inferred from these files. Originals were read only and copied under ignored
artifacts for independent comparison.

| Observation | Identifying reports and result |
| --- | --- |
| Initial recognition and editor return | `040527.7778530` and `040558.0307442` snapshots recognize the previous two-patch graph on layer 2, radius 9,700, with two shells. `040559.5220092-paint` adds patch 3: 16 nodes / 19 frames. Editor closure is owner-reported; the JSON records the returned target. |
| Layer switch | `040614.2050568-paint` starts layer 1 at radius 36,000. Returning to layer 2, `040617.2117729-snapshot` retains prefix 3; `040618.0103250-paint` advances it to prefix 4. |
| Intermediate reload | `040650.0058467-snapshot` returns to prefix 2 at tick 26,263,210, below the prior tick 26,264,335. `040651.8036476-paint` correctly adds patch 3. Positions, frame endpoints, and shell boundaries match the pre-save baseline. |
| Completed graph and return | `040709.2035603-paint` reaches 60 / 90 with the original two shells. The subsequent snapshots and Paint refusals retain completion. Star switch-and-return is owner-reported; every recorded Paint target remains star 60. |
| Completed reloads | `040843.5223495-snapshot` and, after the edits, `041103.2537543-snapshot` retain the same completed graph and shells while ticks rewind to saved state. Their following Paint actions refuse unchanged. These are reload observations within one plugin session. |
| Manual edits | On layer 1, `040934.0441909-paint` refuses the edited 6-node / 5-frame graph; `041005.7821733-paint` refuses 5 / 4 after node removal. Before/after records show no additions or lost construction from either refusal. |
| Recreated layer / reused ID | Layer 1 holds prefix 3 at radius 36,000 in `041118.7863371-snapshot`, then is empty at radius 21,300 in `041133.9286832-snapshot`. A native unrelated node is refused in `041149.3225812-paint`. After its removal, `041159.1733045-paint` starts patch 1 at 6 / 6 rather than carrying over an old index. |

Timestamps above abbreviate filenames beginning `20260912T` and ending `.json`.
Independent checks compared all successful additions with the derived canonical
node/edge unions and all recorded recognized snapshots with their claimed prefix.
The full traversal preserves the original 330 node SP, 1,600 frame SP, and both
shells; shell CP increases naturally between actions. No in-process preservation
failure is reported. Raw-record checks also retain positions, properties,
connectivity, invested construction, and shell associations within each action.
Object identities across reload are not claimed preserved: reconstruction works
with the new native objects.

| Critical input | SHA-256 |
| --- | --- |
| Intermediate reload snapshot | `69E63264E5CA1B7780F54CD0223AEFC516598178F1923E9D9B41A8E73906B52A` |
| First Paint after reload | `F96F9AFC09BB3083AF89EA1766E7413443DDA5AB2BF222C23B1FE307A6A7F3B7` |
| Complete graph Paint | `BFFE988FB5B0B82194C411B316B237104AA405F8BA54D9182F21EB4CEC6DEF50` |
| Node-edit refusal | `127D35DC0A520085E8F8FD524B09CD861CE0A2A49B1FAE3691906165C9BE9F60` |
| Completed refusal after later reload | `52CC71BC5F046E38F4FDC92A7C933F27DA020CD00FC5DB5808412B7642AC0813` |
| Recreated-layer first Paint | `44C861E994971AC5803A24DFE95D2EFE8060363A2F6650E5D5E308F68F44089B` |

Native reconstruction is sufficient for the observed transitions without stored
mod state. Full process restart/removal, placement in another star, and the
remaining envelope/shell cases were not established by this run. The owner's
subsequent acceptance of unverified cases is recorded as SB-D008 in PROJECT.md;
it does not convert them into observations.

## SB-F3.3 — Full workflow evidence and accepted limits

The continuation run supplies the full-graph observations without another probe
implementation. Every successful action matches its intended delta; the final
`20260912T040709.2035603-paint.json` has 60 unique nodes, 90 unique non-Euler
frames, and exactly the canonical connectivity. This includes ring closures,
the equator crossing, and the final cap. All recognized snapshots' normalized
positions differ from the derived reference by at most `6.106743 × 10^-8`
(rounded upward), below SB-F2.1's `2.384185934 × 10^-7` comparison bound.
Maximum relative radius error is `7.008132 × 10^-8` (rounded upward).

The owner advanced patches 4–12 from `040704.1161904` through `040709.2035603`,
about 5.09 seconds. The original 330 node SP and 1,600 frame SP remain invested;
new framework construction was not a prerequisite for further clicks. The two
existing pentagon shells retain their native records and associations throughout
addition. Only pentagon shells are present in this run: hexagon designation and
later addition beside a hexagon remain unobserved.

The existing native Unity panel resolves the selected layer at click time. The
owner reported no observed control or visual problems. No-selection reports at
`040839.1635382-snapshot` and `040839.8135562-paint` have no selected target and no
placement; completed-graph Paint attempts leave 60 / 90 unchanged. Editor return,
selection switching, graph mismatches, and reload are covered by SB-F3.2. This
supports the control location's feasibility, not final UI styling or arbitrary
mod compatibility.

| Scope | Direct observation | Remaining validation boundary |
| --- | --- | --- |
| Star 60, radius 9,700 | Full framework, continued construction, pentagon shells; reported native minimum 9,700 | Endpoint control manipulation was not independently captured; hexagon filling untested |
| Star 60, radii 21,300 and 36,000 | Starts/partial graphs, selection/identity/refusal cases | No complete sphere or both-face shell checks at these radii |
| Native maximum / other systems | Reports identify star 60's maximum as 74,000; owner switched to another star and back | No C60 placement at 74,000 or in another star/giant |
| Research | All successful live reports have unlocked latitude 90; source and derivation require rounded value at least 68 | Below-threshold refusal and named research levels not observed |
| Lifetime / selection | Menu reloads, no selection, layer switching/recreation, edited graphs | Full process restart/removal and multiple selected layers not exercised |

The scale-invariant native-predicate analysis in SB-F2.3 supports the candidate
envelope; these limited observations do not prove universal live coverage. Under
the owner's SB-D008 decision, the unverified cases are accepted for the MVP
contract without another live run. Their expected behavior remains required and
their evidential limits remain explicit. The gate disposition is in PROJECT.md.
