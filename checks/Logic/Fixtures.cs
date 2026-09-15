using DSPSphereBuilder;

static class Fixtures
{
    public static void Check(bool condition, string detail)
    {
        if (!condition) throw new InvalidOperationException(detail);
    }
    public static int NativeId(int canonicalId) => 1000 - canonicalId * 7;
    public static LayerGraph Prefix(int count, float radius = 9700, PlanOrientation orientation = PlanOrientation.Grid)
    {
        var graph = new LayerGraph
        {
            LayerId = 2, Radius = radius,
            Nodes = SpherePlan.Patches.Take(count).SelectMany(p => p.Nodes).Select(id => new GraphNode
            {
                Id = NativeId(id), Position = SpherePlan.Position(id, radius, orientation), Sp = 23, SpMax = 30,
                Color = 0xAABBCCFF, Identity = new object()
            }).Reverse().ToArray(),
            Frames = SpherePlan.Patches.Take(count).SelectMany(p => p.Frames).Select((e, i) => new GraphFrame
            {
                Id = 2000 - i * 3, A = NativeId(e.B), B = NativeId(e.A), SpA = 7, SpB = 3, SpMax = 100,
                Color = 0x112233FF, Identity = new object()
            }).Reverse().ToArray()
        };
        foreach (var node in graph.Nodes)
            node.Frames = graph.Frames.Where(f => f.A == node.Id || f.B == node.Id).Select(f => f.Id).Order().ToArray();
        return graph;
    }
    public static void Shell(LayerGraph graph, int[] face, int id)
    {
        var nodes = face.Select(NativeId).Reverse().ToArray();
        var frames = nodes.Select((n, i) => graph.Frames.Single(f =>
            GraphRecognition.Pair(f.A, f.B) == GraphRecognition.Pair(n, nodes[(i + 1) % nodes.Length])).Id).ToArray();
        graph.Shells = graph.Shells.Append(new GraphShell
        {
            Id = id, Nodes = nodes, Frames = frames, NodeCps = Enumerable.Repeat(10, nodes.Length + 1).ToArray(),
            Identity = new object(), Color = 0xFFEEDDFF
        }).ToArray();
        foreach (var node in graph.Nodes.Where(n => nodes.Contains(n.Id)))
            node.Shells = node.Shells.Append(id).Order().ToArray();
    }
}
