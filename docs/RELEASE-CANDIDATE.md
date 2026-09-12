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

### Initial blocker

SB-R1.2 includes reachable history explicitly. A new content commit cannot remove
the confirmed metadata. Replacing historical identities would change the affected
commit IDs and require a coordinated force-push; old source references, hosted
artifacts and GitHub-retained copies would then need review. A force-push alone
must not be represented as proof that all published remnants are gone.

At the initial stop, the [agent instructions](../AGENTS.md) prohibited history
rewriting/force-pushing and the roadmap excluded it from ordinary push authorization.
No rewrite or privacy exception was inferred. SB-D025 recorded that blocker;
the subsequent explicit owner approval and cleanup are recorded below.

### Authorized cleanup and checks

The owner explicitly approved any necessary history rewrite. SB-D026 records the
result: all 46 commits through the blocker record were recreated with project
handle/GitHub no-reply attribution; the GitHub service identity was retained.
Every tree, message, parent relationship and timestamp was checked against its
original. One old signature was removed because rewritten metadata invalidates it.
The force-push used an exact old-head lease. Public anonymous history inspection
returned 46 commits with only the expected project/service identities.

[HISTORY-REWRITE.json](management/archive/HISTORY-REWRITE.json) maps old evidence
identities to their equivalent source commits. Old build labels/hashes in runtime
records remain historical measurements, not labels of newly compiled binaries.
The rewritten tip was `65ea520ddada2e24a5cf6bdb940fd55f8ddd82c5`.

Local reflog/object cleanup removed only the verified superseded commits. An
unrelated recovery tree and all Codex references were preserved. An initial broad
purge was rejected by automatic approval review; enumerating and protecting the
unrelated object allowed the narrower cleanup. No unknown recoverable work was
discarded. The remaining unreachable object is the protected tree, not a commit
containing private attribution. This verifies repository history; it does not
claim control over third-party copies or GitHub's internal retention of old objects.

Privacy coverage included 258 reachable historical file blobs, 805 local files
including ignored inputs, and 2,218 entries recursively read from ZIPs. All 46
completed hosted runs' logs and all 60 then-available artifacts were downloaded
successfully and included. UTF-8/UTF-16 searches were supplemented by inspection
of the matches, image content/metadata and production debug records.

The additional actual finding was an ignored upstream API-response cache carrying
a contributor's email. It now retains only the source SHA, source URL and tree SHA.
Its recheck found no email. Public project/source handles and corporate license
notices were classified as provenance, not personal contact data; required
attribution and license text were preserved. Email-like matches in PDB bytes were
framework DLL names adjacent to binary bytes; JPEG matches were compressed data.
The three retained screenshots (two unique images) showed only game UI. Their
metadata contained JPEG tables, not author/location fields; the icon's metadata
contained PNG image information. No artwork or accepted screenshots were changed.

Machine-path matches were generated build/diagnostic paths, not additional personal
contacts. To keep future production payloads independent of the developer's chosen
workspace, compiler paths now map to `/_/`. Both shim and real-reference builds
passed with zero warnings/errors and the same 217 emitted references; five mapped
shim assemblies still match the target. DLL/PDB byte inspection found no local
workspace or user-home path in either production build. PE inspection read the
mapped CodeView path directly. No native reference surface or runtime behavior changed.

These checks cover the repository, reachable history and inspected outputs; they
are not an internet-wide erasure claim. Final candidate outputs receive a regression
check in SB-R5.1. Raw download inventories and sanitized match summaries are local
under `artifacts/release-review/`.

## SB-R2.1 — Minimal package and installation paths

The builder now emits the five entries in SB-R1.1's inventory. README and LICENSE
receive the exact-revision source URL; LICENSE combines attribution with the two
unchanged license texts. The former source tree is absent. BUILD.md and fixture
provenance describe the implemented distribution arrangement.

Local rehearsal `0.1.47.e26008d` produced a 52,203-byte ZIP with exactly five entries.
The independent validator checked DLL identity, source access, retained license
texts/icon, UTF-8 and image dimensions. All 13 malformed-package cases were rejected,
including missing source access and a missing license. Expected entries no longer
come from the builder's inventory, and prose wording is not asserted.

Isolated manual and manager-route filesystem rehearsals each installed one DLL
under a BepInEx plugins subfolder; both payload hashes matched the ZIP. These are
path/byte checks against the verified routing rules, not live mod-manager or game
observations. No installed mod or save was touched.

A separate public checkout of `e26008d` compiled with the pinned SDK and its own
source/interface inputs: zero warnings/errors, 217 emitted references and correct
identity. It used no source copied from the working checkout. Source archive
inspection and this clean build establish the accessible source/build route.

An initial local rehearsal supplied an incorrect full revision and was discarded;
the replacement used `git rev-parse HEAD`. The builder currently trusts that
caller's revision argument beyond its format. SB-R4.1's source-identity review must
close that gap before candidate delivery. The working-tree package above is a
rehearsal, not a published or accepted candidate; final bytes come from clean CI.

## SB-R2.2 — Player-facing description and README

The manifest description is 105 characters of benefit-led prose. The README is
158 words before the generated source link: a short introduction, natural use
guidance and separate manager/manual installation paragraphs. It retains the
68° prerequisite, player-filled shells and the continuation limit without API,
build, probe, ownership-history or project-state language. It makes no mathematical
optimality or new compatibility claim.

Installation guidance was checked against the primary
[mod-manager download workflow](https://github.com/ebkr/r2modmanPlus/wiki/Downloading-Mods)
and the DSP routing inputs from SB-R1.1. The copy describes installing a package;
it does not announce an existing public listing. Candidate download logistics stay
in the final handoff.

The finished README, including its source footer, was entered in Thunderstore's
own [Markdown preview](https://thunderstore.io/tools/markdown-preview/). The page
reported successful rendering; visual and accessibility inspection confirmed the
headings, action emphasis, installation paths and credit/source links. No package
was submitted or owner copy approval requested. UTF-8 decoding and the local
`0.1.48.6ce9a97` package check passed with the new copy. No prose assertions were added.
