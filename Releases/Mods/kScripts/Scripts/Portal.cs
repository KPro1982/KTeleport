using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace kScripts
{
    
    public class Portal
    {
        public string Name { get; set; }
        public Vector3i Coords;
        public int Used { get; set; }


        public Portal()
        {
            Coords = new Vector3i(0, 0, 0);
            Name = "";
            Used = 0;
        }
        public Portal(string _name, string _coords)
        {
            this.Coords = Vector3i.Parse(_coords);
            this.Name = _name;
            Used = 0;
        }

        public Portal(string _name, Vector3i _coords)
        {
            // LogAnywhere.Log($"coords from v3i constructor: {_coords.x}, {_coords.y}, {_coords.z}");
            this.Coords = _coords;
            this.Name = _name;
            this.Used = 0;
        }

        void IncrementUsed()
        {
            Used++;
        }

            String BuildConsoleCommand(YRestraint _yRestraint = YRestraint.OnGround)
        {
            return  _yRestraint == YRestraint.OnGround ? $"teleport {Coords.x} {Coords.z}" : $"teleport {Coords.x} {Coords.y} {Coords.z}";
        }

          public  void Teleport(EntityPlayer _entity, YRestraint _yRestraint = YRestraint.OnGround)
        {
            SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(BuildConsoleCommand(_yRestraint), null);
            IncrementUsed();
            // add save timestamp for lasttimeused
        }
    }

}
