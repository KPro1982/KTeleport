using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;


namespace kScripts
{
    public static class KPortalList
    {
        private static String _savepath = BuildSavePath();
        private static string _savedGameDirectory = "";
        private static List<Portal> _locations = Load();


        public static void Add(string _name, Vector3i _loc)
        {
            Portal _portal = new Portal(_name, _loc);
            Add(_portal);
        }

        public static void Add(Portal _portal)
        {
            if (_locations.Exists(x => x.Name.Equals(_portal.Name)))
            {
                int num = _locations.RemoveAll(x => x.Name.Equals(_portal.Name));
            }

            _locations.Add(_portal);
            Save();
        }

        public static bool TryGetLocation(string _name, out Portal _portal)
        {
            _portal = _locations.Find(x => x.Name.Equals(_name));
            return _portal != null;
        }

        public static void Save()
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
                List<Portal> loadedList = (List<Portal>) new XmlSerializer(typeof(List<Portal>)).Deserialize(
                    myFileStream);
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

        private static string BuildSavePath()
        {
            /* string[] gameNameArray = GetSavedGameDirectory().Split('/');
            string gameNameStr = (string) gameNameArray.GetValue(gameNameArray.Length - 1);
            return GetSavedGameDirectory() + "/" + gameNameStr + "_kTeleport.xml"; */

            return "D:\\Desktop\\7D2D_LOGS\\SaveTest.xml";
        }

        private static string GetSavedGameDirectory()
        {
            string saveDirectoryStr = GameUtils.GetSaveGameDir(null, null);
            return saveDirectoryStr;
        }
    }
}