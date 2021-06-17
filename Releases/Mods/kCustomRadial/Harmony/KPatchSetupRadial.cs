using HarmonyLib;
using GUI_2;
using System;
using UnityEngine;
using DMT;
using kScripts;


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
			___playerUI.windowManager.Open("radial", true, true, true);
			return false;
		}
		return true;

	}
}

[HarmonyPatch(typeof(XUiC_Radial))]
[HarmonyPatch("radialButtonPressed")]
class KRadialButtonPressedPatch
{

	public static void  Postfix(PlayerActionsLocal _actionSet, ref bool __result)
	{
		
		__result = _actionSet.Activate.IsPressed || _actionSet.PermanentActions.Activate.IsPressed ||
		       _actionSet.Reload.IsPressed || _actionSet.PermanentActions.Reload.IsPressed ||
		       _actionSet.ToggleFlashlight.IsPressed ||
		       _actionSet.PermanentActions.ToggleFlashlight.IsPressed ||
		       _actionSet.Inventory.IsPressed || _actionSet.VehicleActions.Inventory.IsPressed ||
		       _actionSet.PermanentActions.Inventory.IsPressed || _actionSet.Prefab.IsPressed;
		KHelper.EasyLog("RadialButtonPressed: {__result}", LogLevel.Chat);
		
	}
}