# DSP Sphere Builder

**Paint a perfect C60 Dyson sphere, one patch at a time.**

DSP Sphere Builder is a quality-of-life mod for Dyson Sphere Program.
The interaction is simple: select an empty sphere layer, then click
**Paint Next Patch** whenever you want to extend its framework.

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
- [Roadmaps](docs/management/ROADMAP.md) — the next planning boundary and
  archived plans with their original scope, stories and gates.
- [MVP specification](docs/MVP-SPECIFICATION.md) — the behavioral contract,
  complete patch sequence, acceptance cases, and declared evidence limits.
- [Build and packaging](docs/BUILD.md) — local commands, version translation,
  and the GitHub Actions artifact contract.
- [Agent working practices](AGENTS.md) — scope discipline, validation, and Git
  conventions for automated contributors.

## Build and use

Select exactly one native sphere layer with at least 68 degrees of unlocked
Dyson sphere latitude. Start on an empty layer or continue a recognized staged
framework. Click **Paint Next Patch** to extend it without waiting for
construction. Save/reload continuation comes from the actual framework; layouts
that no longer match a recognized stage are refused. Unexpected errors stop painting for the plugin
session and may leave a partial patch; there is no rollback or automatic retry.

The executable package includes the DLL, supplied icon, metadata and licenses,
with a link to its exact source revision. It requires BepInEx 5.4.17. Follow the [package instructions](packaging/README.md)
for installation and use, and [PROJECT.md](docs/PROJECT.md) for runtime acceptance
and candidate identity. The [workflow](https://github.com/shytamir/DSPSphereBuilder/actions/workflows/build.yaml)
summary identifies its package download.

For a local Windows build with PowerShell 7 and the pinned .NET SDK:

```powershell
$commit = git rev-parse HEAD
./scripts/Build-Package.ps1 -BuildNumber 1 -Commit $commit
```

The command compiles and validates the package. Outputs stay in ignored
`artifacts/`. CI supplies the sequential build number automatically; see the
[build guide](docs/BUILD.md) for native-reference comparison and versioning.

## References and credit

The geometry comes from [Cosmin1490's optimized 60-node sphere](https://www.dysonsphereblueprints.com/en/blueprints/dyson-sphere-best-cost-efficiency-optimized-sphere-design-60-nodes-15-cheaper-than-football).
The author's [Blueprint Generator](https://github.com/Cosmin1490/DysonSphereProgram-BlueprintGenerator)
provides supporting reference material. Attribution and the sample's validation
limits are recorded in the [concept](CONCEPT.md#references).

## License

Original source uses the [Apache License 2.0](LICENSE). Referenced third-party
material retains its own licensing; the combined package carries GPL-3.0 coverage
and both license notices as described in the [build guide](docs/BUILD.md#package-contract).
