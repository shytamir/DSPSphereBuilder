# Project steering and state

## Authority

This is the sole authoritative record of accepted scope and steering decisions,
current phase, epic/story status, readiness, and owner acceptance. Update those
facts here only. Current owner instructions take precedence.

The [concept](../CONCEPT.md) describes the product behavior and reference geometry.
The [roadmap](management/ROADMAP.md) defines work and completion criteria, without
tracking its status. The [README](../README.md) introduces the project, the
[build guide](BUILD.md) defines build procedures, and [AGENTS.md](../AGENTS.md)
governs agent conduct. These documents link here for state.

## Current phase

Executing the [feasibility and MVP definition roadmap](management/ROADMAP.md).
The owner authorized sequential execution, a main-branch push with state updates
after every completed story, and evidence-based decisions that keep the MVP lean.
Stop for a substantial blocker. The owner will operate the later live probe.

| Area | Current state |
| --- | --- |
| Product concept | Agreed; recorded in [CONCEPT.md](../CONCEPT.md) |
| Repository preparation | Complete; local checks and hosted artifact inspection passed |
| Roadmap and active work | SB-F3.1 in progress; first live run failed before placement; corrected probe awaits retest |
| Feasibility gates and MVP specification | G1/M1 and G2/M2 passed; G3–G4 pending; specification not yet authored or accepted |
| Mod implementation | Disposable two-patch feasibility probe only; production implementation not started |
| Runtime validation and owner acceptance | First run established probe serialization failure; corrected build and serialization checks passed; additive validation pending |
| Distribution | Mock artifact only; no release or Thunderstore publication authorized |

## Accepted scope

The accepted product baseline is the [concept](../CONCEPT.md), including its
twelve-click polar progression, missing connections plus one leading spoke,
preservation of existing construction, and player-managed shells. Future changes
to that baseline require a steering decision here and a corresponding update to
the concept where its behavior changes.

This roadmap covers feasibility evidence and MVP constraints only. It does not
select a production architecture, deliver the playable MVP, or authorize the
next implementation roadmap. Repository preparation and its mock pipeline remain
the established foundation.

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

| Story | Execution state | Evidence |
| --- | --- | --- |
| SB-F1.1 | Complete | [Target and probe environment](FEASIBILITY.md#sb-f11--target-and-probe-environment) |
| SB-F1.2 | Complete | [Native placement and lifecycle](FEASIBILITY.md#sb-f12--native-placement-and-lifecycle) |
| SB-F2.1 | Complete | [Canonical reference geometry](FEASIBILITY.md#sb-f21--canonical-reference-geometry) |
| SB-F2.2 | Complete | [Polar twelve-patch traversal](FEASIBILITY.md#sb-f22--polar-twelve-patch-traversal) |
| SB-F2.3 | Complete | [Native placement envelope](FEASIBILITY.md#sb-f23--native-placement-envelope) |
| SB-F3.1 | In progress; serialization fix awaiting owner retest | [First live run and fix](FEASIBILITY.md#first-owner-run-serialization-failure-before-placement), [operator procedure](../probe/README.md) |
| SB-F3.2 | Not started | — |
| SB-F3.3 | Not started | — |
| SB-F4.1 | Not started | — |
| SB-F4.2 | Not started | — |

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

## Evidence and unresolved questions

- The concept preserves the supplied sample and reported format checks; those
  checks were not rerun during repository preparation.
- Canonical geometry, polar progression, and the static native placement envelope
  are verified. Additive preservation, continuation, editor interaction, and
  player shell filling still require matching live observations.
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

The owner replaces the probe DLL, restarts, repeats the brief refusal checks,
and resumes the [two-patch procedure](../probe/README.md) at case A in a disposable
save. The first failure was in probe serialization, before native placement;
its omitted snapshots cannot establish preservation. G2/M2 passed; G3 cannot pass on static
inspection or compilation. Complete SB-F3.1 from the identified live results
before proceeding to SB-F3.2. The next implementation roadmap remains downstream
of G4/M4; no full MVP specification or final acceptance is claimed.
