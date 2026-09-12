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

## SB-R1.2 — Privacy review and historical attribution blocker

Inspection on 2026-09-12 at `767b1ea075df37991bee81db46b7f22e6f753947` found:

- 86 tracked files, no untracked non-ignored files and 609 ignored files. An initial
  tracked-content search found no personal email or machine-home-path matches;
  that search is not a complete privacy assessment of all formats.
- 45 reachable commits on `main`, no tags and no other local work branches.
  44 commits contain the same personal-provider email identity. The earliest is
  `79a72be46a4b81bc3ed93bf6a1a5b264202dec22`, and the latest is the inspected
  revision above. Historical author names also require disposition; one commit
  uses the account's public profile name rather than its repository handle.
- Anonymous GitHub commit-API access returned HTTP 200 and confirmed that the
  personal email occurs in published commit metadata. This is not only a local
  Git setting. The personal values are deliberately omitted from this record.
- No personal-provider email or home-path matches appeared in commit messages.
  The initial ignored-text scan found machine-specific paths in 51 generated
  files, and no personal-provider email matches. These include build intermediates
  and retained diagnostics; their values and binary/image counterparts still need
  classification and cleanup. No whole-repository privacy pass is claimed.
- Hosted artifact metadata was inventoried read-only. Hosted content, image/debug
  metadata and all reachable file versions have not completed their privacy checks.
  The historical attribution finding prevents passing the gate regardless of those
  remaining checks, so no purportedly exhaustive report was generated.

### Forward mitigation

The effective Git configuration was repeating a personal name/email on new commits,
including SB-R1.1. Repository-local `user.name` now uses the existing public project
handle; `user.email` uses the unique GitHub no-reply address already verified in
this repository's history. Effective settings were checked after the change.
Global configuration and existing commits were not altered. This prevents the
same future exposure; it does not sanitize the published history.

### Required resolution

SB-R1.2 includes reachable history explicitly. A new content commit cannot remove
the confirmed metadata. Replacing historical identities would change the affected
commit IDs and require a coordinated force-push; old source references, hosted
artifacts and GitHub-retained copies would then need review. A force-push alone
must not be represented as proof that all published remnants are gone.

The [agent instructions](../AGENTS.md) prohibit history rewriting/force-pushing,
and this roadmap excludes it from ordinary push authorization. No history rewrite,
remote deletion or privacy exception was attempted. SB-D025 records the mitigation
and blocker; PROJECT.md owns the resulting gate state. Resume only after an owner
decision on a separately authorized history cleanup or an explicit scope exception
for historical Git attribution. Neither choice is inferred from release-candidate
implementation authorization.
