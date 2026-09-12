using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Logic")]

namespace DSPSphereBuilder
{
    internal readonly struct Position : IEquatable<Position>
    {
        public readonly float X, Y, Z;
        public Position(float x, float y, float z) { X = x; Y = y; Z = z; }
        public bool IsFinite => !(float.IsNaN(X) || float.IsNaN(Y) || float.IsNaN(Z)
            || float.IsInfinity(X) || float.IsInfinity(Y) || float.IsInfinity(Z));
        public Position AtRadius(float radius)
        {
            double length = Math.Sqrt((double)X * X + (double)Y * Y + (double)Z * Z);
            double scale = radius / length;
            return new Position((float)(X * scale), (float)(Y * scale), (float)(Z * scale));
        }
        public bool Equals(Position other) => X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
        public override bool Equals(object obj) => obj is Position other && Equals(other);
        public override int GetHashCode() => (X, Y, Z).GetHashCode();
    }

    internal readonly struct Edge
    {
        public readonly int A, B;
        public Edge(int a, int b) { A = a; B = b; }
    }

    internal sealed class Patch
    {
        public readonly int[] Nodes;
        public readonly Edge[] Frames;
        public Patch(int[] nodes, Edge[] frames) { Nodes = nodes; Frames = frames; }
    }

    internal static partial class SpherePlan
    {
        public static Position Position(int canonicalId, float radius) => Directions[canonicalId - 1].AtRadius(radius);
    }
}
