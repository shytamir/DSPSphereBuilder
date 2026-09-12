using System;

namespace BepInEx
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BepInPlugin : Attribute
    {
        public BepInPlugin(string guid, string name, string version) => throw new NotSupportedException();
    }

    public abstract class BaseUnityPlugin : UnityEngine.MonoBehaviour
    {
        protected BaseUnityPlugin() => throw new NotSupportedException();
        protected Logging.ManualLogSource Logger => throw new NotSupportedException();
    }
}

namespace BepInEx.Logging
{
    public class ManualLogSource
    {
        private ManualLogSource() { }
        public void LogInfo(object data) => throw new NotSupportedException();
        public void LogError(object data) => throw new NotSupportedException();
    }
}
