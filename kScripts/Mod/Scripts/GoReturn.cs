using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using kScripts;


internal class MinEventActionReturn : MinEventActionBase
{
    private string _command;
    //ClientInfo _cInfo;

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
                Vector3i returnV3I = _entityPlayer.GetBlockPosition();

                KPortalList.Teleport(_entityPlayer, "return");
                KPortalList.Add(new SimplePoint("return", returnV3I));
                
            }
            else
            {
                // SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageConsoleCmdServer>().Setup(GameManager.Instance.World.GetPrimaryPlayerId(), command), false);
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
