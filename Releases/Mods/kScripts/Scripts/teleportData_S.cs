using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace kScripts
{
    
    public class teleportData
    {
        public string name;
        public Vector3i coords;
        
       
        public teleportData()
        {
            coords = new Vector3i(0, 0, 0);
            name = "";
        }
        public teleportData(string _name, string _coords)
        {
            coords = Vector3i.Parse(_coords);
            name = _name;
        }
        public teleportData(string _name, Vector3i _coords)
        {
            LogAnywhere.Log(string.Format("coords from v3i constructor: {0}, {1}, {2}", _coords.x, _coords.y, _coords.z));
            coords = _coords;
            name = _name;
        }

    }

}
