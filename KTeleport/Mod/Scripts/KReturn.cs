using System.Xml;
using KTeleport;

internal class MinEventActionKReturn : MinEventActionBase
{
    private string _command;
    //ClientInfo _cInfo;

    private EntityPlayer _entityPlayer;

    public override void Execute(MinEventParams _params)
    {
        _entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
        var returnV3I = _entityPlayer.GetBlockPosition();

        if (_command == null)
        {
        }
        else
        {
            if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
            {
                KPortalList.Teleport(_entityPlayer, "return");
                KPortalList.Add(new SimplePoint("return", returnV3I));
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