using HarmonyLib;
using GUI_2;
using System;
using UnityEngine;
using DMT;
using kScripts;


[HarmonyPatch]
public class KProCustomRadial
{
	
	[HarmonyPatch(typeof(ItemActionAttack))]
	[HarmonyPatch("SetupRadial")]
	public static bool Prefix(ItemActionAttack __instance, XUiC_Radial _xuiRadialWindow,
		EntityPlayerLocal _epl)
	{
		LogLevel log = LogLevel.None;
		// bool isCustomRadial = false;
		_xuiRadialWindow.ResetRadialEntries();
		string[] magazineItemNames = _epl.inventory.GetHoldingGun().MagazineItemNames;
		int preSelectedCommandIndex = -1;
		KHelper.EasyLog($"Before for loop: magazineItemNames.length: {magazineItemNames.Length}",
			log);
		KHelper.EasyLog(magazineItemNames, log);
		if (magazineItemNames[0] == "KProCustomRadial")
		{
			// isCustomRadial = true;
			KHelper.EasyLog("FOUND a CustomRadial", log);

			for (int i = 0; i < magazineItemNames.Length; i++)
			{
				ItemClass itemClass = ItemClass.GetItemClass(magazineItemNames[i], false);
				if (itemClass != null)
				{
					int itemCount = _xuiRadialWindow.xui.PlayerInventory.GetItemCount(itemClass.Id);
					_xuiRadialWindow.CreateRadialEntry(i, itemClass.GetIconName(), "ItemIconAtlas",
						String.Format(" "), itemClass.GetLocalizedItemName(), false);
				}
			}

			_xuiRadialWindow.SetCommonData(UIUtils.ButtonIcon.FaceButtonEast,
				new Action<XUiC_Radial, int, XUiC_Radial.RadialContextAbs>(KProHandleRadialCommand),
				new KProRadialContextItem((ItemActionRanged) _epl.inventory.GetHoldingGun()),
				preSelectedCommandIndex, false);
		}
		else
		{
			for (int i = 0; i < magazineItemNames.Length; i++)
			{
				ItemClass itemClass = ItemClass.GetItemClass(magazineItemNames[i], false);
				if (itemClass != null && (!_epl.IsInWater() || itemClass.UsableUnderwater))
				{
					int itemCount = _xuiRadialWindow.xui.PlayerInventory.GetItemCount(itemClass.Id);
					bool flag = (int) _epl.inventory.holdingItemItemValue.SelectedAmmoTypeIndex ==
					            i;
					_xuiRadialWindow.CreateRadialEntry(i, itemClass.GetIconName(),
						(itemCount > 0) ? "ItemIconAtlas" : "ItemIconAtlasGreyscale",
						itemCount.ToString(), itemClass.GetLocalizedItemName(), flag);


					if (flag)
					{
						KHelper.EasyLog($"Inside if(flag) i = {i}", log);
						preSelectedCommandIndex = i;
					}
				}
			}
			_xuiRadialWindow.SetCommonData(UIUtils.ButtonIcon.FaceButtonEast,
				new Action<XUiC_Radial, int, XUiC_Radial.RadialContextAbs>(KProHandleRadialCommand),
				new KProRadialContextItem((ItemActionRanged) _epl.inventory.GetHoldingGun()),
				preSelectedCommandIndex, false);
		}
		return false;
	}

	public static void KProHandleRadialCommand(XUiC_Radial _sender, int _commandIndex,
		XUiC_Radial.RadialContextAbs _context)
	{
		LogLevel log = LogLevel.None;
		KHelper.EasyLog($"KProHandleRadialCommand: _commandIndex {_commandIndex}", log);
		KProRadialContextItem radialContextItem = _context as KProRadialContextItem;
		EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
		string[] magazineItemNames = entityPlayer.inventory.GetHoldingGun().MagazineItemNames;
		if (radialContextItem == null)
		{
			return;
		}

		if (radialContextItem.RangedItemAction == entityPlayer.inventory.GetHoldingGun())
		{
			ItemClass itemClass = ItemClass.GetItemClass(magazineItemNames[_commandIndex], false);
			if (itemClass != null)
			{
				KHelper.EasyLog($"itemClass.Name {itemClass.Name}", log);
				bool result = itemClass.HasTrigger(MinEventTypes.onSelfPrimaryActionEnd);
				KHelper.EasyLog($"{itemClass.Name} has a trigger: {result}", log);
				var num = itemClass.Effects.EffectGroups.Count;
				KHelper.EasyLog($"Effects group has {num} elements.", log);
				if (num == 1)
				{
					itemClass.FireEvent(MinEventTypes.onSelfPrimaryActionEnd,
						MinEventParams.CachedEventParam);
				}
				else
				{
					KHelper.EasyLog(
						$"Error Effects group has {num} elements but should have 1 element.", log);
				}
			}
		}
	}
}


public class KProRadialContextItem : XUiC_Radial.RadialContextAbs
{
	LogLevel log = LogLevel.None;
	// Token: 0x060023E5 RID: 9189 RVA: 0x000E53BB File Offset: 0x000E35BB
	public KProRadialContextItem(ItemActionRanged _rangedItemAction)
	{
		KHelper.EasyLog("KPro_RadicalContextItem Constructor: _rangedItemAction:", log);
		KHelper.EasyLog(_rangedItemAction, log);
		this.RangedItemAction = _rangedItemAction;
	}

	// Token: 0x04001CD6 RID: 7382
	public readonly ItemActionRanged RangedItemAction;
}





