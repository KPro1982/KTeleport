using kCustomRadial.Mod.Scripts;
using KTeleport;

public class KEventHandler
{
    public static void KOnHover(XUiC_Radial __instance, XUiController _sender, OnHoverEventArgs _e)
    {
        var xuiC_RadialEntry = (XUiC_RadialEntry) _sender;
        if (_e.IsOver)
        {
            var _test = KRadial.ResetItemName;
            if (xuiC_RadialEntry.SelectionText == KRadial.ResetItemName)
            {
                __instance.xui.PlayMenuConfirmSound();
                __instance.xui.PlayMenuSliderSound();
                KPortalList.RequestReset();
            }
        }
    }
}