# Release-candidate evidence

[PROJECT.md](PROJECT.md) owns execution state, decisions, gates and acceptance.
This document records checks and findings from the
[archived release-candidate roadmap](management/archive/ROADMAP-first-release-candidate.md). Earlier runtime observations
remain in [MVP-VALIDATION.md](MVP-VALIDATION.md); no new gameplay is implied here.

## Owner review packet — 0.1.54

[Download the candidate ZIP](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34700204095/artifacts/10300236790)
from [run 54, attempt 1](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34700204095).
This packet retains the candidate reviewed by the owner. Later documentation
builds do not replace its identity. The [player README](../packaging/README.md) is the packaged copy; the ZIP adds
its exact-source link. The listing description is:

> Precise Dyson sphere planning without the angle-counting, with connected sections that grow at your pace.

| Identity | Value |
| --- | --- |
| File / size | `DSPSphereBuilder-0.1.54.zip` / 51,496 bytes |
| Source commit | `1c04199c490dfb029ba466ebfb53836ed46548f7` |
| Plugin / diagnostic version | `0.1.54` / `0.1.54.1c04199` |
| BepInEx GUID / name | `dsp.spherebuilder` / `DSP Sphere Builder` |
| CLR assembly / file version | `0.1.0.0` / `0.1.0.0` |
| ZIP SHA-256 | `9DE0DC20A8DFEAE635BD091E10E6557E1DE661C770DBC302E11873B9583A3767` |
| DLL SHA-256 | `1D9FC50D70EDC9F5030B375BC17A28575D8D1E522545D5968FA6BE8551F95A91` |

The ZIP contains only manifest, README, icon, LICENSE and the production DLL under
`BepInEx/plugins/DSPSphereBuilder/`. With the game closed and BepInEx already present,
replace the existing DLL in that subfolder with this ZIP's DLL; keep one production
copy. A public mod listing is not required to review this candidate.

Compared with the accepted MVP, this candidate trims package contents, improves
player copy and source/license access, maps compiler paths and strengthens build
identity checks. Painting, geometry, continuation and the accepted UI are unchanged.
Local/native and CI compilation, downloaded-byte validation, 15 malformed-package
cases, source access and final privacy checks passed; details are in SB-R5.1 below.

The packet requested acceptance of this package and its player copy; SB-D028 in
[PROJECT.md](PROJECT.md#sb-d028--accept-the-release-candidate-and-promote-to-10) records the owner's response.
No additional runtime case is indicated by these changes. Existing W1–W6 limits
remain in the [specification](MVP-SPECIFICATION.md#evidence-and-accepted-assumptions), and
upstream advisories were excluded by the owner's SB-D027 instruction. No new game
observation or Thunderstore moderation approval is claimed. Acceptance and any
later manual publication are recorded only in PROJECT.md.

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

## SB-R3.1 — Repository security assessment

Reviewed source: `2d4c8692a8093ef35381a15e8cde501615e2075f`, 87 tracked files.
The offline source assessment included an independent complete audit, architecture
review, focused delivery review and parent verification. It covered all production
and probe code, parsers/generators, tests, reference declarations/maps, workflow,
configuration, documentation, licenses, the reference fixture and PNG structure.
No repository SECURITY.md was present. No confirmed exploitable repository finding
was identified. This is source evidence, not a game or penetration test.

The production plugin has no network, external import or custom persistence route.
It uses compiled geometry and the current native selection. A partial native write
is a documented functional failure boundary, not rollback. Offline tools consume
operator-selected files; the ZIP validator requires exact entry names, extracts
only one fixed destination and reads PE metadata without executing the payload.
Numeric/hash validation constrains generated code and command arguments. The local
Mono.Cecil tool library and game/loader installation remain trusted external inputs.

Checks on 2026-09-12:

| Check | Coverage and result |
| --- | --- |
| Gitleaks 8.30.1 | `git . --log-opts='--all' --redact=100`: 49 reachable commits, no secret findings. `dir . --max-archive-depth=3 --max-decode-depth=2 --redact=100`: 12.75 MB scanned, 14 alerts triaged below. Complements the broader SB-R1.2 privacy/output inspection. |
| actionlint 1.7.12 | `-no-color .github/workflows/build.yaml`: passed. Manual review confirmed full action pins, read-only token, no persisted checkout credentials, ten-minute timeout, fixed output paths and no untrusted PR trigger or publishing action. |
| .NET SDK 10.0.302 | Forced shim restore with `NuGetAudit=true` and `NuGetAuditMode=all` passed. Explicit vulnerable/transitive package queries for production and both check projects returned no packages. All 11 restored asset graphs, including native/probe/check modes, contain no NuGet package libraries. |
| Dependency inputs | All ten project files and three shared property files were inspected. They use project/framework or local assembly references; Python scripts use the standard library. Explicit `NuGetAudit=false` settings were found and assessed, rather than treating normal compilation as an audit. |

The first production package query omitted `ReferenceMode` and could not locate its
mode-specific assets. Setting `ReferenceMode=Shim` resolved that command error;
the successful query and asset inventory establish the result. Portable scanners
were downloaded from their official releases with matching published SHA-256
digests; no scanner or new audit infrastructure was added to the repository.

The 14 local secret alerts comprised ten detections of five signed public-image
URLs in an ignored reference HTML cache (including decoded duplicates), and four
detections of the scanner's own documented examples in its ignored README/archive.
The URLs contain public access-key identifiers and per-object signatures, not an
AWS secret key or repository credential. The example alerts are not real secrets.
No blanket allowlist was added. The cache query strings can be removed without
changing the source fixture; diagnostic path handling remains covered by SB-R1.2.

Public action lockfiles were inventoried (24/79/56/156 runtime entries for checkout,
setup-dotnet, setup-python and upload-artifact). Their advisory query returned
upstream library alerts. The owner then explicitly instructed **“Ignore the upstream
advisories.”** Further upstream triage stopped; those alerts are excluded under
SB-D027, not claimed fixed or absent. Repository workflow controls remain assessed.
The external SDK, operating system, loader/game binaries and hosted infrastructure
were not internally audited. No private source or raw finding was sent to a service;
the dependency query sent only public action package names and versions.

Two delivery/documentation observations remain assigned to phase 4: bind the
supplied build revision to actual HEAD, and correct the historical probe guide's
claim that raw exception exports can never contain filesystem paths. Neither
establishes an exploitable production boundary. No new live test is indicated.

## SB-R3.2 — Security closure

The source scan was sealed successfully for its original `2d4c869` snapshot with
zero reportable findings and complete tracked-file coverage. Its generated report
and canonical JSON are retained locally under `artifacts/release-review/security-report/`.
The later assessment/state documentation changes were reviewed separately; they
do not retroactively change the scan's revision. The scan used the available
worker capacity after a ready preflight; the helper could not identify a six-slot
capacity, which was advisory and did not reduce completed file coverage.

No source vulnerability fix or dependency change was warranted. The explicit
NuGet audit override used in SB-R3.1 and the empty restored package graphs establish
the current dependency result despite the repository's normal audit opt-out.
Upstream advisories retain the owner's SB-D027 exclusion, with no false clean-bill
claim for those libraries.

The five unnecessary signed-image query strings were removed from the ignored
HTML cache while retaining the page and public image paths. Gitleaks rechecked
`artifacts/reference` successfully with no alerts. The scanner's own example
strings remain identifiable test documentation, not suppressed repository secrets.
No runtime behavior, native reference surface or shipped input changed. The
source-identity and probe-guide observations remain with their phase-4 stories;
there is no unresolved repository security finding or material source-coverage gap.

## SB-R4.1 — Delivery and DLL identity

The build now rejects a supplied revision that differs from the checkout's HEAD
before generating identity or compiling. A wrong-revision invocation failed and
left the existing DLL unchanged. Local dirty rehearsals remain explicitly marked;
the hosted record below confirms a clean checkout. Build 52/retry 2 retained
numeric version `0.1.52`; build 53 advanced to `0.1.53`. The diagnostic suffix came
from the actual full revision. Version semantics and workflow numbering did not change.

The metadata inspector now independently reads both the assembly file-version
attribute and the native PE version resource, alongside CLR assembly identity,
BepInPlugin GUID/name/version and informational revision. The negative suite first
accepts its original package, then rejects 15 mutations. Separate file-attribute
and file-resource corruptions exercise both checks; a shared fixture helper
requires one unambiguous byte marker and preserves PE layout. An initially ambiguous
resource marker was corrected after inspection showed distinct FileVersion and
Assembly Version fields. Failure logs confirmed the intended identity checks.

Shim and exact-target compilation passed with zero warnings/errors, the same
217 emitted references and all five native maps matching. No production behavior
or native reference declaration changed. Workflow review found no remaining mock
fallback or obsolete source-payload step; useful pins, timeout, read-only permissions
and separate artifact delivery were retained. actionlint passed. Compilation errors
propagate before archive construction; there is no prebuilt-DLL fallback.

[Hosted run 52, attempt 1](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34699887898)
passed all build, managed logic, independent geometry and malformed-package checks
for `df53e8b8fb02e150a71e4aa57c883b9b91247efd`. The actual downloaded artifact
`10300495368` was directly a 51,499-byte five-entry ZIP, with no wrapper or inner ZIP.
The separate build record identified `0.1.52.df53e8b` and a clean working tree.
Independent validation of those bytes passed, including both file-version fields
at `0.1.0.0` and loader GUID `dsp.spherebuilder`.

- ZIP SHA-256: `1352654105AB78F8E808383797F9A07A2585FBE8958AA3C169A595482DE276F8`.
- DLL SHA-256: `4F03E70985A84BC6DD0D38CE29947B78210A295D0E93572AEA19EE67A1FE8F61`.

This is delivery evidence. SB-R5.1 identifies the final candidate after the remaining
sanity pass; this intermediate package is not owner-accepted or published.

## SB-R4.2 — Sanity and code-quality review

The entry point, current-selection resolver, graph recognition, additive writer,
preservation check, feedback and package path were traced against the accepted
specification. The twelve compiled deltas and native continuation are implemented;
no executable placeholder, mock fallback or omitted MVP behavior was found.
Reference shims remain intentional compile-only declarations, and the historical
probe and archived plans remain clearly separated from production.

The error paths retain the original exception text/stack and operation context.
Painting stops after unexpected mutation/readback failures; successful refusals
remain usable. The UI boundary handles its own failures, detaches listeners/panel
references and respects the stopped session. There is no automatic retry, rollback,
catch/rethrow chain or repeated error logging to remove. No runtime change was
warranted. The passing run-52 managed checks exercise write counts/order, selected
targets, construction preservation, refusal/completion and partial-failure stopping.

Assertions were reviewed by their contract: GUIDs, format keys, fixture hashes,
numeric geometry, emitted references and legal text integrity are meaningful.
No exact player-prose or source-fragment assertion, arbitrary catch/rethrow ban,
planning instruction in production code, or additional compatibility framework was
found. The metadata fixture repetition introduced during SB-R4.1 was already
consolidated into its small marker helper. Existing comments explain rounding and
native snapshot behavior; routine methods do not need duplicated prose.

Two documentation corrections were made: the repository README now describes
refusal when a layout ceases to match a recognized stage, and the historical probe
guide acknowledges that exception details can contain filesystem paths. Neither
changes functionality or suppresses diagnostics. Active state remains in PROJECT;
archive introductions identify their original story language as historical.
The initial build command was verified present; an incomplete earlier tool display
did not justify editing it. Local documentation links and whitespace were checked.
No additional live case is needed for these corrections.

## SB-R5.1 — Final candidate verification

The owner packet identifies the immutable run-54 bytes. CI compiled the clean
`1c04199` source and passed managed recognition/painting checks, all twelve compiled
deltas and 32 faces (final 60 nodes / 90 frames), and all 15 malformed-package cases.
The final geometry comparison measured maximum direction error `6.72304398e-08`
and relative-radius error `7.07654325e-08`, within the established bounds.

The same clean revision and version compiled locally against both mapped shims and
the recorded native target with zero warnings/errors. All five maps matched. The
downloaded DLL's actual assembly/member reference set was compared directly with
the native build: all 217 agreed. Independent inspection verified the five ZIP
entries, GUID/name/numeric version, assembly/file versions, diagnostic revision,
hashes, UTF-8 copy, original 256×256 icon and both retained license texts. Installation
into isolated manual and manager-route plugin subfolders preserved the exact DLL
bytes; no live manager or game session was performed.

Anonymous retrieval of the package's full-commit source URL returned HTTP 200.
All 87 source-archive files matched the commit's Git blobs byte-for-byte, including
the required build/reference/derivation and license material. The earlier clean
source-build rehearsal establishes the documented checkout route. Keep this exact
public source revision accessible while distributing its binary.

Final Gitleaks checks found no secrets in the 54 reachable commits or the downloaded
candidate/build-record/log archives. A separate privacy recheck covered 199 tracked,
source-archive, hosted-output and local DLL/PDB entries for personal-contact/home-path
patterns and verified reachable author/committer attribution. PE CodeView paths in
the downloaded DLL and both local outputs used `/_/`; required provenance and image
metadata remained unchanged. Temporary upstream advisory inputs were discarded
after the owner's exclusion; no raw upstream finding was committed.

The security delta since SB-R3 was reviewed: only source-revision validation,
metadata inspection, malformed fixtures and documentation changed. No network,
privilege, runtime parser or dependency was added. The two documentation corrections
resolve overbroad claims; they do not suppress errors. Local links/anchors and
whitespace passed. All story findings are closed or have the explicit SB-D027
upstream exclusion; the later evidence/state commit does not change this candidate.

Comparison with the rewritten equivalent of the MVP cleanup commit found only
`src/Directory.Build.props` changed under production/reference directories, for
compiler path mapping. Production C# and native declarations were unchanged.
Accepted MVP observations therefore remain the runtime basis. No owner debugging,
repeated twelve-patch demonstration or additional UI workshop is required.

## SB-R5.2 — Owner acceptance and closeout

The owner's 2026-09-12 response explicitly accepted the identified candidate and
requested promotion to 1.0 for manual publication. The authoritative acceptance,
artifact identity and version decision are recorded in PROJECT.md under SB-D028.
The original roadmap was archived with relocated links and a historical header;
a short placeholder reserves the next discussion. No gameplay or publishing
operation formed part of this closeout.
