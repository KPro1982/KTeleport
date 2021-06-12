﻿using System;
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

    private LogLevel log = LogLevel.Both;
    //ClientInfo _cInfo;
    private EntityPlayer _entityPlayer;

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
                _entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();


                Vector3i returnV3i = _entityPlayer.GetBlockPosition();

                var nearbyEnemies = EnemyActivity.GetTargetingEntities(_entityPlayer, new Vector3(50f, 50f, 50f));
                if(nearbyEnemies.Count == 0)
                {
                    if (KPortalList.TryGetLocation(command, out var target))
                    {
                        KPortalList.Add("return", returnV3i);
                        target.Teleport(_entityPlayer);
                    }
                    else
                    {
                        KHelper.ChatOutput(_entityPlayer, "You cannot go home as there is no home location stored.");
                    }
                } else
                {
                    KHelper.EasyLog($"You cannot go home because you are {nearbyEnemies.Count} Zombies targeting you!", log);
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
