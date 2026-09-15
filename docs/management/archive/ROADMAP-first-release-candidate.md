# First Release Candidate Roadmap

> Historical plan, archived on 2026-09-12. It prescribed the scope and completion
> criteria below; its story language is retained as the original plan, not active
> work. [PROJECT.md](../../PROJECT.md) records execution, acceptance and closure.
> The [current roadmap](../ROADMAP.md) defines current work.

## Outcome and authority

Deliver the first release candidate of the accepted DSP Sphere Builder MVP: a
minimal, clearly described Thunderstore package, backed by repository security,
privacy, code and delivery checks. Finish with owner acceptance of an identified
download. The owner will handle actual release work manually after that approval.
This roadmap adds no features and does not authorize publication.

[PROJECT.md](../../PROJECT.md) alone owns accepted scope, execution state, story
status, gate results and decisions. This roadmap defines work and completion
criteria; authoring it does not execute its stories or accept a release candidate.
The [MVP specification](../../MVP-SPECIFICATION.md) remains the behavior contract.
The [implementation evidence](../../MVP-VALIDATION.md),
[implementation roadmap](../archive/ROADMAP-mvp-implementation.md) and
[feasibility roadmap](../archive/ROADMAP-feasibility-and-mvp-definition.md) supply the
starting evidence and history. The [build guide](../../BUILD.md) owns build procedures
and version translation.

## Scope and constraints

Included:

- Necessary package contents and their distribution obligations; short,
  player-facing manifest and README copy with both installation routes.
- Privacy review of the repository and its history, metadata and build outputs;
  removal of personal data without silently discarding required attribution.
- Security checks covering the whole repository, followed by bounded fixes and
  verification of confirmed findings.
- Build workflow, DLL identity/version and downloaded-package validation;
  completion and code-quality checks against the existing MVP contract.
- An attributable release candidate and one final owner acceptance session.

Out of scope:

- New features, geometry, controls, UI workshops, artwork, localization, broader
  game/mod support, changed native construction behavior or reopened MVP design.
- A new build system, general compatibility/recovery framework, permanent audit
  platform, broad dependency modernization or formatting-only rewrite.
- Agent-operated gameplay, early live probes, exhaustive playtesting and replaying
  the accepted W1–W6 gaps as a new owner checklist.
- Tags, GitHub releases, Thunderstore submissions, release credentials, portal
  setup, a publishing workflow or an unsolicited major/minor version change.
- Auditing unrelated repositories, upstream services or the owner's game saves.
  Destructive history rewrites and remote artifact deletion are not authorized
  by an ordinary commit/push instruction.

Keep the accepted twelve-click experience, **Paint Next Patch** caption, supplied
icon, player-filled shells and native continuation/failure behavior. Production
identity remains **`dsp.spherebuilder`**. Numeric package/loader versions keep the
existing `MAJOR.MINOR.github.run_number` contract; the commit suffix remains
diagnostic metadata. “Release candidate” identifies a reviewed artifact, not a
new version scheme. Preserve the workflow's build-number sequence.

W1–W6 retain their explicit accepted evidence limits. They are neither new runtime
results nor new security/privacy waivers. New contrary evidence must be resolved;
it cannot be hidden behind earlier owner acceptance. Passing local validation is
not proof of Thunderstore moderation acceptance.

## Execution, evidence and decisions

1. Start execution only after authorization is recorded in PROJECT.md. Work in
   the dependency order below. Close each story against its definition of done,
   with the appropriate PROJECT update in the same commit; push as authorized.
   Completion of this planning document is not implementation authorization.
2. Investigate unknowns in the earliest dependent story using the actual files,
   target metadata and primary documentation. Record material choices in
   PROJECT.md: question, evidence, alternatives, decision-maker, choice, rationale
   and consequences. Routine fixes do not need a separate decision entry.
3. Create `docs/RELEASE-CANDIDATE.md` when execution first produces findings.
   Keep concise evidence there: scope, source/artifact identity, commands and tool
   versions, results, finding dispositions, limitations and reproducible checks.
   Link to existing evidence and reference maps instead of copying them. Keep
   status and steering decisions only in PROJECT.md; create no empty report now.
4. Keep private/raw findings out of committed reports and CI logs. Use sanitized
   locators and results; never reproduce discovered personal data or secrets as
   examples or test fixtures. Temporary tools and raw diagnostics stay local in
   ignored `artifacts/`, subject to the privacy cleanup in SB-R1.2 and SB-R5.1.
5. Reconfirm the [recorded target](../../PROJECT.md#target-game-reference) before
   compilation. Use mapped compile-only shims in CI and real references locally.
   Each commit changing a referenced surface must include its native type/member
   mapping and affected checks. Do not ship dependency/shim binaries or infer
   runtime behavior from compilation. A changed target is an unresolved baseline
   question, not permission to silently retarget.
6. Complete all agent work before the single owner handoff in SB-R5.2. Earlier
   stories require no copy approval, workshop, installation trial or live probe.
   Reuse accepted MVP observations for unchanged behavior. Any necessary runtime
   recheck belongs in that final session and addresses only a concrete change or
   contrary observation; never substitute static inspection for a live result.
7. Select checks for real contracts and failure cases. Exact GUIDs, version fields,
   file identities and format keys can be legitimate assertions; exact prose,
   source fragments and bans on language constructs are not product tests.
   Diagnose failures and follow AGENTS.md's bounded retry rule. Passing checks
   are repeated only when changed inputs or an unresolved concern warrant it.
8. Fix routine findings within scope using evidence and outcome-driven tradeoffs.
   If a finding requires substantial redesign, unavailable essential evidence,
   or an unauthorized destructive action, stop that affected path and record a
   precise blocker. Continue independent work without claiming its gate passed.
   Do not hide a large remediation inside one story: split it before execution
   if necessary, retaining this roadmap's boundaries and dependencies.

Update BUILD.md when a build/package contract changes, the package README when
player guidance changes, and the root README when its pointers or claims change.
Preserve archived work as history; historical instructions and intentional
compile-only shims are not unfinished product features.

## Phases, milestones and gates

| Phase / epic | Milestone | Exit gate |
| --- | --- | --- |
| 1. Release inputs — SB-R1 | RM1: Distribution and privacy requirements resolved | RG1: Necessary-file contract justified; repository privacy findings resolved without an unapproved exception |
| 2. Player-facing package — SB-R2 | RM2: Minimal package with clear player copy | RG2: ZIP contents match the contract; description, README and both installation routes checked |
| 3. Repository security — SB-R3 | RM3: Security review and remediation complete | RG3: Whole-repository coverage recorded; confirmed findings resolved and checked |
| 4. Delivery and code quality — SB-R4 | RM4: Reviewed delivery path and complete MVP | RG4: CI/metadata contracts verified; sanity and code-quality findings closed |
| 5. Release candidate — SB-R5 | RM5a: Verified candidate ready for review; RM5: Owner-accepted first release candidate | RG5a: Final downloaded bytes and review packet validated; RG5: Explicit owner acceptance of that candidate recorded |

Each phase enters after the preceding gate. RG1–RG4 and RG5a require agent
evidence, not owner participation. SB-R5.2 starts only after RG5a; RG5 cannot pass
without the owner's explicit acceptance. Gate results belong in PROJECT.md.

## Phase 1 — Epic SB-R1: Resolve release inputs

**Return:** A defensible definition of necessary package files and a repository
that can supply the candidate without leaking personal data.

### SB-R1.1 — Establish the minimum distribution contract

**Story:** As a maintainer, I want the required payload and distribution terms
established before trimming files so the smaller package remains usable and compliant.

**Depends on:** Execution authorization in PROJECT.md.

**Scope:** Inspect the current package inventory, retained third-party material,
licenses and primary Thunderstore/DSP installation rules.

**Definition of done:**

- Every current package entry has a supported purpose or a removal disposition.
  Define the expected root files, single production DLL and any necessary legal
  or source material independently of what the existing packager happens to emit.
- Resolve the current `source/` distribution choice against the actual licenses
  and derived geometry inputs. Verify any proposed external source-access route
  and its revision/availability obligations before relying on it. Do not assume
  all source is mandatory, all source is removable, or a link alone is sufficient.
- Check primary manifest, README/icon, archive and game-specific mod-manager
  routing requirements. Record sources and inspection dates. Separate published
  requirements, this project's contracts and any unverified moderation claims.
- Record the proposed inventory and source/attribution arrangement in the evidence,
  with the choice and rationale in PROJECT.md. Keep BUILD.md's current contract
  accurate until SB-R2.1 implements the replacement. Essential distribution
  unknowns are resolved before SB-R2.1; no guessed legal conclusion.

**Produces:** A reviewed distribution contract and removal list.

**Excluded:** Replacing the geometry, relicensing third-party material, implementing
package changes, public submission or asking the owner to choose routine layout details.

### SB-R1.2 — Remove personal data from release inputs

**Story:** As a repository owner, I want personal data found and removed before
release preparation so neither the repository nor its outputs expose it.

**Depends on:** SB-R1.1.

**Scope:** Repository files, reachable Git history and metadata, and existing
repository-related build artifacts/logs; include ignored and untracked local files.

**Definition of done:**

- Inventory and inspect text, fixtures, archives, image metadata/content, binaries
  and debug/source paths, Git author/committer metadata and reachable history.
  Inspect accessible hosted build logs/artifacts and record the coverage boundary.
  An extension filter or current-tree search alone is not a repository privacy pass.
- Classify actual matches without printing them into committed evidence. Distinguish
  project identifiers from personal data; do not assume public attribution or a
  Git identity is exempt. Resolve any attribution conflict against SB-R1.1 rather
  than silently dropping required notices or waiving the privacy requirement.
- Sanitize or remove confirmed personal data within the authorized scope while
  retaining useful, accurately described evidence. Correct generating paths or
  metadata defaults so fresh outputs do not immediately restore the same data.
- Recheck affected surfaces. If history or hosted remnants require an unauthorized
  rewrite/deletion, or access prevents coverage, record the exact remaining surface
  and required remedy without claiming RG1 passed. Never force-push or copy the
  offending values into the blocker report.

**Produces:** Sanitized release inputs and an evidence-based privacy disposition.

**Excluded:** Unrelated machine cleanup, deleting game saves, automated history
rewriting, loss of required attribution or a new permanent privacy-scanning platform.

## Phase 2 — Epic SB-R2: Prepare the player-facing package

**Return:** A small, installable package whose public copy explains the benefit
and gets players started without exposing development details.

### SB-R2.1 — Package only necessary files

**Story:** As a player, I want a download containing only what belongs with the mod
so installation is straightforward and development material is not bundled by default.

**Depends on:** RG1.

**Scope:** Package assembly, inventory validation and installation destinations.

**Definition of done:**

- Implement SB-R1.1's inventory. Retain the supplied icon and required notices;
  remove unnecessary source/tooling/evidence material without breaking the agreed
  source-access arrangement. Keep build diagnostics outside the installable ZIP
  and update BUILD.md to the implemented distribution contract.
- A fresh package contains exactly the justified entries and one production DLL.
  No dependency/shim/probe binaries, stale payload, wrapper directory or nested ZIP
  is present. Validate actual entry names and bytes, not just staging-directory intent.
- Adjust inventory checks and meaningful failure cases together. Reject unexpected
  payload and missing required material; replace obsolete source-specific checks
  using the new contract. Review the expected inventory independently of the builder
  so both cannot agree on an unintended extra file and call that validation.
- Verify the archive's paths against DSP's mod-manager routing and a manual copy
  into an existing `BepInEx/plugins/DSPSphereBuilder` subfolder using an isolated
  filesystem rehearsal. No game launch or change to the installed mod is needed.

**Produces:** Minimal package assembly with checked contents and destinations.

**Excluded:** A new installer, source-distribution service, icon redesign, feature
changes or treating a filesystem rehearsal as a live mod-manager/game observation.

### SB-R2.2 — Write concise player-facing copy

**Story:** As a player, I want to understand the mod's benefit and how to start
using it from a short listing and README.

**Depends on:** SB-R2.1.

**Scope:** Manifest description and packaged README, with only necessary alignment
of the repository README and source/credit pointers.

**Definition of done:**

- The manifest description is short, benefit-led prose suitable for a link preview,
  within Thunderstore's 250-character limit. It contains no implementation status,
  build jargon, instruction sequence or unsupported optimality claim.
- The README briefly describes the experience and naturally explains selecting a
  suitable layer, using **Paint Next Patch**, expanding at the player's pace and
  filling shells themselves. Include essential player prerequisites and limitations
  accurately without turning native internals or the test matrix into a manual.
- Provide concise mod-manager installation guidance grounded in its actual workflow,
  and manual installation into an existing BepInEx plugins subfolder. Distinguish
  installing the mod from installing BepInEx; name the file/destination plainly.
  Do not imply that a public listing already exists. Keep candidate-only download
  logistics in the final handoff rather than the public product description.
- Check UTF-8, links and rendered Thunderstore Markdown, including the installation
  passages and any required credit/source link. Review clarity and factual accuracy
  editorially; add no exact-copy assertions, arbitrary prose quotas or owner copy review.

**Produces:** Final proposed listing description and package README.

**Excluded:** Marketing pages, screenshots, translations, extensive troubleshooting,
new support claims, a user manual or copying another mod's product language.

## Phase 3 — Epic SB-R3: Check repository security

**Return:** Security evidence covering the whole repository, with confirmed
problems fixed and limitations visible before candidate preparation.

### SB-R3.1 — Assess the entire repository

**Story:** As a maintainer, I want security checks across the code and delivery
path so a clean package does not conceal an unchecked repository risk.

**Depends on:** RG2.

**Scope:** All tracked source and retained third-party/probe material, checks,
scripts, workflows, configuration, documentation, dependency inputs and secret exposure.

**Definition of done:**

- Establish the reviewed revision and complete file/dependency inventory. Cover
  production and historical/non-shipping code, build/test tools, action pins and
  permissions, input handling, file/archive operations and sensitive-data paths.
  Secret checks include reachable history and generated outputs; reuse SB-R1.2
  evidence where applicable rather than maintaining competing inventories.
- Run suitable source, secret, direct/transitive dependency and workflow checks,
  supplemented by focused manual review of trust boundaries. Use available tools
  where sufficient; record tool versions, commands, advisory date and coverage.
  Ordinary compilation is not a vulnerability audit: explicitly inspect the current
  NuGet audit settings and the actual restored dependency graph.
- Triage findings against reachable behavior and primary advisories. Record concise,
  sanitized evidence, affected paths, impact and a proposed bounded remedy. Support
  false-positive/not-applicable dispositions with evidence, not blanket suppression.
- Identify unavailable checks and opaque external inputs accurately. A scanner's
  success or zero findings cannot stand in for unexamined repository areas. Do not
  send private repository contents or raw findings to external services by default.

**Produces:** A complete coverage record and actionable findings for SB-R3.2.

**Excluded:** Auditing the game's implementation or upstream infrastructure,
penetration testing, dependency upgrades before triage or a recurring scan programme.

### SB-R3.2 — Resolve confirmed security findings

**Story:** As a player and maintainer, I want confirmed risks removed without
unrelated rewrites so the release candidate retains the accepted behavior.

**Depends on:** SB-R3.1.

**Scope:** Evidence-backed fixes and verification of the findings from SB-R3.1.

**Definition of done:**

- Fix confirmed applicable findings with the smallest sufficient change. Preserve
  native behavior and package contracts; justify any dependency/action change using
  the finding and verified compatibility, not a general preference for newer versions.
- Verify each fix with the affected check and a meaningful regression case where
  needed. Re-run applicable security checks after changes and record dispositions.
  Do not hide findings through disabled checks or unsupported risk acceptance.
- Close coverage gaps needed for the repository pass. RG3 requires no unresolved
  confirmed finding or material unassessed area; an unavailable essential check
  blocks the gate. If no fixes are warranted, record that result without code churn.
- Record any remaining targeted runtime question for the final session only. A
  necessary substantial redesign is a blocker, not scope silently added to this story.

**Produces:** Verified remediation and a substantiated repository security result.

**Excluded:** Broad hardening, speculative guards, automatic recovery, a new audit
framework, unrelated cleanup or early owner testing.

## Phase 4 — Epic SB-R4: Review delivery and code quality

**Return:** A dependable existing workflow and an MVP with no overlooked delivery
placeholders, redundant scaffolding or arbitrary validation restrictions.

### SB-R4.1 — Verify the release delivery and DLL identity

**Story:** As a maintainer, I want CI to produce an attributable, correctly identified
mod package so the owner can trust the downloaded candidate.

**Depends on:** RG3.

**Scope:** The existing build workflow, compilation, version translation and
positive/negative package validation.

**Definition of done:**

- Review `build.yaml` and its scripts for mock-era, stale inventory or redundant
  steps. Remove only demonstrated relics. Preserve useful pinned actions, limited
  permissions, bounded failures and offline checks; add no publishing automation.
- Confirm clean source is compiled for the package and failure cannot fall back to
  a previous DLL. Verify sequential numbering, same-run retry behavior and diagnostic
  commit identity using the existing version contract and focused checks.
- Read the actual PE metadata without loading the plugin: BepInEx GUID, name and
  numeric version, CLR assembly version, file version and informational revision
  all match BUILD.md and the build record. Do not infer file-version correctness
  solely from project properties or assembly version. Compare mapped shim and real
  target builds, including emitted references; never bundle the reference assemblies.
- Exercise valid and corrupted packages for meaningful identity, payload and layout
  failures. Every rejection has a format, security or project-contract reason;
  remove obsolete rules without weakening those contracts. Keep player prose out
  of semantic assertions.
- Inspect an actual hosted download and matching build record. The artifact is
  directly the usable ZIP, with root metadata and no same-name inner ZIP. Build
  evidence remains separate; record the run, attempt, commit, versions and hashes.

**Produces:** Reviewed CI delivery and independently checked DLL/package identity.

**Excluded:** New version semantics, workflow sequence resets, release credentials,
publication, speculative runtime compatibility gates or a new packaging framework.

### SB-R4.2 — Close implementation and agentic code-quality gaps

**Story:** As a maintainer, I want a final contract-based sanity pass so unfinished
paths and unnecessary generated complexity do not reach the candidate.

**Depends on:** SB-R4.1.

**Scope:** The current repository's implementation, checks and supporting docs,
with bounded corrections to concrete completion and maintainability problems.

**Definition of done:**

- Trace the promised MVP action, production entry point and delivery path against
  the accepted specification and evidence. Resolve any executable placeholder,
  omitted implementation or obsolete mock fallback. Distinguish intentional shims,
  archived stories and historical probes from missing production work.
- Review for source/prose assertions, redundant defensive layers, copied planning
  language, missing explanations of non-obvious behavior, inconsistent local style
  and duplicated documentation. Make focused corrections; keep identifiers, format
  contracts and useful technical comments. No blanket comment or assertion removal.
- Inspect error paths for swallowed failures, lost original stacks, needless
  catch/rethrow layers and repeated logging/retry behavior. Preserve the specified
  session stop and diagnostics after partial mutation. Neither rethrowing nor any
  other language construct is prohibited by an invented package-validation rule.
- Reconcile active docs and code claims with actual behavior; leave PROJECT.md as
  state authority and archives clearly historical. Run affected behavioral/build
  checks and revisit relevant security/privacy findings when changed inputs warrant
  it. Record a no-change result where the code already meets the contract.

**Produces:** Closed sanity/code-quality findings and consistent supporting docs.

**Excluded:** New product behavior, exhaustive refactoring, broad formatting,
documentation proliferation, arbitrary style tests or a new UI review cycle.

## Phase 5 — Epic SB-R5: Deliver the first release candidate

**Return:** One fully identified, reviewed package for an efficient owner decision,
followed by a clear boundary between candidate acceptance and manual release.

### SB-R5.1 — Validate and identify the final candidate

**Story:** As the owner, I want a finished candidate with concise, verifiable evidence
so acceptance does not require completing the agent's checks myself.

**Depends on:** RG4.

**Scope:** Final clean build, downloaded artifact, changed-input rechecks and the
complete owner review packet. This story requires no owner participation.

**Definition of done:**

- Build from a clean, identified revision after all preceding fixes. Obtain the
  actual hosted ZIP and matching build information; record full commit, run/attempt,
  numeric version, diagnostic identity and ZIP/DLL hashes. Confirm local native
  compilation and mapped references for that source, separately from CI compilation.
- Independently inspect those downloaded bytes against the distribution contract,
  DLL metadata and expected hashes. Recheck final copy, icon, legal/source access
  and installation destinations. A local ZIP or green CI badge alone is insufficient.
- Check the final repository revision, outputs and hosted logs/artifacts for privacy
  regressions, including binary/debug metadata. Complete the security delta review
  for changes since RG3 and all affected checks. Reference unchanged passing evidence;
  do not rerun unrelated suites merely to create a fresh timestamp.
- Reconcile every story's findings and specification-impact assessment. Distinguish
  accepted MVP runtime observations from new offline checks. If a change needs live
  confirmation, prepare only its shortest necessary case for SB-R5.2; do not quietly
  accept missing evidence or resurrect the full earlier test matrix.
- Complete a short review packet in RELEASE-CANDIDATE.md: exact download and hashes,
  listing/README preview, installation guidance, check results, changes since the
  accepted MVP and precise remaining evidence limits. RG5a requires all agent work
  complete, no unresolved security/privacy/distribution blocker and only the final
  owner decision or explicitly identified targeted runtime check remaining.

**Produces:** An immutable candidate identity and complete owner acceptance packet.

**Excluded:** Publishing, game operation, a new owner test campaign or calling the
candidate approved before an explicit decision.

### SB-R5.2 — Obtain owner acceptance of the release candidate

**Story:** As the owner, I want one focused final review so I can accept the candidate
and then handle its release manually.

**Depends on:** SB-R5.1 and RG5a.

**Scope:** The only planned owner session: review the prepared candidate and resolve
any evidence-based targeted runtime check identified in the packet.

**Definition of done:**

- Present the finished packet and request acceptance of its identified ZIP. The
  owner reviews the product copy and package, with no earlier copy approvals,
  installation troubleshooting assignment or repeated twelve-patch demonstration.
  Runtime checking, if justified by a specific change, is confined to that change.
- Resolve owner corrections within the polishing boundary. Revalidate affected
  checks and rebuild if shipped bytes change; record the replacement identity and
  obtain acceptance of that candidate. Do not widen scope or enter an unbounded
  revision loop. A substantial new request receives a separate scope decision.
- Record explicit owner acceptance and its artifact identity in PROJECT.md before
  closing RG5/RM5. Silence, a successful build or acceptance of an older artifact
  does not pass the gate. Documentation-only follow-up commits must not relabel
  their automatically built packages as the accepted candidate.
- Close and archive this roadmap with working relative links; leave a short
  placeholder for the next owner discussion. Record that actual release work remains
  manual and owner-operated. Candidate acceptance alone authorizes no tag, public
  release, Thunderstore upload, release credential use or new feature work by the agent.

**Produces:** Owner-accepted first release candidate and a closed planning boundary.

**Excluded:** Performing the release, creating a publishing roadmap by assumption,
further polishing after acceptance or a new runtime/support matrix.

## Coverage and source references

| Requested outcome | Owning work |
| --- | --- |
| Necessary Thunderstore contents only | SB-R1.1, SB-R2.1; final bytes in SB-R5.1 |
| Short, benefit-led manifest description | SB-R2.2 |
| Player README and both installation routes | SB-R2.1–SB-R2.2 |
| No PII in repository or outputs | SB-R1.2; final regression check in SB-R5.1 |
| Whole-repository security checks | SB-R3.1–SB-R3.2; changed-input closure in SB-R5.1 |
| No overlooked implementation placeholders | SB-R4.2 |
| Clean pipeline and correct DLL identity/version | SB-R4.1; final hosted download in SB-R5.1 |
| Final agentic code-smell and validation-rule review | SB-R4.2, with validation rules in SB-R4.1 |
| First release candidate and owner acceptance | SB-R5.1–SB-R5.2; publication remains outside the roadmap |

Primary sources inspected during drafting on 2026-09-12:

- [Thunderstore package requirements](https://wiki.thunderstore.io/mods/creating-a-package):
  root metadata, icon/README format, description limit and numeric version format.
- [Thunderstore package routing](https://wiki.thunderstore.io/mods/packaging-your-mods)
  and [DSP installation rules](https://github.com/thunderstore-io/ecosystem-schema/blob/master/games/data/generated/dyson-sphere-program.yml):
  the game's BepInEx plugin route. Refresh relevant rules during SB-R1.1.
- The actual [repository license](../../../LICENSE),
  [third-party license](../../../research/cosmin1490/LICENSE) and
  [retained input provenance](../../../research/cosmin1490/README.md): inputs to the
  distribution investigation, not a pre-decided justification for the current ZIP.
- [Current build/version contract](../../BUILD.md),
  [package inventory](../../../scripts/Get-PackageInputs.ps1) and
  [reference map](../../../references/README.md): local implementation evidence;
  their existence does not itself establish release readiness.
