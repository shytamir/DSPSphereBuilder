# MVP owner session — SB-I5.1

[PROJECT.md](PROJECT.md) owns validation state and acceptance. This is the first
production runtime session, immediately before the UI workshop. Use the candidate
identified below; a prepared procedure is not evidence that it passed.

## Candidate and setup

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

## One short route

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

## Evidence interpretation

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
