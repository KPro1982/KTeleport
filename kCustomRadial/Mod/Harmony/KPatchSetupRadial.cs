using HarmonyLib;
using GUI_2;
using System;
using UnityEngine;
using DMT;


[HarmonyPatch]
public class KPatchCustomRadial
{
	
	[HarmonyPatch(typeof(ItemActionAttack))]
	[HarmonyPatch("SetupRadial")]
	public static bool Prefix(ItemActionAttack __instance, XUiC_Radial _xuiRadialWindow,
		EntityPlayerLocal _epl)
	{
		KRadial.KSetupRadial(_xuiRadialWindow, _epl);
		return false; // no need to return true because KSetupRadial performs all tasks
	}
}