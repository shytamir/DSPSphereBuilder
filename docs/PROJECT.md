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

The first [feasibility and MVP definition roadmap](management/ROADMAP.md) is
authored. Its outcome is an evidence-backed MVP specification for a subsequent
implementation roadmap. The current request authorizes planning and publication
of the roadmap, not execution of its investigations or runtime probes.

| Area | Current state |
| --- | --- |
| Product concept | Agreed; recorded in [CONCEPT.md](../CONCEPT.md) |
| Repository preparation | Complete; local checks and hosted artifact inspection passed |
| Roadmap and active work | SB-F1 through SB-F4 defined; no execution story active |
| Feasibility gates and MVP specification | G1–G4 not passed; specification not yet authored or accepted |
| Mod implementation | Not started; no plugin source or runtime scaffold |
| Runtime validation and owner acceptance | Not performed for the proposed mod |
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
SB-F1.1 must recheck it, identify the managed/runtime context, and determine a
reliable game-build label if available. The sample blueprint's version header
does not substitute for this target. No game code was loaded or executed during
this planning task.

## Work tracking

| Epic | Stories | Execution state |
| --- | --- | --- |
| SB-F1: Native integration evidence | SB-F1.1, SB-F1.2 | Not started |
| SB-F2: Exact staged geometry | SB-F2.1, SB-F2.2, SB-F2.3 | Not started |
| SB-F3: Incremental construction continuity | SB-F3.1, SB-F3.2, SB-F3.3 | Not started |
| SB-F4: Evidence-backed MVP contract | SB-F4.1, SB-F4.2 | Not started |

The grouped state applies to every listed story until execution requires separate
rows. Record story progress, gate/milestone outcomes, and concise evidence links
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

Record subsequent material choices here using the roadmap's decision fields.
No native integration, orientation tolerance, supported placement envelope,
continuation policy, or runtime-test authorization has yet been settled by this
roadmap's execution. Those questions belong to its identified early investigations.

## Evidence and unresolved questions

- The concept preserves the supplied sample and reported format checks; those
  checks were not rerun during repository preparation.
- Pole-centred orientation, a valid route through both pentagon rings, native
  placement constraints, and additive preservation still require the targeted
  verification identified in the concept.
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

Authorize execution of the feasibility roadmap, beginning with SB-F1.1. Later
runtime work needs an identified matching test context and authorized access.
The next implementation roadmap is planned only after the full MVP specification
passes G4/M4; no feasibility story or gate is claimed complete by this document.
