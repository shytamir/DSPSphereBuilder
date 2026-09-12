# MVP implementation evidence

[PROJECT.md](PROJECT.md) owns execution state, gate outcomes, and acceptance.
This record distinguishes metadata inspection, offline checks, compilation,
package inspection, and later owner-operated runtime observations.

## SB-I1.1 — Mapped references and delivery inputs

Inspected 2026-09-12. The local target still has SHA-256
`AE0BA95F75BD879A62AA4CE253B2AB78EAA4FB3C7C595F5E1FEE75EBE0E0EF85`,
MVID `ece4a40e-5e73-43f4-a9f8-4e74970b5942`, and 7,830,016 bytes. The full displayed
game-build suffix remains unknown. No native code was executed.

The [reference map](../references/Map.json) records the inspected BepInEx 5.4.17.0
and UnityEngine.CoreModule 0.0.0.0 identities, seven initial types, required member
signatures, inheritance, and native metadata tokens. Mono.Cecil reads these
without loading game types. The initial entry-point shims compile successfully,
and every declared public/protected member matches the local metadata. Assembly
names/versions/public-key identity match the runtime destinations; the production
DLL will reference those identities, not a separately named shim library.

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

The minimal production plugin uses `dsp.spherebuilder` and the declared identity
mapping. Local compilation in both reference modes passed with no warnings or
errors using SDK 10.0.302. Metadata inspection found identical 19 emitted
assembly/member references in the shim and native builds. The checker reads PE
metadata, never instantiates the plugin or a game type.

Build 70,000 produced plugin/package version `0.1.70000`, assembly/file version
`0.1.0.0`, and informational label `0.1.70000.f85d4a5` from the local worktree based
on `f85d4a5`. This is local working-tree evidence, not a clean source-commit build.
The build record now explicitly reports dirty state. Retry attempts 1 and 2 kept
the same version, build 70,001 advanced it, and the unchanged mock ZIP validator
passed using the shared VERSION translation.

The build fails on missing required paths, a changed local target hash, compile
errors, mismatched plugin metadata, or a changed declared native reference map.
The source project marks references non-copying; later packaging will select the
production DLL explicitly instead of archiving a build directory.

The existing workflow now installs the exact pinned SDK through the official
[setup-dotnet action](https://github.com/actions/setup-dotnet), compiles production
source, and checks DLL identity before publishing its still-explicit mock artifact.
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
not the plugin entry point or any native game/Unity method. CI now runs the same
compiled-plan comparison. Its first hosted run stopped before the checks because
actions/python-versions does not publish Windows 3.12.14. The
[official version manifest](https://github.com/actions/python-versions/blob/main/versions-manifest.json)
lists 3.12.10 as the latest available 3.12 Windows x64 binary; CI now pins that
version. No geometry/code change or local Python replacement was needed.
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
in a game session. SP/CP/identity capture is retained for the next story's
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
and missing result frames. Production UI invocation and native behavior remain
for the later integration/human stories; this is not a new live observation.

## SB-I3.2 — Current-target editor control

The initial panel sits at the top center of the native control panel, 380 by 122
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

This is offline lifecycle review and compilation, not a Unity runtime test.
Visibility, overlap, readable feedback, actual event delivery, click-through,
reopening, and production native preservation await SB-I5.1. The hosted runs for
[SB-I2.2](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34677110100) and
[SB-I3.1](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34677535631)
also passed their affected offline checks.

## SB-I4.1 — Executable package and validator

Local package 0.1.28 was built from the working tree based on `389668a`; this is a
rehearsal identity, not a clean CI revision. Both reference modes compile without
warnings/errors, with identical 210 emitted references and a matching five-assembly
native map. The builder always compiles first and explicitly selects the production
DLL; no stale/missing payload or compiler failure falls back to a mock package.

The ZIP contains 47 files: required root metadata, one production DLL in the
verified BepInEx path, and source/compile inputs with pinned geometry, derivation,
credit and applicable licenses. The supplied icon is unchanged, with the hash and
256×256 PNG dimensions recorded above. Validation checks metadata identity, numeric
version, diagnostic revision, payload hash, UTF-8, real image decoding, retained
input hashes, and the file inventory. Twelve malformed variants were rejected:
missing DLL, wrong manifest/DLL versions, altered GUID, shim as payload, extra dependency/probe,
nested ZIP, wrapper folder, missing reference source, wrong dependency, and invalid
image. This tests package contracts, not README wording.

The root/package/build documentation now describes actual installation and use.
The intermediate workflow still wraps its executable ZIP with build information;
SB-I4.2 replaces that transport and verifies the real download. No game installation,
execution, release, or submission was performed.

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
The next candidate must pass hosted upload and independent download inspection.
