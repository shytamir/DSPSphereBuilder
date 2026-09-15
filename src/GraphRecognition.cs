using System;
using System.Collections.Generic;
using System.Linq;

namespace DSPSphereBuilder
{
    internal enum GraphState { Mismatch, Empty, Prefix, Complete }

    internal sealed class Recognition
    {
        public GraphState State;
        public int CompletedPatches;
        public PlanOrientation Orientation;
        public Dictionary<int, int> NativeIds = new Dictionary<int, int>();
    }

    internal static class GraphRecognition
    {
        // Accumulated binary32 rounding allowance: 10u/(1-10u), where u = 2^-24.
        public const double PositionRelativeTolerance = 10.0 / (16777216.0 - 10.0);

        private static bool SamePosition(Position actual, Position expected, float radius)
        {
            double x = (double)actual.X - expected.X, y = (double)actual.Y - expected.Y, z = (double)actual.Z - expected.Z;
            double tolerance = radius * PositionRelativeTolerance;
            return x * x + y * y + z * z <= tolerance * tolerance;
        }

        internal static (int, int) Pair(int a, int b) => a < b ? (a, b) : (b, a);
        private static HashSet<(int, int)> Boundary(int[] nodes) => new HashSet<(int, int)>(
            nodes.Select((n, i) => Pair(n, nodes[(i + 1) % nodes.Length])));

        public static Recognition Match(LayerGraph graph)
        {
            var match = Match(graph, PlanOrientation.Grid);
            return match.State == GraphState.Mismatch ? Match(graph, PlanOrientation.Legacy) : match;
        }

        private static Recognition Match(LayerGraph graph, PlanOrientation orientation)
        {
            var mismatch = new Recognition();
            if (graph.Radius <= 0 || float.IsNaN(graph.Radius) || float.IsInfinity(graph.Radius)) return mismatch;
            if (graph.Nodes.Length == 0 && graph.Frames.Length == 0 && graph.Shells.Length == 0)
                return new Recognition { State = GraphState.Empty };
            if (graph.Nodes.Length > 60 || graph.Frames.Length > 90 || graph.Shells.Length > 32) return mismatch;

            var positions = Enumerable.Range(1, 60).ToDictionary(id => id, id => SpherePlan.Position(id, graph.Radius, orientation));
            var nativeToCanonical = new Dictionary<int, int>();
            var canonicalNodes = new HashSet<int>();
            foreach (var node in graph.Nodes)
            {
                var matches = positions.Where(p => SamePosition(node.Position, p.Value, graph.Radius)).Select(p => p.Key).ToArray();
                if (node.Id <= 0 || matches.Length != 1 || nativeToCanonical.ContainsKey(node.Id)
                    || !canonicalNodes.Add(matches[0])) return mismatch;
                nativeToCanonical.Add(node.Id, matches[0]);
            }

            var actualEdges = new HashSet<(int, int)>();
            var frameEdges = new Dictionary<int, (int, int)>();
            foreach (var frame in graph.Frames)
            {
                if (frame.Id <= 0 || frameEdges.ContainsKey(frame.Id) || frame.Euler
                    || !nativeToCanonical.TryGetValue(frame.A, out int a)
                    || !nativeToCanonical.TryGetValue(frame.B, out int b)) return mismatch;
                var edge = Pair(a, b);
                if (!actualEdges.Add(edge)) return mismatch;
                frameEdges.Add(frame.Id, edge);
            }

            var shellIds = new HashSet<int>();
            var shellFaces = new HashSet<int>();
            foreach (var shell in graph.Shells)
            {
                if (shell.Id <= 0 || !shellIds.Add(shell.Id)
                    || shell.Nodes.Any(n => !nativeToCanonical.ContainsKey(n))
                    || shell.Frames.Any(f => !frameEdges.ContainsKey(f))) return mismatch;
                var boundary = shell.Nodes.Select(n => nativeToCanonical[n]).ToArray();
                var edges = Boundary(boundary);
                var face = Array.FindIndex(SpherePlan.Faces, f => f.Length == boundary.Length && Boundary(f).SetEquals(edges));
                if (boundary.Distinct().Count() != boundary.Length || edges.Count != boundary.Length
                    || face < 0 || !shellFaces.Add(face) || shell.Frames.Length != edges.Count
                    || !edges.SetEquals(shell.Frames.Select(f => frameEdges[f]))) return mismatch;
            }

            var expectedNodes = new HashSet<int>();
            var expectedEdges = new HashSet<(int, int)>();
            for (int i = 0; i < SpherePlan.Patches.Length; i++)
            {
                expectedNodes.UnionWith(SpherePlan.Patches[i].Nodes);
                expectedEdges.UnionWith(SpherePlan.Patches[i].Frames.Select(e => Pair(e.A, e.B)));
                if (!expectedNodes.SetEquals(canonicalNodes) || !expectedEdges.SetEquals(actualEdges)) continue;
                return new Recognition
                {
                    State = i + 1 == SpherePlan.Patches.Length ? GraphState.Complete : GraphState.Prefix,
                    CompletedPatches = i + 1,
                    Orientation = orientation,
                    NativeIds = nativeToCanonical.ToDictionary(p => p.Value, p => p.Key)
                };
            }
            return mismatch;
        }
    }
}
