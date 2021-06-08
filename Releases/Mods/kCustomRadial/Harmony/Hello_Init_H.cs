using DMT;
using HarmonyLib;
using UnityEngine;



public class KPro_CustomRadial_Init : IHarmony
{
    public void Start()
    {

        var harmony = new Harmony("app.kpro.mod");
        harmony.PatchAll();
    }
}
