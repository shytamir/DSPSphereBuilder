# Feasibility evidence

Current execution state, decisions, and gate results are in [PROJECT.md](PROJECT.md).
This record describes observed inputs and findings. References to native members
are inspection coordinates, not a copied implementation.

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
