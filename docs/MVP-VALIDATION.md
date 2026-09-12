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
