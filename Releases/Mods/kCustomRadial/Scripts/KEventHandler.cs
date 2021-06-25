using System.Collections.Generic;
using kCustomRadial.Mod.Scripts;
using kScripts;

public class KEventHandler
{
    public static void KOnHover(XUiC_Radial __instance, XUiController _sender, OnHoverEventArgs _e)
    {
        XUiC_RadialEntry xuiC_RadialEntry = (XUiC_RadialEntry)_sender;
        if (_e.IsOver )
        {
            string _test = KRadial.ResetItemName;
            if (xuiC_RadialEntry.SelectionText == KRadial.ResetItemName)
            {
                __instance.xui.PlayMenuConfirmSound();
                __instance.xui.PlayMenuSliderSound();
                KPortalList.RequestReset();

            }
        }
    }
}