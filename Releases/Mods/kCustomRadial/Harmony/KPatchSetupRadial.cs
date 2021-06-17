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

[HarmonyPatch(typeof(PlayerMoveController))]
[HarmonyPatch("Update")]
class Patch
{
	public static bool Prefix(PlayerMoveController __instance, LocalPlayerUI ___playerUI, EntityPlayerLocal ___entityPlayerLocal)
	{
		if (__instance.playerInput.Prefab.WasPressed)
		{
			// your stuff here
			___playerUI.xui.RadialWindow.Open();
			KRadial.KSetupRadial(___playerUI.xui.RadialWindow, ___entityPlayerLocal);
			return true;
		}
		return true;

	}
}