using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DSPSphereBuilder.Feasibility
{
    public enum GraphState { Mismatch, Empty, Prefix, Complete }

    [Serializable]
    public class Recognition
    {
        public GraphState state;
        public int completedPatches;
        public NodeMapping[] nodeMapping = Array.Empty<NodeMapping>();
    }

    public static class GraphRecognition
    {
        public const double PositionRelativeTolerance = 10.0 / (16777216.0 - 10.0);

        public static Vector3 Position(Point point, float radius) => point.direction.normalized * radius;

        private static bool SamePosition(Vector3 actual, Vector3 expected, float radius)
        {
            double x = (double)actual.x - expected.x, y = (double)actual.y - expected.y, z = (double)actual.z - expected.z;
            double tolerance = radius * PositionRelativeTolerance;
            return x * x + y * y + z * z <= tolerance * tolerance;
        }

        private static (int, int) Pair(int a, int b) => a < b ? (a, b) : (b, a);

        private static HashSet<(int, int)> Boundary(int[] nodes)
        {
            return new HashSet<(int, int)>(nodes.Select((n, i) => Pair(n, nodes[(i + 1) % nodes.Length])));
        }

        public static Recognition Match(Plan plan, Snapshot snapshot)
        {
            var mismatch = new Recognition { state = GraphState.Mismatch };
            if (snapshot.radius <= 0 || float.IsNaN(snapshot.radius) || float.IsInfinity(snapshot.radius)) return mismatch;
            if (snapshot.nodes.Length == 0 && snapshot.frames.Length == 0 && snapshot.shells.Length == 0)
                return new Recognition { state = GraphState.Empty };

            var positions = plan.nodes.ToDictionary(n => n.id, n => Position(n, snapshot.radius));
            var nativeToCanonical = new Dictionary<int, int>();
            var canonicalNodes = new HashSet<int>();
            foreach (var node in snapshot.nodes)
            {
                var matches = positions.Where(p => SamePosition(node.position, p.Value, snapshot.radius)).Select(p => p.Key).ToArray();
                if (node.id <= 0 || matches.Length != 1 || nativeToCanonical.ContainsKey(node.id)
                    || !canonicalNodes.Add(matches[0])) return mismatch;
                nativeToCanonical.Add(node.id, matches[0]);
            }

            var actualEdges = new HashSet<(int, int)>();
            var frameEdges = new Dictionary<int, (int, int)>();
            foreach (var frame in snapshot.frames)
            {
                if (frame.id <= 0 || frameEdges.ContainsKey(frame.id) || frame.euler
                    || !nativeToCanonical.TryGetValue(frame.a, out int a)
                    || !nativeToCanonical.TryGetValue(frame.b, out int b)) return mismatch;
                var edge = Pair(a, b);
                if (!actualEdges.Add(edge)) return mismatch;
                frameEdges.Add(frame.id, edge);
            }

            foreach (var shell in snapshot.shells)
            {
                if (shell.nodes.Any(n => !nativeToCanonical.ContainsKey(n))
                    || shell.frames.Any(f => !frameEdges.ContainsKey(f))) return mismatch;
                var boundary = shell.nodes.Select(n => nativeToCanonical[n]).ToArray();
                var edges = Boundary(boundary);
                if (boundary.Distinct().Count() != boundary.Length || edges.Count != boundary.Length
                    || !plan.faces.Any(f => f.Length == boundary.Length && Boundary(f).SetEquals(edges))
                    || shell.frames.Length != edges.Count || !edges.SetEquals(shell.frames.Select(f => frameEdges[f]))) return mismatch;
            }

            var expectedNodes = new HashSet<int>();
            var expectedEdges = new HashSet<(int, int)>();
            for (int i = 0; i < plan.patches.Length; i++)
            {
                expectedNodes.UnionWith(plan.patches[i].nodes);
                expectedEdges.UnionWith(plan.patches[i].frames.Select(e => Pair(e.a, e.b)));
                if (!expectedNodes.SetEquals(canonicalNodes) || !expectedEdges.SetEquals(actualEdges)) continue;
                return new Recognition
                {
                    state = i + 1 == plan.patches.Length ? GraphState.Complete : GraphState.Prefix,
                    completedPatches = i + 1,
                    nodeMapping = nativeToCanonical.Select(p => new NodeMapping { nativeId = p.Key, canonicalId = p.Value })
                        .OrderBy(p => p.canonicalId).ToArray()
                };
            }
            return mismatch;
        }
    }
}
