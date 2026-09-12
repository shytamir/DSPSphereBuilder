# Project steering and state

## Authority

This is the sole authoritative record of accepted scope and steering decisions,
current phase, epic/story status, readiness, and owner acceptance. Update those
facts here only. Current owner instructions take precedence.

The [concept](../CONCEPT.md) describes the product behavior and reference geometry.
The [roadmap](management/ROADMAP.md) defines work and completion criteria, without
tracking its status. The [MVP specification](MVP-SPECIFICATION.md) defines the
implementation contract and acceptance cases. The [README](../README.md)
introduces the project, the
[build guide](BUILD.md) defines build procedures, and [AGENTS.md](../AGENTS.md)
governs agent conduct. These documents link here for state.

## Current phase

The owner accepted the MVP specification on 2026-09-12. The feasibility roadmap
is complete and retained in the [archive](management/archive/ROADMAP-feasibility-and-mvp-definition.md).
The owner accepted the [MVP implementation roadmap](management/ROADMAP.md) and
authorized sequential execution with a main push and this state update for each
completed story. Work proceeds to phase 5's owner session, then the workshop and
final acceptance follow their explicit human gates. See SB-D014.

| Area | Current state |
| --- | --- |
| Product concept | Agreed; recorded in [CONCEPT.md](../CONCEPT.md) |
| Repository preparation | Complete; local checks and hosted artifact inspection passed |
| Roadmap and active work | SB-I1.1–1.2 complete; SB-I2.1 next |
| Feasibility gates and MVP specification | G1/M1 and G2/M2 passed; G3/M3 accepted under SB-D008 with documented unverified cases; SB-F4.2 complete and G4/M4 achieved by explicit specification acceptance under SB-D010 |
| Implementation gates | IG1/IM1 passed; IG2–IG5 and workshop-entry IG5a not entered |
| Mod implementation | Minimal production entry point compiles against mapped and real references; no editor action yet |
| Runtime validation and owner acceptance | Additive preservation, continuation, and full graph verified in the observed contexts; owner accepts remaining unverified cases under SB-D008 |
| Distribution | Mock artifact only; no release or Thunderstore publication authorized |

## Accepted scope

The accepted product baseline is the [concept](../CONCEPT.md) and the
[MVP specification](MVP-SPECIFICATION.md), including the
twelve-click polar progression, missing connections plus one leading spoke,
preservation of existing construction, and player-managed shells. Future changes
to that baseline require a steering decision here and a corresponding update to
the concept where its behavior changes.

The implementation roadmap delivers a working MVP and a real, directly usable CI mod
package, followed by publication polish in a later roadmap. The production GUID
is `dsp.spherebuilder`. CI will use mapped compile-only reference shims; every commit
adding or changing a referenced surface must carry the corresponding native
type/member mapping and checks. Local real-reference compilation remains required.

Human validation and game runtime validation begin together immediately before
the UI workshop. The short session reuses accepted feasibility evidence instead
of reopening W1–W6 as a test matrix. Necessary workshop changes and targeted
rechecks precede final MVP acceptance. Production implementation is authorized;
publication and polish remain outside this roadmap. The supplied icon replaces
the package placeholder in SB-I4.1. CI retains mock delivery until SB-I4.2.

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

### MVP implementation roadmap

| Story | Execution state | Planned outcome |
| --- | --- | --- |
| SB-I1.1 | Complete | [Mapped references and delivery inputs](MVP-VALIDATION.md#sb-i11--mapped-references-and-delivery-inputs) |
| SB-I1.2 | Complete | [Local and hosted production compilation](MVP-VALIDATION.md#sb-i12--production-compilation-and-identity) |
| SB-I2.1 | Not started | Exact twelve-patch production plan |
| SB-I2.2 | Not started | Native-content prefix recognition |
| SB-I3.1 | Not started | Additive action and session failure boundary |
| SB-I3.2 | Not started | Current-target editor control and feedback |
| SB-I4.1 | Not started | Executable package and validator |
| SB-I4.2 | Not started | Real hosted download and independent inspection |
| SB-I5.1 | Not started | Focused owner/runtime validation |
| SB-I5.2 | Not started | Owner UI workshop |
| SB-I5.3 | Not started | Agreed UI refinements and targeted rechecks |
| SB-I5.4 | Not started | Working-MVP acceptance and polish handoff |

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
passing mock package build establishes the packaging path, not gameplay behavior
or Thunderstore moderation acceptance.

## Decision record

### SB-D001 — Feasibility before implementation planning

- **Date / decision-maker:** 2026-09-12, owner instruction.
- **Question and choice:** The first roadmap will establish feasibility and MVP
  constraints, ending with a full specification backed by evidence and recorded
  decisions. Planning the playable implementation follows in a separate roadmap.
- **Basis:** The owner's roadmap request and the outstanding verification in
  [CONCEPT.md](../CONCEPT.md#remaining-verification).
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
  scope; MVP proposal pending matching live evidence and final owner review.
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
  live resumption or a production persistence design. G3 remains open.
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
  specification and G4/M4 remains a separate owner review.

### SB-D009 — Lean MVP behavior and failure boundary

- **Date / decision-maker:** 2026-09-12, implementor within the owner's authorized
  scope, using SB-D008's acceptance; final specification review remains with owner.
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
- **Basis / alternatives:** The current workflow uploads a mock ZIP inside an
  Actions artifact with build information. Retaining that nested delivery or
  shipping the disposable probe does not meet the requested outcome. The actual
  upload mechanism will be verified in SB-I4.2, not guessed during planning.
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

## Evidence and unresolved questions

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

Continue SB-I2.1 under the recorded authorization. The first human/runtime handoff
is SB-I5.1 after a directly usable real CI package exists. No early probe is needed.
