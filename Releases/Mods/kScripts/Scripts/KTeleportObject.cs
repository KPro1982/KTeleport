using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


namespace kScripts
{
	public class KTeleportObject
	{
		

		private List<teleportData> _locations = new List<teleportData>();
		readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<teleportData>));
		private static string _savedGameDirectory = "";

		public KTeleportObject()
		{
			LogLevel log = LogLevel.None;
			_savedGameDirectory = KHelper.GetSavedGameDirectory();
			KHelper.EasyLog($"Creating kTeleportObject. SavedDirectory: {_savedGameDirectory}", log);
		}
		public void Add(string _name, Vector3i _loc)
		{
			LogLevel log = LogLevel.None;
			teleportData data = new teleportData(_name, _loc);
			Read();
			if (_locations.Exists(x => x.name.Equals(_name)))
            {
				int num = _locations.RemoveAll(x => x.name.Equals(_name));
				KHelper.EasyLog($"Removed {num} entries with name: {_name}",log);
			}
			_locations.Add(data);
			
			KHelper.EasyLog($"Added location name: {data.name} at {data.coords.ToStringNoBlanks()}", log);
			KHelper.EasyLog(_locations,log);

			Write();
		}
		private void Write()
		{
			LogLevel log = LogLevel.None;
			string savePath = BuildSavePath();
			KHelper.EasyLog("Write()", log);
			TextWriter writer = new StreamWriter(savePath, false);
			_serializer.Serialize(writer, _locations);
			writer.Close();
		}
		private void Read()
		{
			LogLevel log = LogLevel.None;
			string savePath = BuildSavePath();
			FileStream myFileStream = new FileStream(savePath, FileMode.OpenOrCreate);
            try
            {
				List<teleportData> myObject = (List<teleportData>)_serializer.Deserialize(myFileStream);
                _locations = myObject;
                KHelper.EasyLog($"Count: {myObject.Count}", log);
                KHelper.EasyLog(myObject, log);

			} catch
            {
	            KHelper.EasyLog("As no data file exists, creating data file now.", LogLevel.File);
            }
			myFileStream.Close();




		}
		public bool TryGetLocation(string _name, out Vector3i _coords)
        {
	        LogLevel log = LogLevel.Both;
			Read();
			try
            {
				teleportData home = _locations.Find(x => x.name.Equals(_name));
				KHelper.EasyLog($"Found location name: {home.name} at {home.coords.ToString()}.", log);
				KHelper.EasyLog(home, log);
				_coords = home.coords;
				return true;
			} catch
            {
	            KHelper.EasyLog($"Error: location name: {_name} was not found!",log);
				_coords = new Vector3i(0,0,0);
				return false;
            }
        }
		private string BuildSavePath()
        {
	        LogLevel log = LogLevel.None;
	        KHelper.EasyLog("BuildSavePath()", log);
			string[] gameNameArray = _savedGameDirectory.Split('/');
			string gameNameStr = (string)gameNameArray.GetValue(gameNameArray.Length-1);
			string savePath = _savedGameDirectory + "/" + gameNameStr + "_kTeleport.xml";
			KHelper.EasyLog($"Save Path: {savePath}", log);
			return savePath;
		}
	}
}