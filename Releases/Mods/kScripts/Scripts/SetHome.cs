using System.Xml;
using kScripts;




public class MinEventActionSetHome : MinEventActionBase
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
                Vector3i targetVector = _entityPlayer.GetBlockPosition();
                KPortalList.Add(new SimplePoint("home", targetVector));
                KHelper.ChatOutput(_entityPlayer, "Home location stored.");
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
        if (xmlAttribute || !(_attribute.Name == "command"))
        {
            return xmlAttribute;
        }

        this._command = _attribute.Value;
        return true;
    }
}

