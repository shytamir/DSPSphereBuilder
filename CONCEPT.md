# DSP Sphere Builder

**Paint a perfect C60 Dyson sphere, one patch at a time.**

DSP Sphere Builder is a proposed Dyson Sphere Program mod that helps players lay out an exact, cost-efficient C60 sphere gradually. Select a sphere layer and click **Paint next patch** to extend the framework by one connected section.

It combines the precision of a complete blueprint with the construction control of planning a small area at a time. Players can concentrate construction on the current section, then expand whenever they are ready. Shell filling remains entirely with the player.

This document records the agreed concept before scaffolding or implementation. It is not evidence of working in-game incremental placement.

## The experience

Begin on an empty selected layer with a polar pentagon and one leading spoke toward the next pentagon. Each subsequent click completes another pentagon, connects it to the already painted framework, and leads toward the next patch.

The twelve pentagons are painted in a fixed progression along a pentagon-to-pentagon polar axis:

1. One north polar pentagon.
2. Five upper-ring pentagons, painted one at a time around the ring.
3. Five lower-ring pentagons, painted one at a time after crossing the equator.
4. One south polar pentagon, completing the framework.

The first six pentagons complete the northern group before progression moves into the southern group. The rings describe the latitudes of the pentagon centres; individual vertices lie at different latitudes. The polar pentagons cap the axis. Like every other pentagon, each borders five hexagons.

The intended result is a twelve-click progression from an empty layer to the complete framework. The exact route through the rings, its crossing between hemispheres, and its orientation must be verified against the reference geometry so every leading spoke follows an actual design edge.

## What each click paints

Each click:

- Completes the next pentagon, reusing any vertex already placed by an earlier spoke.
- Adds all missing connections between that pentagon and previously painted pentagons.
- Adds one leading spoke toward the next pentagon, except on the final click.

A leading spoke includes its endpoint on the next pentagon, so one vertex of the next patch is deliberately planned early. Shared elements are reused rather than duplicated.

Connections to earlier pentagons are essential: a single chain through the twelve pentagons would leave hexagon boundaries unfinished. As the pentagons and their connecting spokes accumulate, the twenty hexagons form naturally in the intervening space.

The mod places nodes and frames only. It does not designate or fill shells, deliver construction materials, or wait for construction to finish before allowing the next click.

## What it offers

- **Exact geometry:** no estimating angles, counting grid diamonds, or accumulating placement errors.
- **Gradual expansion:** one pentagon-based patch per click, with a predictable path from pole to pole.
- **Player-controlled pacing:** expand on demand without waiting for the current patch to finish.
- **Continuity:** preserve existing structures and their construction progress while adding the next section.
- **Player-controlled shells:** leave shell designation and filling to the player.
- **Flexible placement:** the intended scope is a selected layer in any system, at any game-supported radius, subject to verification of placement constraints and latitude unlocks.

The starting scope is an empty layer followed by continued expansion of that same design. Adapting to arbitrary pre-existing designs is not part of the agreed core interaction.

## Geometry and completion

The reference design consists of twelve equal, regular pentagons and twenty hexagons with alternating longer and shorter edges. “Perfect” means faithfully reproducing that intended C60 geometry throughout the sphere, rather than substituting a conventional equal-edge football.

The completed framework should contain:

- **60 nodes.**
- **90 frames:** 60 pentagon perimeter frames and 30 connecting spokes.
- **No shells placed by the mod.**

Cost efficiency refers to the chosen reference design. Its author's mathematical optimality claims have not been independently established here.

## References

### Reference sphere

[Cosmin1490’s Best Cost Efficiency optimized sphere design — 60 nodes](https://www.dysonsphereblueprints.com/en/blueprints/dyson-sphere-best-cost-efficiency-optimized-sphere-design-60-nodes-15-cheaper-than-football)

This published design provides the geometry underlying the concept. Its mathematical optimality claims belong to its author.

### Supporting tools

[Cosmin1490’s Dyson Sphere Program Blueprint Generator](https://github.com/Cosmin1490/DysonSphereProgram-BlueprintGenerator)

The repository contains sphere-generation tools, blueprint serialization and parsing code, and geometric validation routines. It is reference material; no implementation from it has been incorporated into this project by this concept write-up.

### Reported format validation

The supplied concept reports that a user-supplied game assembly was inspected and its actual blueprint header, checksum, node, and frame readers were exercised against the sample below. That establishes the sample's record format within the reported checks; it does not demonstrate the proposed mod's incremental painting behaviour. Those checks were not rerun when saving this document, and no separate validation report was supplied here.

## Sample: one spoked pentagon patch

The supplied sample was extracted directly from the published C60 blueprint, preserving the selected nodes' exact coordinates and orientation. It contains:

- Five pentagon vertices and five outward spoke endpoints.
- Five pentagon perimeter frames and five spoke frames.
- No shells or additional nodes.

The shorter spokes connect toward five separate neighbouring pentagons. Their endpoints are not the corners of a single larger pentagon.

**This is a geometry illustration, not the agreed first-click output.** The sample has five outward spokes; the mod's intended first click paints the polar pentagon with only one leading spoke. Its other connections appear as the neighbouring pentagons are painted.

```text
DYBP:0,639175925155924425,0.10.34.28529,1,81"H4sIAAAAAAACCnWTPU7EMBCFJ3ES/+wNKFxQIG3FahsoU4CEhIQ4AAVacQD2BttCwxX2GNtDVoiKgo4T0CFxAWL7xRmbYGlkx2++8Xg8ISIqemt7m1EY7rvGHMbjx1e3PHru9M/s3CYK0Xdv6jCsS5BlVHcn6+7zqdrfrFeeLDPyFaQAKaJ6+/D2cn98sL+63MxtogTyAmQFspo6s7OJEsh3kDWzP/f0ZJ2RLcgGWsOzPXPZurVNlEBeg5QgZVTdWTjXV0hm5ClIBVJNkT5blZELkBqkjupQHVcpmyjpqxiQZor02Zp/yLyfuPkm8Ne8o7FvCjb7x264h4AiuId/2C2NfVCwOTZG9Kjz3i5yjwabwzyWbchDQpE8huEeCpuKe5T8FI1NzTNVPIZhgWPFBI/RZn8h/QLFDZvQyQMAAA=="456C5BD65728D00FBEDCDAD865F19D40
```

**Sample use:** import into an **empty selected layer** using single-layer blueprint import. Vanilla import replaces that layer's contents. The sample requires at least **81° of unlocked latitude** and scales to the selected layer's radius. Full in-game placement and visual verification remain untested.

The sample's preserved orientation does not establish the pole-centred orientation intended for the mod, or the latitude requirement of that orientation.

## Remaining verification

Before treating the concept as demonstrated, verify the pole-centred reference geometry and ring traversal, native placement constraints across supported radii and latitude unlocks, and additive placement beside partially built structures without disturbing existing construction progress.

Blueprint record parsing and radius scaling alone do not establish those behaviours. The mod's defining utility is successive additive placement while preserving the player's existing construction.
