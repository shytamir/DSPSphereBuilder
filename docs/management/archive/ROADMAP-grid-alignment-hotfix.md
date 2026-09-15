# Pentagon grid alignment hotfix

> Historical plan, archived on 2026-09-15 after owner acceptance of build 1.0.60.
> Its story language records the original scope and completion criteria.
> [PROJECT.md](../../PROJECT.md) owns acceptance, closure and subsequent promotion.
> The [current roadmap](../ROADMAP.md) defines the maintenance boundary.

## Outcome and authority

Align the twelve pentagon centers of new Sphere Builder designs with the native
icosahedral drawing grid. Continue already painted 1.0.56 layers in their original
orientation, preserving every existing structure. Deliver a validated hotfix
candidate for one focused owner acceptance session; publication remains manual.

[PROJECT.md](../../PROJECT.md) owns authorization, decisions, story status and gate
results. The [MVP specification](../../MVP-SPECIFICATION.md) owns behavior. The
[alignment evidence](../../GRID-ALIGNMENT.md) records measurements and validation.

## Epic SB-H1 — Align new designs without stranding existing layers

**Value:** The native grid gives players a consistent visual reference for the
whole sphere, while work begun before the correction remains usable.

**Included:** One measured rigid rotation, recognition and continuation of the
two known orientations, focused regression checks and a traceable CI candidate.

**Excluded:** Moving existing nodes, deforming or optimizing the geometry,
arbitrary rotation matching or controls, Repair Mode, new UI, shell automation,
new native APIs, dependency modernization and a replacement build system.
Keep the twelve-click route, frame topology, latitude requirement, GUID and
existing version translation. Do not assign a future CI build number in advance.

### SB-H1.1 — Establish the correction

**Story:** As a player, I want alignment based on the game's actual grid so that
all twelve pentagons have the intended centers.

**Scope:** Inspect the recorded target's native grid resource and editor
transforms; compare its pentagon centers with the existing design.

**Done when:** Resource identity, extraction method, coordinate convention,
rotation and numeric allowance are recorded; all twelve centers match uniquely
under one rigid rotation without changing the design's dimensions or topology.
If that model fails, resolve the diagnosis before changing placement.

**Produces:** Reproducible measurements and the orientation decision.
**Excluded:** Runtime modification, visual acceptance and arbitrary mesh tooling.

### SB-H1.2 — Correct placement and preserve continuation

**Depends on:** SB-H1.1 and HG1.

**Story:** As a player, I want new layers aligned and unfinished older layers
continued where I left them, without repositioning construction.

**Scope:** Derive the corrected directions; recognize the two fixed orientations
from native content and use the recognized orientation for subsequent patches.
Update the behavior contract and existing logic/geometry checks.

**Done when:** Empty layers choose the corrected orientation; every valid old and
new prefix continues to 60 nodes and 90 frames in the same twelve-step route.
Checks independently confirm all twelve native centers, unchanged edge lengths
and latitude envelope, old coordinates, shell/preservation behavior and reload
reconstruction. Mixed or unsupported orientations remain refused. No native
reference changes are needed; any demonstrated exception must be inspected and
mapped in the commit introducing it.

**Produces:** Tested production correction and updated contract.
**Excluded:** Save migration, repair, tolerances widened to accept wrong layouts,
and runtime claims based on offline checks.

### SB-H1.3 — Validate the hotfix candidate

**Depends on:** SB-H1.2.

**Story:** As the owner, I want one identified, checked package to validate before
deciding whether to publish the correction.

**Scope:** Run the existing local native-reference build and package checks;
verify the clean CI artifact and conduct one final owner session.

**Done when:** The downloaded package matches its CI source/version/hashes and
passes existing metadata/content checks. The owner confirms a completed new
sphere against the pentagon grid and continuation of an older partial layer
after reload, with no displaced construction or observed regression. Record
observations separately from static results and explicit acceptance separately
from publication. A failed check remains open with its actual evidence.

**Produces:** Identified hotfix candidate, concise validation record and owner
disposition. Supply brief reproduction steps only after agent checks pass.
**Excluded:** Replaying the MVP acceptance matrix, reopening accepted evidence
gaps, tags, GitHub releases and Thunderstore uploads.

## Phases, gates and milestones

| Phase | Milestone | Exit gate |
| --- | --- | --- |
| 1. Measurement — SB-H1.1 | HM1: Correction established | HG1: One rigid rotation matches all twelve native centers within justified precision |
| 2. Correction and delivery — SB-H1.2, agent portion of SB-H1.3 | HM2: Verified candidate ready | HG2: Logic, geometry, native compilation and downloaded-package checks pass |
| 3. Owner validation — remainder of SB-H1.3 | HM3: Accepted hotfix candidate | HG3: Owner observations and explicit acceptance identify the tested candidate |

Work in story order. Record each completed story and material decision in
PROJECT.md alongside its changes; commit and push under the owner's execution
authorization. Keep raw assets, decompilation and temporary tools in ignored
`artifacts/`. Use measured fixtures rather than shipping game assets. Diagnose
failures before retrying under AGENTS.md; stop a substantially blocked path.

## Earlier plans

- [First release candidate](ROADMAP-first-release-candidate.md)
- [MVP implementation](ROADMAP-mvp-implementation.md)
- [Feasibility and MVP definition](ROADMAP-feasibility-and-mvp-definition.md)
