using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;
using UnityEngine;
using Random = System.Random;

namespace kScripts
{
    public enum LogLevel
    {
        None,
        File,
        Chat,
        Both
    }

    public enum YRestraint
    {
        OnGround,
        Unrestrained
    }
    public static class KHelper
    {
        public static void ChatOutput(EntityPlayer _entityPlayer, string msg)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChatMessageServer(_cInfo: null, _chatType: EChatType.Global, _senderEntityId: _entityPlayer.entityId, _msg: msg, _mainName: _entityPlayer.EntityName, _localizeMain: false, _recipientEntityIds: null);
            }
        }

        public static void EasyLog(string msg, LogLevel log)
        {


            if (log == LogLevel.Both || log == LogLevel.Chat)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.ChatMessageServer(_cInfo: null, _chatType: EChatType.Global, _senderEntityId: 0, _msg: msg, _mainName: null, _localizeMain: false, _recipientEntityIds: null);
                }
            }  
            if (log == LogLevel.Both || log == LogLevel.File)
            {
                LogAnywhere.Log(msg);
            }
           

        }

        public static void EasyLog(object obj, LogLevel log)
        {
            if (log == LogLevel.File || log == LogLevel.Both)
            {
                LogAnywhere.Log(obj);
            }
        }

        /*public static void SpawnNearbyZombie(EntityPlayer _entityPlayer, String _zombieType,
            int _num = 1, int _dxz = 10)
        {
            int classId = EntityClass.FromString(_zombieType);

            for (int i = 0; i < _num; i++)
            {
                Vector3 spawnLocation =
                    MakeFuzzy(_entityPlayer.GetBlockPosition(), _dxz).ToVector3();
                Entity entity = EntityFactory.CreateEntity(classId, spawnLocation);
                if (entity != null)
                {
                    GameManager.Instance.World.SpawnEntityInWorld(entity);
                }
            }
        }*/
        
       

        
            
         

        
            

    }
}
