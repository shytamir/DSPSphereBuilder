# Build and packaging

See [PROJECT.md](PROJECT.md) for current readiness and acceptance. This document
defines production compilation and the current mock package procedure.

## Production compilation

Use the SDK pinned in [global.json](../global.json) and PowerShell 7. From the
repository root, compile against the CI declarations with:

```powershell
$commit = git rev-parse HEAD
./scripts/Build-Plugin.ps1 -BuildNumber 1 -Commit $commit
```

To also compile against the local target and compare emitted references:

```powershell
./scripts/Build-Plugin.ps1 -BuildNumber 1 -Commit $commit -DspManagedPath $managedPath -BepInExCorePath $loaderPath
```

Set those variables to the actual `DSPGAME_Data/Managed` and `BepInEx/core`
directories. The script checks the recorded local target hash, compiles both
reference modes, checks the [reference map](../references/README.md), and inspects
DLL metadata without loading the plugin. It invokes no game runtime.

Outputs are `artifacts/plugin/Shim/DSPSphereBuilder.dll` and, with real inputs,
`artifacts/plugin/Native/DSPSphereBuilder.dll`. Reference assemblies in build
directories are compile inputs, never package payload. Build information records
the source commit and whether the local working tree was dirty; local builds with
edits must not be represented as clean reproductions of that commit.

## Local build

Production geometry/logic checks use the compiled DLL and Python 3.12 (CI pins
the published Windows runtime 3.12.10; the bundled local runtime is 3.12.14):

```powershell
dotnet run --project checks/Logic/Logic.csproj -c Release -- artifacts/compiled-plan.json
python -B scripts/check_plan.py artifacts/compiled-plan.json
```

The committed `src/Plan.Data.cs` is generated with
`python -B scripts/write_plan.py src/Plan.Data.cs`. The comparison checks numeric
coordinates and topology from the compiled data against the retained derivation;
it does not assert generated source text. Production builds need no Python at
runtime and load no blueprint parser.

Windows with PowerShell 7 and Git is sufficient. PNG validation uses Windows
System.Drawing. No game installation, SDK, package restore,
or downloaded build dependency is required.

From the repository root:

```powershell
$commit = git rev-parse HEAD
./scripts/Build-Package.ps1 -BuildNumber 1 -Commit $commit
```

Use a positive build number for a local rehearsal. The script reads `VERSION`,
builds and validates a ZIP, and writes build identity to `artifacts/BUILD-INFO.json`.
Local numbers are not reserved CI sequence numbers. The supplied commit identifies
the source revision; a local build can also include uncommitted working-tree edits.

To inspect an existing package independently:

```powershell
./scripts/Test-Package.ps1 -PackagePath artifacts/packages/DSPSphereBuilder-0.1.1.zip -ExpectedVersion 0.1.1
```

Use the actual version/path printed by the build. Generated outputs are ignored
by Git and are never staged as source.

## Version contract

The manually authored root [VERSION](../VERSION) contains exactly two assignments:

```text
MAJOR=0
MINOR=1
```

Edit major/minor deliberately. There is no manually maintained patch or commit
hash in that file. The build uses these inputs as follows:

| Value | Rule | Example for build 42, commit `abcdef012345...` |
| --- | --- | --- |
| Thunderstore `version_number` | `MAJOR.MINOR.BuildNumber` | `0.1.42` |
| Diagnostic build label | `MAJOR.MINOR.BuildNumber.shortCommit` | `0.1.42.abcdef0` |
| Package filename | `DSPSphereBuilder-version_number.zip` | `DSPSphereBuilder-0.1.42.zip` |

The short commit is the first seven hexadecimal characters. The four-part build
label is diagnostic text, not a semantic package version. Hashes never enter the
Thunderstore `version_number`. The loader's plugin version is the same numeric
package version. Assembly/file versions are `MAJOR.MINOR.0.0`, and informational
version is the diagnostic build label. This keeps sequential package versions
independent of CLR assembly-version component limits.

CI uses `github.run_number`, the sequential number for this workflow, as
`BuildNumber`. New runs advance it, including failed runs; gaps are normal.
Rerunning the same run keeps its number and version. The run attempt is tracked
separately in build information and artifact names. Major/minor changes do not
reset the workflow sequence. See [GitHub's variable reference](https://docs.github.com/en/actions/reference/workflows-and-actions/variables).

Keep this single workflow's sequence when extending it to build the real mod.
Do not substitute a short hash, commit count, run ID, or retry attempt for the
numeric patch.

## Mock ZIP contract

The package contains exactly these root entries:

```text
manifest.json
README.md
icon.png
LICENSE
```

The manifest uses `DSPSphereBuilder`, a numeric three-part version, the repository
URL, a description explicitly identifying the mock, and an empty dependency
array. The mock has no runtime payload or dependency to install. The dedicated
[package README](../packaging/README.md) describes that artifact, without copying
project status. The 256x256 PNG is a placeholder, not final product artwork.

The validator checks the ZIP entry set, readable UTF-8 text, manifest fields and
version, and actual PNG decoding and dimensions. These follow the
[Thunderstore package requirements](https://wiki.thunderstore.io/mods/creating-a-package).
A format-valid mock is not a functional mod or a moderation-approved submission.

Build information stays outside the package and identifies the full commit,
numeric version, diagnostic label, build number, and retry attempt.

## GitHub Actions

[build.yaml](../.github/workflows/build.yaml) runs on pushes to `main` and manual
dispatch. It checks out the triggering revision, compiles production source with
the mapped shims and checks its metadata, then builds and validates the mock,
and uploads the ZIP and build information as one Actions artifact. The workflow
has read-only repository permissions and a bounded job timeout.

Download the artifact from a successful run and extract the enclosed
`DSPSphereBuilder-<version>.zip`. The downloaded artifact wrapper contains both
that ZIP and build information; do not submit the wrapper to Thunderstore.

The workflow does not create releases or tags, push version edits, publish to
Thunderstore, or interact with a game installation.
