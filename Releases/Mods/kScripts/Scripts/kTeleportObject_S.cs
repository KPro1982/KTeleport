using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


namespace kScripts
{
	public class kTeleportObject
	{
		// MAKE THIS STATIC AND MAKE SURE TO READ() FROM DISK BEFORE ADD


		private List<teleportData> Locations = new List<teleportData>();
		XmlSerializer serializer = new XmlSerializer(typeof(List<teleportData>));
		private static string savedGameDirectoryStr = "";

		public kTeleportObject()
		{
			savedGameDirectoryStr = kHelper.GetSavedGameDirectory();
			LogAnywhere.Log(string.Format("Creating kTeleportObject. SavedDirectory: {0}", savedGameDirectoryStr),true);
		}
		public void Add(string _name, Vector3i _loc)
		{
			teleportData _data = new teleportData(_name, _loc);
			if (Locations.Exists(x => x.name.Equals(_name)))
            {
				int num = Locations.RemoveAll(x => x.name.Equals(_name));
				LogAnywhere.Log(string.Format("Removed {0} entries with name: {1}", num, _name),true);
			}
			Locations.Add(_data);
			
			LogAnywhere.Log(string.Format("Added location name: {0} at {1}", _data.name, _data.coords.ToStringNoBlanks()), true);
			LogAnywhere.Log(Locations);

			Write();
		}
		private void Write()
		{
			string savePath = BuildSavePath();
			LogAnywhere.Log("Write()", true);
			TextWriter writer = new StreamWriter(savePath, false);
			serializer.Serialize(writer, Locations);
			writer.Close();
		}
		private void Read()
		{
			string savePath = BuildSavePath();
			FileStream myFileStream = new FileStream(savePath, FileMode.OpenOrCreate);
            try
            {
				List<teleportData> myObject = (List<teleportData>)serializer.Deserialize(myFileStream);
                Locations = myObject;
				LogAnywhere.Log(string.Format("Count: {0}", myObject.Count));
				LogAnywhere.Log(myObject);

			} catch
            {
				LogAnywhere.Log("As no data file exists, creating data file now.");
            }
			myFileStream.Close();




		}
		public bool TryGetLocation(string _name, out Vector3i _coords)
        {
			Read();
			try
            {
				teleportData home = Locations.Find(x => x.name.Equals(_name));
				LogAnywhere.Log(string.Format("Found location name: {0} at {1}.", home.name, home.coords.ToString()));
				LogAnywhere.Log(home);
				_coords = home.coords;
				return true;
			} catch
            {
				LogAnywhere.Log(string.Format("Error: location name: {0} was not found!", _name),true);
				_coords = new Vector3i(0,0,0);
				return false;
            }
        }
		private string BuildSavePath()
        {
			LogAnywhere.Log("BuildSavePath()", true);
			string[] gameNameArray = savedGameDirectoryStr.Split('/');
			string gameNameStr = (string)gameNameArray.GetValue(gameNameArray.Length-1);
			LogAnywhere.Log(string.Format("Save Game Name: {0}", gameNameStr));
			string savePath = savedGameDirectoryStr + "/" + gameNameStr + "_kTeleport.xml";
			LogAnywhere.Log(string.Format("Save Path: {0}", savePath));
			return savePath;
		}
	}
}