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

        public static List<KeyValuePair<String, object>> GetXmlPropertyClass(String _itemName, String _className)
        {
            ItemClass itemClass = ItemClass.GetItemClass(_itemName, false);
            if (itemClass.Properties.Classes.ContainsKey(_className))
            {
                var dynamicProperties3 = itemClass.Properties.Classes[_className];
                return ParseProperties(dynamicProperties3);
            }

            return new List<KeyValuePair<string, object>>();

        }

        public static String GetXmlProperty(String _itemName, String _propertyName)
        {
            ItemClass itemClass = ItemClass.GetItemClass(_itemName, false);
            Dictionary<string, object> dict = itemClass.Properties.Values.Dict.Dict;
            String _result = (String) dict[_propertyName];


            return  (string) _result;


        }

        private static List<KeyValuePair<String, object>> ParseProperties(DynamicProperties dynamicProperties3)
        {
            List<KeyValuePair<String, object>> keys = new List<KeyValuePair<String, object>>();

            foreach (KeyValuePair<String, object> keyValuePair in dynamicProperties3.Values.Dict
                .Dict)
            {
             
                keys.Add(keyValuePair);
            }
                
            return keys;
        }

        
       

        
            
         

        
            

    }
}
