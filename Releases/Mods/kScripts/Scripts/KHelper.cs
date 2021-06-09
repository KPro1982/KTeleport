using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        public static void Teleport(Vector3i targetLocation, bool onGround = true)
        {
            SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(BuildConsoleCommand(targetLocation, onGround), null);
        }
        public static string GetSavedGameDirectory() 
        {
            string saveDirectoryStr = GameUtils.GetSaveGameDir(null, null);
            return saveDirectoryStr;
        }
    }
}
