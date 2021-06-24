using HarmonyLib;
using GUI_2;
using System;
using System.Collections.Generic;
using UnityEngine;
using DMT;
using kScripts;
using kCustomRadial.Mod.Scripts;


[HarmonyPatch]
public class KPatchCustomRadial
{

	[HarmonyPatch(typeof(ItemActionAttack))]
	[HarmonyPatch("SetupRadial")]
	public static bool Prefix(ItemActionAttack __instance, XUiC_Radial _xuiRadialWindow,
		EntityPlayerLocal _epl)
	{
		KRadial.KSetupRadial(_xuiRadialWindow, _epl);
		// _xuiRadialWindow.OnDoubleClick += KOnDoubleClicked;
		return false; // no need to return true because KRadial performs all tasks
	}
	
	[HarmonyPatch(typeof(XUiC_Radial))]
	[HarmonyPatch("XUiC_Radial_OnHover")]
	
	public static void Postfix(XUiC_Radial __instance, XUiController _sender, OnHoverEventArgs _e)
	{
		XUiC_RadialEntry xuiC_RadialEntry = (XUiC_RadialEntry)_sender;
		KEventHandler.KOnHover( __instance, _sender, _e);
	}

	

	

}
