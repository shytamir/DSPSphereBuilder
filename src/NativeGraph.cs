using System.Linq;
using UnityEngine;

namespace DSPSphereBuilder
{
    internal static class NativeGraph
    {
        private static uint Color(Color32 c) => (uint)(c.r | c.g << 8 | c.b << 16 | c.a << 24);

        public static LayerGraph Capture(DysonSphereLayer layer) => new LayerGraph
        {
            LayerId = layer.id, Radius = layer.orbitRadius,
            Nodes = layer.nodePool.Where(n => n != null && n.id > 0).Select(n => new GraphNode
            {
                Id = n.id, Prototype = n.protoId, Position = new Position(n.pos.x, n.pos.y, n.pos.z),
                Sp = n.sp, SpMax = n.spMax, Color = Color(n.color), Identity = n,
                Frames = n.frames.Select(f => f.id).OrderBy(id => id).ToArray(),
                Shells = n.shells.Select(s => s.id).OrderBy(id => id).ToArray()
            }).ToArray(),
            Frames = layer.framePool.Where(f => f != null && f.id > 0).Select(f => new GraphFrame
            {
                Id = f.id, Prototype = f.protoId, A = f.nodeA.id, B = f.nodeB.id, Euler = f.euler,
                SpA = f.spA, SpB = f.spB, SpMax = f.spMax, Color = Color(f.color), Identity = f
            }).ToArray(),
            Shells = layer.shellPool.Where(s => s != null && s.id > 0).Select(s => new GraphShell
            {
                Id = s.id, Prototype = s.protoId, Color = Color(s.color), Identity = s,
                Nodes = s.nodes.Select(n => n.id).ToArray(), Frames = s.frames.Select(f => f.id).ToArray(),
                NodeCps = (int[])s.nodecps.Clone()
            }).ToArray()
        };
    }
}
