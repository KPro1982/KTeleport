using System.Xml;
using kScripts;
using UnityEngine;
using System.Collections.Generic;


public class MinEventActionTest : MinEventActionBase
{
    string command;
    //ClientInfo _cInfo;
    private EntityPlayer entityPlayer;


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
                kHelper.EasyLog("Testing...");
                nearbyEnemies = EnemyActivity.GetSurroundingEntities(entityPlayer, new Vector3(50f, 50f, 50f));
                kHelper.EasyLog($"Number of Nearby Enemies: {nearbyEnemies.Count}");
                nearbyEnemies = EnemyActivity.GetTargetingEntities(entityPlayer, new Vector3(50f, 50f, 50f));
                kHelper.EasyLog($"Number of Nearby Enemies targing you: {nearbyEnemies.Count}");
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

