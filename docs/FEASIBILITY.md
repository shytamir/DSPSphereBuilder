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
