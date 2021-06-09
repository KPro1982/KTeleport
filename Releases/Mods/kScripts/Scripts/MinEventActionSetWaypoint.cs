using System.Xml;
using kScripts;

public class MinEventActionSetWaypoint : MinEventActionBase
{
    string command;
    //ClientInfo _cInfo;
    private EntityPlayer _entityPlayer;
    public KTeleportObject SaveTeleport = new KTeleportObject();

    public override void Execute(MinEventParams _params)
    {
        LogLevel log = LogLevel.Both;
        
        if (command == null)
        {
            return;
        }
        else
        {
            KHelper.EasyLog($"SetWayPoint.Execute() -> Command: {command}, _params: {_params}", log);
            if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
            {
                _entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
                SaveTeleport.Add("waypoint", _entityPlayer.GetBlockPosition());
                KHelper.ChatOutput(_entityPlayer, "Waypoint location stored.");
            }
            else
            {
                SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(GameManager.Instance.World.GetPrimaryPlayerId(), command), false);
            }
        }
    }

    public override bool ParseXmlAttribute(XmlAttribute _attribute)
    {
        LogLevel log = LogLevel.Both;
        bool xmlAttribute = base.ParseXmlAttribute(_attribute);
        if (xmlAttribute || _attribute.Name != "command")
        {
            KHelper.EasyLog($"SetWayPoint.ParseXmlAttribute() -> xmlAttribute: {xmlAttribute}, _attribute: {_attribute}", log);
            return xmlAttribute;
        }

        this.command = _attribute.Value;
        return true;
    }
}

