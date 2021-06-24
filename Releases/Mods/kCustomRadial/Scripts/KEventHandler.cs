using System.Collections.Generic;
using kCustomRadial.Mod.Scripts;
using kScripts;

public class KEventHandler
{
    public static void AssignHandler(ref List<XUiC_RadialEntry> ___menuItem)
    {
        for (int j = 0; j < ___menuItem.Count; j++)
        {
            ___menuItem[j].OnDoubleClick += KOnDoubleClicked;
            ___menuItem[j].OnPress += KOnDoubleClicked;
            ___menuItem[j].OnRightPress += KOnDoubleClicked;
            KHelper.EasyLog($"Adding {___menuItem[j].SelectionText} to ___menuItem.");
        }
        KHelper.EasyLog("After for loop in AssignHandler", LogLevel.Both);
    }
    
    public static void KOnDoubleClicked(XUiController _sender, OnPressEventArgs _e)
    {
        KHelper.EasyLog("Double Clicked", LogLevel.Both);
    }
    public static void KDoubleClicked()
    {
        KHelper.EasyLog("Inside KDouble Clicked", LogLevel.Both);
    }

    public static void KOnPress(XUiC_Radial __instance, XUiC_RadialEntry _sender)
    {
        KHelper.EasyLog("Inside KOnPress");
    }

    public static void KOnHover(XUiC_Radial __instance, XUiController _sender, OnHoverEventArgs _e)
    {
        XUiC_RadialEntry xuiC_RadialEntry = (XUiC_RadialEntry)_sender;
        if (_e.IsOver )
        {
            if (xuiC_RadialEntry.SelectionText == "yIvuv - Reset")
            {
                __instance.xui.PlayMenuConfirmSound();
                __instance.xui.PlayMenuSliderSound();
                KPortalList.RequestReset();
                KHelper.EasyLog("Reset Requested.", LogLevel.Chat);
            }
        }
    }
}