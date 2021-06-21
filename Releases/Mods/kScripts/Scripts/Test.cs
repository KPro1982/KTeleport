using System;
using System.Xml;
using kScripts;
using UnityEngine;
using System.Collections.Generic;


public class MinEventActionTest : MinEventActionBase
{
    private string _command;
    private EntityPlayer _entityPlayer;
    
    public override void Execute(MinEventParams _params)
    {
        _entityPlayer = GameManager.Instance.World.GetPrimaryPlayer();
        List<Entity> nearbyEnemies;
        LogLevel log = LogLevel.Both;

        if (_command == null)
        {
            return;
        }
        else
        {
            if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsClient)
            {
                nearbyEnemies = EnemyActivity.GetSurroundingEntities(_entityPlayer, new Vector3(50f, 50f, 50f));
                KHelper.EasyLog($"Number of Nearby Enemies: {nearbyEnemies.Count}", log);
                nearbyEnemies = EnemyActivity.GetTargetingEntities(_entityPlayer, new Vector3(50f, 50f, 50f));
                KHelper.EasyLog($"Number of Nearby Enemies targeting you: {nearbyEnemies.Count}", log);
                KHelper.EasyLog($"You are Here: {_entityPlayer.GetBlockPosition().x},{_entityPlayer.GetBlockPosition().y},{_entityPlayer.GetBlockPosition().z}.", log);

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

