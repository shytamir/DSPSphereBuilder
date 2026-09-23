# Build and packaging

[PROJECT.md](PROJECT.md) owns readiness and acceptance. Use Windows, PowerShell 7,
Git, and the SDK pinned in [global.json](../global.json). Package image validation
uses System.Drawing; compilation and checks do not launch the game.

## Build the package

From the repository root:

```powershell
$commit = git rev-parse HEAD
./scripts/Build-Package.ps1 -BuildNumber 1 -Commit $commit
```

This compiles production source against the mapped reference shims, checks DLL
metadata, builds the executable ZIP, validates it, and writes
`artifacts/BUILD-INFO.json` with full commit, dirty state, version and hashes.
Use a positive local rehearsal number; local numbers do not reserve CI numbers.
Failed compilation or a missing input stops packaging; there is no prebuilt-DLL
or mock fallback. Generated outputs stay under ignored `artifacts/`.
The supplied commit must match the checkout's HEAD. Dirty local builds remain
explicitly marked rehearsals in build information; a candidate comes from clean CI.

To also compile against the actual target and compare emitted references:

```powershell
./scripts/Build-Package.ps1 -BuildNumber 1 -Commit $commit -DspManagedPath $managedPath -BepInExCorePath $loaderPath
```

Set the path variables to the actual `DSPGAME_Data/Managed` and `BepInEx/core`
directories. This native-reference command remains pinned to the
[original build/reference baseline](PROJECT.md#target-game-reference) and its
[reference map](../references/README.md). It rejects the updated 0.10.35.29057
assembly before compilation. The unchanged published mod's
[owner-validated support](PROJECT.md#supported-game-version) for that game update
is separate from retargeting these build inputs; use the original references to
reproduce the native build. The default shim build above is unaffected.
All newly referenced native surfaces
must be inspected and mapped in the commit introducing them.

For compilation alone, `scripts/Build-Plugin.ps1` accepts the same arguments.
Outputs are `artifacts/plugin/Shim/DSPSphereBuilder.dll` and, with real inputs,
`artifacts/plugin/Native/DSPSphereBuilder.dll`. The package selects only the
production DLL; shim and native dependency binaries are never shipped.
Production compiler paths map to `/_/` so DLL/PDB debug records do not expose the
developer's workspace or user-home location.

## Offline checks

```powershell
dotnet run --project checks/Logic/Logic.csproj -c Release -- artifacts/compiled-plan.json
python -B scripts/check_plan.py artifacts/compiled-plan.json
./scripts/Test-PackageFailures.ps1 -PackagePath artifacts/packages/DSPSphereBuilder-1.0.1.zip -ExpectedVersion 1.0.1 -ExpectedCommit $commit
```

Use the version printed by the build. Python 3.12 is needed only for geometry
checks/derivation (CI 3.12.10; bundled local runtime 3.12.14). The compiled-code
checks exercise this project's managed logic; native game methods and the plugin
entry point are not invoked. The independent derivation compares all twelve
deltas and numeric positions. `scripts/write_plan.py src/Plan.Data.cs` regenerates
the committed plan. No generator or parser runs inside the mod.

The compiled comparison covers both orientations, all twelve native-grid centers
and the exact published legacy float values retained under `checks/fixtures/`.
Those fixtures are numerical measurements, not game assets. The generation command
above emits aligned and legacy direction tables with shared topology. Historical
probe/envelope/capture tools retain the original derivation by default; use
`derive(..., grid_aligned=True)` for a new aligned capture. See
[alignment evidence](GRID-ALIGNMENT.md) for provenance and the focused hotfix checks.

To inspect a downloaded package independently:

```powershell
./scripts/Test-Package.ps1 -PackagePath $zipPath -ExpectedVersion $version -ExpectedCommit $commit -ExpectedPayloadHash $payloadHash
```

Take the expected inputs from the matching separate build record. The hash is
optional for a local check; use it when validating downloaded bytes against CI.
The validator checks archive entries, UTF-8, PNG decoding/dimensions, revision-specific
source access, both license texts, retained icon bytes, runtime dependency metadata, and DLL
GUID/version/revision, assembly file-version attribute and PE version resource
without loading it. Negative cases first validate the original package, then cover malformed payload,
identity, dependencies, source access, licenses, image, and archive layout.

## Source layout

| Files | Responsibility |
| --- | --- |
| [Plugin.cs](../src/Plugin.cs) | Editor panel, one click listener, feedback refresh and attachment cleanup |
| [NativeTarget.cs](../src/NativeTarget.cs), [NativeGraph.cs](../src/NativeGraph.cs) | Resolve current native selection; read snapshots and invoke native node/frame creation |
| [PaintSession.cs](../src/PaintSession.cs), [Preservation.cs](../src/Preservation.cs) | Check prerequisites, apply one delta, compare existing records and stop on partial failure |
| [GraphRecognition.cs](../src/GraphRecognition.cs), [LayerGraph.cs](../src/LayerGraph.cs) | Match captured native content to a unique prefix; retain references only for same-action preservation |
| [Geometry.cs](../src/Geometry.cs), [Plan.Data.cs](../src/Plan.Data.cs) | Float position scaling and generated reference directions, deltas and face boundaries |

`checks/Logic` exercises managed records and call sequences without invoking native
methods. `checks/Metadata` reads compiled identities and emitted references without
loading the plugin. The separate `probe/` and `tests/probe-json/` retain the historical
experiment; neither is a production dependency.

## Version contract

[Version promotion and change records](VERSIONING.md) is the authoritative
procedure for VERSION edits, owner authority, DLL/package/internal identities
and changelogs. Its [version-field table](VERSIONING.md#version-fields) defines
all generated values. Use the build commands above to produce and validate them;
do not maintain a second version counter or hand-edit generated metadata.

## Package contract

The ZIP contains exactly five files: root `manifest.json`, `README.md`, `icon.png`,
and `LICENSE`, plus `BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll`.
No source tree, game/shim DLL, probe, evidence dump, cache, wrapper directory or
nested ZIP belongs in the package. [Get-PackageInputs.ps1](../scripts/Get-PackageInputs.ps1)
lists static inputs; the builder generates manifest, README source footer and
combined license material. The validator has an independent expected entry list.

The manifest declares `xiaoye97-BepInEx-5.4.17`. The supplied icon is retained
unchanged as a 256×256 PNG. These follow the
[Thunderstore package requirements](https://wiki.thunderstore.io/mods/creating-a-package)
and [BepInEx directory routing](https://wiki.thunderstore.io/mods/packaging-your-mods).
The combined distribution retains GPL-3.0 coverage and the original Apache-2.0
source grant/notices. LICENSE includes attribution and both complete license texts.
README and LICENSE link to the full-commit public repository archive containing
source, build/interface declarations, derivation and the upstream fixture.
Keep that revision publicly accessible while distributing its binary; verify the
exact source download before candidate acceptance. The install ZIP needs no
separate development tree. Package validation is not runtime acceptance or a
Thunderstore moderation decision.

## GitHub Actions

[build.yaml](../.github/workflows/build.yaml) runs on `main` pushes and manual
dispatch, with read-only repository permissions and a bounded timeout. It builds
production source, validates the package and affected offline logic, and records
build identity. [PROJECT.md](PROJECT.md) identifies the accepted published baseline.
[GitHub releases](https://github.com/shytamir/DSPSphereBuilder/releases) provides
published downloads; each CI run summary identifies its development artifact.
A later CI build does not itself update the published release.

The workflow does not create releases/tags, edit VERSION, submit to Thunderstore,
or interact with a game installation.

The package upload uses the pinned action's supported
[`archive: false` input](https://github.com/actions/upload-artifact/blob/043fb46d1a93c77aae656e7c1c64a875d1fc6a0a/action.yml).
Download `DSPSphereBuilder-<version>.zip` from the run summary or artifact list:
it is the package itself, with `manifest.json` at its root. There is no inner ZIP
to extract. Build information is a separate artifact. A retry preserves the
numeric version and replaces that run's same-named package; use the matching
attempt's build record and hashes. No other workflow run's package is replaced.
