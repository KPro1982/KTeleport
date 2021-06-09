using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using kScripts;
using UnityEngine;


public class MinEventActionGoHome : MinEventActionBase
{

    string command;
    //ClientInfo _cInfo;
    private EntityPlayer entityPlayer;
    public kTeleportObject saveTeleport = new kTeleportObject();

    public override void Execute(MinEventParams _params)
    {
        List<Entity> nearbyEnemies;

        if (command == null)
        {
            return;
        }
        else
        {
            if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
            {
                entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
                kTeleportObject teleportObject = new kTeleportObject();


                Vector3i returnV3i = entityPlayer.GetBlockPosition();

                nearbyEnemies = EnemyActivity.GetTargetingEntities(entityPlayer, new Vector3(50f, 50f, 50f));
                if(nearbyEnemies.Count == 0)
                {
                    if (teleportObject.TryGetLocation("home", out var targetV3i))
                    {
                        teleportObject.Add("return", returnV3i);
                        kHelper.Teleport(targetV3i);
                    }
                    else
                    {
                        kHelper.ChatOutput(entityPlayer, "You cannot go home as there is no home location stored.");
                    }
                } else
                {
                    kHelper.EasyLog($"You cannot go home because you are {nearbyEnemies.Count} Zombies targeting you!");
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
        if (xmlAttribute || !(_attribute.Name == "command"))
        {
            return xmlAttribute;
        }

        this.command = _attribute.Value;
        return true;
    }

}
