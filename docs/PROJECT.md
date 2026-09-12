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

Ready for implementation planning. Repository preparation is complete: the agreed
concept is preserved, working guidance and management placeholders are in place,
and the mock packaging pipeline has passed locally and on GitHub Actions.

| Area | Current state |
| --- | --- |
| Product concept | Agreed; recorded in [CONCEPT.md](../CONCEPT.md) |
| Repository preparation | Complete; local checks and hosted artifact inspection passed |
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
- [Hosted build 1](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34662682423)
  passed checkout, package creation/validation, and artifact upload for commit
  `79a72be46a4b81bc3ed93bf6a1a5b264202dec22`. Its downloaded ZIP was inspected
  independently: exactly `manifest.json`, `README.md`, `icon.png`, and `LICENSE`,
  with valid metadata, UTF-8 text, and a decoded 256x256 PNG. Package version
  `0.1.1` and build label `0.1.1.79a72be` matched the run and source commit.
  This is the preparation baseline, not a rolling latest-build record.

## Next decision

Begin bounded implementation planning when requested by the owner. Use the
concept's outstanding verification to inform that planning; do not populate the
roadmap with speculative architecture or inherited work from reference repos.
