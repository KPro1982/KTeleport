using HarmonyLib;
using GUI_2;
using System;
using UnityEngine;
using DMT;
using kScripts;


[HarmonyPatch]
public class KCustomRadialPatches
{
	
	[HarmonyPatch(typeof(ItemActionAttack))]
	[HarmonyPatch("SetupRadial")]
	public static bool Prefix(ItemActionAttack __instance, XUiC_Radial _xuiRadialWindow,
		EntityPlayerLocal _epl)
	{
		KRadial.KSetupRadial(_xuiRadialWindow, _epl);
		return false; // no need to return true because KSetupRadial performs all tasks
	}


	[HarmonyPatch(typeof(PlayerMoveController))]
	[HarmonyPatch("Update")]
	public static void Postfix(PlayerMoveController __instance, LocalPlayerUI ___playerUI, EntityPlayerLocal ___entityPlayerLocal)
	{
		if (__instance.playerInput.Prefab.WasPressed)
		{
			// your stuff here
			___playerUI.xui.RadialWindow.Open();
			KRadial.KSetupRadial(___playerUI.xui.RadialWindow, ___entityPlayerLocal);
		}
	}


[HarmonyPatch(typeof(XUiC_Radial))]
[HarmonyPatch("radialButtonPressed")]

	public static void  Postfix(PlayerActionsLocal _actionSet, ref bool __result)
	{
		
		__result = _actionSet.Reload.IsPressed || _actionSet.PermanentActions.Reload.IsPressed ||
		           _actionSet.Prefab.IsPressed;
		KHelper.EasyLog("RadialButtonPressed: {__result}", LogLevel.Chat);
		
	}
}