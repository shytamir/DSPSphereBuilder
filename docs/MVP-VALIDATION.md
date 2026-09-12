# MVP implementation evidence

[PROJECT.md](PROJECT.md) owns execution state, gate outcomes, and acceptance.
This record distinguishes metadata inspection, offline checks, compilation,
package inspection, and owner-operated runtime observations. Sections describe
the evidence available at their recorded stage; reproduction commands remain
available, while superseded handoffs are historical.

## SB-I1.1 — Mapped references and delivery inputs

Inspected 2026-09-12. The local target had SHA-256
`AE0BA95F75BD879A62AA4CE253B2AB78EAA4FB3C7C595F5E1FEE75EBE0E0EF85`,
MVID `ece4a40e-5e73-43f4-a9f8-4e74970b5942`, and 7,830,016 bytes. The full displayed
game-build suffix was not established by that inspection. No native code was executed.

The [reference map](../references/Map.json) records the inspected BepInEx 5.4.17.0
and UnityEngine.CoreModule 0.0.0.0 identities, seven initial types, required member
signatures, inheritance, and native metadata tokens. Mono.Cecil reads these
without loading game types. The initial entry-point shims compiled successfully,
and every declared public/protected member matches the local metadata. Assembly
names/versions/public-key identity match the runtime destinations; the production
DLL was to reference those identities, not a separately named shim library.

Reproduction from the repository root (substitute actual local input paths):

```powershell
dotnet build references/BepInEx/BepInEx.csproj -c Release -o artifacts/reference-shims
./scripts/Test-ReferenceMap.ps1 -DspManagedPath $managedPath -BepInExCorePath $loaderPath
```

The verified loader attribute constructor takes three strings and parses its
version with `System.Version`. `MAJOR.MINOR.BuildNumber` therefore fits the
existing positive Int32 build-number contract. SB-D015 fixes the assembly and
diagnostic mappings without imposing the CLR's 16-bit assembly-component limit
on the sequential package build number.

Primary distribution inputs checked on the same date:

- The [DSP loader package](https://thunderstore.io/c/dyson-sphere-program/p/xiaoye97/BepInEx/)
  identifies BepInEx 5.4.17; dependency string `xiaoye97-BepInEx-5.4.17` matches the
  local loader baseline. This package identity differs from our plugin GUID.
- [Package requirements](https://wiki.thunderstore.io/mods/creating-a-package)
  specify root `manifest.json`, UTF-8 README, and a 256×256 PNG named `icon.png`.
  The supplied icon decodes as PNG at exactly 256×256, SHA-256
  `7B76A93024A43690BA64068D6E0F98BD1F0B0DC8311A4F32672B1F551A0B02D4`.
- [BepInEx package routing](https://wiki.thunderstore.io/mods/packaging-your-mods)
  supports `BepInEx/plugins`; use `BepInEx/plugins/DSPSphereBuilder/` for the payload.
  This also gives manual installation an explicit destination.
- Retain original project LICENSE, upstream `60.txt` and its unmodified GPL-3.0
  LICENSE, and source/revision credit for generated geometry. The
  [fixture provenance](../research/cosmin1490/README.md) remains authoritative for
  those inputs; no generator implementation or game binaries are incorporated.

These establish build/distribution inputs, not runtime compatibility or a
Thunderstore submission. The initial declaration check is not a blanket check
of unreferenced game types. Every later added surface extends the same map.

## SB-I1.2 — Production compilation and identity

The initial minimal production plugin used `dsp.spherebuilder` and the declared identity
mapping. Local compilation in both reference modes passed with no warnings or
errors using SDK 10.0.302. Metadata inspection found identical 19 emitted
assembly/member references in the shim and native builds. The checker reads PE
metadata, never instantiates the plugin or a game type.

Build 70,000 produced plugin/package version `0.1.70000`, assembly/file version
`0.1.0.0`, and informational label `0.1.70000.f85d4a5` from the local worktree based
on `f85d4a5`. This is local working-tree evidence, not a clean source-commit build.
The build record was extended to report dirty state. Retry attempts 1 and 2 kept
the same version, build 70,001 advanced it, and the unchanged mock ZIP validator
passed using the shared VERSION translation.

The build fails on missing required paths, a changed local target hash, compile
errors, mismatched plugin metadata, or a changed declared native reference map.
The source project marked references non-copying; explicit production-DLL
selection was subsequently implemented in SB-I4.

The workflow at that stage installed the exact pinned SDK through the official
[setup-dotnet action](https://github.com/actions/setup-dotnet), compiled production
source, and checked DLL identity before uploading its then-mock artifact.
[Hosted run 34676317466](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34676317466)
passed for `fc42073f054d90b5f9cde168b341855932c40a4e`: production version `0.1.20`,
label `0.1.20.fc42073`, 19 inspected emitted references, and successful mock ZIP
validation/upload. A local negative metadata check rejected a different expected
package version. Compilation, metadata inspection, and mock packaging are the
only claims here; no plugin/game runtime was invoked.

## SB-I2.1 — Compiled production plan

The production DLL holds the fixed normalized directions, twelve deltas, and 32
face boundaries generated from the pinned fixture. It contains no generator or
blueprint parser. Coordinates use independent scalar calculations in managed
code, then the native adapter can supply those positions to the game.

The managed check reads the actual compiled DLL's plan and exports it to
`artifacts/compiled-plan.json`. `scripts/check_plan.py` independently derives the
reference again and checks every new-node/edge set and face, plus positions at
five scales (100, 9,700, 36,000, 100,000, 1,000,000). These are arithmetic scales,
not new claims of supported game radii. Results: all twelve deltas and faces
match, final counts 60/90, maximum normalized-direction error `6.72304398e-8`,
maximum relative radius error `7.07654325e-8`, both below SB-MVP-07's bounds.

Both production reference modes compile without warnings/errors and have the
same 29 emitted references. No native API was added; the reference map is
unchanged. This managed test executes only this project's pure plan/math code,
not the plugin entry point or any native game/Unity method. CI was extended to run
the same compiled-plan comparison. Its first hosted run stopped before the checks because
actions/python-versions does not publish Windows 3.12.14. The
[official version manifest](https://github.com/actions/python-versions/blob/main/versions-manifest.json)
listed 3.12.10 as the latest available 3.12 Windows x64 binary at that inspection;
CI was changed to pin it. No geometry change or local Python replacement was needed.
[The corrected run](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34676813261)
passed for `bd61e9087147c05dc696209b590528cb8dfe648a`, including the compiled-plan
comparison and existing mock delivery.

## SB-I2.2 — Native graph recognition

The read adapter captures native node/frame/shell content and identities; the
matcher operates only on managed records. The per-commit map now includes the
target's four graph types plus Vector3 and Color32 fields. All three shim
assemblies match their local native signatures; real and shim production builds
pass with identical 122 emitted references and no warnings/errors.

Compiled-code checks cover empty plus every complete prefix, reversed pool order,
reused/nonsequential IDs, fresh layer identity, missing nodes/frames, duplicate
IDs/positions/edges, Euler mode, dangling endpoints, extra records, a partial next
delta, unrelated content, wrong radius, and invalid numeric radii. Half-tolerance
and one-float-step perturbations match; twice-tolerance displacement refuses.
All 32 reference shells match, including both face types; repeated boundary nodes,
duplicate shell faces, and missing shell frames refuse. Changed SP/CP and colors
do not change recognition. An exact earlier prefix is classified from its current
content; no historical registry is introduced.

The existing geometry comparison remains within the recorded bounds. The tests
run this project's compiled matcher/math on captured-data fixtures; the native
reader is verified by mapped metadata and real-reference compilation, not executed
in a game session. SP/CP/identity capture was retained for SB-I3.1
preservation comparison; there is no report-export or custom-save subsystem.

## SB-I3.1 — Additive operation and failure boundary

The production operation resolves its target through a fresh callback, checks
native-context availability and rounded latitude before capture, recognizes a
complete prefix, and checks every proposed position before allocation. The mapped
native adapter supplies prototype 0 nodes, then non-Euler prototype 0 frames.
`DESelection.singleSelectedLayer`, native running/history getters, and the exact
creation signatures were inspected against the target. Native Mathf.RoundToInt
supplies the unlock comparison; no research-level name is guessed.

The before/after comparison preserves existing identities, exact old positions,
properties, SP/CP, earlier links, and shell associations while allowing construction
increases and new node adjacency. A zero frame result, invalid node ID, exception,
or detected preservation/result discrepancy stops further Paint attempts for the
session. Error reporting does not recapture/retry after a failed native call.

Both builds pass with no warnings/errors, identical 158 emitted references, and
the updated three-assembly native map. Offline tests use prepared before/after
records and a call recorder, not a simulated native allocator. They verify all
twelve call deltas/endpoints, both shell classes with increasing construction,
no-write selection/unlock/edit/completion outcomes, current-target changes,
one-error session stop on zero/exception, no reset through another target or
missing/menu context, rejection of a partial result in a fresh session, and
continuation from a fully finished delta after a result-read failure. Separate
negative outcomes detect lost SP/CP, replaced/moved old nodes, removed shells,
and missing result frames. Production UI invocation and native behavior were
deferred to the integration and human stories; these checks were not live observations.

## SB-I3.2 — Current-target editor control

The initial panel sat at the top center of the native control panel, 380 by 122
UI units, with one Paint button and a short status. It reuses a native editor font.
Inspection of UIDysonEditor's mouse-over calculation confirms that active entries
in `guiRects` exclude native brush input; the panel registers there and also uses
UIBlockZone. Button callbacks invoke one synchronous action and resolve selection
afresh. There is no probe control, exporter, queue, or automatic advance.

Source review traced attachment, editor close/reopen, replacement, destruction,
and failed setup: one owned panel/listener is created per attachment, hidden while
closed, removed before replacement, and unregistered on destruction. A session
error survives target/menu changes; integration exceptions stop repeated setup.
Only presentation caches a layer reference to refresh promptly. Progress otherwise
refreshes at most four times per second; writes never use that cached reference.

Managed checks verify every prefix's current target/progress, no writes during
feedback, menu transitions, readiness/completion, and feedback for every disabled
outcome without assertions on exact wording or Unity widget internals. The action
checks continue to verify click-sized deltas and target changes. Both compilation
modes pass without warnings/errors and emit the same 210 references. Five shim
assemblies match native signatures, now including type abstract/sealed and method
virtual-slot shape. The mapped UI assemblies and inherited declarations were
inspected before use; BepInEx's base plugin is abstract.

That stage provided offline lifecycle review and compilation, not a Unity runtime
test. Visibility, overlap, readable feedback, event delivery, click-through,
reopening and production preservation were deferred to SB-I5.1. The hosted runs for
[SB-I2.2](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34677110100) and
[SB-I3.1](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34677535631)
also passed their affected offline checks.

## SB-I4.1 — Executable package and validator

Local package 0.1.28 was built from the working tree based on `389668a`; this is a
rehearsal identity, not a clean CI revision. Both reference modes compile without
warnings/errors, with identical 210 emitted references and a matching five-assembly
native map. The builder always compiles first and explicitly selects the production
DLL; no stale/missing payload or compiler failure falls back to a mock package.

That initial ZIP contained 47 files: required root metadata, one production DLL in the
verified BepInEx path, and source/compile inputs with pinned geometry, derivation,
credit and applicable licenses. The supplied icon is unchanged, with the hash and
256×256 PNG dimensions recorded above. Validation checks metadata identity, numeric
version, diagnostic revision, payload hash, UTF-8, real image decoding, retained
input hashes, and the file inventory. Twelve malformed variants were rejected:
missing DLL, wrong manifest/DLL versions, altered GUID, shim as payload, extra dependency/probe,
nested ZIP, wrapper folder, missing reference source, wrong dependency, and invalid
image. This tests package contracts, not README wording.

The root/package/build documentation now describes actual installation and use.
The intermediate workflow still wrapped its executable ZIP with build information;
SB-I4.2 subsequently replaced that transport and verified the real download.
No game installation, execution, release or submission was performed in SB-I4.1.

Package-source follow-up: inspection found that the retained patch derivation also
imports `verify_geometry.py`. That file is now included (48 package files). The
source directory extracted from the revised local ZIP successfully regenerated
the production plan with Python, without reaching outside its packaged inputs.

## SB-I4.2 — Hosted transport checks

[Run 34678551277](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34678551277)
passed compilation, package validation, all production logic/geometry checks and
all twelve malformed-package cases. The step then failed because an intentionally
rejected DLL left native exit code 1 in PowerShell. The negative-check script now
returns success explicitly only after every rejection has passed. The local
Actions-style exit-code check confirms zero; unexpected acceptance still throws.
Hosted upload and independent download inspection were still pending for the next candidate.

Run 34678732321 (build 30, `2138818`) passed all hosted checks and direct upload.
The independent API download matched CI's package SHA-256 and had the required
root and single DLL, with no wrapper/nested archive. Its DLL's 210 emitted
references matched the same-revision local real-reference compilation. Inspection
then found CI's Windows checkout converted the upstream LICENSE from LF to CRLF.
The text was identical after newline normalization, but retained bytes differed.
`.gitattributes` was changed to pin license/text inputs to LF. The next download
then had to pass the unchanged byte check before IG4 could close.

The inspected SB-I4.2 download was
[run 34678932740](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34678932740),
build 31 / attempt 1, source `a71fd79244054b1fe0695cff2d07631eb5ae803c` (clean CI
checkout). The [direct package artifact](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34678932740/artifacts/10293890904)
is `DSPSphereBuilder-0.1.31.zip`, 95,390 bytes. The pinned upload action uses its
verified `archive: false` input; build information is a separate artifact.

- Package SHA-256: `05362278070F49000290269F8CD288297343F205BF95D22C12F3B6EB49864A39`.
- DLL SHA-256: `EBDBCDB96ADAAE7A64CFB043F8375426309F27E7392EDE648B93843B35A2D662`.
- Plugin/package version: `0.1.31`; diagnostic identity: `0.1.31.a71fd79`;
  GUID: `dsp.spherebuilder`; assembly version: `0.1.0.0`.

The independent REST download's exact bytes match CI's hash and artifact digest.
The unchanged validator passes all 48 entries, root layout, dependency metadata,
UTF-8, image decoding, retained license/reference/icon bytes, and DLL metadata.
No wrapper folder, second ZIP, probe DLL, shim DLL, or native dependency exists.
The packaged plan source matches the compiled/check-tested plan after text newline
normalization. CI passed all managed logic, independent geometry, and twelve
negative package checks. Downloaded DLL metadata matches the same production
source compiled locally against the actual target: 210 emitted references in both
modes, five checked shim assemblies, no warnings/errors. Local handoff-only files
were uncommitted during that compile and were not production inputs; CI itself
was clean. The raw download/build record are retained under ignored
`artifacts/hosted/34678932740/`. No runtime or installation was performed.

## SB-I5.1 — Prepared owner capture and handoff

[OWNER-SESSION.md](OWNER-SESSION.md) retains the integrated procedure prepared for
verified build 0.1.31.a71fd79. The agent has not installed or run that plugin.
The procedure requested two native single-layer exports (prefix 3 with a designated
pentagon, then prefix 12), a useful panel screenshot, and the normal BepInEx log.
The route uses one menu reload and a second-layer manual edit, without new-system,
endpoint, restart/removal, hexagon, construction-wait, or forced-failure cases.

Offline inspection followed `UIDELayerPanel.OnCopyClick1`, the selected-layer
blueprint generation path, and the target's layer/node/frame/shell blueprint
writers. This is a clipboard export, not layer import. CheckLayerNodeCount only
compares record counts and does not require completed construction. The decoder
already covers container 0, layer 1, node 5, frame 1 and shell 2; the pinned source
contains the target's node/frame/shell versions. The writer's pool/recycle and
optional layer-color tails were reconciled with the reader.

The small capture comparison reuses that decoder and the independent derivation.
Five offline cases pass: valid prefix-3/final pair, missing frame, wrong radius,
lost manual shell, and changed old position inside the geometry tolerance. These
are captured-data checks, not native export execution. The tool compares graph,
numeric bounds, retained numeric IDs/positions/endpoints/prototypes and shell
boundary. It does not authenticate the custom blueprint checksum, recover memory
identities, or infer invested SP/CP that the blueprint omits. In-action production
checks and retained feasibility evidence remain the support for those properties;
owner observations/logs and captures were subsequently reviewed together. See SB-D017.

## SB-I5.1 — Owner evidence review

The owner reported **"MVP is owner accepted"** on 2026-09-12 and supplied the two
exports, log and screenshot. Originals were read from the established evidence
folder and copied unchanged into ignored `artifacts/owner-session/0.1.31/`.
These hashes identify the reviewed files; raw captures are not repository source.

| File | Bytes | SHA-256 |
| --- | ---: | --- |
| prefix-3.txt | 1,546 | `133C1455340B33BAA66522816ADFCDCD55869DAB18618A1F20294036169E7F50` |
| complete-12.txt | 3,150 | `F14C4257B13C073A0F7A3DB6AD0DA5B10BA419A12CE0E6DACADE0CA3C2601B77` |
| LogOutput.log | 2,966 | `6B805B9BE6BEBE0C8A72C8B6EBF6F8B370BB6A422DAE88F2DCD577D69D5797AC` |
| 20260912134823_1.jpg | 1,691,655 | `23B51231BC0E452F20417B55C3639B5B038DF47C8E73CA9EDA6BEBE9236E38DE` |

The log identifies production **0.1.31.a71fd79**, the expected target MVID,
BepInEx 5.4.17.0, and no loaded feasibility probe. The target DLL hash still matches
the pinned baseline. All twelve logged additions on star 60, layer 1, radius
36,000 match the derived cumulative counts, from 6/6 to 60/90. A later entry starts
layer 3 at radius 26,200 with 6/6. The supplied log has no Error/Fatal entry; that
fact alone is not the runtime conclusion.

The exports are **whole-sphere type 4**, not the requested single-layer type 1.
The checker initially refused this format. Before extending the offline reader,
the target's enum, DysonBlueprintData.Export and DysonOrbitBlueprintData.Export
were inspected: container 0, twenty swarm orbit records, color and render fields,
layer orbits, then the ID-indexed layer array. Orbit record version is 0. The
existing layer reader was reused without changing its record rules. Explicit
`--layer 1` selection and complete payload consumption locate the intended layer;
its encoded radius also agrees with the log. Nothing was imported or run in game.
The native export headers report `0.10.34.28529`; no separate displayed version
capture is claimed.

The manually designated shell is a **reference hexagon**, not a pentagon. The
checker also initially refused its procedure-specific pentagon expectation. Both
face classes are already required by the product specification, so the capture
comparison now accepts one validated reference face and reports its boundary
size. This changes evidence tooling only; no runtime behavior or design scope was
relaxed. Seven focused capture checks and ten existing geometry/decoder checks
pass, including whole-sphere selection, absent/unspecified layers, truncation,
trailing data, and retained hexagon coverage.

Reproduction from the repository root:

```powershell
python -B scripts/check_owner_capture.py --before artifacts/owner-session/0.1.31/prefix-3.txt --after artifacts/owner-session/0.1.31/complete-12.txt --radius 36000 --layer 1
```

| Capture | Nodes / frames / shells | Maximum direction error | Maximum relative radius error |
| --- | --- | --- | --- |
| Prefix 3 | 16 / 19 / 1 | `3.753450739e-8` | `3.743727878e-8` |
| Complete 12 | 60 / 90 / 1 | `4.307964797e-8` | `4.186824723e-8` |

Both graphs match the fixed plan within SB-MVP-07 bounds. All earlier numeric node
IDs, exact captured positions, prototypes, frame IDs/endpoints and non-Euler modes
are retained. Shell 1 keeps the same six-node boundary through patches 4–12. The
other exported layer (2, radius 9,700) keeps the same decoded 6/6/0 graph; its raw
serialized bytes differ, so byte-for-byte preservation of its bookkeeping is not
claimed. The full comparison JSON is retained beside the copied evidence.

The screenshot shows the top-center control with `Layer 1: 3/12 patches planned`,
native counts 16 nodes / 19 frames / one shell, radius 36,000, and zero constructed
structure/cell points. The button and status are readable. The panel overlaps the
native center `Painting mode` caption; the left/right controls and bottom toolbar
are visible. This is concrete input to the owner's current-UI disposition, not a
request to redesign the control.

**Limits:** No itemized route report accompanies the owner's overall acceptance.
The log does not mark menu reload, no-selection/disabled feedback, manual-edit
refusal, or a completed no-op; the still image cannot prove click-through behavior.
These are not newly instrumented production observations. Native inspection,
managed behavior checks, prior feasibility observations, and the owner's explicit
acceptance remain their evidence/disposition. The exports omit invested SP/CP and
object identity; this screenshot has no invested construction to preserve. The
new hexagon observation covers designation/boundary retention beside later
addition, not completed hexagon CP delivery or the radius endpoints. See SB-D018.

## SB-I5.2 — Owner UI workshop

After reviewing the current screenshot, the owner selected: **"Place it near the
bottom, left of the native bottom bar controls."** The owner confirmed **"The
wording is fine"**, and requested a sleeker design because the enclosing box is
too large and bulky. These are the finite refinement inputs recorded in SB-D019;
there is no no-change disposition. The capture's top-center overlap and unused
panel area provide the visual basis. No new action, setting, preview, or product
behavior was requested.

## SB-I5.3 — Compact bottom-left control

The refinement keeps the existing editor integration, Paint handler, feedback
wording, target resolution and session-stop behavior. The title and button share
one row, with feedback underneath. The ordinary one-line panel is 280 by 64 UI
units, down from 380 by 122; button/status fonts remain 14/13, while the title
changes from 16 to 14. Longer feedback uses Unity Text's preferred height at the
fixed content width so the smaller box can expand vertically for wrapped text.

Placement follows the native toolbar's bottom-left corner with a 12-unit gap,
aligning the panel's bottom edge. Read-only inspection of the target's
`UIDEControlPanel` and `UIDEToolbox` established the `toolbox.selfRect` reference
and the native bar's changing width. The panel stays under `controlPanel`, so
closing the toolbox for no selection does not hide Sphere Builder's explanation.
World corners are converted into that parent's local coordinates; no screen
resolution or native toolbar width is hard-coded. Unity documents corner 0 as
bottom-left in [GetWorldCorners](https://docs.unity3d.com/ScriptReference/RectTransform.GetWorldCorners.html).

The same change maps `UIDEToolbox`, `UIDEControlPanel.toolbox`, `selfRect`,
`RectTransform.GetWorldCorners`, `Transform.InverseTransformPoint`,
`Transform.localPosition` and `Text.preferredHeight` to the local native metadata
in `references/Map.json`, including declaration shape. Both compile modes pass
with no warnings/errors; all five shim assemblies match the recorded target and
all 217 emitted plugin references agree between native and shim builds. The
managed logic checks still pass for the full plan, recognition, shell boundaries,
preservation, current-target binding, prerequisites, completion and bounded failure.

Source review confirms the same single click listener, native input-block zone,
editor GUI-rectangle registration and detach cleanup. Compilation and this
review do not establish the new layout's live readability or input behavior.
The affected owner recheck uses one patch and the control's displayed states,
without repeating the full progression, reload/shell route or W1–W6.

### Hosted refinement candidate

[Run 34693238801](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34693238801),
build 36 / attempt 1, passed compilation, package/geometry/logic checks, all twelve
malformed-package cases and direct upload. Source is
`97b7f863c45f4fe7d6b799e32ec447b7a98edc76`; CI reports a clean checkout.
The independently downloaded [DSPSphereBuilder-0.1.36.zip](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34693238801/artifacts/10297338736)
is 96,095 bytes and matches both CI's package hash and the hosted artifact digest.

- Package SHA-256: `1810D1535D49AAE5A425A583ABB899EB730FA9198A27BA81279354287105B6E4`.
- DLL SHA-256: `5F9DF4FCF0C2751C65ADEB730CF93E5906E53BC2F05C0F9B11D74644273344B8`.
- Plugin/package version: `0.1.36`; diagnostic identity: `0.1.36.97b7f86`;
  GUID: `dsp.spherebuilder`; assembly version: `0.1.0.0`.

The unchanged package validator passes the actual download's 48 entries, direct
root, one production DLL, source/licenses, metadata and supplied 256x256 icon.
There is no nested ZIP or bundled native/shim/probe DLL. The downloaded plugin's
217 emitted references match the local real-reference build at the same revision
and version, compiled from a clean working tree. Raw download and CI build record
are retained under ignored `artifacts/hosted/34693238801/`.

Compared with runtime-tested 0.1.31, production changes are confined to the layout
in `Plugin.cs` and its mapped reference declarations. The geometry, recognition,
mutation, preservation and feedback wording are unchanged. Offline capture tools
also changed during SB-I5.1's evidence review; they are not the runtime plugin.
[The short owner procedure](OWNER-SESSION.md#ui-refinement-recheck--sb-i53) targeted
the new placement/readability/input before the confirmation recorded below.

### Owner refinement evidence

On 2026-09-12 the owner confirmed the refinement worked as requested and asked
only for **Paint Next Patch** capitalization, explicitly requiring no new live
recheck for that correction. The control's dimensions, action and feedback remain
the same. SB-D020 records the acceptance and final reconciliation authorization.
The corrected label compiles in both modes with no warnings/errors; all five
mapped shim assemblies and 217 emitted references still agree with the target.

The supplied `workshop-0.1.36/LogOutput.log` is 1,801 bytes, SHA-256
`8AC56F580CF06DA04E3B5468C50A9E519951268D3D6303BB534AF2E089F102B1`.
It records BepInEx 5.4.17.0, production 0.1.36.97b7f86, the expected target MVID,
and one patch on star 60, layer 1, radius 36,000: 6 nodes / 6 frames, progress 1/12.
No Error/Fatal entry or feasibility probe load appears. It does not independently
instrument editor reopen, readability or native click-through; those outcomes
use the owner's explicit confirmation.

The supplied `20260912134823_1.jpg` is byte-identical to the earlier screenshot
(1,691,655 bytes, SHA-256
`23B51231BC0E452F20417B55C3639B5B038DF47C8E73CA9EDA6BEBE9236E38DE`).
Visual inspection confirms it shows the old top-center panel, not the refinement.
It is retained as supplied and not relabeled as a new UI capture. Both original
files were copied unchanged to ignored `artifacts/owner-session/0.1.36/`.

The owner then supplied the replacement screenshot. It was retained as
`ui-refinement.jpg` (726,085 bytes, SHA-256
`783EBF39EED2C7380158597CB1C0CAC5958E3EF285A0AB80E7283CF26A787858`)
beside those files. It shows the compact title/button row and readable
`Layer 2: 1/12 patches planned` feedback to the left of the bottom toolbar, with
the center caption unobstructed. The native inspector also selects layer 2,
radius 9,700, with 6 nodes / 6 frames / no shells. This is a layout/current-selection
capture, not evidence that the separate logged layer-1 action painted layer 2.
It supplies visual corroboration for the owner's refinement acceptance.

## SB-I5.4 — Final MVP reconciliation

The final review covered all 25 requirements through all 15 acceptance cases.
The implementation paths were read alongside their existing checks; no new full
runtime matrix was introduced. The following is an evidence map, not a second
work-status table. E1–E7 and W1–W6 refer to the specification's evidence catalogue.

| Case | Requirements (SB-MVP) | Implementation and checked evidence | Limits retained |
| --- | --- | --- | --- |
| SB-A01 | 01,03,25 | Mapped/native compilation, target checks and DLL metadata in SB-I1.1–1.2 and final package below; both owner logs identify the target | Recorded target only; compilation is distinct from gameplay |
| SB-A02 | 03,08,10,20 | `NativeTarget.Resolve`, `Plugin`, `PaintSession`; selection/readiness/no-write checks in SB-I3; E5 and accepted owner sessions | Multiple-selection live case W5; not every UI transition is separately logged |
| SB-A03 | 03,10,14,20 | Native rounded latitude read, pre-write refusal, managed 67/68 boundary checks; sufficient unlock in owner sessions | Below-threshold runtime and research names W4 |
| SB-A04 | 04–07,09,11 | Compiled plan, independent derivation, all twelve call deltas and 0.1.31 export comparison: 60/90 within numeric bounds | No optimality or new radius claim |
| SB-A05 | 09,12,14 | `NativeGraph` and `Preservation`; retained/replaced identities and lost SP negative checks; partial/completed native construction in E4 | Production exports omit identity/SP/CP; no new completed-construction wait |
| SB-A06 | 12,13,16 | All 32 face boundaries in recognition checks; both shell classes in additive checks; E4 pentagon and 0.1.31 retained hexagon boundary | Completed hexagon CP and endpoints W2 |
| SB-A07 | 02,04,05,13,14 | Native radius inspection, independent scalar positions and checked numeric scales; actual full frameworks at 9,700 and 36,000 | Maximum-radius/both-face endpoint coverage W1/W2 |
| SB-A08 | 02,08,15,17 | Fresh target callback and independent current-layer checks; 0.1.31 layer-1 progression and layer-3 start; refinement screenshot reflects selected layer 2 | Other-system painting W3; screenshot is not an extra logged paint |
| SB-A09 | 08,15,17,19 | Stateless graph recognition, editor attach/detach review, E5 menu reload and owner refinement confirmation | No separately instrumented production reload record |
| SB-A10 | 15,17,19,22 | No custom save/progress writer; fresh recognition of complete and partial outcomes; native save inspection | Full exit/removal/reinstallation W6 |
| SB-A11 | 15,17,18,20 | Permuted/reused numeric IDs in recognition checks; recreated native layers in E5 | Content equivalence cannot establish historical ownership |
| SB-A12 | 10,15,16,18,20 | Missing/extra/duplicate/displaced/ambiguous graphs, partial deltas and float boundaries in `RecognitionChecks`; no-write edit refusal in `PaintChecks`; E5 native edits | In-bound rounding is deliberately indistinguishable from an equally small edit |
| SB-A13 | 06,10,20 | Complete graph is read-only; repeated complete requests checked with zero writes; E5/E6 and 0.1.31 final capture | Final graph capture alone does not instrument an extra click |
| SB-A14 | 12,21,22 | Zero/throw/capture-failure and preservation negatives in `PaintChecks`; one diagnostic and persistent session stop; E4 native rejection | No atomicity, retry or rollback claim; no forced production fault |
| SB-A15 | 23–25 | Actual downloaded ZIP checks, sequential identity, native reference comparison, retained source/license/icon and negative package cases | No release, upload or Thunderstore moderation acceptance |

The user accepted the production MVP and then the workshop refinement, with a
specific no-recheck disposition for the label capitalization (SB-D018–020).
No unresolved core defect was found. The existing stop boundary was kept because
native allocation can partially succeed; diagnostics retain the original exception
and no retry/rollback or speculative compatibility gate is present.

### Final milestone package

[Run 34694058740](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34694058740),
build 38 / attempt 1, passed all CI checks. Its clean source revision is
`f761feed8d58c3852bb5c272cf1973b366a40546`; the diagnostic identity is
`0.1.38.f761fee`, numeric version `0.1.38`, assembly version `0.1.0.0` and GUID
`dsp.spherebuilder`.
The [direct package](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34694058740/artifacts/10297468797)
was independently downloaded and checked: 96,096 bytes, 48 valid entries,
one production DLL, required root/source/license/icon inputs, and no nested ZIP
or dependency/probe payload. Package SHA-256 and hosted digest agree:
`4A64DAC4A208F2B2C0430EDCC2A4358984C6CBA0C1186E386E71DFB431E09B45`.
DLL SHA-256 is `7752FF5E21844466BBD1907B25FFADF015C4DD8FF9BFF1396C1982FB113FC313`.
All 217 emitted references match the same-revision/version local native build;
both modes have no warnings/errors and all five shim assemblies match the map.
The download/build record are retained in ignored `artifacts/hosted/34694058740/`.

The sole runtime-source difference from owner-tested 0.1.36 is **Paint Next Patch**
capitalization. SB-D020 explicitly accepts that difference without another live
check. Runtime geometry/preservation evidence remains the 0.1.31 session and E4–E6;
the 0.1.36 session and replacement screenshot establish the UI refinement.

Publication polish is the next planning boundary: presentation/package copy,
any agreed cosmetic refinements and release preparation. This review did not
add product features, reopen W1–W6, author that plan, or authorize publication.

## Closeout management and code review

The management pass archived the implementation plan with its original scope,
story bodies and criteria intact apart from relative links; this was mechanically
compared with the pre-archive plan. It left an empty polishing roadmap boundary,
updated active usage/contract references, and changed superseded phase/handoff
claims to historical language. All 128 local Markdown links/anchors passed after
repairing the renamed concept-section link. Historical operator procedures remain
explicitly marked as reproduction material. State authority remains PROJECT.md.

The code pass read production source, native declarations, logic/metadata checks
and package/capture scripts. Tests compare behavior and data, not exact UI or
exception prose. Exact GUIDs, signatures, checksums and serialized keys establish
external contracts and were retained. No catch-and-rethrow wrapper, nested
recovery loop, speculative compatibility gate, or project steering comment was
found in production code. The existing catches preserve diagnostic exception
details and stop non-atomic mutation as required; they were not broadened or removed.

Cleanup adds only two technical comments explaining the float allowance and
partial-write boundary, orders the plugin imports consistently, documents the
source layout in BUILD.md, and removes redundant reference-map prose. The source
diff changes no executable expression, dependency, native declaration or generated
plan. No new runtime recheck is required for these documentation-only code edits.
