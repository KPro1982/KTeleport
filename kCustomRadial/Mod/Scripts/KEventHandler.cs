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
}