using System;
using DMT;
using HarmonyLib;
using KTeleport;

public class KHarmonyInit : IHarmony
{
    public void Start()
    {
        // KTeleport.kHelper.EasyLog("Version 1");
        var harmony = new Harmony("app.kpro.mod");
        harmony.PatchAll();
        KHelper.EasyLog($"Log File Created: {DateTime.Now}", LogLevel.File);
    }
}