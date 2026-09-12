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
    public struct Vector2
    {
        public Vector2(float x, float y) => throw new NotSupportedException();
    }
    public struct Color
    {
        public Color(float r, float g, float b, float a) => throw new NotSupportedException();
    }
    public class Object
    {
        public static bool operator ==(Object a, Object b) => throw new NotSupportedException();
        public static bool operator !=(Object a, Object b) => throw new NotSupportedException();
        public override bool Equals(object other) => throw new NotSupportedException();
        public override int GetHashCode() => throw new NotSupportedException();
        public static void Destroy(Object value) => throw new NotSupportedException();
    }
    public class Component : Object
    {
        public Transform transform => throw new NotSupportedException();
        public GameObject gameObject => throw new NotSupportedException();
        public T GetComponent<T>() => throw new NotSupportedException();
        public T[] GetComponentsInChildren<T>(bool includeInactive) => throw new NotSupportedException();
    }
    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }
    public class Transform : Component
    {
        protected Transform() => throw new NotSupportedException();
        public void SetParent(Transform parent, bool worldPositionStays) => throw new NotSupportedException();
        public void SetAsLastSibling() => throw new NotSupportedException();
    }
    public sealed class RectTransform : Transform
    {
        private RectTransform() { }
        public Vector2 anchorMin { set => throw new NotSupportedException(); }
        public Vector2 anchorMax { set => throw new NotSupportedException(); }
        public Vector2 pivot { set => throw new NotSupportedException(); }
        public Vector2 anchoredPosition { set => throw new NotSupportedException(); }
        public Vector2 sizeDelta { set => throw new NotSupportedException(); }
    }
    public sealed class GameObject : Object
    {
        public GameObject(string name, params Type[] components) => throw new NotSupportedException();
        public Transform transform => throw new NotSupportedException();
        public T GetComponent<T>() => throw new NotSupportedException();
        public T AddComponent<T>() => throw new NotSupportedException();
        public void SetActive(bool active) => throw new NotSupportedException();
    }
    public class Time
    {
        private Time() { }
        public static float unscaledTime => throw new NotSupportedException();
    }
}

namespace UnityEngine.Events
{
    public delegate void UnityAction();
    public abstract class UnityEventBase
    {
        protected UnityEventBase() => throw new NotSupportedException();
    }
    public class UnityEvent : UnityEventBase
    {
        public UnityEvent() => throw new NotSupportedException();
        public void AddListener(UnityAction call) => throw new NotSupportedException();
        public void RemoveListener(UnityAction call) => throw new NotSupportedException();
    }
}
