using BepInEx;

namespace DSPSphereBuilder
{
    [BepInPlugin("dsp.spherebuilder", "DSP Sphere Builder", BuildInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("DSP Sphere Builder " + BuildInfo.Label);
        }
    }
}
