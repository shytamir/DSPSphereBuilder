using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using BepInEx;
using UnityEngine;
using UnityEngine.UI;

namespace DSPSphereBuilder.Feasibility
{
    [Serializable] public class Point { public int id; public Vector3 direction; }
    [Serializable] public class Edge { public int a, b; }
    [Serializable] public class Patch { public int[] nodes; public Edge[] frames; }
    [Serializable] public class Plan { public string referenceSha256; public Point[] nodes; public Patch[] patches; }
    [Serializable] public class NodeMapping { public int canonicalId, nativeId; }
    [Serializable] public class Evidence
    {
        public string action, outcome, sourceRevision, targetSha256, targetMvid, gameVersion, unityVersion, referenceSha256;
        public int gameBuild, loadedNodePrototypes, loadedFramePrototypes, completedPatches, returnedFrameId;
        public Snapshot before, after;
        public string[] preservationFailures;
        public NodeMapping[] nodeMapping;
    }

    [BepInPlugin("shytamir.dspspherebuilder.feasibility", "DSP Sphere Builder Feasibility Probe", "0.0.0")]
    public class Probe : BaseUnityPlugin
    {
        private const string TargetHash = "AE0BA95F75BD879A62AA4CE253B2AB78EAA4FB3C7C595F5E1FEE75EBE0E0EF85";
        private Plan plan;
        private string outputDirectory, targetHash, revision;
        private UIDysonEditor attemptedEditor, panelEditor;
        private RectTransform panel;
        private Text status;
        private DysonSphereLayer trackedLayer;
        private readonly Dictionary<int, DysonNode> nodes = new Dictionary<int, DysonNode>();
        private readonly Dictionary<DysonFrame, Edge> frames = new Dictionary<DysonFrame, Edge>();
        private int completed;
        private bool stopped;
        private float trackedRadius;

        private void Awake()
        {
            try { Initialize(); }
            catch (Exception error) { Logger.LogError(error); enabled = false; }
        }

        private void Initialize()
        {
            var ownAssembly = typeof(Probe).Assembly;
            revision = ownAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            outputDirectory = Path.Combine(Path.GetDirectoryName(ownAssembly.Location), "evidence");
            Directory.CreateDirectory(outputDirectory);
            using (var input = File.OpenRead(typeof(GameMain).Assembly.Location))
            using (var sha = SHA256.Create()) targetHash = BitConverter.ToString(sha.ComputeHash(input)).Replace("-", "");
            if (targetHash != TargetHash)
            {
                Logger.LogError("Probe target mismatch: " + targetHash);
                enabled = false;
                return;
            }
            using (var input = new StreamReader(ownAssembly.GetManifestResourceStream("plan.json")))
                plan = JsonUtility.FromJson<Plan>(input.ReadToEnd());
            Logger.LogInfo("Probe source " + revision + "; target " + targetHash + "; evidence " + outputDirectory);
        }

        private void Update()
        {
            var editor = UIRoot.instance?.uiGame?.dysonEditor;
            if (editor == null || !editor.active || ReferenceEquals(editor, attemptedEditor)) return;
            attemptedEditor = editor;
            try { CreatePanel(editor); }
            catch (Exception error) { Logger.LogError(error); enabled = false; }
        }

        private void CreatePanel(UIDysonEditor editor)
        {
            var canvas = editor.controlPanel.GetComponentInParent<Canvas>();
            if (canvas == null || canvas.worldCamera == null) throw new InvalidOperationException("Editor UI camera unavailable");
            var font = editor.GetComponentsInChildren<Text>(true).First(t => t.font != null).font;
            var root = new GameObject("SphereBuilderProbe", typeof(RectTransform), typeof(Image));
            panel = (RectTransform)root.transform;
            panel.SetParent(editor.controlPanel.transform, false);
            panel.anchorMin = panel.anchorMax = panel.pivot = new Vector2(0.5f, 1);
            panel.anchoredPosition = new Vector2(0, -12);
            panel.sizeDelta = new Vector2(630, 122);
            root.GetComponent<Image>().color = new Color(0.06f, 0.09f, 0.13f, 0.97f);
            root.AddComponent<UIBlockZone>();
            panelEditor = editor;
            editor.guiRects = editor.guiRects.Concat(new[] { panel }).ToArray();
            Label("Sphere Builder feasibility probe", 10, 5, 610, 25, font);
            Button("Paint next probe patch", 10, font, () => Run("paint"));
            Button("Capture snapshot", 220, font, () => Run("snapshot"));
            Button("Native rejection test", 430, font, () => Run("rejection"));
            status = Label("Select an empty layer. This probe paints two patches.", 10, 77, 610, 40, font);
            panel.SetAsLastSibling();
        }

        private RectTransform Rect(GameObject item, Transform parent, float x, float y, float width, float height)
        {
            var rect = (RectTransform)item.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(x, -y);
            rect.sizeDelta = new Vector2(width, height);
            return rect;
        }

        private Text Label(string caption, float x, float y, float width, float height, Font font)
        {
            var item = new GameObject("Label", typeof(RectTransform), typeof(Text));
            Rect(item, panel, x, y, width, height);
            var text = item.GetComponent<Text>();
            text.font = font; text.fontSize = 15; text.color = Color.white;
            text.text = caption; text.raycastTarget = false;
            return text;
        }

        private void Button(string caption, float x, Font font, UnityEngine.Events.UnityAction action)
        {
            var item = new GameObject("Action", typeof(RectTransform), typeof(Image), typeof(Button));
            Rect(item, panel, x, 37, 190, 32);
            item.GetComponent<Image>().color = new Color(0.13f, 0.28f, 0.36f);
            item.GetComponent<Button>().targetGraphic = item.GetComponent<Image>();
            item.GetComponent<Button>().onClick.AddListener(action);
            var text = Label(caption, 0, 0, 190, 32, font);
            text.transform.SetParent(item.transform, false);
            text.alignment = TextAnchor.MiddleCenter;
            text.fontSize = 14;
        }

        private Evidence Report(string action)
        {
            return new Evidence
            {
                action = action, sourceRevision = revision, targetSha256 = targetHash,
                targetMvid = typeof(GameMain).Assembly.ManifestModule.ModuleVersionId.ToString(),
                gameVersion = GameConfig.gameVersion.ToString(), gameBuild = GameConfig.build,
                unityVersion = Application.unityVersion, referenceSha256 = plan.referenceSha256,
                loadedNodePrototypes = DysonSphereSegmentRenderer.nodeProtoCount,
                loadedFramePrototypes = DysonSphereSegmentRenderer.frameProtoCount
            };
        }

        private void Save(Evidence evidence, string name)
        {
            File.WriteAllText(Path.Combine(outputDirectory, name + ".json"), JsonUtility.ToJson(evidence, true));
        }

        private void Run(string action)
        {
            var report = Report(action);
            var name = DateTime.UtcNow.ToString("yyyyMMddTHHmmss.fffffff") + "-" + action;
            DysonSphereLayer layer = null;
            try
            {
                var editor = UIRoot.instance?.uiGame?.dysonEditor;
                layer = editor?.selection?.singleSelectedLayer;
                if (editor == null || !editor.active || !GameMain.isRunning || layer == null)
                { Refuse(report, name, "Select exactly one sphere layer in a running game."); return; }
                report.before = Snapshot.Capture(layer);
                Save(report, name + "-before");
                if (action == "snapshot")
                { report.after = report.before; report.outcome = "Snapshot captured"; }
                else
                {
                    if (stopped) { Refuse(report, name, "Probe stopped after an error. Restart before another run.", layer); return; }
                    if (Mathf.RoundToInt(GameMain.history.dysonNodeLatitude) < 68)
                    { Refuse(report, name, "Insufficient latitude unlock; rounded value must reach 68 degrees.", layer); return; }
                    if (action == "rejection")
                    {
                        if (!Empty(layer)) { Refuse(report, name, "The rejection test requires a separate empty layer.", layer); return; }
                        var first = plan.nodes[0];
                        int node = layer.NewDysonNode(0, first.direction.normalized * layer.orbitRadius);
                        report.returnedFrameId = layer.NewDysonFrame(0, node, node, false);
                        report.outcome = "One node added, then same-endpoint frame attempted";
                    }
                    else
                    {
                        if (!ReferenceEquals(trackedLayer, layer))
                        {
                            if (!Empty(layer)) { Refuse(report, name, "Start this probe on an empty layer.", layer); return; }
                            trackedLayer = layer; trackedRadius = layer.orbitRadius;
                            nodes.Clear(); frames.Clear(); completed = 0;
                        }
                        if (!Matches(layer)) { Refuse(report, name, "The tracked node/frame graph changed; no patch added.", layer); return; }
                        if (completed == plan.patches.Length) { Refuse(report, name, "Both probe patches are complete.", layer); return; }
                        var patch = plan.patches[completed];
                        foreach (int canonical in patch.nodes)
                        {
                            var position = plan.nodes.Single(p => p.id == canonical).direction.normalized * layer.orbitRadius;
                            if (float.IsNaN(position.sqrMagnitude) || float.IsInfinity(position.sqrMagnitude))
                                throw new InvalidOperationException("Non-finite node position");
                        }
                        foreach (int canonical in patch.nodes)
                        {
                            var position = plan.nodes.Single(p => p.id == canonical).direction.normalized * layer.orbitRadius;
                            int id = layer.NewDysonNode(0, position);
                            nodes.Add(canonical, layer.nodePool[id]);
                        }
                        foreach (var edge in patch.frames)
                        {
                            int id = layer.NewDysonFrame(0, nodes[edge.a].id, nodes[edge.b].id, false);
                            if (id == 0) throw new InvalidOperationException("Native frame creation returned zero");
                            frames.Add(layer.framePool[id], edge);
                        }
                        completed++;
                        report.outcome = "Patch applied";
                    }
                    report.after = Snapshot.Capture(layer);
                    if (action == "rejection" && (report.returnedFrameId != 0 || report.after.nodes.Length != 1 || report.after.frames.Length != 0))
                        throw new InvalidOperationException("Unexpected native rejection result");
                    report.preservationFailures = Snapshot.Compare(report.before, report.after);
                    if (report.preservationFailures.Length > 0)
                    {
                        stopped = true;
                        report.outcome = "Preservation check failed; probe stopped";
                        Logger.LogError(string.Join("; ", report.preservationFailures));
                    }
                    if (action == "paint" && !Matches(layer)) throw new InvalidOperationException("Result does not match the applied patches");
                }
                report.completedPatches = ReferenceEquals(layer, trackedLayer) ? completed : 0;
                if (ReferenceEquals(layer, trackedLayer))
                    report.nodeMapping = nodes.Select(n => new NodeMapping { canonicalId = n.Key, nativeId = n.Value.id }).ToArray();
                Save(report, name);
                status.text = report.outcome + "; " + report.after.nodes.Length + " nodes, " + report.after.frames.Length + " frames.";
                Logger.LogInfo(status.text + " Evidence: " + name);
            }
            catch (Exception error)
            {
                stopped = true;
                report.outcome = error.ToString();
                if (layer != null) report.after = Snapshot.Capture(layer);
                Save(report, name + "-error");
                status.text = "Probe stopped. Keep the evidence files; do not retry this layer.";
                Logger.LogError(error);
            }
        }

        private void Refuse(Evidence report, string name, string reason, DysonSphereLayer layer = null)
        {
            report.outcome = reason;
            report.after = layer == null ? null : Snapshot.Capture(layer);
            if (report.before != null && report.after != null)
                report.preservationFailures = Snapshot.Compare(report.before, report.after);
            report.completedPatches = ReferenceEquals(layer, trackedLayer) ? completed : 0;
            Save(report, name);
            status.text = reason;
            Logger.LogInfo(reason);
        }

        private bool Empty(DysonSphereLayer layer) => layer.nodeCount == 0 && layer.frameCount == 0 && layer.shellCount == 0;

        private bool Matches(DysonSphereLayer layer)
        {
            if (layer.orbitRadius != trackedRadius || layer.nodeCount != nodes.Count || layer.frameCount != frames.Count) return false;
            foreach (var entry in nodes)
            {
                var node = entry.Value;
                if (node.id <= 0 || node.id >= layer.nodePool.Length || !ReferenceEquals(layer.nodePool[node.id], node)) return false;
                var expected = plan.nodes.Single(p => p.id == entry.Key).direction.normalized * trackedRadius;
                if (!node.pos.Equals(expected)) return false;
            }
            return frames.All(entry => entry.Key.id > 0 && entry.Key.id < layer.framePool.Length
                && ReferenceEquals(layer.framePool[entry.Key.id], entry.Key)
                && ReferenceEquals(entry.Key.nodeA, nodes[entry.Value.a])
                && ReferenceEquals(entry.Key.nodeB, nodes[entry.Value.b]) && !entry.Key.euler);
        }

        private void OnDestroy()
        {
            if (panelEditor != null && panel != null) panelEditor.guiRects = panelEditor.guiRects.Where(r => r != panel).ToArray();
            if (panel != null) Destroy(panel.gameObject);
        }
    }
}
