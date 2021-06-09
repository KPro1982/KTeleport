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

        private static String BuildConsoleCommand(Vector3i _location, bool _onground)
        {
            
           return  _onground ? $"teleport {_location.x} {_location.z}" : $"teleport {_location.x} {_location.y} {_location.z}";
         

        }
        public static void Teleport(EntityPlayer _entity, Vector3i targetLocation, Vector3i _fuzzy, bool onGround = true)
        {
            
            SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(BuildConsoleCommand(MakeFuzzy(targetLocation, _fuzzy), onGround), null);
           
        }
        public static Vector3i MakeFuzzy(Vector3i _target, Vector3i _fuzzy)
        {
            int _dx, _dz;
            var rand = new Random();
            _dx = rand.Next(-_fuzzy.x, +_fuzzy.x);
            _dz = rand.Next(-_fuzzy.z, +_fuzzy.z);

            return _target + new Vector3i(_dx, 0, _dz);

        }
        public static string GetSavedGameDirectory() 
        {
            string saveDirectoryStr = GameUtils.GetSaveGameDir(null, null);
            return saveDirectoryStr;
        }
    }
}
