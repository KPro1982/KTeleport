using System;
using GUI_2;
using kScripts;

public class KRadial
{
    public static void KSetupRadial(XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
    {
        LogLevel log = LogLevel.Both;

        _xuiRadialWindow.ResetRadialEntries();
        string[] magazineItemNames = _epl.inventory.GetHoldingGun().MagazineItemNames;
        string[] radialItemNames = GetRadialItems();
        int preSelectedCommandIndex = -1;
        
        KHelper.EasyLog($"Before for loop: {radialItemNames.Length} radial items found", log);

        if (radialItemNames.Length > 0)
        {

            for (int i = 0; i < radialItemNames.Length; i++)
            {
                ItemClass itemClass = ItemClass.GetItemClass(radialItemNames[i], false);
                if (itemClass != null)
                {
                    // int itemCount = _xuiRadialWindow.xui.PlayerInventory.GetItemCount(itemClass.Id);
                    _xuiRadialWindow.CreateRadialEntry(i, itemClass.GetIconName(), "ItemIconAtlas",
                        String.Format(" "), itemClass.GetLocalizedItemName(), false);
                }
            }

            _xuiRadialWindow.SetCommonData(UIUtils.ButtonIcon.FaceButtonEast,
                new Action<XUiC_Radial, int, XUiC_Radial.RadialContextAbs>(
                    KProHandleCustomRadialCommand),
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
                    bool flag = (int) _epl.inventory.holdingItemItemValue.SelectedAmmoTypeIndex == i;
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
                new Action<XUiC_Radial, int, XUiC_Radial.RadialContextAbs>(
                    KProHandleVanillaRadialCommand),
                new KProRadialContextItem((ItemActionRanged) _epl.inventory.GetHoldingGun()),
                preSelectedCommandIndex, false);
        }
    }

    private static string[] GetRadialItems()
    {
        string strRadialItems = KHelper.GetXmlProperty("KTeleport", "RadialItems");
        if (strRadialItems.Length > 0)
        {
            return strRadialItems.Split(',');
        }
        return new[] {""};
    }

    public static void KProHandleVanillaRadialCommand(
        XUiC_Radial _sender,
        int _commandIndex,
        XUiC_Radial.RadialContextAbs _context)
    {
        KProRadialContextItem vanillaRadialContextItem = _context as KProRadialContextItem;
        if (!(_context is KProRadialContextItem radialContextItem))
            return;
        EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
        if (vanillaRadialContextItem.RangedItemAction != entityPlayer.inventory.GetHoldingGun())
            return;
        radialContextItem.RangedItemAction.SwapSelectedAmmo((EntityAlive) entityPlayer, _commandIndex);
    }

    public static void KProHandleCustomRadialCommand(XUiC_Radial _sender, int _commandIndex,		// Add a standard handle radial command
        XUiC_Radial.RadialContextAbs _context)
    {
        KProRadialContextItem customRadialContextItem = _context as KProRadialContextItem;
        EntityPlayerLocal entityPlayer = _sender.xui.playerUI.entityPlayer;
        string[] magazineItemNames = entityPlayer.inventory.GetHoldingGun().MagazineItemNames;
        if (customRadialContextItem == null)
        {
            return;
        }

        if (customRadialContextItem.RangedItemAction == entityPlayer.inventory.GetHoldingGun())
        {
            ItemClass itemClass = ItemClass.GetItemClass(magazineItemNames[_commandIndex], false);
            if (itemClass != null)
            {
                bool result = itemClass.HasTrigger(MinEventTypes.onSelfPrimaryActionEnd);
                var num = itemClass.Effects.EffectGroups.Count;

                if (num == 1)
                {
                    itemClass.FireEvent(MinEventTypes.onSelfPrimaryActionEnd,
                        MinEventParams.CachedEventParam);
                }
                else
                {
                    KHelper.EasyLog(
                        $"Error Effects group has {num} elements but should have 1 element.", LogLevel.File);
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
}