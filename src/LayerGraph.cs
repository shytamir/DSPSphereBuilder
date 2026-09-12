using System;

namespace DSPSphereBuilder
{
    internal sealed class GraphNode
    {
        public int Id, Prototype, Sp, SpMax;
        public Position Position;
        public uint Color;
        public int[] Frames = Array.Empty<int>(), Shells = Array.Empty<int>();
        public object Identity;
    }

    internal sealed class GraphFrame
    {
        public int Id, Prototype, A, B, SpA, SpB, SpMax;
        public bool Euler;
        public uint Color;
        public object Identity;
    }

    internal sealed class GraphShell
    {
        public int Id, Prototype;
        public uint Color;
        public int[] Nodes = Array.Empty<int>(), Frames = Array.Empty<int>(), NodeCps = Array.Empty<int>();
        public object Identity;
    }

    internal sealed class LayerGraph
    {
        public int LayerId;
        public float Radius;
        public GraphNode[] Nodes = Array.Empty<GraphNode>();
        public GraphFrame[] Frames = Array.Empty<GraphFrame>();
        public GraphShell[] Shells = Array.Empty<GraphShell>();
    }
}
