# MVP owner sessions

[PROJECT.md](PROJECT.md) owns validation state and acceptance. The UI refinement
uses the short recheck below. The original integrated procedure is retained
after it for reference; a prepared procedure is not evidence that it passed.

## UI refinement recheck — SB-I5.3

Use **0.1.36**, build label **0.1.36.97b7f86**, source
`97b7f863c45f4fe7d6b799e32ec447b7a98edc76`:
[direct package download](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34693238801/artifacts/10297338736).
The ZIP was independently checked; its hashes and native compilation comparison
are in [the candidate evidence](MVP-VALIDATION.md#hosted-refinement-candidate).
Use this identified build even if a later documentation push produces a newer one.

With the game closed, replace the existing production DLL with
`BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll` from this ZIP. Keep one
production copy and the feasibility probe inactive. Use the same game/loader and
disposable save as the previous session. The startup line should identify
`0.1.36.97b7f86` and target MVID `ece4a40e-5e73-43f4-a9f8-4e74970b5942`.

1. Open the sphere editor. View the no-selection feedback, then select an existing
   test layer. Check that the smaller panel sits left of the native bottom bar,
   stays clear of native controls and the center caption, and keeps its button
   and feedback readable. If the existing layer is complete or edited, its usual
   disabled explanation should still fit.
2. Close and reopen the editor once; there should be one control. Select a fresh
   disposable layer with the same unlocked latitude and ordinary legal radius.
   Click **Paint next patch** once: the panel should show 1/12 and the editor should
   add one starting patch, with no extra native brush action beneath the control.
3. Save one useful panel screenshot and this session's `BepInEx/LogOutput.log` in
   the familiar evidence folder, preferably under `workshop-0.1.36/`. Report whether
   placement, readability and the click behaved as requested, plus any issue.

This recheck ends there: no exports, full sphere, shell designation, save/menu
reload, construction wait or W1–W6 matrix. If the panel is absent, an error occurs
or the click behaves unexpectedly, stop and retain the log/screenshot for a
targeted correction. The disposable layer need not be kept after evidence capture.

## Original integrated session — SB-I5.1

This was the first production runtime procedure, immediately before the workshop.
Its supplied evidence and differences from the proposed route are reviewed in
[MVP-VALIDATION.md](MVP-VALIDATION.md#sb-i51--owner-evidence-review).

### Candidate and setup

- Package: **DSPSphereBuilder 0.1.31**, build label **0.1.31.a71fd79**.
- Source: `a71fd79244054b1fe0695cff2d07631eb5ae803c`.
- [Direct package download](https://github.com/shytamir/DSPSphereBuilder/actions/runs/34678932740/artifacts/10293890904).
  Choose `DSPSphereBuilder-0.1.31.zip`, not the separate build-info artifact.
  The download has `manifest.json` at its root; there is no inner package ZIP.
- Use the same local game target as feasibility (Assembly-CSharp MVID
  `ece4a40e-5e73-43f4-a9f8-4e74970b5942`) and BepInEx 5.4.17.
  The startup log records the actual MVID and build label.
- Package SHA-256: `05362278070F49000290269F8CD288297343F205BF95D22C12F3B6EB49864A39`.
- DLL SHA-256: `EBDBCDB96ADAAE7A64CFB043F8375426309F27E7392EDE648B93843B35A2D662`.

With the game closed, move `DSPSphereBuilder.Feasibility.dll` outside BepInEx's
plugins directory so the probe cannot load. Extract the package's `BepInEx` folder
into the game directory, preserving folders. This installs only
`BepInEx/plugins/DSPSphereBuilder/DSPSphereBuilder.dll`. Keep a single production
copy. The owner installs and operates the game; no agent runtime action is needed.

Use a disposable test save and a fresh native sphere layer at an ordinary legal
radius (9,700 is suitable if that star offers it), with at least 68 degrees of
unlocked sphere latitude. No material supply or completed construction is required.

Create an evidence subfolder under the familiar location:
`BepInEx/plugins/DSPSphereBuilder/evidence/mvp-0.1.31/`. Save the two text exports,
one useful panel screenshot, and the session log there. The mod does not create
these files automatically.

### One short route

1. Open the sphere editor with no layer selected. The panel should explain the
   missing selection and disable Paint. Select the empty test layer; it should
   offer patch 1. Note any overlap or interference with native controls.
2. Click **Paint next patch** three times, deliberately once per patch. Progress
   should advance to 3/12, with no extra native brush placement beneath the button.
   Designate one closed pentagon shell using the game's normal shell tool. Do not
   wait for construction or sail absorption. Take one screenshot showing the
   panel and framework.
3. With that single layer selected, expand its native blueprint controls and use
   the **single-layer copy/export** action. Paste the copied `DYBP:` text into
   `prefix-3.txt` in the evidence folder. Use copy/export only, not blueprint import.
   Save the game, return to the main menu, and reload that save.
4. Reopen the editor and select the same layer. Progress should still be 3/12.
   Continue clicking to 12/12 without construction waits. The pentagon shell should
   remain designated and the framework should grow continuously to the opposite
   polar cap. The completed control should be disabled; an attempted extra click
   must add nothing. Copy that single layer again into `complete-12.txt`.
5. Create a second disposable empty layer. Its panel should offer patch 1 without
   affecting the completed first layer. Paint once, then manually remove one of
   that patch's perimeter frames. The panel should refuse the altered design and
   disable Paint. Reselect the first layer: it should still report completion.
6. Copy `BepInEx/LogOutput.log` into the evidence folder before another launch can
   replace it. Report the folder location, whether the route worked, and any UI
   issue. Note the layer radius if the route stopped before the first successful
   click; otherwise the log supplies it. No coordinate counting or large form.

If Paint stops after an error, an unexpected placement/removal occurs, or the
control is missing, stop the route and retain the log plus the last visible state.
Do not retry, restart to clear an error, or repair/import over the evidence layer.
Report where it stopped; the next check will target that defect. A minor layout
issue that still permits the route can be discussed in the workshop.

After saving evidence, the disposable save/layers can be discarded. To remove the
MVP, close the game and remove only its DLL. Retain the exports/log separately.
No full restart/removal test, radius/star matrix, hexagon filling, or forced error
is part of this session.

### Evidence interpretation

The layer copy path was inspected offline: UIDELayerPanel's single-layer copy
uses UIDysonEditor and DysonBlueprintData to serialize the selected native layer
to the clipboard. Its integrity check concerns record counts, not completed
construction. The existing decoder covers this target's blueprint versions:
container 0, layer 1, node 5, frame 1, shell 2, including pool gaps/recycle lists
and the optional layer painting colors. The retained reference exercises those
node/frame/shell versions. The full game-version suffix comes from the native
export header; the startup MVID identifies the actual assembly.

The supplied files used whole-sphere copy and a hexagon shell. The evidence review
in [MVP-VALIDATION.md](MVP-VALIDATION.md#sb-i51--owner-evidence-review) records that
difference from this original procedure. The decoder can select the native layer
from those exports by adding `--layer 1` to the comparison:

```powershell
python -B scripts/check_owner_capture.py --before $prefixFile --after $completeFile --radius $recordedRadius
```

This checks the 3/12 and 12/12 graphs, final 60 nodes/90 frames, direction/radius
bounds, prior numeric IDs/positions/endpoints/prototypes, and a retained reference-face
boundary. Capture checks cover both face classes, whole-sphere layer selection,
payload bounds, missing frames, wrong radius, lost shell and changed old positions.

Blueprints omit invested SP/CP and object identities. They cannot independently
prove those properties across the session. Production actions check them in memory
before/after each addition; accepted feasibility observations and managed negative
checks remain the supporting evidence. Blueprint parsing is a record-format and
geometry check, not independent verification of the game's custom checksum.
Visible behavior, matching startup/action logs, and captures must agree. Quiet
logs alone do not pass the story. W1–W6 keep their existing accepted/unverified
status; this session adds no new waiver.
