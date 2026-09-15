using DSPSphereBuilder;
using static Fixtures;

static class PaintChecks
{
    // The writer records calls and returns prepared snapshots; it implements no native allocation.
    private sealed class ScriptedLayer : IPatchLayer
    {
        private readonly Queue<LayerGraph> captures;
        private readonly Queue<int> nodeIds;
        public readonly List<(int A, int B)> Edges = new();
        public readonly List<Position> Positions = new();
        public bool RejectFrame, ThrowFrame, ThrowAfter;
        private int reads;
        public int StarId { get; set; } = 60;
        public int LayerId { get; }
        public float Radius { get; }
        public int Writes => Positions.Count + Edges.Count;

        public ScriptedLayer(LayerGraph before, LayerGraph after, int nextPatch)
        {
            captures = new(new[] { before, after });
            nodeIds = new(nextPatch < 12 ? SpherePlan.Patches[nextPatch].Nodes.Select(NativeId) : Array.Empty<int>());
            LayerId = before.LayerId; Radius = before.Radius;
        }
        public LayerGraph Capture()
        {
            if (++reads == 2 && ThrowAfter) throw new InvalidOperationException("Capture failed.");
            return captures.Dequeue();
        }
        public int AddNode(Position p) { Positions.Add(p); return nodeIds.Dequeue(); }
        public int AddFrame(int a, int b)
        {
            Edges.Add((a, b));
            if (ThrowFrame) throw new InvalidOperationException("Write failed.");
            return RejectFrame ? 0 : 1;
        }
    }

    private static LayerGraph Next(LayerGraph before, int count, PlanOrientation orientation = PlanOrientation.Grid)
    {
        var after = Prefix(count, before.Radius, orientation);
        foreach (var node in before.Nodes)
        {
            var next = after.Nodes.Single(n => n.Id == node.Id);
            next.Identity = node.Identity; next.Sp = node.Sp + 1; next.Color = node.Color;
            next.Shells = node.Shells.ToArray();
        }
        foreach (var frame in before.Frames)
        {
            var next = after.Frames.Single(f => f.Id == frame.Id);
            next.Identity = frame.Identity; next.SpA = frame.SpA + 1; next.SpB = frame.SpB;
            next.Color = frame.Color;
        }
        after.Shells = before.Shells.Select(s => new GraphShell
        {
            Id = s.Id, Prototype = s.Prototype, Identity = s.Identity, Color = s.Color,
            Nodes = s.Nodes.ToArray(), Frames = s.Frames.ToArray(), NodeCps = s.NodeCps.Select(cp => cp + 1).ToArray()
        }).ToArray();
        return after;
    }

    public static void Run()
    {
        var logs = new List<string>();
        PaintSession Session() => new("offline fixture", logs.Add);
        PaintTarget Target(ScriptedLayer layer, int latitude = 90) => new(layer, latitude);
        var session = Session();
        var before = Prefix(0);
        for (int k = 0; k < 12; k++)
        {
            if (k == 3)
            {
                Shell(before, SpherePlan.Faces.First(f => f.Length == 5 && f.All(id => before.Nodes.Any(n => n.Id == NativeId(id)))), 1);
                Shell(before, SpherePlan.Faces.First(f => f.Length == 6 && f.All(id => before.Nodes.Any(n => n.Id == NativeId(id)))), 2);
            }
            var after = Next(before, k + 1);
            var layer = new ScriptedLayer(before, after, k);
            var result = session.Paint(() => Target(layer));
            var patch = SpherePlan.Patches[k];
            Check(result.State == (k == 11 ? PaintState.Complete : PaintState.Applied) && result.CompletedPatches == k + 1, "One-click progression failed.");
            Check(layer.Positions.Count == patch.Nodes.Length && layer.Edges.Count == patch.Frames.Length, "Unexpected write count.");
            Check(layer.Positions.SequenceEqual(patch.Nodes.Select(id => SpherePlan.Position(id, layer.Radius))), "Unexpected node positions/order.");
            Check(layer.Edges.SequenceEqual(patch.Frames.Select(e => (NativeId(e.A), NativeId(e.B)))), "Wrong endpoint mapping/order.");
            before = after;
        }
        Check(logs.Count == 0 && !session.Stopped, "Successful progression stopped or logged failure.");

        foreach (var orientation in new[] { PlanOrientation.Grid, PlanOrientation.Legacy })
        {
            var previous = Prefix(1, orientation: orientation);
            for (int k = 1; k < 12; k++)
            {
                var after = Next(previous, k + 1, orientation);
                var layer = new ScriptedLayer(previous, after, k);
                var resumed = Session();
                var result = resumed.Paint(() => Target(layer));
                Check(result.State == (k == 11 ? PaintState.Complete : PaintState.Applied)
                    && result.CompletedPatches == k + 1, "Reconstructed continuation failed.");
                Check(layer.Positions.SequenceEqual(SpherePlan.Patches[k].Nodes.Select(id =>
                    SpherePlan.Position(id, layer.Radius, orientation))), "Continuation changed orientation.");
                previous = after;
            }
        }
        Check(logs.Count == 0, "Reconstructed continuation logged failure.");

        var wrongOrientation = new ScriptedLayer(Prefix(0), Prefix(1, orientation: PlanOrientation.Legacy), 0);
        Check(Session().Paint(() => Target(wrongOrientation)).State == PaintState.Stopped,
            "Unexpected orientation after writing was accepted.");
        logs.Clear();
        var completed = new ScriptedLayer(before, before, 12);
        Check(session.Paint(() => Target(completed)).State == PaintState.Complete && completed.Writes == 0, "Completed sphere was changed.");
        Check(session.Paint(() => null!).State == PaintState.Unavailable, "Missing context accepted.");
        var locked = new ScriptedLayer(Prefix(0), Prefix(1), 0);
        Check(session.Paint(() => Target(locked, 67)).State == PaintState.LatitudeRequired && locked.Writes == 0, "Research refusal wrote data.");
        var unlocked = new ScriptedLayer(Prefix(0), Prefix(1), 0);
        Check(session.Paint(() => Target(unlocked, 68)).State == PaintState.Applied, "Unlock boundary rejected.");
        var edited = Prefix(2); edited.Frames = edited.Frames.Skip(1).ToArray();
        var invalid = new ScriptedLayer(edited, edited, 2);
        Check(session.Paint(() => Target(invalid)).State == PaintState.Mismatch && invalid.Writes == 0 && !session.Stopped, "Edit refusal changed data or stopped the session.");

        var oldLayer = new ScriptedLayer(Prefix(0), Prefix(0), 0);
        var newLayer = new ScriptedLayer(Prefix(0), Prefix(1), 0) { StarId = 8 };
        PaintTarget selected = Target(oldLayer);
        Check(session.Describe(() => selected).CanPaint, "Empty layer not ready.");
        selected = Target(newLayer);
        Check(session.Paint(() => selected).StarId == 8 && oldLayer.Writes == 0 && newLayer.Writes > 0, "Action used an old target.");

        session = Session();
        Check(!session.Describe(() => null!).CanPaint, "Missing selection exposed an action.");
        for (int k = 0; k <= 12; k++)
        {
            var graph = Prefix(k);
            var layer = new ScriptedLayer(graph, graph, k) { StarId = k + 1 };
            var display = session.Describe(() => Target(layer));
            Check(display.CompletedPatches == k && display.StarId == k + 1 && display.LayerId == layer.LayerId, "Feedback kept another selection's progress.");
            Check(display.CanPaint == (k < 12) && layer.Writes == 0, "Feedback wrote data or exposed the wrong action.");
            Check(!string.IsNullOrWhiteSpace(Feedback.Text(display)), "Missing progress feedback.");
            Check(session.Describe(() => null!).State == PaintState.Unavailable && !session.Stopped, "Menu transition changed session state.");
        }
        foreach (PaintState state in Enum.GetValues<PaintState>())
        {
            var display = new PaintResult { State = state };
            Check(!string.IsNullOrWhiteSpace(Feedback.Text(display)), "Missing action feedback.");
            Check(display.CanPaint == (state == PaintState.Ready || state == PaintState.Applied), "Refusal exposes an action.");
        }

        foreach (bool throws in new[] { false, true })
        {
            logs.Clear(); session = Session();
            var layer = new ScriptedLayer(Prefix(0), Prefix(1), 0) { RejectFrame = !throws, ThrowFrame = throws };
            Check(session.Paint(() => Target(layer)).State == PaintState.Stopped && session.Stopped, "Native failure did not stop the session.");
            Check(layer.Positions.Count == 6 && layer.Edges.Count == 1 && logs.Count == 1, "Failure retried, rolled back, or emitted repeated diagnostics.");
            var other = new ScriptedLayer(Prefix(0), Prefix(1), 0);
            Check(session.Paint(() => Target(other)).State == PaintState.Stopped && other.Writes == 0, "Target change cleared failure stop.");
            session.Describe(() => null!);
            Check(session.Paint(() => Target(other)).State == PaintState.Stopped && logs.Count == 1, "Menu context cleared stop or repeated the error.");
            var partial = Prefix(1); partial.Frames = Array.Empty<GraphFrame>();
            var partialLayer = new ScriptedLayer(partial, partial, 0);
            Check(Session().Paint(() => Target(partialLayer)).State == PaintState.Mismatch && partialLayer.Writes == 0, "Fresh session repaired a partial delta.");
        }

        logs.Clear(); session = Session();
        var finished = new ScriptedLayer(Prefix(0), Prefix(1), 0) { ThrowAfter = true };
        Check(session.Paint(() => Target(finished)).State == PaintState.Stopped, "Result-read exception did not stop.");
        var prefix = Prefix(1);
        var resumable = new ScriptedLayer(prefix, Next(prefix, 2), 1);
        Check(Session().Paint(() => Target(resumable)).CompletedPatches == 2, "Fresh session duplicated/rejected an already finished delta.");

        void Corruption(Action<LayerGraph> corrupt, string detail)
        {
            var start = Prefix(3);
            Shell(start, SpherePlan.Faces.First(f => f.Length == 5 && f.All(id => start.Nodes.Any(n => n.Id == NativeId(id)))), 1);
            var end = Next(start, 4); corrupt(end);
            var layer = new ScriptedLayer(start, end, 3);
            Check(Session().Paint(() => Target(layer)).State == PaintState.Stopped, detail);
        }
        Corruption(g => g.Nodes.Single(n => n.Id == NativeId(1)).Sp = 0, "Lost invested SP accepted.");
        Corruption(g => g.Nodes.Single(n => n.Id == NativeId(1)).Identity = new object(), "Replaced old node accepted.");
        Corruption(g => { var n = g.Nodes.Single(n => n.Id == NativeId(1)); n.Position = new Position(MathF.BitIncrement(n.Position.X), n.Position.Y, n.Position.Z); }, "Moved old node accepted inside matching tolerance.");
        Corruption(g => g.Shells[0].NodeCps[0] = 0, "Lost invested CP accepted.");
        Corruption(g => g.Shells = Array.Empty<GraphShell>(), "Removed shell accepted.");
        Corruption(g => g.Frames = g.Frames.Skip(1).ToArray(), "Missing result frame accepted.");
        Console.WriteLine("PASS: all additive call deltas, preservation, prerequisites, current targets, completion, and bounded partial-failure handling.");
    }
}
