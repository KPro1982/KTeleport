using System.Xml;
using kScripts;

public class MinEventActionSetWaypoint : MinEventActionBase
{
    string command;
    //ClientInfo _cInfo;
    private EntityPlayer entityPlayer;
    public KTeleportObject saveTeleport = new KTeleportObject();

    public override void Execute(MinEventParams _params)
    {



        if (command == null)
        {
            return;
        }
        else
        {
            if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
            {
                entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
                saveTeleport.Add(command, entityPlayer.GetBlockPosition());
                KHelper.ChatOutput(entityPlayer, $"{command} location stored.");
            }
            else
            {
                SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(GameManager.Instance.World.GetPrimaryPlayerId(), command), false);
            }
        }
    }

    public override bool ParseXmlAttribute(XmlAttribute _attribute)
    {
        bool xmlAttribute = base.ParseXmlAttribute(_attribute);
        LogLevel log = LogLevel.File;
        KHelper.EasyLog($"xmlAttribute: {xmlAttribute}, name: {_attribute.Name}, value: {_attribute.Value}, _attribute:{_attribute}", log);
        
        if (xmlAttribute || !(_attribute.Name == "command"))
        {
            return xmlAttribute;
        }

        this.command = _attribute.Value;
        return true;
    }
}