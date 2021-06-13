using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using kScripts;
using UnityEngine;


public class MinEventActionGoWaypoint : MinEventActionBase
{

    string command;
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

                if (KPortalList.Teleport(entityPlayer, command))
                {
                    Debug.Log("This is print in the player.log file");
                    KPortalList.Add(new SimplePoint("return", entityPlayer.GetBlockPosition()));
                }
                else
                {
                    KHelper.ChatOutput(entityPlayer, "You cannot go home as there is no home location stored.");
                }


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
        if (xmlAttribute || _attribute.Name != "command")
        {
            return xmlAttribute;
        }

        this.command = _attribute.Value;
        return true;
    }

}
