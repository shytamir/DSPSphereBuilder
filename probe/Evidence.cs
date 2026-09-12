using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DSPSphereBuilder.Feasibility
{
    [Serializable]
    public class NodeRecord
    {
        public int id, protoId, sp, spMax, spOrdered, cpOrdered;
        public Vector3 position;
        public Color32 color;
        public int[] frames, shells;
        [NonSerialized] public DysonNode instance;
    }

    [Serializable]
    public class FrameRecord
    {
        public int id, protoId, a, b, spA, spB, spMax;
        public bool euler;
        public Color32 color;
        [NonSerialized] public DysonFrame instance;
    }

    [Serializable]
    public class ShellRecord
    {
        public int id, protoId;
        public int[] nodes, frames, nodecps;
        public Color32 color;
        [NonSerialized] public DysonShell instance;
    }

    [Serializable]
    public class Snapshot
    {
        public long tick;
        public int starId, starSeed, layerId;
        public string starType;
        public float radius, minimumRadius, maximumRadius, unlockedLatitude;
        public NodeRecord[] nodes;
        public FrameRecord[] frames;
        public ShellRecord[] shells;

        public static Snapshot Capture(DysonSphereLayer layer)
        {
            return new Snapshot
            {
                tick = GameMain.gameTick, starId = layer.starData.id, starSeed = layer.starData.seed,
                starType = layer.starData.type.ToString(), layerId = layer.id, radius = layer.orbitRadius,
                minimumRadius = layer.dysonSphere.minOrbitRadius, maximumRadius = layer.dysonSphere.maxOrbitRadius,
                unlockedLatitude = GameMain.history.dysonNodeLatitude,
                nodes = layer.nodePool.Where(n => n != null && n.id > 0).Select(n => new NodeRecord
                {
                    id = n.id, protoId = n.protoId, position = n.pos, sp = n.sp, spMax = n.spMax,
                    spOrdered = n.spOrdered, cpOrdered = n.cpOrdered, color = n.color,
                    frames = n.frames.Select(f => f.id).OrderBy(i => i).ToArray(),
                    shells = n.shells.Select(s => s.id).OrderBy(i => i).ToArray(), instance = n
                }).ToArray(),
                frames = layer.framePool.Where(f => f != null && f.id > 0).Select(f => new FrameRecord
                {
                    id = f.id, protoId = f.protoId, a = f.nodeA.id, b = f.nodeB.id,
                    euler = f.euler, spA = f.spA, spB = f.spB, spMax = f.spMax, color = f.color, instance = f
                }).ToArray(),
                shells = layer.shellPool.Where(s => s != null && s.id > 0).Select(s => new ShellRecord
                {
                    id = s.id, protoId = s.protoId, nodes = s.nodes.Select(n => n.id).ToArray(),
                    frames = s.frames.Select(f => f.id).ToArray(), nodecps = (int[])s.nodecps.Clone(),
                    color = s.color, instance = s
                }).ToArray()
            };
        }

        public static string[] Compare(Snapshot before, Snapshot after)
        {
            var failures = new List<string>();
            foreach (var n in before.nodes)
            {
                var next = after.nodes.SingleOrDefault(x => x.id == n.id);
                if (next == null || !ReferenceEquals(n.instance, next.instance) || !n.position.Equals(next.position)
                    || n.protoId != next.protoId || !n.color.Equals(next.color) || next.sp < n.sp || next.spMax != n.spMax
                    || n.frames.Except(next.frames).Any() || !n.shells.SequenceEqual(next.shells))
                    failures.Add("Changed existing node " + n.id);
            }
            foreach (var f in before.frames)
            {
                var next = after.frames.SingleOrDefault(x => x.id == f.id);
                if (next == null || !ReferenceEquals(f.instance, next.instance) || f.a != next.a || f.b != next.b
                    || f.protoId != next.protoId || f.euler != next.euler || !f.color.Equals(next.color)
                    || next.spA < f.spA || next.spB < f.spB || next.spMax != f.spMax)
                    failures.Add("Changed existing frame " + f.id);
            }
            foreach (var s in before.shells)
            {
                var next = after.shells.SingleOrDefault(x => x.id == s.id);
                if (next == null || !ReferenceEquals(s.instance, next.instance) || s.protoId != next.protoId
                    || !s.color.Equals(next.color) || !s.nodes.SequenceEqual(next.nodes) || !s.frames.SequenceEqual(next.frames)
                    || next.nodecps.Length != s.nodecps.Length || s.nodecps.Where((cp,i) => next.nodecps[i] < cp).Any())
                    failures.Add("Changed existing shell " + s.id);
            }
            if (after.shells.Length != before.shells.Length) failures.Add("Shell count changed");
            return failures.ToArray();
        }
    }
}
