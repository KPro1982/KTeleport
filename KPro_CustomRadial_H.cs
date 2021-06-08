using HarmonyLib;
using GUI_2;
using System;
using UnityEngine;
using DMT;


[HarmonyPatch]
public class KPro_CustomRadial
{
	
	[HarmonyPatch(typeof(ItemActionAttack))]
	[HarmonyPatch("SetupRadial")]
	public static  bool Prefix(ItemActionAttack __instance, XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
	{
		bool isCustomRadial = false;
				_xuiRadialWindow.ResetRadialEntries();
		string[] magazineItemNames = _epl.inventory.GetHoldingGun().MagazineItemNames;
		int preSelectedCommandIndex = -1;

        LogAnywhere.Log(String.Format($"Before for loop: magazineItemNames.length: {magazineItemNames.Length}"));
        LogAnywhere.Log(magazineItemNames);
        if (magazineItemNames[0] == "KPro_CustomRadial")
		{
			isCustomRadial = true;
			LogAnywhere.Log("FOUND a CustomRadial");

			for (int i = 0; i < magazineItemNames.Length; i++)
			{
				ItemClass itemClass = ItemClass.GetItemClass(magazineItemNames[i], false);
				if (itemClass != null)
				{
					int itemCount = _xuiRadialWindow.xui.PlayerInventory.GetItemCount(itemClass.Id);
					_xuiRadialWindow.CreateRadialEntry(i, itemClass.GetIconName(), "ItemIconAtlas", String.Format(" "), itemClass.GetLocalizedItemName(), false);
				}
			}



			_xuiRadialWindow.SetCommonData(UIUtils.ButtonIcon.FaceButtonEast, new Action<XUiC_Radial, int, XUiC_Radial.RadialContextAbs>(KPro_handleRadialCommand), new KPro_RadialContextItem((ItemActionRanged)_epl.inventory.GetHoldingGun()), preSelectedCommandIndex, false);

		} else
        {
			for (int i = 0; i < magazineItemNames.Length; i++)
			{
				ItemClass itemClass = ItemClass.GetItemClass(magazineItemNames[i], false);
				if (itemClass != null && (!_epl.IsInWater() || itemClass.UsableUnderwater))
				{
					int itemCount = _xuiRadialWindow.xui.PlayerInventory.GetItemCount(itemClass.Id);
					bool flag = (int)_epl.inventory.holdingItemItemValue.SelectedAmmoTypeIndex == i;
					_xuiRadialWindow.CreateRadialEntry(i, itemClass.GetIconName(), (itemCount > 0) ? "ItemIconAtlas" : "ItemIconAtlasGreyscale", itemCount.ToString(), itemClass.GetLocalizedItemName(), flag);


					if (flag)
					{
						LogAnywhere.Log(String.Format($"Inside if(flag) i = {i}"));
						preSelectedCommandIndex = i;
					}
				}
			}
	



			_xuiRadialWindow.SetCommonData(UIUtils.ButtonIcon.FaceButtonEast, new Action<XUiC_Radial, int, XUiC_Radial.RadialContextAbs>(KPro_handleRadialCommand), new KPro_RadialContextItem((ItemActionRanged)_epl.inventory.GetHoldingGun()), preSelectedCommandIndex, false);


		}
		

		return false;
    }

	public static void KPro_handleRadialCommand(XUiC_Radial _sender, int _commandIndex, XUiC_Radial.RadialContextAbs _context)
	{
		LogAnywhere.Log(String.Format($"KPro_handleRadialCommand: _commandIndex {_commandIndex}"));
		
		KPro_RadialContextItem radialContextItem = _context as KPro_RadialContextItem;
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

				LogAnywhere.Log(String.Format($"itemClass.Name {itemClass.Name}"));
				bool result = itemClass.HasTrigger(MinEventTypes.onSelfPrimaryActionEnd);
				LogAnywhere.Log(String.Format($"{itemClass.Name} has a trigger: {result}"));

				var num = itemClass.Effects.EffectGroups.Count;
				LogAnywhere.Log(String.Format($"Effects group has {num} elements."));
				if (num == 1)
                {
					itemClass.FireEvent(MinEventTypes.onSelfPrimaryActionEnd, MinEventParams.CachedEventParam);
				} else
                {
					LogAnywhere.Log(String.Format($"Error Effects group has {num} elements but should have 1 element."));
				}
				

				
			}
		


		}
	}



}


public class KPro_RadialContextItem : XUiC_Radial.RadialContextAbs
{
	// Token: 0x060023E5 RID: 9189 RVA: 0x000E53BB File Offset: 0x000E35BB
	public KPro_RadialContextItem(ItemActionRanged _rangedItemAction)
	{
		//LogAnywhere.Log("KPro_RadicalContextItem Constructor: _rangedItemAction:");
		//LogAnywhere.Log(_rangedItemAction);
		this.RangedItemAction = _rangedItemAction;
	}

	// Token: 0x04001CD6 RID: 7382
	public readonly ItemActionRanged RangedItemAction;
}





