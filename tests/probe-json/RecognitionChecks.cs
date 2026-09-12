using System;
using System.IO;
using System.Linq;
using System.Text;
using DSPSphereBuilder.Feasibility;
using UnityEngine;

static class RecognitionChecks
{
    private static void Check(bool condition, string detail)
    {
        if (!condition) throw new InvalidOperationException(detail);
    }

    private static Snapshot Prefix(Plan plan, int count)
    {
        var points = plan.nodes.ToDictionary(n => n.id);
        return new Snapshot
        {
            starId = 60, layerId = 2, radius = 9700,
            nodes = plan.patches.Take(count).SelectMany(p => p.nodes).Select(n => new NodeRecord
            {
                id = 1000 - n * 7, position = GraphRecognition.Position(points[n], 9700),
                sp = 23, spMax = 30, frames = Array.Empty<int>(), shells = Array.Empty<int>()
            }).Reverse().ToArray(),
            frames = plan.patches.Take(count).SelectMany(p => p.frames).Select((e, i) => new FrameRecord
            {
                id = 2000 - i * 3, a = 1000 - e.b * 7, b = 1000 - e.a * 7, spA = 7, spB = 3
            }).Reverse().ToArray(),
            shells = Array.Empty<ShellRecord>()
        };
    }

    private static Snapshot RoundTrip(Snapshot snapshot)
    {
        using var input = new MemoryStream(Encoding.UTF8.GetBytes(ProbeJson.Write(snapshot)));
        return ProbeJson.Read<Snapshot>(input);
    }

    public static void Run(Plan plan)
    {
        for (int i = 0; i <= plan.patches.Length; i++)
        {
            var snapshot = RoundTrip(Prefix(plan, i));
            snapshot.starId = 8;
            snapshot.layerId = 1;
            var result = GraphRecognition.Match(plan, snapshot);
            Check(result.state == (i == 0 ? GraphState.Empty : i == plan.patches.Length ? GraphState.Complete : GraphState.Prefix)
                && result.completedPatches == i, "Prefix not recognized after reconstruction");
            Check(result.nodeMapping.All(n => n.nativeId == 1000 - n.canonicalId * 7), "Recycled IDs mapped incorrectly");
        }

        var edited = Prefix(plan, 2);
        edited.frames = edited.frames.Skip(1).ToArray();
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Missing frame accepted");
        edited = Prefix(plan, 2);
        edited.nodes = edited.nodes.Skip(1).ToArray();
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Missing node accepted");
        edited = Prefix(plan, 2);
        var pos = edited.nodes[0].position;
        edited.nodes[0].position = new Vector3(MathF.BitIncrement(pos.x), pos.y, pos.z);
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Prefix, "Float rounding prevented recognition");
        edited.nodes[0].position = new Vector3(pos.x + (float)(edited.radius * GraphRecognition.PositionRelativeTolerance * 2), pos.y, pos.z);
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Displaced coordinate accepted");
        edited = Prefix(plan, 2);
        edited.radius = 36000;
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Different radius accepted without scaled geometry");
        edited = Prefix(plan, 2);
        edited.nodes[0].position = edited.nodes[1].position;
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Duplicate canonical node accepted");
        edited = Prefix(plan, 2);
        edited.frames[0].euler = true;
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Euler frame accepted");
        edited = Prefix(plan, 2);
        edited.frames = edited.frames.Append(new FrameRecord { id = 9000, a = edited.nodes[0].id, b = edited.nodes[1].id }).ToArray();
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Additional frame accepted");
        edited = Prefix(plan, 2);
        edited.frames[0].a = edited.frames[1].a;
        edited.frames[0].b = edited.frames[1].b;
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Duplicate edge accepted");
        edited = Prefix(plan, 1);
        edited.nodes = edited.nodes.Append(Prefix(plan, 2).nodes.First(n => edited.nodes.All(old => old.id != n.id))).ToArray();
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Partial next patch accepted");
        edited = Prefix(plan, 0);
        edited.nodes = new[] { new NodeRecord { id = 1, position = new Vector3(9700, 0, 0) } };
        Check(GraphRecognition.Match(plan, edited).state == GraphState.Mismatch, "Unrelated node accepted");

        var complete = Prefix(plan, plan.patches.Length);
        complete.shells = plan.faces.Select((face, i) =>
        {
            var ids = face.Select(n => 1000 - n * 7).Reverse().ToArray();
            var frameIds = ids.Select((n, j) => complete.frames.Single(f =>
                (f.a == n && f.b == ids[(j + 1) % ids.Length]) || (f.b == n && f.a == ids[(j + 1) % ids.Length])).id).ToArray();
            return new ShellRecord { id = i + 1, nodes = ids, frames = frameIds, nodecps = new int[ids.Length + 1] };
        }).ToArray();
        Check(GraphRecognition.Match(plan, RoundTrip(complete)).state == GraphState.Complete, "Player shells prevented recognition");
        complete.shells[0].nodes[0] = complete.shells[0].nodes[1];
        Check(GraphRecognition.Match(plan, complete).state == GraphState.Mismatch, "Invalid shell boundary accepted");

        foreach (float radius in new[] { 0f, -1f, float.NaN, float.PositiveInfinity })
        {
            var empty = Prefix(plan, 0);
            empty.radius = radius;
            Check(GraphRecognition.Match(plan, empty).state == GraphState.Mismatch, "Invalid radius accepted");
        }
        Console.WriteLine("Continuation checks passed for every prefix, reconstructed IDs, edits, partial additions, and shell boundaries.");
    }
}
