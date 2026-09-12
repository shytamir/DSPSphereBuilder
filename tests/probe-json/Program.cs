using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using DSPSphereBuilder.Feasibility;
using UnityEngine;

static void Check(bool condition, string detail)
{
    if (!condition) throw new InvalidOperationException(detail);
}

using var input = typeof(Plan).Assembly.GetManifestResourceStream("plan.json");
using var copy = new MemoryStream();
input.CopyTo(copy);
copy.Position = 0;
var plan = ProbeJson.Read<Plan>(copy);
using var expected = JsonDocument.Parse(copy.ToArray());
var expectedNodes = expected.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
var expectedPatches = expected.RootElement.GetProperty("patches").EnumerateArray().ToArray();
Check(plan.nodes.Length == expectedNodes.Length && plan.patches.Length == expectedPatches.Length, "Plan collections missing");
for (int i = 0; i < expectedNodes.Length; i++)
{
    var node = expectedNodes[i];
    var direction = node.GetProperty("direction");
    Check(plan.nodes[i].id == node.GetProperty("id").GetInt32(), "Node identity changed");
    Check(plan.nodes[i].direction.x == direction.GetProperty("x").GetSingle()
        && plan.nodes[i].direction.y == direction.GetProperty("y").GetSingle()
        && plan.nodes[i].direction.z == direction.GetProperty("z").GetSingle(), "Node coordinates changed");
}
for (int i = 0; i < expectedPatches.Length; i++)
{
    var patch = expectedPatches[i];
    Check(plan.patches[i].nodes.SequenceEqual(patch.GetProperty("nodes").EnumerateArray().Select(n => n.GetInt32())), "Patch nodes changed");
    var edges = patch.GetProperty("frames").EnumerateArray().Select(e => (e.GetProperty("a").GetInt32(), e.GetProperty("b").GetInt32()));
    Check(plan.patches[i].frames.Select(e => (e.a, e.b)).SequenceEqual(edges), "Patch edges changed");
}

var before = new Snapshot
{
    tick = 123456789, starId = 3, layerId = 2, radius = 10000, unlockedLatitude = 72,
    nodes = new[] { new NodeRecord { id = 8, sp = 12, spMax = 30, position = plan.nodes[0].direction,
        frames = new[] { 4 }, shells = new[] { 2 } } },
    frames = new[] { new FrameRecord { id = 4, a = 8, b = 9, spA = 15, spB = 10, spMax = 40 } },
    shells = new[] { new ShellRecord { id = 2, nodes = new[] { 8, 9, 10 }, frames = new[] { 4, 5, 6 }, nodecps = new[] { 7, 8, 9, 24 } } }
};
var after = new Snapshot { tick = before.tick + 1, nodes = Array.Empty<NodeRecord>(), frames = Array.Empty<FrameRecord>(), shells = Array.Empty<ShellRecord>() };
var report = new Evidence { before = before, after = after, nodeMapping = new[] { new NodeMapping { canonicalId = 1, nativeId = 8 } } };
var json = ProbeJson.Write(report);
using var document = JsonDocument.Parse(json);
var root = document.RootElement;
Check(root.GetProperty("before").GetProperty("nodes")[0].GetProperty("sp").GetInt32() == 12, "Node progress missing");
Check(root.GetProperty("before").GetProperty("frames")[0].GetProperty("spB").GetInt32() == 10, "Frame progress missing");
Check(root.GetProperty("before").GetProperty("shells")[0].GetProperty("nodecps")[3].GetInt32() == 24, "Shell progress missing");
Check(root.GetProperty("after").GetProperty("nodes").GetArrayLength() == 0, "Empty snapshot omitted");
Check(root.GetProperty("nodeMapping")[0].GetProperty("nativeId").GetInt32() == 8, "Identity mapping missing");
Check(!root.GetProperty("before").GetProperty("nodes")[0].TryGetProperty("instance", out _), "Native object reference serialized");
using var roundTrip = new MemoryStream(Encoding.UTF8.GetBytes(json));
var restored = ProbeJson.Read<Evidence>(roundTrip);
Check(restored.before.tick == before.tick && restored.before.nodes[0].position.x == before.nodes[0].position.x, "Snapshot values changed");
Check(restored.before.nodes[0].shells.SequenceEqual(before.nodes[0].shells), "Shell associations changed");
Console.WriteLine("Embedded plan and populated/empty snapshot serialization checks passed.");
