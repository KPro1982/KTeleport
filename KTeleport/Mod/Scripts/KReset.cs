using System.Xml;
using KTeleport;

internal class MinEventActionKReset : MinEventActionBase
{
    private string _command;


    private EntityPlayer _entityPlayer;

    public override void Execute(MinEventParams _params)
    {
        _entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();

        if (_command == null)
        {
        }
        else
        {
            if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
            {
                KPortalList.RequestReset();
                KHelper.EasyLog("Reset Requested.", LogLevel.Chat);
            }
        }
    }

    public override bool ParseXmlAttribute(XmlAttribute _attribute)
    {
        var xmlAttribute = base.ParseXmlAttribute(_attribute);
        if (xmlAttribute || !(_attribute.Name == "command")) return xmlAttribute;

        _command = _attribute.Value;
        return true;
    }
}