using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


namespace kScripts
{
	public class KTeleportObject
	{
		

		private List<Portal> _locations = new List<Portal>();
		readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Portal>));
		private static string _savedGameDirectory = "";
		
		public KTeleportObject()
		{
			_savedGameDirectory = KHelper.GetSavedGameDirectory();
		}
		public void Add(string _name, Vector3i _loc)
		{
			Portal data = new Portal(_name, _loc);
			
			Read();
			
			// if (_locations.Exists(x => x.Name.Equals(_name)))
   //          {
			// 	int num = _locations.RemoveAll(x => x.Name.Equals(_name));
			// }
			_locations.Add(data);

			Write();
		}
		private void Write()
		{
			TextWriter writer = new StreamWriter(BuildSavePath(), false);
			_serializer.Serialize(writer, _locations);
			writer.Close();
		}
		private void Read()
		{
			FileStream myFileStream = new FileStream(BuildSavePath(), FileMode.OpenOrCreate);
			if (!TryDeserialize(myFileStream))
			{
				KHelper.EasyLog("Deserialize failed.", LogLevel.File);	
			}
			myFileStream.Close();
		}

		private bool TryDeserialize(FileStream myFileStream)
		{
			List<Portal> myObject = null;
			try
			{
				myObject = (List<Portal>) _serializer.Deserialize(myFileStream);
				_locations = myObject;
				return true;
			}
			catch
			{
				KHelper.EasyLog("As no data file exists, creating data file now.", LogLevel.File);
				return false;
			}

		}

		public bool TryGetLocation(string _name, out Portal _portal)
		{
			Read();
			_portal = _locations.Find(x => x.Name.Equals(_name));
			return _portal != null;

		}

		private string BuildSavePath()
        {
	        string[] gameNameArray = _savedGameDirectory.Split('/');
			string gameNameStr = (string)gameNameArray.GetValue(gameNameArray.Length-1);
			return  _savedGameDirectory + "/" + gameNameStr + "_kTeleport.xml";
		}
	}
}