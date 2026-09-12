# Feasibility and MVP Definition Roadmap

> Historical plan. The original story language below prescribed its work and
> criteria; it is not active work. Closure, owner acceptance, and the SB-D008 live-evidence
> exception are recorded in [PROJECT.md](../../PROJECT.md). The
> [current roadmap](../ROADMAP.md) is the maintenance placeholder; this archive retains
> the original scope and completion criteria.

## Outcome and authority

Produce a complete, evidence-backed MVP specification from which a separate
implementation roadmap can be planned. Establish whether the agreed C60 design
can be painted incrementally on the target game build, identify its actual
constraints, and resolve the decisions needed to implement it.

[PROJECT.md](../../PROJECT.md) owns current scope, roadmap/story status, gate results,
decisions, and acceptance. This roadmap defined work and completion criteria only.
The [concept](../../../CONCEPT.md) was its product baseline. Authoring or publishing
the original plan did not execute its stories or establish feasibility.

## Scope

Included:

- Identify the local target assembly and the minimum native surfaces needed for
  selection, placement, construction continuity, and resumption.
- Verify the reference geometry, polar orientation, twelve-patch sequence, and
  supported placement envelope against that target.
- Use focused offline checks and a minimal disposable in-game probe to settle
  behavior that inspection cannot establish.
- Define the MVP's observable behavior, restrictions, failure handling, evidence,
  and acceptance criteria; retain the reasons for material decisions.

Out of scope:

- Shipping the MVP, designing its production architecture, or writing its next
  implementation roadmap within this one.
- Alternative sphere designs, general blueprint editing/import, arbitrary
  existing-design conversion, geometry optimization, or a proof of global cost
  optimality.
- Automatic shell filling, material delivery, construction scheduling, or
  changing native construction rates, research unlocks, or placement limits.
- Broad compatibility/mod matrices, multiplayer support, performance benchmarking
  programmes, custom rendering, and polished UI/artwork.
- Rebuilding the established package pipeline, adding hosted game-assembly shims,
  Thunderstore publication, public releases, or redistribution of game code.
  A private test-probe handoff was allowed within the recorded test authorization;
  the CI artifact at that stage was a mock package.

## Evidence and execution rules

1. Use the local `Assembly-CSharp.dll` identified in
   [the target reference record](../../PROJECT.md#target-game-reference). A blueprint
   header, web page, or another mod's target is not the game-version authority.
   Recheck identity before consuming evidence; a changed assembly requires an
   explicit baseline decision and review of affected findings.
2. Distinguish native source inspection, deterministic derivation, compilation,
   live observations, and inference. Every decisive claim identifies its input
   hashes, relevant type/member or source location, procedure, result, and limit.
   Do not invent API names, numeric tolerances, or supported ranges in advance.
3. Investigate statically first. A runtime probe exists only to answer a named
   unresolved question. Use the smallest real-reference probe; no reusable test
   platform, compatibility framework, substitute game implementation, or new
   production subsystem is a deliverable here.
4. Before live testing, record the operator, matching game/loader context, and
   authorized access in PROJECT.md. Use a disposable test save/layer and a bounded
   procedure with cleanup. The drafting task authorized no installation, game
   launch, or save mutation. Existing authorization need not be requested again.
   If runtime access is unavailable, finish independent analysis and leave the
   dependent gate unmet; compile-only substitutes do not pass it.
5. Retain concise findings and reproducible probe/check source in the repo when
   useful. Keep generated output, binaries, dumps, local paths, and test saves in
   ignored `artifacts/`; never redistribute game assemblies or decompiled code.
   Record enough inputs, commands, and observed values to reproduce the finding
   after temporary artifacts disappear.
6. Follow AGENTS.md's bounded retry rule. A disproved hypothesis is a useful
   result, not a reason to keep adding mechanisms until the answer becomes yes.
   A story may finish with a supported, constrained, or not-feasible conclusion;
   its dependent gate still requires its stated evidence and product decision.
7. Prefer native allocation, validation, construction, and persistence behavior.
   Add a proposed mechanism only when a measured gap requires it. Keep failure
   and recovery investigation with the behavior they affect. Do not create a new
   document or test suite for every observation.

## Records produced during execution

- `docs/FEASIBILITY.md`: one evidence record, organised by story ID, with source
  identities, findings, repeatable checks, observations, limitations, and links
  to relevant probe source. No duplicate execution-status table.
- PROJECT.md decision entries: record material choices when they are made, not
  reconstructed at the end. Each entry has an ID, date, question, alternatives
  actually considered, evidence, chosen rule and rationale, decision-maker,
  consequences, and affected specification sections. Preserve superseded entries
  with a successor reference. Track unresolved decisions there too.
- `docs/MVP-SPECIFICATION.md`: the final behavioral contract and its evidence and
  acceptance-case references. Its approval/readiness state lives in PROJECT.md.

Create these outputs when their stories produce content; do not seed empty
reports now. Ordinary commands are not decisions. Technical choices within the
agreed scope may be settled from evidence; changes to player behavior, supported
scope, or the meaning of "perfect" require an owner decision before adoption.

## Phases, milestones, and gates

| Phase | Epic | Milestone | Exit gate |
| --- | --- | --- | --- |
| 1. Establish native evidence | SB-F1 | M1: Reproducible target and native operation map | G1: Target identified, relevant paths inspected, and remaining runtime questions assigned |
| 2. Prove the staged design | SB-F2 | M2: Exact patch sequence and candidate placement envelope | G2: Reference and all twelve deltas verified; native constraints evaluated; live cases bounded |
| 3. Establish behavioral feasibility | SB-F3 | M3: Evidence for a viable incremental workflow | G3: Required live observations obtained and core continuity risks resolved within an owner-approved scope |
| 4. Define the MVP | SB-F4 | M4: Complete MVP specification and planning handoff | G4: Specification traceable to evidence and decisions, no blocking unknowns, and owner acceptance recorded |

Each phase enters only after the preceding gate. Stories follow their listed
dependencies: placement limits are evaluated after the polar traversal is fixed.
Do not duplicate live checks across stories when one identified run can supply
the relevant observations. Gate results and activation belong in PROJECT.md.

If an essential promise proves infeasible, report the counterexample and pause
the dependent path for an owner scope decision. Do not quietly replace the
geometry, drop preservation, bypass native limits, or call a conditional draft
a completed MVP specification.

## Phase 1 — Epic SB-F1: Native integration evidence

**Return:** A reproducible account of the game inputs and native operations the
proposed interaction would depend on.

### SB-F1.1 — Identify the target and minimum probe environment

**Story:** As a maintainer, I want findings tied to the actual local game build
so the MVP is not specified against a sample header or another mod's runtime.

**Depends on:** Roadmap execution authorized in PROJECT.md.

**Scope:** Inspect the selected assembly and only the dependencies necessary for
the proposed native inspection and probe.

**Definition of done:**

- Reconfirm the recorded assembly hash and size. Record managed identity/MVID and
  the actual game-build identifier if reliably obtainable; distinguish it from
  file metadata. If the label cannot be established, use the hash as the exact
  target and state the limitation.
- Identify the relevant runtime/loader and minimum compile references from local
  evidence. Record reproducible inspection/build commands and missing prerequisites
  without selecting a framework merely because another repo uses it.
- Establish how a later live probe will identify a matching game, its test context,
  and operator. An unavailable prerequisite has a precise unblock condition.

**Produces:** Target/probe baseline in FEASIBILITY.md and the authoritative target
record in PROJECT.md.

**Excluded:** Plugin scaffolding, loader migration, dependency installation, live
execution, and a general machine/bootstrap inventory.

### SB-F1.2 — Trace the native placement and lifecycle contract

**Story:** As a maintainer, I want the actual native creation and selection paths
understood before choosing how a click can extend a layer safely.

**Depends on:** SB-F1.1.

**Scope:** Inspect the smallest relevant editor, sphere-layer, node/frame,
construction, and save/load paths in the target assembly.

**Definition of done:**

- Cite the types/members that resolve selected star/layer, create and identify
  nodes/frames, validate placement, refresh the editor, and provide a possible
  control integration point. Establish required call order, execution context,
  and relevant return/failure behavior.
- Identify which native records own positions, connectivity, construction progress,
  and shell associations; trace how selection changes, layer removal, and save/load
  affect their identity and lifetime.
- Contrast the native additive path with replacement import only as needed to
  establish preservation. Identify where progress could be reset or data partially
  changed, without assuming transactions or rollback support.
- Map each unresolved question to SB-F2 or SB-F3 and a bounded check. A missing
  essential surface is an explicit feasibility finding, not a guessed API.

**Produces:** Native operation map, constraints to evaluate, and targeted probe
questions in FEASIBILITY.md.

**Excluded:** Production hooks, custom allocation/persistence systems, broad
decompilation, UI implementation, and speculative compatibility guards.

**G1 / M1:** Both stories meet their definitions of done. The exact input and
candidate additive path are attributable. A fundamental missing capability blocks
dependent work until evidence or an owner scope decision resolves it; recording
the question alone does not pass the gate.

## Phase 2 — Epic SB-F2: Exact staged geometry

**Return:** A verified geometric plan for each click, with a defensible native
placement envelope rather than an untested universal-radius claim.

### SB-F2.1 — Establish the canonical reference geometry

**Story:** As a player, I want the framework to match the chosen optimized C60
design rather than a similar-looking equal-edge football.

**Depends on:** G1.

**Scope:** Inspect the full published reference and supplied sample, using the
generator/parser only as supporting material.

**Definition of done:**

- Pin the exact full blueprint/source revision and content hash. Record attribution
  and applicable reuse terms before adopting data or code; unresolved reuse
  constraints block incorporation rather than prompting copied implementation.
- Verify node/frame counts, adjacency, the twelve pentagons and twenty hexagons,
  and their intended edge-length relationships. Account for native arc/coordinate
  conventions and differences between the full blueprint and sample.
- Establish a canonical node/edge mapping and a reproducible numeric comparison
  rule. Derive tolerances from encoding and native precision; record measured
  deviations without labelling an approximation exact.

**Produces:** Reference identity and a checked geometry fixture/derivation, with
no game binaries or copied generator implementation.

**Excluded:** Searching for a better shape, proving optimality, generating a design
library, and treating successful parsing as placement evidence.

### SB-F2.2 — Prove the polar twelve-patch traversal

**Story:** As a player, I want each click to complete one predictable neighbouring
pentagon while the sphere grows continuously from pole to pole.

**Depends on:** SB-F2.1.

**Scope:** Derive and check one fixed orientation, ring order, and per-click delta
under the agreed missing-connections-plus-leading-spoke rule.

**Definition of done:**

- Verify that a rigid rotation of the reference supports the intended polar caps
  and 1–5–5–1 progression. Every step, including the equator crossing, uses an
  actual reference edge. Record the chosen orientation and deterministic order.
- For all twelve clicks, list the newly added/reused nodes and frames, the leading
  spoke where present, and which boundaries close. Check that the painted graph
  remains connected and includes no future elements except the leading endpoint
  and its spoke.
- Check shared vertices/edges are not duplicated, the last click adds no leading
  spoke, and the final union matches the full reference: 60 nodes, 90 frames, and
  all intended face boundaries. Do not mistake pentagons plus a single path for
  the complete framework.
- Repeating the derivation produces the same deltas. Record any conflict with the
  agreed traversal for decision; do not silently change its progression.

**Produces:** Reproducible patch sequence and geometric checks in FEASIBILITY.md.

**Excluded:** Adjustable routes/orientation controls, bulk-paint options, shell
creation, and runtime placement.

### SB-F2.3 — Determine the native placement envelope

**Story:** As a player, I want a requested patch to obey the game's real limits
and to understand any restriction before construction is exposed.

**Depends on:** SB-F2.2 and the native map from SB-F1.2.

**Scope:** Evaluate reference geometry against the target's actual radius,
latitude/research, coordinate, node/frame, and face constraints.

**Definition of done:**

- Derive which restrictions vary with radius, orientation, star/layer context,
  and unlock state. Distinguish coordinate scaling from legal native placement;
  do not transfer the sample's 81-degree header to the rotated plan without proof.
- Check every planned node/frame and the eventual faces against the applicable
  native predicates. Identify any difference between editor creation and import
  constraints and the exact path the proposed mod would use.
- State a candidate supported envelope with reasons and boundary cases. Use native
  predicates/derivations to cover a range; a few successful radii do not prove all
  radii or systems. Identify the live observations still required by SB-F3.3.
- Give each unsupported or unresolved case an observable prerequisite/refusal
  proposal and evidence. A restriction of the concept's intended scope remains a
  proposal until the owner decides it.

**Produces:** Placement-envelope findings and a small justified runtime case set.

**Excluded:** Unlock bypasses, silently altering geometry to fit, exhaustive seed
or radius sweeps, and modifying native validation.

**G2 / M2:** All three stories meet their definitions of done. The sequence and
candidate envelope are consistent, core deviations have an owner disposition,
and each required live claim has a concrete expected observation. Unproven radius
coverage is not advertised as support.

## Phase 3 — Epic SB-F3: Incremental construction continuity

**Return:** Direct evidence that the candidate operation adds the planned framework
without damaging existing work, and a defined boundary for continuing it later.

### SB-F3.1 — Prove additive placement and bounded failure behavior

**Story:** As a player, I want the next patch added while my earlier construction
continues, without losing anything already planned or built.

**Depends on:** G2 and the recorded live-test context/authorization.

**Scope:** Build only the disposable probe needed to apply checked deltas through
the native path and compare before/after state.

**Definition of done:**

- In a matching live game, apply the first patch and a following patch while the
  first is partially built; also cover existing completed structure. Verify the
  delta and reuse of the leading endpoint against SB-F2.2.
- Compare existing object identities, positions, connections, invested construction,
  and shell associations using the native fields identified in SB-F1.2. Account
  for legitimate construction during observation; unchanged snapshots alone are
  not the definition of preservation in a running game.
- Check representative rejected prerequisites and a reachable partial-failure path
  if the native operation permits one. Establish what may already have changed,
  whether a retry is safe, and the bounded stop/reconciliation rule needed to avoid
  duplication or damage. If no such path is reachable, justify that conclusion
  from the inspected native flow. Do not promise atomicity without evidence or
  add a fault-injection framework to manufacture cases.
- Record probe source revision, inputs, actual observations, and unresolved limits.
  Failure to demonstrate preservation leaves G3 unmet; it does not justify
  replacement import or an invented recovery framework.

**Produces:** Additive-placement and failure evidence with a repeatable procedure.

**Excluded:** Shipping plugin structure, general undo/rollback, automatic repair,
continuous automation, and changes to native construction.

### SB-F3.2 — Establish continuation and layer identity rules

**Story:** As a player, I want returning to my sphere to continue the right design
without painting into the wrong layer or duplicating earlier patches.

**Depends on:** SB-F3.1.

**Scope:** Determine the minimum reliable identification/resumption policy for a
design started by this workflow, including native user edits.

**Definition of done:**

- Probe closing/reopening the editor, switching selected layer/star, and save/reload
  at an intermediate patch and at completion. Determine which native information
  can establish design identity and the next missing delta after each transition.
- Check layer deletion/recreation or reused IDs, an unrelated non-empty layer,
  and a manual node/frame edit to the started design. Define when continuation
  succeeds or refuses without touching the wrong structure.
- Evaluate native reconstruction before proposing stored mod state. If extra state
  is necessary, identify only the information and lifetime justified by evidence;
  custom save formats and migrations are not assumed requirements.
- Record an explicit MVP proposal for interruption, ambiguity, missing elements,
  completion, and mod removal/restart. Do not claim arbitrary existing-design
  adoption or automatic repair. Resolve blocking scope decisions at discovery
  and reconcile them in SB-F4.1.

**Produces:** Evidence-backed design/layer identity and continuation rules.

**Excluded:** Blueprint recognition for unrelated designs, general history/repair
systems, cross-save migration, and a production persistence implementation.

### SB-F3.3 — Verify the complete player workflow and native shell handoff

**Story:** As a player, I want the whole twelve-click operation to remain usable
in the sphere editor, with normal shell filling left under my control.

**Depends on:** SB-F3.1 and SB-F3.2; use SB-F2.3's bounded case set.

**Scope:** Integrate only enough temporary editor control to exercise one explicit
paint action and collect remaining live observations in a coherent test session.

**Definition of done:**

- Complete the twelve-step traversal and compare live positions and connectivity
  against the checked deltas and SB-F2.1's numeric comparison rule, including
  ring closure, equator crossing, and final cap.
  Demonstrate advancement without waiting for prior construction to finish.
- Establish a feasible native editor location and selected-layer binding for the
  action. Check repeated deliberate clicks, invalid/no selection, editor exit,
  and clicking after completion; record expected feedback and any input conflict.
  One deliberate click means one next patch, not an invented queued batch.
- Exercise the live boundary cases justified in SB-F2.3 and record failures as well
  as successes. Combine these with the native-rule evidence to state exactly what
  radius/unlock/system coverage has been established.
- Verify normal player shell designation for both face types and any distinct
  native face restrictions. Include a player-filled shell before a later patch
  and confirm it is preserved. The probe itself creates no shells automatically.
- Supply one short, self-contained procedure for observations requiring a human:
  artifact identity, setup, actions, expected results, and cleanup. Record actual
  results; a prepared procedure or screenshot alone does not pass the story.

**Produces:** Full-flow evidence, remaining UI/placement constraints, and a compact
human observation record where needed.

**Excluded:** Final UI styling, custom previews, configuration panels, automatic
shell filling, repeated proof of already-settled arithmetic, and broad playtesting.

**G3 / M3:** All required live claims have observations tied to the target and
probe revision. Preservation, exact geometry, progression, targeting, and the
chosen continuation policy have no unresolved blocker. Restrictions have an
owner disposition before they are used to narrow the MVP. If live evidence is
unavailable or contradicts an essential promise, this gate does not pass.

## Phase 4 — Epic SB-F4: Evidence-backed MVP contract

**Return:** A complete, internally consistent specification and decision record
that lets the next roadmap plan implementation without rediscovering feasibility.

### SB-F4.1 — Resolve MVP constraints and material decisions

**Story:** As the owner, I want supported behavior and its tradeoffs made explicit
so the MVP reflects the agreed experience and what the evidence can support.

**Depends on:** G3. Record and resolve earlier blocking decisions at their point
of discovery; this story reconciles them rather than postponing all decisions.

**Scope:** Select the minimum supported behavior from the findings without adding
features or reopening settled choices without contrary evidence.

**Definition of done:**

- Reconcile concept promises with evidence for orientation/order, exactness,
  selected-layer/system scope, radius/unlocks, action feedback, construction and
  manual-shell preservation, resumption, edits, failure, and completion.
- For each material choice, complete its decision record in PROJECT.md with
  alternatives, evidence, rationale, consequences, and responsible decision-maker.
  Record owner decisions for narrowed scope or changed player behavior; do not
  convert recommendations into acceptance by silence.
- Separate supported MVP behavior, explicitly excluded behavior, and unknowns.
  Unknowns affecting an included promise are resolved by a bounded investigation
  or remain blockers. Excluding a non-core unknown needs a stated reason, not a
  new research programme.
- Reconcile superseded decisions and concept changes, preserving the history.
  Carry forward no unresolved contradiction into specification closure.

**Produces:** Coherent MVP constraints and complete material decision records.

**Excluded:** Feature expansion, estimates, production architecture, and creating
the next roadmap's epics or stories.

### SB-F4.2 — Complete and review the MVP specification

**Story:** As a maintainer, I want one testable MVP contract backed by evidence
so implementation planning can start from settled requirements.

**Depends on:** SB-F4.1.

**Scope:** Write and review MVP-SPECIFICATION.md using the established evidence
and decisions; define the handoff, not the implementation plan.

**Definition of done:**

- Specify purpose, scope/exclusions, target identity, prerequisites and supported
  envelope, geometry and patch deltas, action/selection states, construction and
  shell ownership, continuation, error/refusal/partial-failure behavior, and
  completion. Include required state/data lifetimes and native integration
  constraints only to the extent established by evidence.
- Give requirements stable identifiers and map each to evidence and, where
  applicable, a decision ID. Every acceptance case states setup, action, expected
  observable result, and validation boundary. Included claims have evidence;
  planned acceptance tests do not substitute for missing feasibility results.
- Review the contract against all twelve deltas and representative valid,
  boundary, interrupted, invalid, and completed scenarios. Resolve contradictions,
  missing outcomes, scope leakage, unsupported assertions, and requirements that
  introduce mechanisms without a demonstrated need.
- Identify the inputs and remaining implementation-only choices for the next
  roadmap. None may hide an unanswered core feasibility or product decision.
  Link retained probe/check source and the conditions for revalidating evidence.
- Present the complete specification and decision/evidence record for owner review.
  Record acceptance and the handoff in PROJECT.md; publication of this document
  alone does not establish owner acceptance or activate implementation.

**Produces:** Full MVP specification, traceable acceptance catalogue, and a bounded
handoff for planning the next roadmap.

**Excluded:** Implementing the MVP, creating an installable release, authoring its
implementation roadmap, and claiming runtime acceptance of a production mod.

**G4 / M4 — Final milestone:** The full MVP specification is accepted, its included
requirements are backed by the declared evidence, all material decisions are
recorded, and no unresolved question prevents implementation planning. PROJECT.md
records the milestone and the next planning boundary. Stop here.
