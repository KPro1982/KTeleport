using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


namespace kScripts
{
    public class Portal
    {

        private string _name;
        private Vector3i _coords;
        private int _used;
        private DateTime? _timeLastUsed;
        private DateTime _timeCreated;



        public String Name
        {
            get => _name;
            set => _name = value;
        }

        public Vector3i Coords
        {
            get => _coords;
            set => _coords = value;
        }

        public int Used
        {
            get => _used;
            set => _used = value;
        }

        public DateTime? TimeLastUsed
        {
            get => _timeLastUsed;
            set => _timeLastUsed = value;
        }

        public DateTime TimeCreated
        {
            get => _timeCreated;
            set => _timeCreated = value;
        }


        public Portal()
        {
            this._coords = new Vector3i(0, 0, 0);
            this._name = "";
            this._used = 0;
            this._timeCreated = DateTime.Now;
            this._timeLastUsed = null;

        }

        public Portal(string _name, Vector3i _coords)
        {
            LogAnywhere.Log($"coords from v3i constructor: {_coords.x}, {_coords.y}, {_coords.z}");
            this._coords = _coords;
            this._name = _name;
            this._used = 0;
            this._timeCreated = DateTime.Now;
            this._timeLastUsed = null;

        }


        void IncrementUsed()
        {
            Used++;
        }


        String BuildConsoleCommand(YRestraint _yRestraint = YRestraint.OnGround)
        {
            return _yRestraint == YRestraint.OnGround
                ? $"teleport {Coords.x} {Coords.z}"
                : $"teleport {Coords.x} {Coords.y} {Coords.z}";
        }

        public void Teleport(EntityPlayer _entity, YRestraint _yRestraint = YRestraint.OnGround)
        {
            SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(BuildConsoleCommand(_yRestraint), null);
            IncrementUsed();
            StoreTimeStamp();
        }

        private void StoreTimeStamp()
        {
            this._timeLastUsed = DateTime.Now;
        }
    }

}