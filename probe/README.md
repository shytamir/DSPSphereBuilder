# Sphere Builder feasibility probe

This disposable probe tests staged placement and continuation through the native
node/frame methods. It includes the twelve fixed patches so completion can be
checked after save/reload.
It is not the playable mod. Use a disposable copy of a save with an existing
BepInEx installation; no game files are replaced by this artifact.

The [project record](https://github.com/shytamir/DSPSphereBuilder/blob/main/docs/PROJECT.md)
owns execution scope and acceptance. These procedures are retained for reproduction;
their presence does not request another test run.

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

For a replacement build, close the game and replace the DLL in your existing
probe folder with the ZIP's DLL. Preserve its `evidence/` folder. Restart the
game before testing; do not install a second copy of the probe.

Evidence JSON files are written to `evidence/` beside the probe DLL. The files
include source/target identities, game version/build, selected star/layer/radius,
unlock value, native prototype counts, before/after structure and shell records,
and direct in-process object-preservation comparisons. Keep these files, including
any `-before` or `-error` files. They do not include your save or filesystem paths.

## Run the continuation story (SB-F3.2)

Use the same disposable test save and corrected-probe installation. Replace only
its DLL as described above; keep the evidence folder. A layer made by the previous
two-patch probe is suitable. No need to wait for construction or repeat cases A–C.
Leave **Native rejection test** unused; it is retained for the earlier experiment.

**Capture snapshot** now reports `Empty`, `Prefix` plus its completed patch count,
`Complete` (12 patches), or `Mismatch`. Paint accepts only Empty or Prefix.
Matching uses the fixed coordinates within their float-rounding bound and exact
connectivity, independently of layer/node/frame IDs. Shells on reference face
boundaries are allowed; their construction is untouched. A refusal adds nothing.

Keep a short note of the case label and the evidence timestamp shown in the log
after each transition. A report cannot tell us that you closed the editor or
loaded a save unless you identify that action. Report skipped/unavailable cases.

| Case | Actions | Expected observation |
| --- | --- | --- |
| D1: baseline | Select the previous two-patch layer, or paint twice on a fresh empty layer. Capture a snapshot and save a checkpoint for this test | Prefix 2; 11 nodes / 12 frames; existing shells allowed |
| D2: editor return | Close/reopen the editor, reselect that layer, snapshot, then Paint once | Prefix 2 before; Prefix 3 with 16 / 19 after |
| D3: layer switch | On another empty layer in the same star, Paint once. Return to the original layer, snapshot, then Paint once | Other layer has 6 / 6. Original remains Prefix 3, then becomes Prefix 4 with 21 / 26 |
| D4: star switch | Select another star in the editor, create/select a disposable empty layer there and Paint once. Return to the original star/layer, snapshot, then Paint once | Other star has 6 / 6. Original remains Prefix 4, then becomes Prefix 5 with 26 / 33 |
| D5: intermediate reload | Reload the D1 checkpoint. Select the original layer, snapshot, then Paint once | Prefix 2 after load; Prefix 3 with 16 / 19 after painting; earlier construction/shells retained |
| D6: completion | Continue individual Paint clicks to patch 12 without waiting for construction. Snapshot and save a second checkpoint | Complete; 60 nodes / 90 frames; no automatically added shells |
| D7: completed transitions | Close/reopen the editor; switch to another layer and back; switch to another star and back. After each return, select the original layer, snapshot, then Paint | Each snapshot remains Complete; every Paint refuses with 60 / 90 unchanged |
| D8: completed reload/restart | Reload the completed checkpoint, snapshot, then Paint. Quit/restart the game, load that checkpoint again, snapshot, then Paint | Complete and refusal after both transitions; restart reports a new session ID |
| D9: manual edits | On a separate empty layer, Paint once. Delete one frame using the native editor, snapshot, then Paint. Delete one node from that edited layer, snapshot, then Paint again | Mismatch and no probe additions after either edit; retain the edited layer for D10 |
| D10: recreated layer and unrelated graph | Note D9's layer ID from evidence. Delete that layer and create a new one. Snapshot while empty. Place one unrelated node with the native editor, snapshot, then Paint. Delete that node and Paint again | Empty after recreation; Mismatch/refusal with unrelated node; after removing it, first patch at 6 / 6. Record whether the layer ID was reused; no old patch index carries over |

The complete progression is:

| Patch | Nodes | Frames |
| --- | --- | --- |
| 1 | 6 | 6 |
| 2 | 11 | 12 |
| 3 | 16 | 19 |
| 4 | 21 | 26 |
| 5 | 26 | 33 |
| 6 | 31 | 41 |
| 7 | 36 | 48 |
| 8 | 41 | 56 |
| 9 | 46 | 64 |
| 10 | 51 | 72 |
| 11 | 56 | 81 |
| 12 | 60 | 90 |

If the save's actual rounded latitude unlock is below 68°, the probe refuses
painting without additions. Record this if encountered; do not alter research
through the probe. Report if research or construction availability prevents a case.
Normal construction can progress between snapshots, so increased SP/CP is allowed.
A preservation error, unexpected count, or unexpected recognition result is a
stop condition; keep the evidence and report the first failing case. Do not retry
that layer, repair it to make the probe pass, or restart to bypass an error stop.

## Return the observations

Provide the probe's `evidence` folder, the case/timestamp notes, any visible
glitches, and the other enabled mods in that profile. A screenshot of the complete
framework helps establish visual placement. Report skipped cases plainly.
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
