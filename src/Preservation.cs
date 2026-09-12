using System;
using System.Linq;

namespace DSPSphereBuilder
{
    internal static class Preservation
    {
        public static void Check(LayerGraph before, LayerGraph after)
        {
            if (before.LayerId != after.LayerId || before.Radius != after.Radius)
                throw new InvalidOperationException("Layer identity or radius changed during addition.");
            foreach (var node in before.Nodes)
            {
                var next = after.Nodes.SingleOrDefault(n => n.Id == node.Id);
                if (next == null || !ReferenceEquals(node.Identity, next.Identity) || !node.Position.Equals(next.Position)
                    || node.Prototype != next.Prototype || node.Color != next.Color || next.Sp < node.Sp || node.SpMax != next.SpMax
                    || node.Frames.Except(next.Frames).Any() || !node.Shells.SequenceEqual(next.Shells))
                    throw new InvalidOperationException("Existing node changed: " + node.Id);
            }
            foreach (var frame in before.Frames)
            {
                var next = after.Frames.SingleOrDefault(f => f.Id == frame.Id);
                if (next == null || !ReferenceEquals(frame.Identity, next.Identity) || frame.A != next.A || frame.B != next.B
                    || frame.Prototype != next.Prototype || frame.Euler != next.Euler || frame.Color != next.Color
                    || next.SpA < frame.SpA || next.SpB < frame.SpB || frame.SpMax != next.SpMax)
                    throw new InvalidOperationException("Existing frame changed: " + frame.Id);
            }
            foreach (var shell in before.Shells)
            {
                var next = after.Shells.SingleOrDefault(s => s.Id == shell.Id);
                if (next == null || !ReferenceEquals(shell.Identity, next.Identity) || shell.Prototype != next.Prototype
                    || shell.Color != next.Color || !shell.Nodes.SequenceEqual(next.Nodes) || !shell.Frames.SequenceEqual(next.Frames)
                    || shell.NodeCps.Length != next.NodeCps.Length || shell.NodeCps.Where((cp, i) => next.NodeCps[i] < cp).Any())
                    throw new InvalidOperationException("Existing shell changed: " + shell.Id);
            }
            if (before.Shells.Length != after.Shells.Length) throw new InvalidOperationException("Shell count changed during addition.");
        }
    }
}
