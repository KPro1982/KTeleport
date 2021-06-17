using System.Xml;
using kScripts;




public class MinEventActionSetHome : MinEventActionBase
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
                Vector3i targetVector = entityPlayer.GetBlockPosition();
                KPortalList.Add(new SimplePoint("home", targetVector));
                KHelper.ChatOutput(entityPlayer, "Home location stored.");
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
        if (xmlAttribute || !(_attribute.Name == "command"))
        {
            return xmlAttribute;
        }

        this.command = _attribute.Value;
        return true;
    }
}

