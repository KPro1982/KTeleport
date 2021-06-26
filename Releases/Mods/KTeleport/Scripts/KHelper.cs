using System.Collections.Generic;

namespace KTeleport
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
                GameManager.Instance.ChatMessageServer(null, EChatType.Global,
                    _entityPlayer.entityId, msg, _entityPlayer.EntityName, false, null);
        }

        public static void EasyLog(string msg, LogLevel log)
        {
            if (log == LogLevel.Both || log == LogLevel.Chat)
                if (GameManager.Instance != null)
                    GameManager.Instance.ChatMessageServer(null, EChatType.Global, 0, msg, null,
                        false, null);
            if (log == LogLevel.Both || log == LogLevel.File) LogAnywhere.Log(msg);
        }

        public static void EasyLog(object obj, LogLevel log)
        {
            if (log == LogLevel.File || log == LogLevel.Both) LogAnywhere.Log(obj);
        }

        public static List<KeyValuePair<string, object>> GetXmlPropertyClass(string _itemName,
            string _className)
        {
            var itemClass = ItemClass.GetItemClass(_itemName);
            if (itemClass.Properties.Classes.ContainsKey(_className))
            {
                var dynamicProperties3 = itemClass.Properties.Classes[_className];
                return ParseProperties(dynamicProperties3);
            }

            return new List<KeyValuePair<string, object>>();
        }

        public static string GetXmlProperty(string _itemName, string _propertyName)
        {
            var itemClass = ItemClass.GetItemClass(_itemName);
            var dict = itemClass.Properties.Values.Dict.Dict;
            var result = (string) dict[_propertyName];

            if (result == null) result = "";
            return result;
        }

        private static List<KeyValuePair<string, object>> ParseProperties(
            DynamicProperties dynamicProperties3)
        {
            var keys = new List<KeyValuePair<string, object>>();

            foreach (var keyValuePair in dynamicProperties3.Values.Dict.Dict)
                keys.Add(keyValuePair);

            return keys;
        }


        public static void EasyLog(string msg)
        {
            EasyLog(msg, LogLevel.Both);
        }
    }
}