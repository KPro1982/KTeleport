using System.Xml;
using KTeleport;

public class MinEventActionKTest : MinEventActionBase
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
                SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync("killall", null);
                SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync("settime day", null);
                KHelper.EasyLog("All cleaned up!", LogLevel.Chat);
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
        if (xmlAttribute || !(_attribute.Name == "command")) return xmlAttribute;

        _command = _attribute.Value;
        return true;
    }
}