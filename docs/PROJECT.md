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

Repository preparation, before implementation planning. The owner accepted the
core concept and requested repository guidance, management placeholders, and a
working mock Thunderstore packaging pipeline. Preparation may be committed and
pushed to `main` as it is completed.

| Area | Current state |
| --- | --- |
| Product concept | Agreed; recorded in [CONCEPT.md](../CONCEPT.md) |
| Repository preparation | Local validation passed; hosted packaging verification pending |
| Roadmap and active work | Placeholder only; no epics or stories defined or active |
| Mod implementation | Not started; no plugin source or runtime scaffold |
| Runtime validation and owner acceptance | Not performed for the proposed mod |
| Distribution | Mock artifact only; no release or Thunderstore publication authorized |

## Accepted scope

The accepted product baseline is the [concept](../CONCEPT.md), including its
twelve-click polar progression, missing connections plus one leading spoke,
preservation of existing construction, and player-managed shells. Future changes
to that baseline require a steering decision here and a corresponding update to
the concept where its behavior changes.

The preparation task establishes documentation and packaging only. It does not
select a runtime architecture, define implementation stories, add a plugin,
install anything into the game, or establish in-game feasibility.

## Work tracking

When planning defines work, add its identifiers, current status, and concise
evidence links here. Keep purpose, scope, dependencies, and completion criteria
in the roadmap. Do not maintain a second status table there or in the README.

Technical validation, owner acceptance, and publication are separate facts. A
passing mock package build establishes the packaging path, not gameplay behavior
or Thunderstore moderation acceptance.

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
- Hosted preparation evidence: to be recorded after the first successful workflow
  run and inspection of its downloaded package.

## Next decision

Begin bounded implementation planning when requested by the owner. Use the
concept's outstanding verification to inform that planning; do not populate the
roadmap with speculative architecture or inherited work from reference repos.
