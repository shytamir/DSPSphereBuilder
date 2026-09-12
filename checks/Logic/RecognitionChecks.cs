using DSPSphereBuilder;
using static Fixtures;

static class RecognitionChecks
{
    public static void Run()
    {
        for (int i = 0; i <= 12; i++)
        {
            var graph = Prefix(i);
            graph.LayerId = 1;
            var match = GraphRecognition.Match(graph);
            Check(match.State == (i == 0 ? GraphState.Empty : i == 12 ? GraphState.Complete : GraphState.Prefix)
                && match.CompletedPatches == i, "Fresh prefix classification failed.");
            Check(match.NativeIds.All(p => p.Value == NativeId(p.Key)), "Unordered/reused IDs mapped incorrectly.");
            foreach (var node in graph.Nodes) { node.Sp += 2; node.Color = 0; }
            foreach (var frame in graph.Frames) { frame.SpA += 3; frame.Color = 0; }
            Check(GraphRecognition.Match(graph).State == match.State, "Construction or colors affected continuation.");
        }

        void Refuses(Action<LayerGraph> edit, string detail, int prefix = 2)
        {
            var graph = Prefix(prefix);
            edit(graph);
            Check(GraphRecognition.Match(graph).State == GraphState.Mismatch, detail);
        }
        Refuses(g => g.Nodes = g.Nodes.Skip(1).ToArray(), "Missing node accepted.");
        Refuses(g => g.Frames = g.Frames.Skip(1).ToArray(), "Missing frame accepted.");
        Refuses(g => g.Nodes[0].Id = g.Nodes[1].Id, "Duplicate native node ID accepted.");
        Refuses(g => g.Nodes[0].Position = g.Nodes[1].Position, "Duplicate canonical position accepted.");
        Refuses(g => g.Frames[0].Id = g.Frames[1].Id, "Duplicate native frame ID accepted.");
        Refuses(g => g.Frames[0].Euler = true, "Euler frame accepted.");
        Refuses(g => { g.Frames[0].A = g.Frames[1].A; g.Frames[0].B = g.Frames[1].B; }, "Duplicate edge accepted.");
        Refuses(g => g.Frames[0].A = -7, "Dangling endpoint accepted.");
        Refuses(g => g.Frames = g.Frames.Append(new GraphFrame { Id = 9000, A = g.Nodes[0].Id, B = g.Nodes[1].Id }).ToArray(), "Extra frame accepted.");
        Refuses(g => g.Nodes = g.Nodes.Append(Prefix(2).Nodes.First(n => g.Nodes.All(old => n.Id != old.Id))).ToArray(), "Partial next delta accepted.", 1);
        Refuses(g => g.Nodes = new[] { new GraphNode { Id = 1, Position = new Position(9700, 0, 0) } }, "Unrelated node accepted.", 0);
        Refuses(g => g.Radius = 36000, "Wrong-radius geometry accepted.");
        foreach (float radius in new[] { 0f, -1f, float.NaN, float.PositiveInfinity })
            Refuses(g => g.Radius = radius, "Invalid radius accepted.", 0);

        foreach (float fraction in new[] { 0.5f, 2f })
        {
            var graph = Prefix(2);
            var p = graph.Nodes[0].Position;
            graph.Nodes[0].Position = new Position(p.X + (float)(graph.Radius * GraphRecognition.PositionRelativeTolerance * fraction), p.Y, p.Z);
            Check(GraphRecognition.Match(graph).State == (fraction < 1 ? GraphState.Prefix : GraphState.Mismatch), "Position tolerance boundary failed.");
        }
        var rounded = Prefix(2);
        var old = rounded.Nodes[0].Position;
        rounded.Nodes[0].Position = new Position(MathF.BitIncrement(old.X), old.Y, old.Z);
        Check(GraphRecognition.Match(rounded).State == GraphState.Prefix, "Float rounding prevented recognition.");

        var complete = Prefix(12);
        for (int i = 0; i < SpherePlan.Faces.Length; i++) Shell(complete, SpherePlan.Faces[i], i + 1);
        Check(GraphRecognition.Match(complete).State == GraphState.Complete, "Reference pentagon/hexagon shells prevented completion.");
        foreach (var shell in complete.Shells) { shell.Color = 0; shell.NodeCps[0] += 10; }
        Check(GraphRecognition.Match(complete).State == GraphState.Complete, "Shell construction/colors affected recognition.");
        complete.Shells[0].Nodes[0] = complete.Shells[0].Nodes[1];
        Check(GraphRecognition.Match(complete).State == GraphState.Mismatch, "Repeated shell boundary node accepted.");
        complete = Prefix(12);
        Shell(complete, SpherePlan.Faces[0], 1);
        Shell(complete, SpherePlan.Faces[0], 2);
        Check(GraphRecognition.Match(complete).State == GraphState.Mismatch, "Duplicate shell face accepted.");
        complete.Shells = complete.Shells.Take(1).ToArray();
        complete.Shells[0].Frames[0] = 9999;
        Check(GraphRecognition.Match(complete).State == GraphState.Mismatch, "Missing shell frame accepted.");
        Console.WriteLine("PASS: all prefixes, reconstruction, numeric boundaries, edits, partial deltas, and both shell classes.");
    }
}
