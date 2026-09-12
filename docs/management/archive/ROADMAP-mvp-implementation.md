# MVP Implementation Roadmap

> Historical plan, archived on 2026-09-12. It prescribed the scope and completion
> criteria below; its story language is retained as the original plan, not active
> work. [PROJECT.md](../../PROJECT.md) records execution, acceptance and closure.
> The [current roadmap](../ROADMAP.md) defines subsequent planning.

## Outcome and authority

Deliver a working DSP Sphere Builder MVP: one **Paint next patch** action that
builds the exact twelve-patch C60 framework on the selected native layer,
preserves construction and player-filled shells, and resumes from native content.
Deliver it as a real CI-built package suitable for later Thunderstore upload.
Finish a focused owner validation and UI workshop so the next roadmap can address
publication polish from a working product.

[PROJECT.md](../../PROJECT.md) alone owns scope acceptance, execution state, story
status, gate results, and decisions. This roadmap defines work and completion
criteria. The [MVP specification](../../MVP-SPECIFICATION.md) supplies the behavioral
contract; the [archived feasibility roadmap](ROADMAP-feasibility-and-mvp-definition.md)
and [findings](../../FEASIBILITY.md) supplied its provenance. Publishing the original
draft did not itself execute stories or accept an executable build.

## Scope and constraints

Included:

- The specification's fixed geometry, twelve deltas, native additive operation,
  current-target binding, content-based continuation, and bounded failure policy.
- Production plugin identity **`dsp.spherebuilder`**, local real-reference builds,
  mapped compile-only CI shims, focused offline checks, and a concise editor UI.
- The existing workflow extended to compile and package the production DLL, with
  sequential numeric versions and attributable diagnostic build identity.
- A downloadable package with `manifest.json`, README, icon, and license material
  at its root, plus the installable mod payload. The downloaded package itself
  must be usable without extracting another ZIP or removing a wrapper directory.
  No same-name nested ZIP; build metadata must not create a second package layer.
- One efficient owner-operated runtime validation immediately before the UI
  workshop; bounded usability changes from that workshop and a final MVP handoff.

Out of scope:

- New geometry, routes or orientation controls, bulk/automatic painting, shell
  creation, construction scheduling, research bypasses, or altered native costs.
- Arbitrary blueprint adoption, ownership history, automatic repair/rollback,
  custom save data, multiplayer, or a general compatibility framework.
- Shipping feasibility-probe controls, routine snapshot export, telemetry, a new
  test platform, or unrelated repository/tooling cleanup. CI shims describe only
  compile references; they do not simulate the game or prove runtime behavior.
- Early live probes, agent-operated gameplay, exhaustive radius/star/mod matrices,
  and replaying accepted feasibility gaps as a new human checklist.
- Publication polish: final artwork, marketing copy, extensive visual styling,
  localization, release automation, tags/releases, and Thunderstore publication.
  Basic readable UI and accurate installation/package documentation are included.

The MVP's support obligations are unchanged. The specification's W1–W6 remain
accepted unverified cases under SB-D008, not observed production results and not
a reason to narrow support. Contrary evidence still needs a fix or an explicit
owner decision. Package validation does not establish moderation acceptance,
and a successful build does not establish runtime behavior.

## Execution, evidence, and decisions

1. Execute only after authorization is recorded in PROJECT.md, in the dependency
   order below. Close each story against its definition of done and record its
   evidence there. Keep story commits and their state updates together; push as
   authorized. A gate cannot pass just because its documents exist.
2. Use the [recorded local assembly](../../PROJECT.md#target-game-reference) as the
   target authority. Reconfirm its identity before implementation builds; a
   changed target requires a baseline decision and review of affected findings.
   Reuse retained native findings and geometry checks unless invalidated.
3. Use compile-only shims for CI type references under SB-D013. Every commit that
   adds or changes a referenced surface must include its target-backed type/member
   mapping, corresponding shim declarations, and affected compile checks. Map only
   what production code uses; do not copy decompiled implementation. Verify local
   compilation against real references separately, including emitted member kinds
   and declaring assembly identities that a source-level compile may not catch.
   Never ship shim assemblies or treat shim compilation as runtime verification.
4. Until SB-I5.1, validation is limited to source/metadata inspection,
   deterministic checks, compilation, and package inspection. Do not launch or
   attach to the game, load the plugin in Unity, or run a new native probe. Plain
   managed tests of plan/recognition/action logic are offline checks; they must
   not rely on a simulated game implementation in the reference shims.
5. SB-I5.1 is the first human-validation handoff and the start of runtime
   validation. The owner operates a disposable save with the identified production
   package. No earlier story waits for human playtesting or claims it occurred.
   Later rechecks address only changes or failures that warrant them.
6. Keep reproducible commands and concise results in `docs/MVP-VALIDATION.md`,
   organized by story and specification case, when the first story produces
   evidence. Include source/build/input identities, observed values, method,
   failures, limits, and relevant source references. Keep binaries, captures,
   decompilation, and raw local data under ignored `artifacts/`. Do not create an
   empty report now or duplicate historical FEASIBILITY.md.
7. Record material decisions in PROJECT.md at discovery: question, actual
   alternatives, evidence, choice and rationale, decision-maker, consequences,
   and affected requirements. Routine details need no decision entry. An unknown
   is investigated in the earliest affected story; an essential unresolved input
   or contradiction blocks its gate with a precise unblock condition. Do not
   infer owner acceptance from silence or widen scope to make a check pass.
8. Prefer the proven native path and direct code. Adapt this project's probe
   logic only after checking the production contract; remove experimental UI,
   tracking, and reporting obligations. Keep failure handling and tests with the
   behavior they exercise. No generic recovery or compatibility framework.
9. Test observable outcomes and numeric/graph invariants, including meaningful
   negative cases. Do not assert exact feedback strings, source-code fragments,
   or implementation choices as product behavior. Follow AGENTS.md's retry bound;
   diagnose failures before repeating checks and stop a persistent blocked path.

Update BUILD.md and the package README when the real build exists; update the
root README when its documented capabilities change. PROJECT.md remains the only
state authority. Neither workshop notes nor validation findings become a second
backlog or decision register. Keep the reference map next to the shim sources,
with assembly/type/member signatures and native evidence locators; no parallel
copy of that inventory is required in the validation document.

## Phases, milestones, and gates

| Phase / epic | Milestone | Exit gate |
| --- | --- | --- |
| 1. Reproducible production build — SB-I1 | IM1: Attributable production DLL from local and CI builds | IG1: Mapped shim build and real-reference build established; stable identity/version metadata verified |
| 2. Recognizable staged design — SB-I2 | IM2: Exact plan and unambiguous prefix recognition | IG2: All deltas, numeric bounds, shell boundaries, and refusal cases pass offline checks |
| 3. Native editor action — SB-I3 | IM3: Integrated one-click MVP, awaiting runtime validation | IG3: Additive action, preservation, failure stop, targeting, and feedback implemented and checked offline |
| 4. Real package delivery — SB-I4 | IM4: Downloadable CI-built MVP candidate | IG4: Actual hosted download validates directly as the package with the attributable production DLL |
| 5. Owner validation and UI workshop — SB-I5 | IM5a: Focused production observations; IM5: Working MVP ready for publication polish | IG5a: Human validation resolved before the workshop; IG5: Workshop closed, final package verified, owner MVP acceptance recorded |

Phases enter only after the preceding gate. IG1–IG4 establish offline/build
readiness only. Within phase 5, SB-I5.1 and IG5a precede the workshop. Final runtime
claims name the tested artifact and distinguish any subsequent UI-only changes.

## Phase 1 — Epic SB-I1: Reproducible production build

**Return:** A minimal production plugin with mapped compile references, stable
identity, and repeatable builds without redistributing game dependencies.

### SB-I1.1 — Establish mapped compile references and delivery inputs

**Story:** As a maintainer, I want every referenced native type grounded in the
local target so CI can compile the mod without guessing the game's API.

**Depends on:** Implementation authorization in PROJECT.md.

**Scope:** Establish the bounded shim/reference mapping practice and remaining
package inputs before wiring the production project.

**Definition of done:**

- Reconfirm target hash/MVID and the minimum game, Unity, and loader references.
  Record identities and parameterized locations, not checked-in machine paths.
  Retain the known game-version suffix limitation if unresolved.
- Define and populate the initial compile-only shim/reference map for the plugin
  entry point. Each used type/member has its declaring assembly, complete
  signature, relevant inheritance/interface shape, and target evidence locator.
  Preserve field/property, instance/static, parameter/return, and assembly identity
  distinctions needed by emitted references. No runtime bodies are copied.
- Verify how those declarations compile and how the production DLL will resolve
  them to real runtime assemblies. Record a bounded check for new/changed entries
  in every affected commit; unmapped references block that commit's completion.
  Use source/metadata inspection and compilation only, not a game-load test.
- Verify runtime dependency metadata and install layout from primary loader and
  distribution sources. Settle the required framework/reference and
  loader/assembly/informational-version mapping using the existing build-number
  contract. Preserve applicable geometry/source/licenses. Any missing essential
  metadata or reference identity is investigated here, not guessed from another mod.

**Produces:** Initial shim declarations/map, reproducible input findings, and the
minimum build/distribution decisions.

**Excluded:** Full-game stubs, functional mock runtime, automated shim-generation
platform, installation, loader migration, and broad compatibility research.

### SB-I1.2 — Compile the production plugin with stable identity

**Story:** As a maintainer, I want a real, attributable plugin build so features
attach to the same production project that CI will deliver.

**Depends on:** SB-I1.1.

**Scope:** Add the smallest production project and local/CI build entry points,
using mapped shims in CI and real local references for target compilation.

**Definition of done:**

- Compile a minimal BepInEx plugin with GUID `dsp.spherebuilder` against the real
  local references and mapped CI references. CI builds the production project
  from the triggering source, not a checked-in DLL. Record inputs and revisions;
  verify emitted assembly/type references against the native mapping.
- Read VERSION without rewriting it. Translate `MAJOR.MINOR.BuildNumber` into
  verified loader/assembly metadata and retain the first seven commit digits in
  diagnostic identity. Preserve run-number sequencing and stable versions on
  retry; neither a hash nor run attempt supplies the numeric patch.
- Inspect compiled metadata and dependencies. Missing required references or
  mapping drift fail clearly, without stale-probe/DLL fallback. Native dependency
  binaries and shim assemblies are excluded from the mod's distributable payload.
- Document repeatable commands for both reference modes and retain focused
  metadata/version checks. CI's published artifact stays explicitly mock until
  SB-I4.2 replaces it; compilation makes no claim that the plugin loaded in game.

**Produces:** Production project, local/CI source-to-DLL build path, and compile
evidence with maintained reference mapping.

**Excluded:** Player UI, geometry/action implementation, game launch, executable
package assembly, probe installation, and new bootstrap tooling.

**IG1 / IM1:** Both stories meet their definitions of done. CI builds the production
project with mapped references and local real-reference compilation also passes.
No unresolved identity or dependency decision prevents the later real package.

## Phase 2 — Epic SB-I2: Recognizable staged design

**Return:** The correct next patch can be computed from native content without
persisted progress, guessed coordinates, or arbitrary blueprint recognition.

### SB-I2.1 — Supply the exact twelve-patch production plan

**Story:** As a player, I want each click to extend the intended reference sphere
so gradual planning preserves its geometry and connected progression.

**Depends on:** IG1.

**Scope:** Supply fixed normalized positions, deltas, and face boundaries from
the pinned reference and established derivation to production code.

**Definition of done:**

- Retain reference identity, P1-to-P4 orientation, and the specified 1–5–5–1 route.
  Choose a direct representation of the established plan; no new geometry solver
  or user-facing generator is introduced.
- Compare all twelve production deltas with the retained independent derivation
  and specification: new/reused nodes, perimeter/closing frames, leading
  spoke/endpoints, cumulative connectivity, and final 60 nodes / 90 frames.
- Verify production coordinate generation at representative scales against
  SB-MVP-07's direction/radius bounds. Each position derives independently from
  the reference; there is no accumulated placement or movement of old nodes.
- Retain both reference face classes and closure steps for recognition. Add no
  shell or future geometry beyond the requested delta. Preserve source attribution
  through any generated/embedded representation and map any new native references.

**Produces:** Production plan with reproducible numeric/topology checks.

**Excluded:** Alternative geometry/orientation, optimization claims, native
allocation, automatic shell creation, and new visualization tooling.

### SB-I2.2 — Recognize the next patch from native layer content

**Story:** As a returning player, I want continuation to reflect the selected
layer's actual framework so reloads and reused IDs cannot advance the wrong plan.

**Depends on:** SB-I2.1.

**Scope:** Implement read-only native graph extraction and unique prefix matching,
including shell boundaries and refusal/completion outcomes.

**Definition of done:**

- Classify empty, unique prefixes 1–11, complete, and unmatched/ambiguous content;
  return the canonical-to-native mapping needed for addition. Apply SB-MVP-16's
  recognition tolerance separately from new-node generation precision.
- Match exact unordered non-Euler frames and shells on closed reference faces
  using present frames. Ignore construction amounts and cosmetic colors without
  ignoring extra/missing structural content. Recognition changes no native data.
- Use focused offline fixtures for every prefix, shuffled/reused IDs, both shell
  classes, missing/extra/displaced/ambiguous elements, incomplete deltas, and
  tolerance boundaries. Verify stable recognition as construction/colors change
  and refusal of invalid shell boundaries. Map referenced native fields/types.
- Reconstruct from fresh content without an ownership registry or saved counter.
  A recreated empty layer starts at zero, an exact earlier prefix is accepted as
  that prefix, and a full graph stays complete. Preserve the specification's lack
  of historical-provenance guarantees.

**Produces:** Production recognizer/native read adapter and behavioral checks.

**Excluded:** General blueprint recognition, save hooks/sidecars, snapping, repair,
live reload experiments, and a substitute native game implementation.

**IG2 / IM2:** Both stories pass offline checks attributable to the pinned
reference. No missing delta, match policy, or shell rule is left for the human
session to discover by manually counting nodes.

## Phase 3 — Epic SB-I3: Native editor action

**Return:** The production plugin can expose the correct next action and apply
one native additive patch while preserving the player's existing framework.

### SB-I3.1 — Add one patch and stop safely on unexpected failure

**Story:** As a player, I want a click to add just the next section while my
existing construction remains intact and an error cannot trigger repeated writes.

**Depends on:** IG2.

**Scope:** Implement the synchronous Paint operation and its prerequisites,
preservation checks, completion/refusal handling, and session failure boundary.

**Definition of done:**

- Resolve the current running editor, sphere, and exactly one selected layer at
  action time. Check rounded latitude at least 68, recognized content, finite
  planned positions, and the session stop before allocation. Refusals and
  completion add nothing; no arbitrary radius whitelist or layer creation check
  replaces the established prerequisites.
- Add nodes before frames through the inspected native calls, using prototype 0,
  `euler=false`, and the current canonical-to-native mapping. Preserve old records,
  positions, properties, invested SP/CP, shells, and prior links; permit legitimate
  new adjacency and native request bookkeeping. Map every added native reference.
- Check the intended delta and preservation contract. Frame-creation return zero,
  an unexpected exception, or a detected mismatch stops Paint for the plugin
  session and emits bounded diagnostic context. Do not retry, roll back, or
  promise atomicity.
- Exercise expected no-write outcomes, successful deltas, and partial-failure
  stop behavior offline through only the small call boundary needed to supply
  known success/failure outcomes. A stopped session cannot write again after a
  target change or menu-context reset; a fresh session recognizes full or partial
  content per the specification. Do not implement game behavior in CI shims or
  add a fault-injection control to the shipped mod.

**Produces:** Native additive operation, bounded diagnostics, and meaningful
refusal/preservation/failure checks.

**Excluded:** UI styling, queues, construction waits, shell mutation, research
changes, persisted failure markers, rollback, and live failure experiments.

### SB-I3.2 — Expose Paint and current-target feedback in the native editor

**Story:** As a player, I want one understandable control that acts on what I
have selected now and tells me whether I can continue.

**Depends on:** SB-I3.1.

**Scope:** Integrate the action and concise feedback into the native editor
lifecycle. Establish a usable initial layout for the later workshop.

**Definition of done:**

- Provide one Paint next patch control in the native sphere editor. Inspect and
  map the exact integration/input surfaces before adapting the proven probe
  approach; omit its snapshot/rejection controls and evidence exporter.
- One deliberate click invokes at most one synchronous UI-thread operation,
  without queued/background work, automatic advance, or construction waits.
  Register/clean up handlers so reopening cannot duplicate controls or actions;
  prevent the button click painting through into the native editor underneath it.
- Present ready/start/continue, prerequisite refusal, applied progress, completed,
  mismatch, and stopped states, with a reason when disabled. Visible progress
  belongs to the current selection. Editor/layer/star changes and menu reload
  cannot expose stale targets or silently clear the session error stop.
- Compile and check state transitions and handler/lifetime ownership offline.
  Tests concern action counts, targets, and outcomes rather than exact text or
  widget internals. Record the proposed placement and observations necessarily
  awaiting SB-I5.1; do not call the UI runtime-validated yet.

**Produces:** Production editor control and feedback with offline lifecycle and
selection checks, updated reference mappings, and real-reference compilation.

**Excluded:** UI workshop, game operation, custom rendering, previews, hotkeys,
configuration panels, localization, and polished artwork.

**IG3 / IM3:** The action and selection-state contract is implemented, compiled in
both reference modes, and checked offline. Runtime preservation, input interaction,
and visible usability await production observations in phase 5.

## Phase 4 — Epic SB-I4: Real package delivery

**Return:** A CI download contains the built MVP and required distribution
material, ready to install for the owner session and eventually upload unchanged
in structure to Thunderstore.

### SB-I4.1 — Assemble and validate the executable mod package

**Story:** As a player, I want a package that installs the actual mod with clear
instructions and dependencies, rather than the old metadata-only mock.

**Depends on:** IG3; use SB-I1.1's distribution findings.

**Scope:** Extend the existing package builder/validator for the production DLL,
required metadata, retained source/licenses, and direct package-root layout.

**Definition of done:**

- Package the freshly built production payload in the verified install layout.
  Put required metadata and documentation at the root; include applicable
  reference/source/license material. Exclude native dependencies, shim assemblies,
  probe payload, raw evidence, caches, and wrapper/nested archives.
- Generate verified runtime dependency metadata and numeric version from the
  existing VERSION/run-number contract. Match DLL identity/version to the
  package/build record, including `dsp.spherebuilder` and the source revision.
  A missing/stale payload fails instead of producing a mock success.
- Update BUILD.md, the root README, and package README for the real commands,
  installation, native prerequisites, one-click operation, manual shells, and
  failure limits. Retain the valid placeholder icon unless package validity
  requires a change; promotional polish stays deferred.
- Validate archive entries, metadata, image/text validity, payload identity, and
  required license/source files. Retain focused negative cases for missing DLL,
  wrong version/identity, accidental dependency/shim/probe inclusion, and nested
  package layout. Check the package contract, not exact README prose.

**Produces:** Real local package, updated validator, and usable build/install docs.

**Excluded:** Game installation/testing, final branding, release campaigns,
publishing automation, credentials, and Thunderstore submission.

### SB-I4.2 — Deliver and inspect the actual hosted download

**Story:** As a maintainer, I want the downloaded CI artifact itself to be the
validated mod package so it needs no second ZIP extraction before use.

**Depends on:** SB-I4.1.

**Scope:** Extend the existing workflow from compile/mock output to the complete
source-to-package path and verify what a user actually downloads.

**Definition of done:**

- Build production source with the mapped references, run affected offline/package
  checks, and publish the real package. Preserve the workflow's sequential
  `run_number`, retry semantics, bounded execution, and minimum permissions.
  Add no publishing, game execution, or fallback/prebuilt DLL path.
- Select a supported upload arrangement that gives the download a valid package
  root with no enclosed same-name ZIP or wrapper directory. Verify current action
  behavior; local staging alone is not proof. Keep ancillary build information in
  a separate artifact or run record without changing direct package usability.
- Download the successful run's package independently and check those bytes.
  Confirm DLL/plan, GUID, version, source identity, dependencies, license/source
  material, and absence of shims match the build. Record run, commit, artifact/
  package identities, and results. Cite the matching local real-reference check.
- Point the workflow summary/build guide to the correct direct download. Prepare
  that candidate for SB-I5.1; do not install it or request early human testing.
  A malformed hosted download leaves IG4 unmet even if the local ZIP passed.

**Produces:** Real CI delivery and evidence from the actual downloaded package.

**Excluded:** Tags/releases, Thunderstore upload, platform/infrastructure expansion,
and runtime validation.

**IG4 / IM4:** The hosted download passes the executable package contract and is
traceable to source and compile inputs. A short owner procedure can now exercise
that candidate without a second packaging or diagnostic-tool setup project.

## Phase 5 — Epic SB-I5: Usable MVP acceptance

**Return:** The owner has used the actual MVP, resolved a focused UI workshop, and
accepted a working build with a bounded handoff to publication polish.

### SB-I5.1 — Validate the integrated candidate with the owner

**Story:** As the owner, I want a short check of the production package so runtime
integration problems are resolved before discussing its usability.

**Depends on:** IG4. This is the first human validation and runtime handoff.

**Scope:** One integrated session on the pinned target using the identified CI
candidate, then evidence review and only necessary corrective work.

**Definition of done:**

- Supply one self-contained procedure: direct artifact/build identity, loader and
  target prerequisites, disposable save/layer setup, actions/expected results,
  evidence location, stop conditions, and cleanup. Use the production plugin
  without the feasibility probe active. The owner installs and operates the game;
  missing access leaves this story pending, not silently waived.
- Keep the baseline route short: start empty at an ordinary legal radius with
  sufficient unlock; check no-selection feedback, then paint three patches;
  designate a pentagon shell; save and menu-reload at that prefix, reselect and
  continue through patch 12 without construction waits; verify completion refuses
  further addition. On a disposable second layer, start a patch, remove one frame,
  and confirm non-prefix refusal without affecting the completed first layer.
  This combines progression, shell handoff, continuation, target binding, feedback,
  and a manual edit without a radius/star/restart matrix.
- Collect build/target identity, visible results, relevant logs, and the minimum
  read-only capture needed to compare the final graph and preservation. Use
  existing inspection/check tools where sufficient; establish the capture method
  and its format coverage offline before the handoff. The owner need not count
  coordinates or fill a large form. Do not require completed construction, sail
  supply, or delivery waits. Distinguish a graph capture from evidence of object
  identity/invested construction, and retain prior evidence where this run cannot
  observe those properties; do not claim new coverage there.
- Distinguish production observations, offline checks, inherited feasibility
  evidence, and W1–W6. Do not add full restart, mod removal, hexagon/endpoint
  matrices, new-system painting, or forced failure as routine exercises. Add a
  targeted case only for a concrete changed path or observed contradiction.
- Fix an observed core defect within its owning behavior and recheck only the
  affected path. A substantial blocker stops advancement; an essential missing
  observation is pending evidence or an explicit owner disposition. Quiet logs
  alone cannot pass the story. Record the validated artifact and remaining UI
  issues for the immediately following workshop.

**Produces:** Focused owner/runtime evidence, defect disposition, and concrete
workshop input from the working candidate.

**Excluded:** Broad playtesting, new feasibility probes, intrusive failure
injection, acceptance by compilation alone, and publication.

**IG5a / IM5a — Workshop entry:** The owner session has matching evidence. Core
runtime failures are resolved or have an explicit owner disposition preserving
the agreed MVP. Cosmetic/usability issues can enter the workshop; missing human
validation cannot be deferred until after it.

### SB-I5.2 — Workshop the MVP control with the owner

**Story:** As the owner, I want to shape the control using the working interaction
so it is understandable and practical before publication polish begins.

**Depends on:** IG5a.

**Scope:** Discuss the existing Paint control and feedback using SB-I5.1's
observations and current UI. This is a human workshop, not unilateral redesign
or another runtime test matrix.

**Definition of done:**

- Present the current control and concrete discussion points: discoverability/
  placement, readability, progress/refusal wording, disabled/stopped feedback, and
  interference with native controls. Use the actual build/capture; make a small
  mockup only if a choice needs one.
- Obtain explicit owner choices on necessary adjustments, acceptable existing
  behavior, and cosmetic work deferred to the next roadmap. Elapsed time or lack
  of feedback is not agreement.
- Record the finite agreed changes and observable acceptance outcomes in
  PROJECT.md, with reasons/evidence for material decisions. A no-change result is
  valid. A core behavior change or new feature needs a separate scope decision;
  it is not an implicit workshop task.

**Produces:** Owner-agreed UI outcomes and a bounded refinement scope.

**Excluded:** New commands/previews/settings, native-editor redesign, brand/artwork,
extensive theme variants, and an open-ended usability programme.

### SB-I5.3 — Apply the agreed UI refinements

**Story:** As a player, I want the workshop's practical improvements in the same
working MVP without changing what a Paint action does.

**Depends on:** SB-I5.2.

**Scope:** Implement only agreed control/layout/feedback adjustments and verify
their effects; deferred polish does not become a completion requirement.

**Definition of done:**

- Apply the finite agreed changes using the existing integration. Preserve target
  binding, single-click semantics, native input handling, and failure lifetime.
  Map any new/changed references in the same commit. Add no settings, persistence,
  or rendering framework.
- Run affected compilation/offline checks, produce a CI package when source
  changes, and inspect its identity. A visible/input change gets a short owner
  recheck on the identified build; do not replay the complete route or accepted
  feasibility gaps by default.
- Record results against the agreed outcomes and obtain owner confirmation of
  the UI disposition. If no change was requested, reference that decision and
  the existing artifact; do not manufacture code changes or tests.

**Produces:** Verified workshop refinements or a recorded no-change disposition.

**Excluded:** Publication polish, unrelated refactoring, expanded runtime matrices,
and additional feature work discovered during implementation.

### SB-I5.4 — Close the MVP and hand off publication polish

**Story:** As a maintainer, I want a traceable working build and reconciled
acceptance record so the next roadmap starts with polish rather than unfinished
core behavior or package plumbing.

**Depends on:** SB-I5.3.

**Scope:** Reconcile implementation, evidence, package, and owner acceptance;
define the next boundary without authoring the next roadmap.

**Definition of done:**

- Review all 25 requirements and 15 acceptance cases against implementation and
  the map below. Link actual checks/observations and accepted assumptions; no
  requirement is silently omitted or declared tested from a prepared procedure.
  Reconcile material decisions and any changed evidence basis in PROJECT.md and
  affected contract documents.
- Identify the final successful CI run, source commit, package identity/version,
  and downloaded-package check. Explain differences from the runtime-tested build
  and link affected rechecks. An unchanged artifact needs no rebuild to refresh a
  report; an untested core-code change cannot inherit UI-only acceptance.
- Confirm workshop outcomes are closed, build/install/use documents match the
  package, and no core defect blocks the specification. Reuse passing checks
  unless a change, failure, or concrete concern warrants more testing. Add no
  final all-cases human acceptance marathon.
- Present the working build, concise evidence limits, and bounded deferred polish
  to the owner. Record explicit MVP acceptance and IG5/IM5 in PROJECT.md. Keep
  Thunderstore submission, public release, and the next roadmap unexecuted.

**Produces:** Accepted working-MVP handoff with a directly usable CI package and
traceable evidence/decision record.

**Excluded:** Publishing, release approval by implication, implementing polish,
and reopening settled feasibility without contrary evidence.

**IG5 / IM5 — Final milestone:** The specified MVP works in the validated context,
all stories and workshop outcomes meet their completion criteria, the final CI
download is a real installable package without nested ZIPs, evidence limits remain
explicit, and owner MVP acceptance is recorded. Remaining product work belongs
to publication polish. Stop here.

## Acceptance coverage and validation boundaries

This map assigns the existing catalogue; it adds no behavior and does not require
all cases to be rerun live. SB-I5.4 reconciles the whole map. Evidence is written
during execution; state stays in PROJECT.md.

| Specification case | Owning implementation/check work | Runtime treatment |
| --- | --- | --- |
| SB-A01: target/build | SB-I1.1–1.2; SB-I4.2 | Candidate identity in SB-I5.1; no early game load |
| SB-A02: selection/editor | SB-I3.1–3.2 | No-selection/current binding in SB-I5.1; W5 retained |
| SB-A03: unlock | SB-I3.1–3.2 boundary checks | Sufficient native unlock in SB-I5.1; W4 retained |
| SB-A04: twelve deltas | SB-I2.1; SB-I3.1 | Integrated traversal/final graph in SB-I5.1 |
| SB-A05: construction | SB-I3.1 preservation checks | Available observations in SB-I5.1 plus E4; no completion wait |
| SB-A06: shells | SB-I2.2; SB-I3.1 | Pentagon handoff/preservation in SB-I5.1; W2 retained |
| SB-A07: radius | SB-I2.1; SB-I3.1 native-boundary inspection | One ordinary legal radius in SB-I5.1; W1 retained |
| SB-A08: layer/star targeting | SB-I2.2; SB-I3.1–3.2 | Two layers in SB-I5.1; retained E5 and W3 |
| SB-A09: editor/reload | SB-I2.2; SB-I3.2 | Intermediate menu reload in SB-I5.1; E5 supports other transitions |
| SB-A10: restart/removal | SB-I2.2; SB-I3.1–3.2 state-lifetime checks | W6 retained; no removal/restart exercise added |
| SB-A11: recreated IDs | SB-I2.2; SB-I3.1–3.2 | Retained E5; no separate live recreation exercise |
| SB-A12: edits/ambiguity | SB-I2.2; SB-I3.1 | One frame-edit refusal in SB-I5.1; other cases offline |
| SB-A13: completion | SB-I2.1–2.2; SB-I3.1–3.2 | Post-completion action in SB-I5.1 |
| SB-A14: partial failure | SB-I3.1; SB-I2.2 partial-prefix checks | Retained E4/native analysis; investigate if a failure occurs |
| SB-A15: package/evidence | SB-I1.2; SB-I4.1–4.2 | CI candidate used in SB-I5.1; final lineage in SB-I5.4 |
