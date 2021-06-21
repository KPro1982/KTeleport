using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;


namespace kScripts
{
    public static class KPortalList
    {
        private static String _savepath;
        public static TeleportConfigData teleportListConfig;
        private static List<Portal> _locations;

        static KPortalList()
        {
            teleportListConfig = new TeleportConfigData();
            _savepath = BuildSavePath();
            _locations = Load();
        }

        public static void Add(Portal _portal)
        {
            
            if (_locations.Exists(x => x.Name.Equals(_portal.Name)))
            {
                int num = _locations.RemoveAll(x => x.Name.Equals(_portal.Name));
            }

            _portal.Config = teleportListConfig;
            _locations.Add(_portal);
            Save();
        }

        public static bool Teleport(EntityPlayer _entityPlayer, String _name)
        {
            Portal portal;
            if (TryGetLocation(_name, out portal))
            {
                portal.Teleport(_entityPlayer);
                return true;
            }

            return false;
        }
        private static bool TryGetLocation(string _name, out Portal _portal)
        {
            _portal = _locations.Find(x => x.Name.Equals(_name));
            return _portal != null;
        }

        private static void Save()
        {
            TextWriter writer = new StreamWriter(_savepath, false);
            new XmlSerializer(typeof(List<Portal>)).Serialize(writer, _locations);
            writer.Close();
        }

        private static List<Portal> Load()
        {
            try
            {
                FileStream myFileStream = new FileStream(_savepath, FileMode.Open);
                List<Portal> loadedList = new List<Portal>();
                TryDeserializeList(myFileStream, out loadedList);
                myFileStream.Close();
                return loadedList;
            }
            catch
            {
                FileStream myFileStream = new FileStream(_savepath, FileMode.OpenOrCreate);
                myFileStream.Close();
                return new List<Portal>();
            }
        }

        private static bool TryDeserializeList(FileStream myFileStream, out List<Portal> loadedList)
        {
            try
            {
                loadedList =
                    (List<Portal>) new XmlSerializer(typeof(List<Portal>)).Deserialize(myFileStream);
                return true;
            }
            catch 
            {
                KHelper.EasyLog("Deserialize Error.", LogLevel.Both);
                loadedList = new List<Portal>();
                return false;
            }

        }

        private static string BuildSavePath()
        {
            string[] gameNameArray = GetSavedGameDirectory().Split('/');
            string gameNameStr = (string) gameNameArray.GetValue(gameNameArray.Length - 1);
            return GetSavedGameDirectory() + "/" + gameNameStr + "_kTeleport.xml"; 
            
        }

        private static string GetSavedGameDirectory()
        {
            string saveDirectoryStr = GameUtils.GetSaveGameDir(null, null);
            return saveDirectoryStr;
        }
    }
}