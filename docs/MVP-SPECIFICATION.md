# DSP Sphere Builder — MVP specification

## Purpose and authority

Let a player build the chosen C60 framework gradually by selecting a sphere layer
and clicking **Paint next patch**. Twelve deliberate clicks complete the fixed
polar progression while native construction continues and shell filling remains
under the player's control.

This document defines the behavioral contract for the next implementation plan.
[PROJECT.md](PROJECT.md) alone records scope acceptance, execution state, and
milestones. [CONCEPT.md](../CONCEPT.md) retains the original concept, references,
and illustrative five-spoke sample. That sample is not the first-click output.
Decision references below refer to PROJECT.md. SB-D003–009 establish the behavior;
SB-D011–013 add the production identity, delivery, and validation constraints.

## Scope

Included: one fixed reference design and orientation; incremental node/frame
placement; current selected-layer binding; preservation of native construction
and player-designated reference-face shells; continuation from native save data;
bounded feedback, refusal, completion, and failure behavior.

Excluded: arbitrary blueprint recognition/conversion, alternative geometry or
routes, orientation controls, bulk painting or automatic progression, shell
designation/filling, material delivery or construction scheduling, research or
native-limit bypasses, general repair/undo/history, custom save formats/migrations,
multiplayer, broad mod/version compatibility, custom rendering, configuration
panels, and mathematical optimality claims. Probe snapshot/rejection controls and
routine evidence-file export are not MVP features. No polished artwork or new
telemetry subsystem is required.

## Evidence and accepted assumptions

The requirements use the following evidence references. A requirement is an
implementation obligation, not a claim that production implementation exists.

| Reference | Basis |
| --- | --- |
| E1 | [Target and runtime](FEASIBILITY.md#sb-f11--target-and-probe-environment): inspected assembly identity and real-reference compilation |
| E2 | [Native operation map](FEASIBILITY.md#sb-f12--native-placement-and-lifecycle): placement, validation, selection, construction, and save/load source inspection |
| E3 | [Geometry](FEASIBILITY.md#sb-f21--canonical-reference-geometry), [all deltas](FEASIBILITY.md#sb-f22--polar-twelve-patch-traversal), and [native envelope](FEASIBILITY.md#sb-f23--native-placement-envelope): reproducible derivation and numeric checks |
| E4 | [Additive/rejection observations](FEASIBILITY.md#second-owner-run-additive-results-and-case-b-gap) and [completed-shell retest](FEASIBILITY.md#completed-shell-retest): matching live native preservation and non-atomic call behavior |
| E5 | [Continuation investigation](FEASIBILITY.md#sb-f32--continuation-probe-preparation) and [owner run](FEASIBILITY.md#owner-continuation-run): local recognition checks and live editor/layer return, menu reload, edits, and ID reuse |
| E6 | [Full workflow and limits](FEASIBILITY.md#sb-f33--full-workflow-evidence-and-accepted-limits): full 60/90 graph at radius 9,700, rapid progression, retained pentagon shells, completion/no-selection refusals, and owner UI observations |
| E7 | [Build contract](BUILD.md) and [reference provenance](../research/cosmin1490/README.md): existing package/version conventions and retained asset/license identities |

Under [SB-D008](PROJECT.md#sb-d008--accept-remaining-unverified-cases-and-proceed-to-specification),
the owner explicitly accepted the following unverified cases instead of requiring
another feasibility run. They remain part of the intended MVP behavior; acceptance
does not make them measured results.

| Accepted assumption | Supporting basis and missing live observation |
| --- | --- |
| W1: radius envelope | E3's unit-direction predicates and scale checks support native legal radii. E6 verifies the full graph at the logged minimum 9,700 in star 60; no C60 maximum-radius or full both-face endpoint test was run. |
| W2: manual hexagon shells | E3 validates both face classes and later additions outside prior faces. E4/E6 observe pentagon preservation; manual hexagon filling and addition beside it were not exercised. |
| W3: other systems, including giants | E2/E3 resolve the viewed sphere and its native bounds. The owner switched stars and returned; no Paint report targets another star. |
| W4: research refusal | E2/E3 establish the rounded native latitude predicate and the required value 68. Live successful runs use 90; a below-threshold refusal and named research levels were not observed. |
| W5: multiple selected layers | E2 returns a layer only for exactly one selected layer. No-selection refusal is observed; the multiple-selection case was not exercised. |
| W6: process restart and mod removal | Native save data owns the graph, and E5 observes reconstruction after menu reload. Full application exit/relaunch and removal/reinstallation were not exercised in that run. |

These are explicit validation limits, not requests for further feasibility work.
Later contradictory evidence requires a fix or an owner scope decision. This
specification does not claim universal tested compatibility or owner acceptance
of a future executable build.

## Target and supported envelope

| ID | Requirement | Basis |
| --- | --- | --- |
| SB-MVP-01 | Implement against the recorded local `Assembly-CSharp.dll`: SHA-256 `AE0BA95F75BD879A62AA4CE253B2AB78EAA4FB3C7C595F5E1FEE75EBE0E0EF85`, MVID `ece4a40e-5e73-43f4-a9f8-4e74970b5942`, 7,830,016 bytes. The known baseline is GameConfig `0.10.34`, Unity `2022.3.62f3c1`, BepInEx `5.4.17.0`, and a successful `netstandard2.1` probe build. The full displayed game-build suffix is unknown; do not infer it from the blueprint header. | E1; SB-D002,009 |
| SB-MVP-02 | Operate on one native-created sphere layer in the currently viewed system, at its existing game-supported radius. Do not create, resize, select, or reorient layers automatically. Support the native legal-radius envelope, including giant-star rules; do not impose an arbitrary radius/star whitelist. | E2/E3/E6, W1/W3; SB-D006,008 |
| SB-MVP-03 | Before adding anything, require a running native game/editor context, exactly one selected layer, rounded `GameMain.history.dysonNodeLatitude` at least 68, a recognized empty/prefix graph, and finite planned positions. Use the actual native value, not a guessed technology name or the sample's 81° header. Below the threshold, refuse the whole patch; do not paint a low-latitude subset. | E2/E3/E5, W4/W5; SB-D006–009 |

The target hash identifies the baseline and triggers revalidation when it changes;
a production hard-hash compatibility gate is not required. No claim extends to
another game/loader version without reviewing the affected native surfaces.
Under SB-D013, CI uses compile-only type-reference shims, with target-backed
type/member mappings updated in every commit adding or changing a referenced
surface. Local real-reference compilation remains a separate required check;
shims neither implement game behavior nor establish runtime compatibility.
Native layer-creation constraints remain the game's responsibility. Applying its
creation-time radius check to an already-existing layer would collide with that
layer itself and is not an appropriate Paint prerequisite.

## Geometry and patch sequence

| ID | Requirement | Basis |
| --- | --- | --- |
| SB-MVP-04 | Reproduce the optimized reference: 60 nodes and 90 frames, comprising twelve equal regular pentagons and twenty hexagons with alternating edge lengths, to the declared numeric precision. Do not replace it with an equal-edge football, optimize it further, or claim proven global cost optimality. | E3/E6; SB-D004 |
| SB-MVP-05 | Use the fixed P1-to-P4 polar orientation and route below. Derive each node independently from the pinned reference and rigid rotation; scale its normalized direction by layer radius. Use layer-local positions and non-Euler great-circle frames, independent of animated layer rotation. | E2/E3/E6; SB-D005–006 |
| SB-MVP-06 | Each successful click adds exactly the next table row: its pentagon perimeter, all listed closing connections, and one leading spoke/endpoint except on the final click. Reuse the incoming endpoint and every existing element. Add no other future node/frame. | E3/E4/E6; SB-D005 |
| SB-MVP-07 | Compare newly generated node directions with the derived reference using Euclidean distance between normalized vectors at most `4u/(1-u) = 2.384185934 × 10^-7`, where `u = 2^-24`; require exact graph connectivity. Relative radius error must not exceed `10u/(1-10u)`, the float arithmetic allowance defined below. Never accumulate coordinates from previous placements or move existing nodes to satisfy a comparison. | E3/E5/E6; SB-D004,007 |

The pinned input is [Cosmin1490's retained 60.txt](../research/cosmin1490/60.txt),
upstream revision `bf00f4b2334c93215f63e0291f9acb6003c9a663`, SHA-256
`96bd7badd6d0bd971df477b74622c0296b483e7b60e77aa3074cdf0b239620e0`.
E3 records its measured difference from the published sphere. Source node IDs
1–60 are canonical labels, not required native pool IDs. Pentagon Pk owns
`5k-4` through `5k`, joined consecutively and closed back to its first vertex.

To define the rotation, normalize source directions; set north to the normalized
mean of P1's five directions. Set +X to node 1's direction projected perpendicular
to north, then normalize it; set +Z to `X × north`. Transform each direction by
dot products with X, north, and Z. Use the full-precision derivation in
[derive_patches.py](../scripts/derive_patches.py), not rounded display matrices.
P4 is the opposite cap. The ring centers lie near ±26.565052°; the maximum vertex
latitude is approximately ±67.607230°. Cap centers are not extra nodes.

The route is **P1 → P2 → P6 → P12 → P11 → P8 → P7 → P9 → P10 → P5 → P3 → P4**.
The first six pentagons complete the northern group. The leading spoke 35–38 on
click 6 crosses toward the lower group. Each row adds five perimeter frames of
its Pk plus the listed connections. Edges are unordered. A numeric range in New
nodes is inclusive.

| Click / Pk | New nodes | Reused node | Other closing connections | Leading spoke | Total nodes / frames |
| --- | --- | --- | --- | --- | --- |
| 1 / P1 | 1–6 | — | — | 4–6 | 6 / 6 |
| 2 / P2 | 7,8,9,10,27 | 6 | — | 10–27 | 11 / 12 |
| 3 / P6 | 26,28,29,30,56 | 27 | 5–26 | 30–56 | 16 / 19 |
| 4 / P12 | 54,57,58,59,60 | 56 | 1–60 | 54–59 | 21 / 26 |
| 5 / P11 | 37,51,52,53,55 | 54 | 2–55 | 37–51 | 26 / 33 |
| 6 / P8 | 35,36,38,39,40 | 37 | 3–36; 7–40 | 35–38 | 31 / 41 |
| 7 / P7 | 31,32,33,34,42 | 35 | 31–52 | 34–42 | 36 / 48 |
| 8 / P9 | 41,43,44,45,49 | 42 | 8–45; 39–41 | 44–49 | 41 / 56 |
| 9 / P10 | 22,46,47,48,50 | 49 | 9–50; 28–46 | 22–47 | 46 / 64 |
| 10 / P5 | 12,21,23,24,25 | 22 | 21–29; 25–57 | 12–24 | 51 / 72 |
| 11 / P3 | 11,13,14,15,19 | 12 | 11–58; 14–32; 15–53 | 13–19 | 56 / 81 |
| 12 / P4 | 16,17,18,20 | 19 | 16–48; 17–43; 18–33; 20–23 | — | 60 / 90 |

The 32 reference face boundaries and their closure steps are defined by E3's
derivation. The first hexagon closes on click 3 between P1, P2, and P6. Hexagon
closure means its boundary exists; it does not create a shell.

## Player action and feedback

| ID | Requirement | Basis |
| --- | --- | --- |
| SB-MVP-08 | Provide one explicit **Paint next patch** action within the native Dyson sphere editor. Bind it to the selected layer at the time of the click; never use a previously selected target. Outside a running editor context it cannot mutate a sphere. | E2/E5/E6; SB-D009 |
| SB-MVP-09 | One deliberate click performs at most one synchronous patch addition on the native UI thread. Permit the next deliberate click without waiting for construction. Do not queue clicks, run background construction, or auto-advance after time, reload, or selection changes. | E2/E4/E6; SB-D009 |
| SB-MVP-10 | Give concise feedback for the selected target: unavailable prerequisite, ready to start/continue, patch applied, completed, unmatched graph, or stopped after failure. A successful action identifies progress through the twelve patches. Disabled controls must explain why they are unavailable; feedback must not present a previous layer's progress as current. Exact wording and visual styling are implementation choices. | E5/E6; SB-D009 |

| State at action time | Result |
| --- | --- |
| No running editor or not exactly one selected layer | No additions; unavailable/refusal feedback where the control is visible |
| Session stopped after unexpected failure | No further Paint additions, regardless of selection or menu reload |
| Insufficient rounded latitude or invalid planned position | Refuse without partial placement |
| Empty native layer: no nodes, frames, or shells | If prerequisites hold, add patch 1 |
| Unique matching prefix k, 1 ≤ k < 12 | If prerequisites hold, add patch k+1 |
| Complete matching graph | No additions; show completion |
| Nonempty graph not uniquely matching a prefix | No additions; explain that the layer does not match the staged design |

Multiple simultaneous refusal reasons need not have a prescribed display order;
none permits mutation. Completion concerns the planned framework, not delivery of
rockets or sails. Native construction continues independently of Paint availability.

## Native construction and shell ownership

| ID | Requirement | Basis |
| --- | --- | --- |
| SB-MVP-11 | Add nodes and frames through the native additive path. Create each delta's nodes before its frames; resolve endpoints through the canonical-to-native mapping. On this target use creation prototype 0 for nodes and frames and `euler=false`. Do not extend by blueprint import, pool replacement, or a custom allocator. | E2/E3/E4; SB-D003,006 |
| SB-MVP-12 | Preserve every preexisting node/frame/shell object within an ordinary successful action, its position, properties, invested construction, and earlier connectivity. New frames may extend endpoint adjacency and native derived request totals. Legitimate construction increases are allowed; unchanged aggregate request counters are not the preservation criterion. | E2/E4/E6; SB-D003,009 |
| SB-MVP-13 | Leave shell designation and filling entirely to the player. Allow existing shells on closed reference pentagon/hexagon boundaries and preserve their boundaries, frame/node associations, properties, and invested CP during later addition. Never add or remove a shell automatically. | E3/E4/E6, W2; SB-D008–009 |
| SB-MVP-14 | Leave native material requirements, construction rate, sail absorption, research, and frame segmentation/cost intact. Do not promise a fixed rocket cost for all radii. | E2/E3; SB-D006,009 |

The native constructors do not repeat all brush validation. The fixed-plan
derivation and recognized-prefix boundary establish the checked geometry; runtime
context/unlock/finite-position checks still precede allocation. No need is
established for a general replacement placement validator.

## Continuation and identity

| ID | Requirement | Basis |
| --- | --- | --- |
| SB-MVP-15 | Reconstruct the next delta from the selected layer's native graph on each action. Accept only an empty layer or a unique complete prefix of the fixed plan. Native pool order and numeric star/layer/node/frame IDs are not durable design ownership or a saved patch counter. | E2/E5; SB-D007,009 |
| SB-MVP-16 | Match nodes uniquely by expected layer-local position with Euclidean distance at most `radius × 10u/(1-10u)` (`≈ radius × 5.960468 × 10^-7`). Require the exact prefix's unordered frame set, non-Euler mode, and reference-face shell boundaries using present frames. Ignore construction amount and cosmetic colors when finding the prefix. Never snap, move, replace, or repair a record during recognition. | E3/E5; SB-D007,009 |
| SB-MVP-17 | Closing/reopening the editor, switching layer/star, or loading a save must not advance progress or bind the next action to stale native references. Recompute against current native content. Recreated IDs start from that recreated layer's content. | E2/E5, W3/W6; SB-D007–009 |
| SB-MVP-18 | Refuse unrelated, ambiguous, incomplete-delta, or edited non-prefix graphs without mutation. An edit that leaves an exact earlier prefix is treated as that prefix; an identical matching design created by another route is indistinguishable. This is content matching, not a provenance or arbitrary-blueprint adoption guarantee. | E5; SB-D007,009 |
| SB-MVP-19 | Store no mod-specific progress in a save, sidecar, layer registry, or report file. Native save data owns positions, links, construction, and shells. Plugin state is limited to transient UI/action/error handling. Removal leaves native construction in the save; reinstallation may continue a matching prefix. | E2/E5, W6; SB-D007–009 |

SB-MVP-07 governs generation/comparison of new reference nodes. SB-MVP-16 has a
separate allowance for recognizing existing float coordinates across evaluations;
it is not permission to alter the design. A sub-tolerance coordinate difference
is intentionally indistinguishable from arithmetic rounding. The rounding basis
and measured cross-runtime discrepancy are recorded in E5.

## Refusal, interruption, and failure

| ID | Requirement | Basis |
| --- | --- | --- |
| SB-MVP-20 | Expected refusal occurs before any node/frame mutation. After completion, repeated Paint attempts are harmless and add no duplicate nodes, frames, or leading spoke. | E4/E5/E6, W4/W5; SB-D009 |
| SB-MVP-21 | Treat native frame-creation return zero, unexpected exception, or detected preservation/result mismatch as a stopped action. Stop further Paint mutations for that plugin session, give clear feedback that partial additions may remain, and log enough context to diagnose the action: build/target, selected star/layer/radius, intended patch, and available failure details. Do not automatically retry or undo. | E2/E4/E5; SB-D009 |
| SB-MVP-22 | Do not promise atomicity. Earlier successful native additions may survive a later failed call or interruption. A fresh plugin session reevaluates the graph: a full matching prefix can continue, a full graph is complete, and any unmatched partial result refuses. Do not persist a speculative recovery transaction or failure marker. | E2/E4/E5, W6; SB-D007,009 |

A menu reload does not clear the session's error stop. A deliberate application
restart is not automatic recovery and cannot make an unmatched graph valid.
If all native additions finished before an interruption, recognition accounts
for the resulting complete prefix rather than duplicating that patch. Unexpected
failure diagnostics need no continuous snapshot archive, game-save export, or
new retry framework. A prototype failure is investigated before further testing.

## Delivery and retained evidence

The production plugin GUID is **`dsp.spherebuilder`** (SB-D011). The implementation
roadmap must deliver the actual CI-built mod as a directly usable package download,
without a wrapper directory or nested package ZIP. Shim assemblies are build
inputs and must not be shipped. Publication polish and upload remain later work;
SB-D012 places owner/runtime validation immediately before the UI workshop.

| ID | Requirement | Basis |
| --- | --- | --- |
| SB-MVP-23 | Keep the established VERSION and sequential-build contract when later producing the executable package: numeric `MAJOR.MINOR.BuildNumber` for Thunderstore and the short commit in diagnostic build identity. Do not replace the build sequence with a hash or retry count. Packaging extension belongs to the next implementation plan. | E7; SB-D009 |
| SB-MVP-24 | Retain reference attribution and the applicable license/source material when distributing derived geometry. Preserve the upstream fixture/license identity separately from the original project's license. Do not bundle game, Unity, or BepInEx binaries or decompiled game code. | E1/E7; SB-D004,009 |
| SB-MVP-25 | Retain the focused geometry/envelope derivation, relevant recognition/serialization checks, and attributable feasibility records as implementation inputs. Revalidate affected requirements when target APIs/serialization, reference data, orientation/deltas, arithmetic, or native integration materially changes. Do not build a broad compatibility or benchmark programme without a demonstrated need. | E1–E6; SB-D002,009 |

Useful implementation inputs are the [probe](../probe/Probe.cs),
[recognizer](../probe/GraphRecognition.cs), [preservation snapshots](../probe/Evidence.cs),
[focused local checks](../tests/probe-json/Program.cs),
[geometry tests](../scripts/test_geometry.py), and
[envelope calculation](../scripts/check_envelope.py). They are research/prototype
assets, not a mandate to ship the probe's structure or diagnostic controls.
The existing pipeline remains a mock package; this document authorizes no release,
Thunderstore publication, game installation changes, or next-roadmap execution.

## Acceptance catalogue

These cases define observable targets for the implementation. They are not a
request to repeat the waived feasibility cases now. "Observed" below refers to
the identified prototype evidence, not validation of a production mod.

| Case / requirements | Setup and action | Expected result | Validation boundary |
| --- | --- | --- | --- |
| SB-A01 — 01,03,25 | Build against the pinned local references and mapped CI references; identify inputs and artifacts | Correct attribution and successful local real-reference compilation; CI shim declarations match the target mapping and do not substitute for the real-reference check | E1/probe compilation observed; production mapping/compilation and runtime validation remain distinct obligations under SB-D013 |
| SB-A02 — 03,08,10,20 | With no layer, then with multiple layers selected, request Paint; also leave the editor | No nodes/frames added and no stale target used; actionable feedback where visible | No-selection refusal and editor return observed; multiple selection W5 |
| SB-A03 — 03,10,14,20 | Use a native unlock below rounded 68, then a sufficient native unlock, on an otherwise eligible empty layer; request Paint | First request adds nothing; sufficient state adds exactly 6 nodes / 6 frames; research is unchanged | Sufficient value 90 observed; below-threshold case W4; names/increments not assumed |
| SB-A04 — 04–07,09,11 | Begin empty at a legal radius; click through all twelve rows without waiting | Every per-click delta matches the table, remains connected, and reuses shared elements; final 60 / 90 with no extra spoke; numeric comparisons pass | E3 derivation and E4/E6 combined live deltas; full graph observed at 9,700 |
| SB-A05 — 09,12,14 | Allow some earlier construction to arrive, then add the next patch; repeat beside completed structure | Existing identities/positions/properties/links and invested SP retained; only valid new adjacency/request bookkeeping changes | E4 observed for partial and completed construction |
| SB-A06 — 12,13,16 | On a closed pentagon and hexagon, designate shells through native controls before continuing Paint | Both native shells remain valid and retain progress/associations; later delta adds no shell | Pentagon observed; hexagon and both-face endpoint coverage W2/W1 |
| SB-A07 — 02,04,05,13,14 | Use the smallest and largest radii accepted by a star's native controls; complete the design and designate both face types | Same normalized design; native radius/cost rules respected; both shell types usable | E3 static; minimum-radius framework observed, remaining endpoint cases W1/W2 |
| SB-A08 — 02,08,15,17 | Switch between started layers and stars, including a giant where native creation is legal; Paint on the current selection | Only that selected layer advances its own prefix; earlier layers remain intact | Same-star layers and star return observed; other-system painting W3 |
| SB-A09 — 08,15,17,19 | At an intermediate prefix and at completion, close/reopen the editor and reload saved state; request Paint | Intermediate graph resumes at its next delta; complete graph refuses; no cached-reference dependence | E5 menu reload/editor return observed |
| SB-A10 — 15,17,19,22 | Save a prefix or complete graph, fully exit/relaunch; repeat with the mod removed and reinstalled | Native construction remains; reinstalled mod recognizes current content; no sidecar/mod-progress dependency | E2 static and local reconstruction checks; process/removal coverage W6 |
| SB-A11 — 15,17,18,20 | Delete a started layer, recreate a layer reusing its ID; add an unrelated node and request Paint, then remove it and start empty | Refusal for unrelated content; new first patch from empty; no previous index reused | E5 observed, including changed radius and reused layer ID |
| SB-A12 — 10,15,16,18,20 | Remove a frame/node from a prefix, or provide an extra/duplicate/displaced/ambiguous element; request Paint | Non-prefix content refuses without mutation; exact earlier prefix is treated as its current state; float-rounding differences inside the bound do not trigger repair | Native deletions observed; other recognition cases locally checked; historical provenance is not detectable |
| SB-A13 — 06,10,20 | Complete patch 12 and request further Paint actions | Completion feedback, 60 / 90 unchanged, no new spoke or duplicated element | E5/E6 observed |
| SB-A14 — 12,21,22 | Examine the retained native rejection experiment and partial-prefix checks; on any reachable unexpected placement failure inspect the result and attempt further Paint in that session | Earlier native additions may remain; session stops, diagnostic context is available, no automatic retry/rollback; a partial non-prefix refuses after fresh reconstruction | Native rejection observed; session stop is established by source and partial graphs checked locally. No spontaneous valid-patch exception was manufactured |
| SB-A15 — 23–25 | Inspect the later executable package/build identity and retained reference/check inputs | Numeric sequential version, attributable commit, required source credit/licenses, no bundled dependency/game binaries; changes identify affected revalidation | Existing mock pipeline and probe packaging observed; executable package is future implementation work |

## Implementation-planning handoff

The settled behavioral inputs are this requirement/acceptance catalogue, the
pinned target and reference, fixed delta derivation, native operation map,
SB-D003–009, and the observed/accepted boundaries above. No additional feasibility
programme is a prerequisite under the owner's acceptance decision.

The [implementation roadmap](management/ROADMAP.md) may decide project/source
layout, exact native UI placement/styling, reference-shim wiring and mapping checks,
diagnostic formatting, assembly-version mapping, and the direct package delivery
mechanism. Production GUID and CI reference strategy are fixed by SB-D011/013.
These choices must preserve this contract;
they do not authorize new product features, custom persistence, repair, or wider
compatibility work. They are implementation choices, not hidden unanswered core
behavior questions.

Only PROJECT.md records specification acceptance, the archived G4/M4 handoff,
and authorization/readiness for implementation. This contract and its acceptance
catalogue do not themselves activate the next roadmap or accept an executable.
