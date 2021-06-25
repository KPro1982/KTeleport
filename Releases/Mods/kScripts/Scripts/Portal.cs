using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;


namespace kScripts
{
    [XmlInclude(typeof(SimplePoint))]
    [XmlInclude(typeof(WayPoint))]
    [XmlInclude(typeof(HomePoint))]
    public abstract class Portal
    {
        protected object teleportObject;
        protected string _name;
        protected Vector3i _coords;
        public bool IsValid { get; set; } = true;
        public static int ChargesUsed { get; set; }
        protected int Used { get; }
        public DateTime _timeLastUsed;
        public DateTime _timeCreated;
        public TeleportConfigData Config { get; set; }


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
        

        public DateTime TimeLastUsed
        {
            get => _timeLastUsed;
            set => _timeLastUsed = (DateTime) value;
        }

        public DateTime TimeCreated
        {
            get => _timeCreated;
            set => _timeCreated = value;
        }


        public Portal()
        {
            // Portal._coords = new Vector3i(0, 0, 0);
            this._name = "";
            this.TimeCreated = DateTime.Now;
            this.TimeLastUsed = DateTime.Now;
            this.teleportObject = null;

        }

        public Portal(string _name, Vector3i _coords)
        {
            // LogAnywhere.Log($"coords from v3i constructor: {_coords.x}, {_coords.y}, {_coords.z}");
            
            this.Coords = _coords;
            this._name = _name;
            this.TimeCreated = DateTime.Now;
            this.TimeLastUsed = DateTime.Now;

        }
        
        
        public TimeSpan GetAge()
        {
            return (DateTime.Now - TimeCreated);
        }

        public TimeSpan GetTimeSinceLastUse()
        {
            TimeSpan tSpan = DateTime.Now - TimeLastUsed;
            return tSpan;
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
                SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(BuildConsoleCommand(Coords, _yRestraint), null);
                StoreTimeStamp();
                ImposeConsequences(GameManager.Instance.World.GetPrimaryPlayer());
            }

           
        }

        protected virtual bool CanTeleport(EntityPlayer _entityPlayer)
        {

            if (!Config.CanTeleportNearEnemies)
            {
                Vector3 scanRange = new Vector3( Config.EnemyScanningRange,
                    Config.EnemyScanningRange,  Config.EnemyScanningRange);
                var nearbyEnemies = EnemyActivity.GetTargetingEntities(_entityPlayer, scanRange);

                if (nearbyEnemies.Count == 0)
                {
                    return true;
                } else
                {
                    KHelper.EasyLog($"Cannot teleport just now. There are {nearbyEnemies.Count} within {Config.EnemyScanningRange}!");
                    return false;
                }


            }

            return true;

        }

        protected virtual void ImposeConsequences(EntityPlayer entityPlayer)
        {
        }

        protected void StoreTimeStamp()
        {
            this.TimeLastUsed = DateTime.Now;
        }

        
    }


}