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
Hosted compile evidence is recorded after the first matching run completes.
