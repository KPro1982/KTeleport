using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace KTeleport
{
    public static class KPortalList
    {
        private static readonly string _savepath;
        public static PortalConfigData PortalConfig;
        private static readonly List<Portal> _locations;
        private static bool resetRequested;
        private static int ownerPlayerId;

        static KPortalList()
        {
            PortalConfig = new PortalConfigData();
            _savepath = BuildSavePath();
            _locations = Load();
        }

        public static void Add(Portal _portal)
        {
            if (_portal.IsValid && _locations.Exists(x => x.Name.Equals(_portal.Name)))
            {
                var num = _locations.RemoveAll(x => x.Name.Equals(_portal.Name));
            }

            _portal.Config = PortalConfig;
            _locations.Add(_portal);
            Save();
        }


        public static void Remove(string _portalName)
        {
            if (_locations.Exists(x => x.Name.Equals(_portalName)))
            {
                var num = _locations.RemoveAll(x => x.Name.Equals(_portalName));
            }

            Save();
        }

        public static bool Teleport(EntityPlayer _entityPlayer, string _name)
        {
            Portal portal;

            if (resetRequested)
            {
                Remove(_name);
                resetRequested = false;
                return false;
            }

            if (TryGetLocation(_name, out portal))
            {
                portal.Teleport(_entityPlayer);
                Save();
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
                var myFileStream = new FileStream(_savepath, FileMode.Open);
                var loadedList = new List<Portal>();
                TryDeserializeList(myFileStream, out loadedList);
                myFileStream.Close();
                return loadedList;
            }
            catch
            {
                var myFileStream = new FileStream(_savepath, FileMode.OpenOrCreate);
                myFileStream.Close();
                return new List<Portal>();
            }
        }

        private static bool TryDeserializeList(FileStream myFileStream, out List<Portal> loadedList)
        {
            try
            {
                loadedList =
                    (List<Portal>) new XmlSerializer(typeof(List<Portal>))
                        .Deserialize(myFileStream);
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
            var gameNameArray = GetSavedGameDirectory().Split('/');
            var gameNameStr = (string) gameNameArray.GetValue(gameNameArray.Length - 1);
            return GetSavedGameDirectory() + "/" + gameNameStr + "_kTeleport.xml";
        }

        private static string GetSavedGameDirectory()
        {
            var saveDirectoryStr = GameUtils.GetSaveGameDir();
            return saveDirectoryStr;
        }

        public static void RequestReset()
        {
            resetRequested = true;
        }

        public static void AcceptCrystal()
        {
            foreach (var p in
                _locations) 
            {
                Portal.ChargesUsed = 0;
            }

            Save();
        }
    }
}