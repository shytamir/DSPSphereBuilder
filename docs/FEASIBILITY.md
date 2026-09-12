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
