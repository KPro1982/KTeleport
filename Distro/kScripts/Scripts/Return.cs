using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using kScripts;



class MinEventActionReturn : MinEventActionBase
{
    string command;
    //ClientInfo _cInfo;

    private EntityPlayer entityPlayer;

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
                Vector3i returnV3I = entityPlayer.GetBlockPosition();

                if (KPortalList.Teleport(entityPlayer, "return"))
                {
                    KPortalList.Add(new SimplePoint("return", returnV3I));
                }
                else
                {
                    KHelper.ChatOutput(entityPlayer, "No return location was stored.");
                }
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

        this.command = _attribute.Value;
        return true;
    }
}
