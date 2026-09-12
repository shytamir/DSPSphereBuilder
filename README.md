# DSP Sphere Builder

**Paint a perfect C60 Dyson sphere, one patch at a time.**

DSP Sphere Builder is a proposed quality-of-life mod for Dyson Sphere Program.
Its intended interaction is simple: select an empty sphere layer, then click
**Paint next patch** whenever you want to extend its framework.

The design grows through twelve pentagons: one polar cap, five in the upper
ring, five in the lower ring, and the opposite cap. Each click completes the
next pentagon, connects it to earlier patches, and adds a leading spoke toward
the next. Hexagon boundaries emerge between the pentagons. Existing construction
is preserved, and shell filling stays with the player.

"Perfect" means reproducing the chosen reference geometry accurately. The
reference author's claims of mathematical optimality are not independently
established by this project.

## Start here

- [Project steering and current state](docs/PROJECT.md) — the sole authority for
  scope, phase, work status, readiness, and acceptance.
- [Product concept](CONCEPT.md) — the agreed experience, geometry, references,
  and copyable sample blueprint with its import caveats.
- [Roadmap](docs/management/ROADMAP.md) — the place to define bounded work and
  completion criteria when planning begins.
- [Build and packaging](docs/BUILD.md) — local commands, version translation,
  and the GitHub Actions artifact contract.
- [Agent working practices](AGENTS.md) — scope discipline, validation, and Git
  conventions for automated contributors.

## Mock package

The [build workflow](https://github.com/shytamir/DSPSphereBuilder/actions/workflows/build.yaml)
produces a **mock Thunderstore ZIP** containing package metadata, a package README,
a placeholder icon, and the license. It contains no executable mod and installs
no gameplay functionality. Download an Actions artifact and extract its enclosed
package ZIP; the artifact wrapper is not itself the Thunderstore package.

For a local build on Windows, use PowerShell 7 from the repository root:

```powershell
$commit = git rev-parse HEAD
./scripts/Build-Package.ps1 -BuildNumber 1 -Commit $commit
```

The command validates the ZIP before returning its path. Outputs stay under
ignored `artifacts/`. CI supplies the sequential build number automatically;
see [the version contract](docs/BUILD.md#version-contract) for how it maps to
Thunderstore's numeric version.

## References and credit

The geometry comes from [Cosmin1490's optimized 60-node sphere](https://www.dysonsphereblueprints.com/en/blueprints/dyson-sphere-best-cost-efficiency-optimized-sphere-design-60-nodes-15-cheaper-than-football).
The author's [Blueprint Generator](https://github.com/Cosmin1490/DysonSphereProgram-BlueprintGenerator)
provides supporting reference material. Attribution and the sample's validation
limits are recorded in the [concept](CONCEPT.md#references).

## License

This repository uses the [Apache License 2.0](LICENSE). Referenced third-party
material retains its own licensing.
