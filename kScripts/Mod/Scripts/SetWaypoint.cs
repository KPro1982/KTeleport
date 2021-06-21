using System.Xml;
using kScripts;

public class MinEventActionSetWaypoint : MinEventActionBase
{
    private string _command;
    private EntityPlayer _entityPlayer;

    public override void Execute(MinEventParams _params)
    {

       _entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();

        if (_command == null)
        {
            return;
        }
        else
        {
            if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
            {
               
                KPortalList.Add(new WayPoint(_command, _entityPlayer.GetBlockPosition()));
                KHelper.ChatOutput(_entityPlayer, $"{_command} location stored.");
            }
            else
            {
                SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(GameManager.Instance.World.GetPrimaryPlayerId(), _command), false);
            }
        }
    }

    public override bool ParseXmlAttribute(XmlAttribute _attribute)
    {
        bool xmlAttribute = base.ParseXmlAttribute(_attribute);
        LogLevel log = LogLevel.None;
        KHelper.EasyLog($"xmlAttribute: {xmlAttribute}, name: {_attribute.Name}, value: {_attribute.Value}, _attribute:{_attribute}", log);
        
        if (xmlAttribute || !(_attribute.Name == "command"))
        {
            return xmlAttribute;
        }

        this._command = _attribute.Value;
        return true;
    }
}