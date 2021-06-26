using GUI_2;
using KTeleport;

namespace kCustomRadial.Mod.Scripts
{
    public class KRadial
    {
        public static string ResetItemName { get; private set; }

        public static void KSetupRadial(XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
        {
            var log = LogLevel.None;

            _xuiRadialWindow.ResetRadialEntries();
            var magazineItemNames = _epl.inventory.GetHoldingGun().MagazineItemNames;
            var radialItemNames = GetRadialItems();
            var preSelectedCommandIndex = -1;

            KHelper.EasyLog($"Before for loop: {radialItemNames.Length} radial items found", log);

            if (radialItemNames.Length > 0 &&
                _epl.inventory.GetHoldingGun().item.Name == "KTeleport")
            {
                for (var i = 0; i < radialItemNames.Length; i++)
                {
                    var itemClass = ItemClass.GetItemClass(radialItemNames[i]);
                    if (itemClass != null)
                        _xuiRadialWindow.CreateRadialEntry(i, itemClass.GetIconName(),
                            "ItemIconAtlas", " ", itemClass.GetLocalizedItemName());
                }


                _xuiRadialWindow.SetCommonData(UIUtils.ButtonIcon.FaceButtonEast,
                    KProHandleCustomRadialCommand,
                    new KProRadialContextItem((ItemActionRanged) _epl.inventory.GetHoldingGun()),
                    preSelectedCommandIndex);
            }
            else
            {
                for (var i = 0; i < magazineItemNames.Length; i++)
                {
                    var itemClass = ItemClass.GetItemClass(magazineItemNames[i]);
                    if (itemClass != null && (!_epl.IsInWater() || itemClass.UsableUnderwater))
                    {
                        var itemCount =
                            _xuiRadialWindow.xui.PlayerInventory.GetItemCount(itemClass.Id);
                        var flag = _epl.inventory.holdingItemItemValue.SelectedAmmoTypeIndex == i;
                        _xuiRadialWindow.CreateRadialEntry(i, itemClass.GetIconName(),
                            itemCount > 0 ? "ItemIconAtlas" : "ItemIconAtlasGreyscale",
                            itemCount.ToString(), itemClass.GetLocalizedItemName(), flag);


                        if (flag)
                        {
                            KHelper.EasyLog($"Inside if(flag) i = {i}", log);
                            preSelectedCommandIndex = i;
                        }
                    }
                }

                _xuiRadialWindow.SetCommonData(UIUtils.ButtonIcon.FaceButtonEast,
                    KProHandleVanillaRadialCommand,
                    new KProRadialContextItem((ItemActionRanged) _epl.inventory.GetHoldingGun()),
                    preSelectedCommandIndex);
            }
        }

        private static string[] GetRadialItems()
        {
            var strRadialItems = KHelper.GetXmlProperty("KTeleport", "RadialItems");

            if (KHelper.GetXmlProperty("KTeleport", "ResetItem") != "")
            {
                var _name = KHelper.GetXmlProperty("KTeleport", "ResetItem");
                var itemClass = ItemClass.GetItemClass(_name);
                ResetItemName = itemClass.GetLocalizedItemName();
            }

            if (strRadialItems.Length > 0) return strRadialItems.Split(',');


            return new[] {""};
        }

        public static void KProHandleVanillaRadialCommand(XUiC_Radial _sender, int _commandIndex,
            XUiC_Radial.RadialContextAbs _context)
        {
            var vanillaRadialContextItem = _context as KProRadialContextItem;
            if (!(_context is KProRadialContextItem radialContextItem))
                return;
            var entityPlayer = _sender.xui.playerUI.entityPlayer;
            if (vanillaRadialContextItem.RangedItemAction != entityPlayer.inventory.GetHoldingGun())
                return;
            radialContextItem.RangedItemAction.SwapSelectedAmmo(entityPlayer, _commandIndex);
        }

        public static void KProHandleCustomRadialCommand(XUiC_Radial _sender,
            int _commandIndex, // Add a standard handle radial command
            XUiC_Radial.RadialContextAbs _context)
        {
            var customRadialContextItem = _context as KProRadialContextItem;

            var entityPlayer = _sender.xui.playerUI.entityPlayer;
            // string[] magazineItemNames = entityPlayer.inventory.GetHoldingGun().MagazineItemNames;
            var radialItemNames = GetRadialItems();

            if (customRadialContextItem == null) return;

            if (customRadialContextItem.RangedItemAction == entityPlayer.inventory.GetHoldingGun())
            {
                var itemClass = ItemClass.GetItemClass(radialItemNames[_commandIndex]);
                if (itemClass != null)
                {
                    var result = itemClass.HasTrigger(MinEventTypes.onSelfPrimaryActionEnd);
                    var num = itemClass.Effects.EffectGroups.Count;

                    if (num == 1)
                        itemClass.FireEvent(MinEventTypes.onSelfPrimaryActionEnd,
                            MinEventParams.CachedEventParam);
                    else
                        KHelper.EasyLog(
                            $"Error Effects group has {num} elements but should have 1 element.",
                            LogLevel.File);
                }
            }
        }


        public class KProRadialContextItem : XUiC_Radial.RadialContextAbs
        {
            // Token: 0x04001CD6 RID: 7382
            public readonly ItemActionRanged RangedItemAction;
            private readonly LogLevel _log = LogLevel.None;

            // Token: 0x060023E5 RID: 9189 RVA: 0x000E53BB File Offset: 0x000E35BB
            public KProRadialContextItem(ItemActionRanged _rangedItemAction)
            {
                KHelper.EasyLog("KPro_RadicalContextItem Constructor: _rangedItemAction:", _log);
                KHelper.EasyLog(_rangedItemAction, _log);
                RangedItemAction = _rangedItemAction;
            }
        }
    }
}