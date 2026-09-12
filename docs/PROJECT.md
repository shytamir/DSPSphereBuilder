# Project steering and state

## Authority

This is the sole authoritative record of accepted scope and steering decisions,
current phase, epic/story status, readiness, and owner acceptance. Update those
facts here only. Current owner instructions take precedence.

The [concept](../CONCEPT.md) describes the product behavior and reference geometry.
The [release-candidate roadmap](management/ROADMAP.md) defines the authorized
polishing work and completion criteria.
Archived roadmaps retain their original work and completion criteria. The
[MVP specification](MVP-SPECIFICATION.md) defines the
implementation contract and acceptance cases. The [README](../README.md)
introduces the project, the
[build guide](BUILD.md) defines build procedures, and [AGENTS.md](../AGENTS.md)
governs agent conduct. These documents link here for state.

## Current phase

The MVP implementation roadmap is complete. The owner accepted the functional
MVP, the UI workshop refinement and the label-only correction; final reconciliation
passed under SB-D021. The [implementation plan](management/archive/ROADMAP-mvp-implementation.md)
and [feasibility plan](management/archive/ROADMAP-feasibility-and-mvp-definition.md)
are archived. The management/code hygiene passes and final cleanup package
inspection were completed before this planning round. The owner authorized
sequential execution under SB-D023 and the necessary history rewrite under SB-D026.
SB-R1.1–5.1 are complete. RG1/RM1–RG4/RM4 and RG5a/RM5a passed under SB-D027.
SB-R5.2 awaits the owner's acceptance of candidate **0.1.54**, identified in the
[review packet](RELEASE-CANDIDATE.md#owner-review-packet--0154). Candidate acceptance
and manual publication remain separate; neither has occurred.

| Area | Current state |
| --- | --- |
| Product concept | Agreed; recorded in [CONCEPT.md](../CONCEPT.md) |
| Repository preparation | Complete; local checks and hosted artifact inspection passed |
| Roadmap and active work | All agent work complete; SB-R5.2 owner candidate acceptance pending |
| Feasibility gates and MVP specification | G1/M1 and G2/M2 passed; G3/M3 accepted under SB-D008 with documented unverified cases; SB-F4.2 complete and G4/M4 achieved by explicit specification acceptance under SB-D010 |
| Implementation gates | IG1/IM1 through IG5/IM5 passed, including IG5a/IM5a and the owner UI workshop |
| Release-candidate gates | RG1/RM1 through RG4/RM4 and RG5a/RM5a passed; RG5/RM5 awaits explicit owner acceptance |
| Mod implementation | Working MVP retained in candidate 0.1.54; production C# and native declarations unchanged from the MVP cleanup |
| Runtime validation and owner acceptance | Owner accepted MVP 0.1.31 and the 0.1.36 UI recheck; capitalization correction accepted without another live recheck under SB-D020 |
| Distribution | [Candidate 0.1.54, run 54 / attempt 1](RELEASE-CANDIDATE.md#owner-review-packet--0154) independently verified; acceptance pending, no publication authorized |

## Accepted scope

The accepted product baseline is the [concept](../CONCEPT.md) and the
[MVP specification](MVP-SPECIFICATION.md), including the
twelve-click polar progression, missing connections plus one leading spoke,
preservation of existing construction, and player-managed shells. Future changes
to that baseline require a steering decision here and a corresponding update to
the concept where its behavior changes.

The implementation roadmap delivered a working MVP and a real, directly usable CI
mod package. The owner requested a polishing roadmap whose sole output is the
first accepted release candidate: minimal package contents, player copy, privacy,
repository security, delivery validation and a final sanity/code-quality pass.
No features are added. The only planned owner participation is the final candidate
session after the agent's work is complete. Actual release work will be manual
and owner-operated after acceptance; no publishing automation is included.
The production GUID is `dsp.spherebuilder`. CI uses mapped compile-only reference shims; every commit
adding or changing a referenced surface must carry the corresponding native
type/member mapping and checks. Local real-reference compilation remains required.

Human and game runtime validation began together before the UI workshop. The
short sessions reused accepted feasibility evidence; W1–W6 were not reopened as
a test matrix. Agreed refinements and the targeted recheck preceded final MVP
acceptance. Phase 4 replaced mock delivery with the real package and supplied
icon. The requested MVP closeout cleanup is complete. SB-D023 authorizes executing
the polishing plan and pushing each completed story with its state update.
Accepted behavior and W1–W6 evidence limits remain unchanged; publication is excluded.

## Target game reference

The local `DSPGAME_Data/Managed/Assembly-CSharp.dll` was inspected read-only on
2026-09-12 while authoring the roadmap:

- Size: **7,830,016 bytes**.
- SHA-256: `AE0BA95F75BD879A62AA4CE253B2AB78EAA4FB3C7C595F5E1FEE75EBE0E0EF85`.
- File version and product version: both `0.0.0.0`; neither establishes the
  player-facing game-build number.

This hash identifies the initial target, not a claim of game compatibility.
SB-F1.1 reconfirmed it. Managed MVID is
`ece4a40e-5e73-43f4-a9f8-4e74970b5942`; GameConfig declares `0.10.34`, while the
complete build suffix remains unknown. See [the evidence](FEASIBILITY.md#sb-f11--target-and-probe-environment).
The sample header is not a substitute for this target. Live gameplay observations
are owner-operated; the agent has not launched or operated a game session.

## Work tracking

### First release-candidate roadmap

| Story | Execution state | Evidence |
| --- | --- | --- |
| SB-R1.1 | Complete | [Minimum distribution contract](RELEASE-CANDIDATE.md#sb-r11--minimum-distribution-contract), SB-D024 |
| SB-R1.2 | Complete; RG1/RM1 passed | [Authorized privacy cleanup and checks](RELEASE-CANDIDATE.md#authorized-cleanup-and-checks), SB-D026 |
| SB-R2.1 | Complete | [Minimal package and path checks](RELEASE-CANDIDATE.md#sb-r21--minimal-package-and-installation-paths) |
| SB-R2.2 | Complete; RG2/RM2 passed | [Player copy and rendered preview](RELEASE-CANDIDATE.md#sb-r22--player-facing-description-and-readme) |
| SB-R3.1 | Complete | [Repository security assessment](RELEASE-CANDIDATE.md#sb-r31--repository-security-assessment), SB-D027 |
| SB-R3.2 | Complete; RG3/RM3 passed under SB-D027 | [Security closure](RELEASE-CANDIDATE.md#sb-r32--security-closure) |
| SB-R4.1 | Complete | [Delivery and DLL identity](RELEASE-CANDIDATE.md#sb-r41--delivery-and-dll-identity) |
| SB-R4.2 | Complete; RG4/RM4 passed | [Sanity and code-quality review](RELEASE-CANDIDATE.md#sb-r42--sanity-and-code-quality-review) |
| SB-R5.1 | Complete; RG5a/RM5a passed | [Final candidate verification](RELEASE-CANDIDATE.md#sb-r51--final-candidate-verification) |
| SB-R5.2 | Awaiting owner acceptance of 0.1.54 | [Owner review packet](RELEASE-CANDIDATE.md#owner-review-packet--0154); no additional runtime case indicated |

### Completed MVP implementation roadmap

| Story | Execution state | Evidence |
| --- | --- | --- |
| SB-I1.1 | Complete | [Mapped references and delivery inputs](MVP-VALIDATION.md#sb-i11--mapped-references-and-delivery-inputs) |
| SB-I1.2 | Complete | [Local and hosted production compilation](MVP-VALIDATION.md#sb-i12--production-compilation-and-identity) |
| SB-I2.1 | Complete | [Compiled production plan](MVP-VALIDATION.md#sb-i21--compiled-production-plan) |
| SB-I2.2 | Complete | [Native graph recognition](MVP-VALIDATION.md#sb-i22--native-graph-recognition) |
| SB-I3.1 | Complete | [Additive operation and failure boundary](MVP-VALIDATION.md#sb-i31--additive-operation-and-failure-boundary) |
| SB-I3.2 | Complete | [Current-target editor control](MVP-VALIDATION.md#sb-i32--current-target-editor-control) |
| SB-I4.1 | Complete | [Executable package and validator](MVP-VALIDATION.md#sb-i41--executable-package-and-validator) |
| SB-I4.2 | Complete | [Hosted download inspection](MVP-VALIDATION.md#sb-i42--hosted-transport-checks) |
| SB-I5.1 | Complete under SB-D018; IG5a/IM5a passed | [Owner evidence review](MVP-VALIDATION.md#sb-i51--owner-evidence-review) |
| SB-I5.2 | Complete | [Owner workshop](MVP-VALIDATION.md#sb-i52--owner-ui-workshop), SB-D019 |
| SB-I5.3 | Complete under SB-D020 | [Owner refinement evidence](MVP-VALIDATION.md#owner-refinement-evidence) |
| SB-I5.4 | Complete; IG5/IM5 passed under SB-D021 | [Final reconciliation](MVP-VALIDATION.md#sb-i54--final-mvp-reconciliation) |

### Completed feasibility roadmap

| Story | Execution state | Evidence |
| --- | --- | --- |
| SB-F1.1 | Complete | [Target and probe environment](FEASIBILITY.md#sb-f11--target-and-probe-environment) |
| SB-F1.2 | Complete | [Native placement and lifecycle](FEASIBILITY.md#sb-f12--native-placement-and-lifecycle) |
| SB-F2.1 | Complete | [Canonical reference geometry](FEASIBILITY.md#sb-f21--canonical-reference-geometry) |
| SB-F2.2 | Complete | [Polar twelve-patch traversal](FEASIBILITY.md#sb-f22--polar-twelve-patch-traversal) |
| SB-F2.3 | Complete | [Native placement envelope](FEASIBILITY.md#sb-f23--native-placement-envelope) |
| SB-F3.1 | Complete | [Completed-shell retest](FEASIBILITY.md#completed-shell-retest), [earlier additive and rejection results](FEASIBILITY.md#second-owner-run-additive-results-and-case-b-gap) |
| SB-F3.2 | Complete; full process restart/removal remains unverified and accepted under SB-D008 | [Owner continuation run](FEASIBILITY.md#owner-continuation-run), SB-D007–008 |
| SB-F3.3 | Complete under SB-D008; remaining live cases accepted without execution | [Full workflow and limits](FEASIBILITY.md#sb-f33--full-workflow-evidence-and-accepted-limits) |
| SB-F4.1 | Complete | [Constraint reconciliation](FEASIBILITY.md#sb-f41--mvp-constraint-reconciliation), SB-D009 |
| SB-F4.2 | Complete; owner accepted the specification under SB-D010 | [MVP specification](MVP-SPECIFICATION.md), [review record](FEASIBILITY.md#sb-f42--specification-review) |

Record story progress, gate/milestone outcomes, and concise evidence links
here. Keep purpose, scope, dependencies, and completion criteria in the roadmap.
Do not maintain a second status table there or in the README.

Technical validation, owner acceptance, and publication are separate facts. A
passing package build establishes the packaging path, not gameplay behavior
or Thunderstore moderation acceptance.

## Decision record

These dated decisions retain the choices and evidence available at the time.
Later decisions supersede earlier phase boundaries; the current state is above.
Historical instructions in a decision are not a new request to execute that work.

### SB-D001 — Feasibility before implementation planning

- **Date / decision-maker:** 2026-09-12, owner instruction.
- **Question and choice:** The first roadmap was to establish feasibility and MVP
  constraints, ending with a full specification backed by evidence and recorded
  decisions. Planning the playable implementation follows in a separate roadmap.
- **Basis:** The owner's roadmap request and the outstanding verification in
  [CONCEPT.md](../CONCEPT.md#validation-boundary).
- **Alternatives:** None evaluated; this is the requested phase boundary, not an
  inferred technical tradeoff.
- **Consequence:** Probe work must answer feasibility questions without becoming
  production implementation. Publishing this plan does not activate that work.
- **Affected contract:** Roadmap scope and G4/M4; the future MVP specification's
  evidence and implementation-planning handoff.

### SB-D002 — Execute sequentially; owner-operated live observations

- **Date / decision-maker:** 2026-09-12, owner instruction and live-probe reply.
- **Choice:** Execute the roadmap in order, push each completed story with its
  state update, and stop on a substantial blocker. The owner runs the eventual
  probe in a disposable test save; the agent prepares the evidence and artifact.
- **Alternatives:** Agent-operated gameplay was not selected. Offline checks alone
  cannot satisfy the roadmap's live-observation gate.
- **Basis:** Current owner authorization; SB-F1.1 confirmed the local static
  inspection inputs and tools.
- **Consequence:** No repeated permission request for authorized repository work.
  Runtime conclusions wait for matching owner observations. Technical choices
  favour native behavior and the minimum mechanism needed by the concept.
- **Affected contract:** Execution rules, SB-F3, and G3. This supersedes SB-D001's
  planning-only execution boundary, not its specification-first outcome.

### SB-D003 — Use the native additive path; preserve its validation boundary

- **Date / decision-maker:** 2026-09-12, implementor within authorized scope.
- **Question / choice:** Investigate direct native node/frame creation for the
  probe; do not use blueprint replacement to extend a layer.
- **Alternatives and evidence:** SB-F1.2 found individual native allocation and
  bookkeeping methods; blueprint import instead resets the layer pools.
- **Rationale / consequence:** Native creation avoids replacing existing records.
  Its availability is not proof of live preservation, and its constructors omit
  editor geometry/unlock checks. SB-F2.3 must establish those constraints before
  mutation; SB-F3 must establish behavior. No compatibility gate or custom
  allocator is introduced.
- **Affected contract:** SB-F2.3, SB-F3.1, and future MVP placement/preservation.

### SB-D004 — Attributed research fixture and explicit precision comparison

- **Date / decision-maker:** 2026-09-12, implementor within authorized scope.
- **Question / choice:** Use the author's pinned GPL-3.0 `60.txt` as reproducible
  research input, retaining its license and attribution separately from original
  Apache-2.0 work. Keep the published sphere as the comparison baseline.
- **Alternatives and evidence:** The published file has no independently
  established redistribution grant. SB-F2.1 measured identical topology and a
  maximum normalized-coordinate difference of 2.545625 × 10⁻⁸ for the licensed
  repository counterpart, below the derived encoding bound.
- **Rationale / consequence:** This supplies reviewable data without copying
  generator implementation or silently substituting an equal-edge football.
  Comparisons preserve the measured precision distinction. The fixture stays out
  of the mock package; a later artifact using it must preserve applicable terms.
- **Affected contract:** SB-F2 derivation, probe inputs, and future specification
  geometry/provenance and distribution requirements.

### SB-D005 — One fixed polar orientation and route

- **Date / decision-maker:** 2026-09-12, implementor within authorized scope.
- **Question / choice:** Use the P1-to-P4 rotation and increasing-azimuth ring
  traversal derived in SB-F2.2, with canonical IDs resolving equivalent starts.
- **Alternatives / evidence:** Symmetry permits other starting caps and ring
  directions. This route satisfies the agreed 1–5–5–1 order, adjacent transitions,
  leading-spoke rule, and complete final graph in all twelve checked deltas.
- **Rationale / consequence:** One deterministic route is sufficient. Orientation
  selection and alternate routes add no required feasibility capability.
- **Affected contract:** Future specification's geometry, click sequence, and
  continuation matching; SB-F2.3 evaluates this fixed orientation's unlock limits.

### SB-D006 — Enforce the measured native prerequisites

- **Date / decision-maker:** 2026-09-12, implementor within authorized scope.
- **Question / choice:** The probe uses a native-created selected layer at its
  existing radius, geodesic prototype-0 frames, and the actual rounded latitude
  prerequisite of 68. It neither imports a replacement nor bypasses research.
- **Alternatives / evidence:** SB-F2.3 found unit-direction geometry passes the
  target predicates. Reusing the sample's 81° header or checking layer creation
  against the already-existing layer would impose incorrect restrictions.
- **Rationale / consequence:** No arbitrary radius whitelist is needed. The
  scale-invariant candidate envelope still requires live boundary observations;
  any runtime failure that narrows the concept needs an owner disposition.
- **Affected contract:** Probe preconditions and future specification support,
  research refusal, and native layer selection. No technology level name is guessed.

### SB-D007 — Probe continuation from the native graph

- **Date / decision-maker:** 2026-09-12, implementor within authorized feasibility
  scope; the MVP proposal was then pending live evidence and owner review.
- **Question / choice:** Reconstruct a unique complete prefix of the fixed plan
  from the selected layer at each action. Accept an empty layer as a new start;
  continue prefixes 1–11; refuse completion and all unmatched graphs without
  mutation. Existing reference-face shells and construction do not prevent
  continuation. Use the measured/derived float recognition bound in SB-F3.2;
  do not alter placement coordinates or move existing nodes.
- **Alternatives / evidence:** The previous object-reference cache cannot survive
  native reload or layer recreation. Native serialization retains the geometry
  and endpoints needed for reconstruction. A persisted patch index would still
  need graph validation after edits; no need for extra stored state has emerged.
  Exact float equality failed the cross-runtime check of actual owner evidence.
- **Rationale / consequence:** The probe uses native save data alone. Closing the
  editor, switching layer/star, restart, or mod removal stores no progress in the
  plugin. Reinstallation can reconstruct matching native content. This is not an
  ownership claim: indistinguishable matching prefixes are treated alike, even
  after edits or ID reuse. Non-prefix edits, partial additions, and ambiguous
  matches refuse; there is no arbitrary blueprint recognition or automatic repair.
  An edit that exactly restores an earlier prefix is treated as that prefix.
- **Failure boundary:** An unexpected addition/preservation error stops further
  painting for that plugin session and retains before/after evidence. Do not
  bypass that stop by restarting during the probe. After investigation, only a
  fully matched prefix could provide a candidate resumption point; a partial
  graph is not retried or rolled back automatically.
- **Affected contract:** SB-F3.2 and future MVP continuation, edits, failure,
  completion, and state lifetime. This is a tested local candidate, not accepted
  live resumption or a production persistence design. G3 was open at that point.
  **Later disposition:** the owner continuation run supports the observed native
  reload/selection/edit cases; SB-D008 accepts the remaining unverified cases and
  SB-D009 settles the MVP mechanism choice.

### SB-D008 — Accept remaining unverified cases and proceed to specification

- **Date / decision-maker:** 2026-09-12, explicit owner instruction: "Let's treat
  the unverified cases as accepted and proceed to the MVP specification epic please."
- **Choice:** Accept the remaining feasibility cases without further live testing
  and proceed through SB-F4. This supersedes the roadmap's requirement to obtain
  every outstanding live observation before advancing G3. Preserve the difference
  between measured results and owner acceptance; do not manufacture evidence.
- **Remaining cases covered:** C60 placement at the native maximum radius;
  manual hexagon filling and later addition beside that hexagon; both-face shell
  checks across radius endpoints; placement in another system/giant; below-threshold
  research refusal and actual research-level metadata; multiple-layer selection;
  full application restart and mod removal/reinstallation. No-selection refusal,
  menu reload, and star switch-and-return have separate actual observations.
- **Basis / alternatives:** The native-rule derivations, compiled probe, and
  successful owner runs support the candidate behavior. The alternative was the
  remaining human test handoff; the owner explicitly chose acceptance instead.
- **Consequence:** These cases remain required MVP behavior based on their static
  evidence and accepted assumptions. They are not advertised as live-verified
  coverage. Later contrary evidence must be addressed, not hidden by this waiver.
  This does not authorize bypassing native limits, narrowing the concept, or
  shipping a release. Keep accepted assumptions visible in the specification.
- **Affected contract:** SB-F3.2–3.3, G3/M3, SB-F4's evidence reconciliation, and
  specification validation boundaries. Acceptance of the as-yet unwritten full
  specification and G4/M4 still required a separate owner review at that point.

### SB-D009 — Lean MVP behavior and failure boundary

- **Date / decision-maker:** 2026-09-12, implementor within the owner's authorized
  scope, using SB-D008's acceptance; final specification review was still pending.
- **Choice:** Carry SB-D003–007 into the MVP: one fixed native-frame design,
  one selected layer per click, native additive allocation, no construction wait,
  player-managed shells, and content-based continuation from native saves. Keep
  the native radius envelope and rounded latitude requirement, with SB-D008's
  unverified coverage plainly identified. No alternative geometry or partial
  low-latitude start is introduced.
- **Feedback / failure:** Provide one Paint next patch action in the native sphere
  editor and concise feedback for the actual selected target. Completion and
  unmet prerequisites produce no additions. An unexpected allocation or
  preservation failure stops further Paint actions for the plugin session,
  reports that partial additions may remain, and retains useful diagnostic
  context. No retry loop, rollback, automatic repair, or queued action is required.
  A later fresh session evaluates native content again; only a complete prefix
  can resume, and an unmatched partial delta refuses.
- **Alternatives / evidence:** A stored patch counter, automatic recovery, batch
  queue, global ownership registry, or a persisted failure marker adds machinery
  without a demonstrated need. SB-F3.1 proves non-atomic native calls and additive
  preservation; SB-F3.2 proves reconstruction for observed transitions. A session
  stop is the probe's bounded response, not a promise of transactional recovery.
- **Consequence:** Save data owns geometry, progress, and shell associations.
  The plugin needs only transient action/feedback/error state. Removing it leaves
  native construction in the save; no custom format or migration is selected.
  The probe's snapshot/rejection buttons and bulk evidence-file export are not
  MVP features. Retain concise diagnostics for actual failures, not a telemetry
  or reporting subsystem.
- **Target / delivery:** Use the recorded assembly/runtime as the implementation
  baseline. The probe's hard hash gate is an experiment safeguard, not a required
  production compatibility framework. Preserve the established build/version
  contract and reference attribution/licenses when later packaging executable
  work; this epic does not change the mock pipeline or authorize publication.
- **Affected contract:** MVP action states, preservation, continuation, failure,
  support envelope, data lifetime, and planning handoff. This resolves SB-D007's
  provisional mechanism choice using the observations and SB-D008; neither the
  original concept's progression nor its shell ownership changes.

### SB-D010 — Accept the specification and close feasibility

- **Date / decision-maker:** 2026-09-12, explicit owner instruction: "MVP
  specification accepted, Close and archive the current roadmap as completed."
- **Choice:** Accept the written MVP specification, complete SB-F4.2 and G4/M4,
  and archive the feasibility roadmap with its original scope/criteria intact.
  Author the next implementation roadmap as a draft on main.
- **Basis / alternatives:** The specification, its three-pass review, and the
  retained feasibility/decision record were presented for acceptance. No further
  feasibility handoff was requested; SB-D008's unverified cases stay explicit.
- **Consequence:** The first roadmap is closed. Authoring the implementation draft
  does not execute it, validate a production plugin, or authorize publication.
- **Affected contract:** Current phase, SB-F4.2, G4/M4, roadmap archive, and the
  next implementation-planning boundary.

### SB-D011 — Stable plugin identity and a directly usable CI package

- **Date / decision-maker:** 2026-09-12, owner instruction.
- **Choice:** Use production GUID `dsp.spherebuilder`. The implementation roadmap
  ends with a working MVP and CI producing the real mod package. Its downloaded
  archive must expose a valid package root directly, without a wrapper directory
  or an enclosed same-name ZIP. Preserve the sequential VERSION contract.
- **Basis / alternatives:** The workflow then uploaded a mock ZIP inside an
  Actions artifact with build information. Retaining that nested delivery or
  shipping the disposable probe does not meet the requested outcome. The actual
  upload mechanism was assigned to SB-I4.2 for verification rather than guessed.
- **Consequence:** Basic executable packaging and installation docs belong to
  this roadmap. Final artwork, publication polish, release automation, and actual
  Thunderstore upload remain outside it. The GUID is no longer an open choice.
- **Affected contract:** Specification delivery/planning handoff; SB-I1.2,
  SB-I4.1–4.2, and the final IG5/IM5 package requirement.

### SB-D012 — Focused late validation followed by a UI workshop

- **Date / decision-maker:** 2026-09-12, owner instruction.
- **Choice:** Defer human validation until immediately before the UI workshop;
  game runtime validation begins with that owner session. Before it, use static
  inspection, offline logic checks, compilation, and package inspection only.
  The workshop precedes the final gate and may yield bounded usability changes.
- **Basis / alternatives:** The owner requested efficient validation at this
  point, not early probes or an enlarged replay of the old feasibility matrix.
  SB-D008 already accepts W1–W6 without claiming those cases were observed.
- **Consequence:** SB-I5.1 uses one CI candidate and a short integrated route:
  ordinary-radius progression, a manual pentagon shell, intermediate menu reload,
  completion, and one edit refusal on a second layer. No construction-completion
  waits or routine W1–W6 matrix. Existing evidence covers declared limits; new
  contradictory evidence still requires action. Workshop changes get only the
  affected rechecks; final acceptance reviews the record rather than restarting
  the live checklist.
- **Affected contract:** Validation boundaries, IG1–IG4, SB-I5.1–5.4, IG5a and IG5.

### SB-D013 — Compile-only shims with per-commit native mapping

- **Date / decision-maker:** 2026-09-12, explicit owner steering during roadmap
  authoring: "We'll use shims to provide type references, carefully mapping any
  referenced type we add with each commit."
- **Choice:** CI compiles production source against minimal type-reference shims.
  Every commit adding or changing a referenced native surface includes its
  target-backed declaring assembly/type/member signature mapping, corresponding
  shim declarations, and affected checks. Keep one mapping beside the shim source.
- **Basis / alternatives:** The owner selected shims rather than acquiring full
  game binaries in CI. The recorded local assembly remains the authority for
  signatures and behavior. The available probe's local real-reference build
  establishes a separate compile path, not evidence that future shims are correct.
- **Consequence:** Establish initial assembly identity/signature mapping in
  SB-I1.1 and both compile modes in SB-I1.2; maintain the map in each later affected
  commit. No unmapped guessed type, copied runtime body, simulated game, or
  substituted shim-only proof of target compatibility. Exclude all shim/native
  dependency assemblies from packages and use the actual CI DLL in late runtime
  validation. Compile-only shims are authorized in the new implementation scope;
  the archived roadmap's feasibility-only restriction and historical evidence
  remain intact.
- **Affected contract:** Specification target/delivery notes and SB-A01;
  implementation execution rules, SB-I1, all new native references, and SB-I4.

### SB-D014 — Execute the implementation roadmap and use the supplied icon

- **Date / decision-maker:** 2026-09-12, owner instruction.
- **Choice:** Implement each story in order, push to main with its PROJECT update,
  and progress through the phase 5 owner session. Runtime observations remain
  owner-operated and wait for that handoff. Use the supplied spherebuilder icon
  as the Thunderstore package icon.
- **Basis / alternatives:** The owner accepted the draft and prioritized the
  concept promise, verified unknowns, bounded scope, and outcome-led tradeoffs.
  No further planning approval or alternative artwork is needed.
- **Consequence:** Authoring-only restrictions from SB-D010 are superseded for
  this roadmap. Substantial blockers stop dependent work; ordinary implementation
  decisions stay with the implementor. Final owner/UI gates cannot be inferred.
- **Affected contract:** Current phase, story execution, SB-I4.1, and SB-I5.

### SB-D015 — Build identity and package inputs

- **Date / decision-maker:** 2026-09-12, implementor within authorized scope.
- **Choice:** Use netstandard2.1, mapped references in CI and real local references
  for target compilation. BepInPlugin version is `MAJOR.MINOR.BuildNumber`;
  assembly/file versions are `MAJOR.MINOR.0.0`; informational/log identity is
  `MAJOR.MINOR.BuildNumber.shortCommit`. Package the mod under
  `BepInEx/plugins/DSPSphereBuilder/` and depend on `xiaoye97-BepInEx-5.4.17`.
- **Basis / alternatives:** Local loader metadata and primary distribution docs
  are recorded in SB-I1.1 evidence. Putting the sequential build into a CLR
  assembly-version component would impose an unrelated 16-bit limit; keeping
  assembly binding stable avoids that without changing the package sequence.
- **Consequence:** Keep native and shim binaries out of the payload. Retain
  upstream fixture/source/license credit separately from original project terms.
  The supplied icon already meets the decoded PNG/dimension requirement.
- **Affected contract:** SB-I1.2, SB-I4, SB-MVP-23–24, and package metadata.

### SB-D016 — Direct hosted package and retained source

- **Date / decision-maker:** 2026-09-12, implementor within SB-I4.
- **Choice:** Upload the validated ZIP with the pinned action's `archive: false`;
  retain build metadata separately. Include production source, compile inputs,
  geometry derivation/reference and applicable licenses as plain files in source/.
- **Basis / alternatives:** The pinned action declares raw single-file upload;
  independent run-31 download matches CI's exact ZIP bytes. Uploading a ZIP inside
  the default artifact archive creates the unwanted second package. A separate
  source ZIP would also add avoidable nesting. Windows license newline conversion
  was found by the unchanged retained-byte check and corrected with Git attributes.
- **Consequence:** The download is directly usable in package structure. Retry
  keeps the numeric version and replaces only that run's same-named package;
  matching attempt/build hashes distinguish it. Publication remains deferred.
- **Affected contract:** SB-I4, SB-MVP-23–24, BUILD.md and package delivery.

### SB-D017 — Minimal owner-session evidence

- **Date / decision-maker:** 2026-09-12, implementor within SB-I5.1.
- **Choice:** Use native single-layer clipboard exports at prefix 3 and completion,
  one useful panel screenshot, and the normal session log. Use the independently
  verified 0.1.31.a71fd79 package even when later documentation pushes produce
  newer CI versions. At that handoff, production code was unchanged from that candidate.
- **Basis / alternatives:** The native export writer and existing decoder were
  checked offline. Two exports support the final geometry and prior record/link/
  shell comparison without another probe or continuous snapshot exporter.
  Blueprint records omit invested SP/CP and object identity; collecting these
  independently would require intrusive capture or extra live setup.
- **Consequence:** Do not claim those omitted properties from blueprints or quiet
  logs. Retain the existing feasibility basis plus production in-action checks,
  and review visible outcomes, startup/action logs and captures together. W1–W6
  are unchanged. Core failures or missing essential evidence still block IG5a
  unless the owner explicitly disposes of them; this is no new acceptance waiver.
- **Affected contract:** SB-I5.1, SB-MVP-12–13, SB-A04–06/09/12–13 and owner handoff.

### SB-D018 — Accept the observed production MVP and record evidence limits

- **Date / decision-maker:** 2026-09-12, owner: "MVP is owner accepted", with the
  produced exports, normal log and screenshot supplied for review.
- **Choice:** Accept the tested 0.1.31.a71fd79 MVP. The owner explicitly clarified
  this did not skip the UI workshop; SB-I5.2–5.4 still remained. No no-change UI
  disposition or final IG5 acceptance was inferred. Close the integrated session
  using the actual captures and overall owner acceptance, retaining the distinction
  between independently checked outcomes and observations not separately recorded.
- **Basis / alternatives:** The log identifies the expected plugin/target and all
  twelve deltas; whole-sphere layer 1 checks as prefix 3 and complete 60/90 at
  radius 36,000, with earlier records and one reference hexagon boundary retained.
  The capture reader was adapted to the verified native container and existing
  two-face product contract instead of requesting replacement exports or gameplay.
- **Limits / consequence:** The owner supplied overall acceptance, not an itemized
  report of reload, no-selection, edit refusal, completion no-op or click-through.
  Do not relabel these as newly instrumented observations. SP/CP and memory
  identity remain supported by in-action checks and prior feasibility evidence;
  the screenshot shows zero construction. No core defect is exposed. W2 now has
  production designation/boundary-retention evidence for a hexagon; completed CP
  delivery and endpoint coverage remain unverified. Other W1–W6 limits stand.
- **Affected contract:** SB-I5.1, IG5a/IM5a, SB-MVP-12–13, W2, final evidence review
  and owner acceptance. Acceptance does not authorize publication or polish work.

### SB-D019 — Bottom-left compact control; unchanged wording

- **Date / decision-maker:** 2026-09-12, explicit owner workshop answers.
- **Choice:** Place Sphere Builder near the bottom, left of the native bottom bar
  controls. Make its enclosing panel smaller and sleeker. Keep the current wording.
- **Basis / alternatives:** The owner rejected retaining the top-center placement
  and chose the bottom toolbar area over the suggested left-side layer controls.
  The screenshot showed the then-current large panel and its overlap with the native
  center caption. Wording was explicitly accepted.
- **Acceptance outcomes:** The panel sits to the left of the native bottom toolbar,
  keeps clear of its controls and the center caption, and uses visibly less space.
  Button/status remain readable. Painting, current selection, refusal feedback,
  single-click input and error lifetime remain unchanged. Implementation may choose
  modest dimensions/spacing within this outcome; no new commands or settings.
- **Consequence:** SB-I5.3 was limited to this placement/layout refinement and a
  short owner check; no repeat of twelve-patch progression, shell tests or accepted
  feasibility gaps. Final IG5 was still open until SB-D021.
- **Affected contract:** SB-I5.2–5.4 and SB-MVP-08–10 presentation only.

### SB-D020 — Accept the workshop refinement and title-case the action

- **Date / decision-maker:** 2026-09-12, explicit owner confirmation: "Everything
  looks good", with authorization to finish the roadmap after changing the label
  to **Paint Next Patch**, without another live recheck for that correction.
- **Choice:** Accept SB-D019's layout/input outcomes and complete SB-I5.3. Retain
  the same words, dimensions and action; change only their capitalization.
- **Basis / limits:** The new log identifies 0.1.36.97b7f86 and the target MVID,
  with one successful 6-node/6-frame patch and no Error/Fatal entry. The supplied
  initial screenshot duplicated the earlier 0.1.31 capture. The owner then supplied
  a replacement showing the compact bottom-left control; it corroborates the
  owner's layout/input confirmation.
- **Consequence:** Final reconciliation and the corrected package still require
  offline/CI checks. No new live session is needed for this label-only change.
  After the final milestone, archive the completed roadmap, leave a polishing
  placeholder, and perform the requested management and code hygiene passes.
  Authoring or executing the polishing roadmap and publication are not included.
- **Affected contract:** SB-I5.3–5.4, SB-MVP-08 presentation, roadmap lifecycle.

### SB-D021 — Complete the working MVP milestone

- **Date / decision-maker:** 2026-09-12, implementor's evidence reconciliation
  under the owner's explicit MVP/UI acceptance and instruction to finish.
- **Choice:** Complete SB-I5.4 and IG5/IM5. All 25 requirements and 15 acceptance
  cases were reviewed against production source, offline checks, live evidence
  and the existing accepted limits. No core blocker remained.
- **Basis:** Final milestone package 0.1.38.f761fee passed CI and independent
  download/native-reference checks. Its only runtime-source difference from the
  UI-tested 0.1.36 is SB-D020's accepted capitalization change. Functional evidence
  comes from 0.1.31 and retained feasibility; no extra live coverage is inferred.
- **Consequence:** Archive the completed roadmap and leave a polishing placeholder.
  Complete the requested management/code hygiene passes before the next planning
  discussion. Keep W1–W6 and their actual evidence distinctions visible. No new
  feature, polishing implementation, public release or submission is authorized.
- **Affected contract:** SB-I5.4, IG5/IM5, roadmap lifecycle and next planning boundary.

### SB-D022 — Plan the first release candidate; leave publication to the owner

- **Date / decision-maker:** 2026-09-12, explicit owner planning instruction.
- **Choice:** Author and push a polishing-only roadmap ending in acceptance of
  the first release candidate. Cover necessary package files, short player copy
  and installation guidance, no PII, whole-repository security, implementation
  sanity, CI/DLL identity and a final agentic code-quality pass. Plan no features
  and no owner participation before the final candidate handoff.
- **Basis / alternatives:** The completed MVP and retained acceptance evidence
  provide the baseline. The owner chose candidate preparation followed by manual
  owner release, rather than adding features or automating publication. The current
  workflow, package inventory, validators, licenses and primary Thunderstore package
  and DSP routing rules were inspected while drafting.
- **Planning choices:** Five epics contain ten sequential stories. Resolve minimum
  distribution/source obligations and privacy findings before trimming the package;
  do not presume either a required source tree or permission to remove it. Check
  the entire repository and close security findings, then review delivery/code and
  recheck changed inputs against the final downloaded bytes. Keep one evidence
  document, created only when execution produces findings, with state here alone.
- **Consequence:** Publishing the draft does not execute stories. RG5a requires
  completed agent checks and an identified candidate; RG5 requires explicit owner
  acceptance of that artifact. Reuse unchanged MVP observations; any necessary
  runtime check is limited to a concrete change in the final session. New privacy,
  security or distribution blockers cannot inherit W1–W6's earlier acceptance.
  GUID, mapped-reference discipline and sequential versioning remain unchanged.
- **Affected contract:** SB-R1–SB-R5, RG1–RG5/RM1–RM5 including RG5a/RM5a, the
  current planning boundary and the separation of acceptance from publication.

### SB-D023 — Execute the polishing roadmap through candidate handoff

- **Date / decision-maker:** 2026-09-12, explicit owner instruction.
- **Choice / basis:** Implement the entire roadmap in sequence, update PROJECT.md
  and push each completed story to main. The owner will review the finished
  candidate. This supersedes SB-D022's planning-only execution boundary.
- **Consequence:** Complete authorized agent work without routine interim approvals;
  preserve bounded scope, investigate unknowns and stop on a substantial blocker.
  SB-R5.2/RG5 still require actual owner acceptance. Publication remains manual
  and owner-operated, outside this authorization.
- **Affected contract:** SB-R1–SB-R5 and the current execution boundary.

### SB-D024 — Minimal install ZIP with revision-specific source access

- **Date / decision-maker:** 2026-09-12, implementor within SB-R1.1.
- **Choice:** Reduce the install ZIP to manifest, player README, unchanged icon,
  one combined LICENSE/attribution file and the production DLL. Remove its complete
  `source/` subtree; provide anonymous, full-commit source archive links in the
  generated README and LICENSE. Keep build diagnostics separate.
- **Basis:** [Distribution investigation](RELEASE-CANDIDATE.md#sb-r11--minimum-distribution-contract):
  actual inventory/licenses, GPL-3.0 section 6(d), Apache's compatibility guidance,
  primary Thunderstore/DSP routing rules and a successful anonymous source download
  containing build and derivation inputs.
- **Alternatives:** Retaining the entire source tree would preserve access but add
  unnecessary install files. Removing source without directions/terms, linking a
  mutable branch, or using expiring CI source artifacts would not meet the chosen
  distribution basis. No separate source-hosting service is needed.
- **Consequence:** Treat the combined distribution under GPL-3.0 while retaining
  the original Apache-2.0 source grant and notices. Preserve upstream attribution;
  make no claim that numerical derivation erases its licensing. Verify source access
  for the exact candidate and keep it available while that binary is distributed.
  BUILD.md keeps describing current behavior until SB-R2.1 implements this change.
- **Affected contract:** SB-R1.1, SB-R2.1–2.2, SB-R5.1; supersedes SB-D016's bundled
  source choice without changing the game's behavior or original-source license.

### SB-D025 — Prevent new private attribution; stop at the historical privacy gate

- **Date / decision-maker:** 2026-09-12, implementor within SB-R1.2.
- **Finding / basis:** [Privacy inspection](RELEASE-CANDIDATE.md#sb-r12--privacy-review-and-historical-attribution-blocker)
  confirmed a personal email in 44 of 45 reachable commits, including the effective
  identity used for new commits. Anonymous GitHub access confirmed published
  exposure. Historical names and remaining file/artifact surfaces also need review.
- **Choice:** Change only this repository's future Git identity to the existing
  public handle and verified GitHub no-reply address. Record SB-R1.2/RG1 as blocked;
  do not rewrite history, delete remote material or silently exempt attribution.
- **Alternatives / consequence:** A normal new commit cannot remove old metadata.
  History cleanup requires separate authorization, revised commit identities and
  review of old references/hosted remnants. Excluding historical attribution would
  instead require an explicit owner scope change. Neither is authorized by the
  current plan. Global Git settings and existing history remain unchanged.
- **Affected contract:** SB-R1.2/RG1 and dependent stories; no privacy pass, release
  candidate readiness or new acceptance waiver is asserted.

### SB-D026 — Authorize and complete the attribution rewrite

- **Date / decision-maker:** 2026-09-12, owner: "I approve any history rewrite
  necessary so commits can proceed"; implementor selected the bounded cleanup.
- **Choice:** Replace human attribution with the repository's public handle and
  verified GitHub no-reply identity. Preserve all source trees, messages, timestamps
  and topology; retain service attribution. Push with an exact remote-head lease.
  Keep the [old-to-new identity map](management/archive/HISTORY-REWRITE.json) so
  historical build/evidence identities remain traceable without rewriting their claims.
- **Basis / result:** [Cleanup evidence](RELEASE-CANDIDATE.md#authorized-cleanup-and-checks)
  verifies 46 rewritten commits, public reachable history, scoped local object
  removal, privacy inspection of source/outputs and the sanitized upstream cache.
  Compiler path mapping prevents future production debug-path exposure; both
  target-backed builds and existing reference checks passed without API changes.
- **Alternatives / consequence:** No historical attribution exemption was used.
  A broad local purge was rejected; a narrower verified cleanup preserved unrelated
  recovery data. No global Git configuration or third-party history was changed.
  GitHub internal retention and other people's copies are outside repository control;
  no internet-wide erasure claim is made. Future candidate outputs are rechecked.
  This explicit owner authorization superseded the prior no-rewrite restriction
  for this cleanup and resolves SB-D025. RG1/RM1 passed; proceed to SB-R2.1.
- **Affected contract:** SB-R1.2/RG1, history provenance and production debug metadata.

### SB-D027 — Exclude upstream advisories from this polishing scope

- **Date / decision-maker:** 2026-09-12, owner: “Ignore the upstream advisories.”
- **Choice:** Stop further triage or fixes of advisories in upstream action/runtime
  dependencies. Continue the repository-owned source, workflow, secret, privacy
  and delivery checks. Retain the dependency inventory and disclose the exclusion.
- **Basis:** The action lockfile query had returned upstream library alerts after
  repository source review found no confirmed exploitable issue. The owner supplied
  this explicit scope correction before remediation.
- **Alternatives / consequence:** No dependency upgrade or claim that upstream
  alerts are fixed, absent or accepted by a scanner. RG3 applies to the assessed
  repository with this exclusion; new findings in repository-owned controls still
  require resolution. Publication remains outside this roadmap.
- **Affected contract:** SB-R3.1–3.2 and the final security evidence in SB-R5.1.

## Evidence and unresolved questions

- The release-candidate draft was reviewed in three passes on 2026-09-12:
  scope/decomposition and sequential dependencies; all nine requested outcomes,
  early unknowns and the final-only owner workload; authority, links and historical
  integrity. The inventory decision was separated from changing the implemented
  build contract, and publication exclusions were clarified to preserve CI artifact
  delivery. Five epics and ten stories have explicit scope, done criteria, outputs
  and exclusions. All 74 local links/anchors in the five changed documents passed,
  as did story-field, dependency, tracking and whitespace checks. Archived plan
  bodies were preserved; only their current-roadmap link captions changed. These
  are planning checks, not security, privacy, package or runtime validation.
- The implementation draft was reviewed in three passes on 2026-09-12:
  scope/decomposition and backward-only dependencies; requirement/failure-state
  coverage and the late human-validation workload; archive/authority integrity,
  per-commit reference mapping, and direct package delivery. All 15 acceptance
  cases map to owning work and cover the 25 specification requirements. The
  archived body matches the prior roadmap apart from adjusted relative links;
  local links/anchors and whitespace checks pass. These are document checks, not
  implementation, build, or runtime evidence.
- The concept preserves the supplied sample and reported format checks; those
  checks were not rerun during repository preparation.
- Canonical geometry, polar progression, and the static native placement envelope
  are verified. Two-patch addition preserved partial and completed construction
  and preexisting shells in the live target. Native reconstruction survived the
  observed menu reloads and selection/edit cases. Remaining live cases have the
  explicit owner disposition SB-D008; their verification gaps remain documented.
- Local preparation checks passed: numeric build progression, stable version on
  retry, strict VERSION input rejection, and validation of the four-file mock ZIP.
  Negative checks rejected missing README, unexpected DLL, wrong version, broken
  PNG, and invalid UTF-8. PowerShell parsing and whitespace checks also passed.
- [Hosted build 1](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34662682423)
  passed checkout, package creation/validation, and artifact upload for commit
  `79a72be46a4b81bc3ed93bf6a1a5b264202dec22`. Its downloaded ZIP was inspected
  independently: exactly `manifest.json`, `README.md`, `icon.png`, and `LICENSE`,
  with valid metadata, UTF-8 text, and a decoded 256x256 PNG. Package version
  `0.1.1` and build label `0.1.1.79a72be` matched the run and source commit.
  This is the preparation baseline, not a rolling latest-build record.

## Next decision

The owner reviews candidate **0.1.54** from source
`1c04199c490dfb029ba466ebfb53836ed46548f7`, run **34700204095**, attempt **1**.
The [packet](RELEASE-CANDIDATE.md#owner-review-packet--0154) contains the exact download,
hashes, copy and evidence limits. No new runtime check is required for the polishing
changes. Record explicit acceptance before closing RG5/RM5 and archiving this roadmap.
Subsequent documentation builds do not replace this candidate. Actual release work
remains manual and owner-operated; no release/tag/upload has been performed.
