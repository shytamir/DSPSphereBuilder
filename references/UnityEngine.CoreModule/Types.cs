using System;

namespace UnityEngine
{
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) => throw new NotSupportedException();
    }
    public struct Mathf
    {
        public static int RoundToInt(float value) => throw new NotSupportedException();
    }
    public struct Color32 { public byte r, g, b, a; }
    public class Object
    {
        public static bool operator ==(Object a, Object b) => throw new NotSupportedException();
        public static bool operator !=(Object a, Object b) => throw new NotSupportedException();
        public override bool Equals(object other) => throw new NotSupportedException();
        public override int GetHashCode() => throw new NotSupportedException();
    }
    public class Component : Object { }
    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }
}
