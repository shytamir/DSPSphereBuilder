# Build and packaging

See [PROJECT.md](PROJECT.md) for current readiness and acceptance. This document
defines the mock package and its build procedure.

## Local build

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
Thunderstore `version_number`. No assembly version is generated until there is an
assembly to build.

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
dispatch. It checks out the triggering revision, builds and validates the mock,
and uploads the ZIP and build information as one Actions artifact. The workflow
has read-only repository permissions and a bounded job timeout.

Download the artifact from a successful run and extract the enclosed
`DSPSphereBuilder-<version>.zip`. The downloaded artifact wrapper contains both
that ZIP and build information; do not submit the wrapper to Thunderstore.

The workflow does not create releases or tags, push version edits, publish to
Thunderstore, or interact with a game installation.
