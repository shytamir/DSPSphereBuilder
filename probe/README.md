# Sphere Builder feasibility probe

This disposable probe tests two patches through the native node/frame methods.
It is not the playable mod. Use a disposable copy of a save with an existing
BepInEx installation; no game files are replaced by this artifact.

## Install and identify

1. Close the game. Extract the ZIP's `DSPSphereBuilder.Feasibility` folder into
   your active profile's `BepInEx/plugins` folder, then start the test save.
2. Open the Dyson sphere editor. A small panel near the top says **Sphere Builder
   feasibility probe**. If it is missing or overlaps controls so they cannot be
   used, stop and report that observation plus the probe's loader error. Do not
   repeatedly reinstall or retry.
3. The probe logs its source revision and exact assembly SHA-256. It refuses a
   different target from `AE0BA95F75BD879A62AA4CE253B2AB78EAA4FB3C7C595F5E1FEE75EBE0E0EF85`.
   The build script labels uncommitted source with `-dirty`; the handed-off ZIP
   should have a clean source revision.

Evidence JSON files are written to `evidence/` beside the probe DLL. The files
include source/target identities, game version/build, selected star/layer/radius,
unlock value, native prototype counts, before/after structure and shell records,
and direct in-process object-preservation comparisons. Keep these files, including
any `-before` or `-error` files. They do not include your save or filesystem paths.

## Run the first live story

Keep this initial run in one game session. The probe deliberately has only two
patches and only tracks the most recently started layer. Save/reload continuation
and the full twelve-patch workflow will be investigated after this evidence.

| Case | Action | Expected observation |
| --- | --- | --- |
| No selected layer | Click **Paint next probe patch** with no layer selected | Refusal; nothing placed |
| Unrelated nonempty layer | Select a disposable layer with an unrelated node, then click Paint | Refusal; existing design unchanged |
| A: partial construction | Select an empty layer with sufficient latitude research. Paint once, let some real construction arrive, then paint again while the first patch is still incomplete | 6 nodes / 6 frames, then 11 nodes / 12 frames. One leading endpoint is reused. Existing progress is retained; the preservation-failure array is empty |
| B: completed construction and shell | On a second empty layer, paint once. Let its nodes/frames finish and manually fill the pentagon shell. Capture a snapshot, then paint again | Same geometry counts; earlier completed construction and the shell remain. No new shell is created |
| End of this probe | Click Paint once more after either two-patch run | Refusal; counts stay 11 / 12 |
| C: native call failure | On a third, separate empty layer, click **Native rejection test** once | One node remains and no frame is added; returned frame ID is 0. Capture a snapshot to confirm the retained node |

Case C intentionally makes one valid node call followed by a frame call with the
same endpoint twice. It demonstrates that a rejected later call does not undo
earlier native additions. It does **not** claim that a valid patch spontaneously
fails this way. Do not continue painting that layer or expect automatic repair.

If the save's actual rounded latitude unlock is below 68°, the probe refuses
painting without additions. Record this if encountered; do not alter research
through the probe. Report if research or construction availability prevents a case.
Normal construction can progress between snapshots, so increased SP/CP is allowed.
A preservation error or unexpected count is a stop condition; keep the evidence
and do not retry that layer.

## Return the observations

Provide the probe's `evidence` folder, which cases you completed, any visible
glitches, and the other enabled mods in that profile. A screenshot of the two
patches helps establish visual placement. Report skipped cases plainly.
Do not send the full save or unrelated mod logs unless investigation needs them.

To remove the probe, close the game and remove its folder. Native nodes, frames,
and any shells stay in the test save; discard the disposable save to discard the
test changes. There is no automatic cleanup or rollback.

## Source and licenses

Source: [shytamir/DSPSphereBuilder](https://github.com/shytamir/DSPSphereBuilder),
at the revision recorded in the evidence. Original probe code uses the included
Apache-2.0 license. Its geometry is derived from Cosmin1490's `60.txt` at revision
`bf00f4b2334c93215f63e0291f9acb6003c9a663` of
[DysonSphereProgram-BlueprintGenerator](https://github.com/Cosmin1490/DysonSphereProgram-BlueprintGenerator/blob/bf00f4b2334c93215f63e0291f9acb6003c9a663/60.txt).
The unmodified reference and its GPL-3.0 license accompany the probe as
`reference-60.txt` and `REFERENCE-LICENSE.txt`; their terms are not replaced by
the original-code license. No game, Unity, or BepInEx binaries are bundled.
