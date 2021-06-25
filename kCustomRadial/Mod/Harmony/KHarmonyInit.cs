using System;
using DMT;
using HarmonyLib;
using kScripts;


public class KHarmonyInit : IHarmony
{
    public void Start()
    {
        // kScripts.kHelper.EasyLog("Version 1");
        var harmony = new Harmony("app.kpro.mod");
        harmony.PatchAll();
        KHelper.EasyLog($"Log File Created: {DateTime.Now}", LogLevel.File);
        
    }
}
