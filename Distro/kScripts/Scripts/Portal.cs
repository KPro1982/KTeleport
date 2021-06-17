using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


namespace kScripts
{
    [XmlInclude(typeof(SimplePoint))]
    [XmlInclude(typeof(WayPoint))]
    
    public abstract class Portal
    {
        public string _name;
        public Vector3i _coords;
        public int _used;
        public DateTime _timeLastUsed;
        public DateTime _timeCreated;



        public String Name
        {
            get => _name;
            set => _name = value;
        }

        // public Vector3i Coords
        // {
        //     get => _coords;
        //     set => _coords = value;
        // }

        /*public int Used
        {
            get => _used;
            set => _used = value;
        }

        public DateTime? TimeLastUsed
        {
            get => _timeLastUsed;
            set => _timeLastUsed = (DateTime) value;
        }

        public DateTime TimeCreated
        {
            get => _timeCreated;
            set => _timeCreated = value;
        }*/


        public Portal()
        {
            // Portal._coords = new Vector3i(0, 0, 0);
            this._name = "";
            this._used = 0;
            this._timeCreated = DateTime.Now;
            this._timeLastUsed = DateTime.Now;

        }

        public Portal(string _name, Vector3i _coords)
        {
            // LogAnywhere.Log($"coords from v3i constructor: {_coords.x}, {_coords.y}, {_coords.z}");
            
            this._coords = _coords;
            this._name = _name;
            this._used = 0;
            this._timeCreated = DateTime.Now;
            this._timeLastUsed = DateTime.Now;

        }

        public TimeSpan GetAge()
        {
            return (DateTime.Now - _timeCreated);
        }

        public TimeSpan GetTimeSinceLastUse()
        {
            return (DateTime.Now - _timeLastUsed);
        }


        protected void IncrementUsed()
        {
            _used++;
        }


        protected String BuildConsoleCommand(Vector3i _target, YRestraint _yRestraint = YRestraint.OnGround)
        {
            return _yRestraint == YRestraint.OnGround
                ? $"teleport {_target.x} {_target.z}"
                : $"teleport {_target.x} {_target.y} {_target.z}";
        }

        public virtual void Teleport(EntityPlayer _entityPlayer, YRestraint _yRestraint = YRestraint.OnGround)
        {
            if (CanTeleport(_entityPlayer))
            {
                SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(BuildConsoleCommand(_coords, _yRestraint), null);
                IncrementUsed();
                StoreTimeStamp();
                ImposeConsequences(GameManager.Instance.World.GetPrimaryPlayer());
            }

           
        }

        protected virtual bool CanTeleport(EntityPlayer _entityPlayer)
        {
            var nearbyEnemies = EnemyActivity.GetTargetingEntities(_entityPlayer, new Vector3(50f, 50f, 50f));
            return (nearbyEnemies.Count == 0);

        }

        protected virtual void ImposeConsequences(EntityPlayer entityPlayer)
        {
        }

        protected void StoreTimeStamp()
        {
            this._timeLastUsed = DateTime.Now;
        }
    }


}