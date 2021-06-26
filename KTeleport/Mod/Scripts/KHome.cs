using System.Xml;
using KTeleport;

public class MinEventActionKHome : MinEventActionBase
{
    private string _command;
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
                if (KPortalList.Teleport(_entityPlayer, "home"))
                {
                    KPortalList.Add(new SimplePoint("return", returnV3I));
                }
                else
                {
                    KPortalList.Add(new HomePoint("home", _entityPlayer.GetBlockPosition()));
                    KHelper.EasyLog("Stored home.", LogLevel.Chat);
                }
            }
            else
            {
                SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager
                    .GetPackage<NetPackageConsoleCmdServer>()
                    .Setup(GameManager.Instance.World.GetPrimaryPlayerId(), _command));
            }
        }
    }

    public override bool ParseXmlAttribute(XmlAttribute _attribute)
    {
        var xmlAttribute = base.ParseXmlAttribute(_attribute);
        if (xmlAttribute || _attribute.Name != "command") return xmlAttribute;

        _command = _attribute.Value;
        return true;
    }
}