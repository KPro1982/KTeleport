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
    private EntityPlayer _entityPlayer;
    private String _command;

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
                if (KPortalList.Teleport(_entityPlayer, "home"))
                {
                    KPortalList.Add(new WayPoint("return", _entityPlayer.GetBlockPosition()));
                }
               
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
        if (xmlAttribute || _attribute.Name != "command")
        {
            return xmlAttribute;
        }

        this._command = _attribute.Value;
        return true;
    }

}
