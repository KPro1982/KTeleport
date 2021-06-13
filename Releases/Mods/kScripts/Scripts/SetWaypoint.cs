using System.Xml;
using kScripts;

public class MinEventActionSetWaypoint : MinEventActionBase
{
    string command;
    EntityPlayer entityPlayer;

    public override void Execute(MinEventParams _params)
    {

       entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();

        if (command == null)
        {
            return;
        }
        else
        {
            if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
            {
               
                KPortalList.Add(new SimplePoint(command, entityPlayer.GetBlockPosition()));
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
        LogLevel log = LogLevel.None;
        KHelper.EasyLog($"xmlAttribute: {xmlAttribute}, name: {_attribute.Name}, value: {_attribute.Value}, _attribute:{_attribute}", log);
        
        if (xmlAttribute || !(_attribute.Name == "command"))
        {
            return xmlAttribute;
        }

        this.command = _attribute.Value;
        return true;
    }
}