# Changelog

Feature history for maintainers, governed by the [version procedure](docs/VERSIONING.md#change-records).
[PROJECT.md](docs/PROJECT.md) owns current validation, acceptance and publication.
The initial release entry was reconstructed from the retained implementation
commits and publication record; pre-release builds are not listed as releases.

## Unreleased

### Changed

- Aligned new spheres with the native pentagon grid by rotating the design 18°
  around its polar axis. Already painted layers retain their original orientation
  and can continue; geometry, twelve-patch order and existing construction are
  preserved. The prior release's recognizer supports only the original orientation,
  so it cannot continue newly aligned layers after a downgrade.
  [Implementation](https://github.com/shytamir/DSPSphereBuilder/commit/145a03bb49e242327ba518f082989366bc822a23).

## 1.0.56 — 2026-09-12

[Published release](https://github.com/shytamir/DSPSphereBuilder/releases/tag/1.0).

### Added

- Precise C60 framework planning in twelve connected patches, progressing from
  one polar cap to the other with 60 nodes and 90 frames. Each click completes a
  pentagon, closes connections to previous patches and adds a leading spoke.
  [Geometry](https://github.com/shytamir/DSPSphereBuilder/commit/5a9d3e44c73a5d22ce10718ceba827e3b20494a9),
  [incremental placement](https://github.com/shytamir/DSPSphereBuilder/commit/61b3553093bb5adb2c7b3fda1778905d289f49db).
- Continuation from the layer's existing framework after save/reload or selection
  changes, with existing construction and player-filled shells preserved. Edited
  layouts that no longer match a supported stage are refused.
  [Recognition](https://github.com/shytamir/DSPSphereBuilder/commit/924ad324c1a5df31d71142259be0961cd849e809),
  [preservation](https://github.com/shytamir/DSPSphereBuilder/commit/61b3553093bb5adb2c7b3fda1778905d289f49db).
- A **Paint Next Patch** control in the sphere editor with progress and refusal
  feedback for the selected layer.
  [Editor integration](https://github.com/shytamir/DSPSphereBuilder/commit/036db35ec0de619be5abb16015d854c11e0fce9c).

### Changed

- Refined the control into a compact panel beside the native bottom toolbar and
  made its action caption **Paint Next Patch**.
  [Compact layout](https://github.com/shytamir/DSPSphereBuilder/commit/af16c4f832729ff5a118065503a6d6d485ea5aa8),
  [caption](https://github.com/shytamir/DSPSphereBuilder/commit/2cf5cbff0150d054a4ba6c5791289ef0283a2eac).
