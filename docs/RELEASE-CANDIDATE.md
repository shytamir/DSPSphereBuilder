# Release-candidate evidence

[PROJECT.md](PROJECT.md) owns execution state, decisions, gates and acceptance.
This document records checks and findings from the
[release-candidate roadmap](management/ROADMAP.md). Earlier runtime observations
remain in [MVP-VALIDATION.md](MVP-VALIDATION.md); no new gameplay is implied here.

## SB-R1.1 — Minimum distribution contract

Inspected on 2026-09-12 against source
`81378a82011e825269155229319407b98f1f88e0`. The prior verified 0.1.41 ZIP contained
48 entries; `Get-PackageInputs.ps1` and `Build-Package.ps1` account for them through
the installable DLL, root metadata/legal material and the retained `source/` tree.

### Proposed inventory

| ZIP entry | Necessary purpose |
| --- | --- |
| `manifest.json` | Thunderstore identity, numeric version, dependency and description |
| `README.md` | Player description, installation/use and a source-download link for this build |
| `icon.png` | The owner's existing 256×256 PNG package icon |
| `LICENSE` | Attribution and both applicable license texts, with the exact source-download location |
| `BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll` | The only executable payload |

Remove every `source/` entry from the install ZIP: its README/revision marker,
production source, shim declarations, metadata checks, derivation scripts,
configuration and research fixture/license copies. Their purpose is source access,
not installation; the revision-specific repository archive supplies that material.
Keep build information as a separate CI artifact. No other install-package entry
has a demonstrated need. SB-R2.1 will implement this proposal and update BUILD.md.

### Source and license basis

The original source carries Apache-2.0. The retained reference input declares
GPL-3.0; no upstream generator implementation is incorporated. Treat the derived
geometry conservatively as covered material, retain its credit and GPL terms,
and provide the full corresponding project source. Do not base removal on an
assumption that the geometric input is outside copyright protection.

The actual [retained GPL text](../research/cosmin1490/LICENSE), section 6(d),
allows separately downloaded source, including another server with equivalent
copying facilities, when clear directions accompany the object download and
source remains available. The [Apache compatibility guidance](https://www.apache.org/licenses/GPL-compatibility.html)
confirms Apache-2.0 material can be included in GPLv3 distributions; the reverse
does not turn GPL material into Apache-2.0. The package's combined distribution
will retain GPL-3.0 coverage and the original Apache-2.0 notices/grant, rather than
presenting the entire payload as Apache-only. Neither upstream terms nor the
original-source grant is removed. SB-D024 records this distribution choice.

Use `https://github.com/shytamir/DSPSphereBuilder/archive/<full-commit>.zip` in the
generated README and LICENSE. No mutable branch link, expiring Actions artifact,
account requirement or written-offer service is used for corresponding source.
The maintainer must keep that public revision accessible while distributing its
binary. Final candidate verification rechecks the actual revision's archive;
moving/private/deleted source would invalidate this arrangement.

Anonymous Node `fetch` (no token or authentication header) retrieved the source
archive for the inspected revision with HTTP 200: 244,305 bytes, 108 archive
entries including its root/directories. SHA-256:
`FF4D580A651C68EBBC9131AC5EB55B51B0892A6AFD6A8F4B692AE0C7A9BC4022`.
Inspection found production source, build scripts, mapped reference declarations,
SDK/version inputs, geometry derivation, unmodified fixture, both license texts
and build instructions. GitHub reports the repository as public. The retained
local download is under ignored `artifacts/release-review/source-input.zip`.
SB-R2.1 will check this distribution's source build as part of package changes.

### Package and routing requirements

Primary [Thunderstore package requirements](https://wiki.thunderstore.io/mods/creating-a-package)
were refreshed on 2026-09-12: case-sensitive root manifest/README/icon, UTF-8,
256×256 PNG, three numeric version parts and description up to 250 characters.
The [routing guidance](https://wiki.thunderstore.io/mods/packaging-your-mods) and
actual [DSP rules](https://github.com/thunderstore-io/ecosystem-schema/blob/master/games/data/generated/dyson-sphere-program.yml)
identify the BepInEx loader, `.dll` plugin route and subdirectory tracking.
The existing DLL path fits that route and manual copying into an existing
`BepInEx/plugins/DSPSphereBuilder` directory. SB-R2.1 checks actual file placement.

The five-entry inventory, GPL/source arrangement, GUID, exact dependency and
diagnostic version mapping are project contracts grounded in these inputs.
No published rule found here prohibits exception rethrowing or mandates shipping
the existing development tree. These checks do not constitute moderation approval.
