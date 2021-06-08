using DMT;
using HarmonyLib;



public class KPro_CustomRadial_Init : IHarmony
{
    public void Start()
    {
        // kScripts.kHelper.EasyLog("Version 1");
        var harmony = new Harmony("app.kpro.mod");
        harmony.PatchAll();
    }
}
