using System;
using System.Xml.Serialization;
using UnityEngine;

namespace KTeleport
{
    [XmlInclude(typeof(SimplePoint))]
    [XmlInclude(typeof(WayPoint))]
    [XmlInclude(typeof(HomePoint))]
    public abstract class Portal
    {
        protected Vector3i _coords;
        protected string _name;
        public DateTime _timeCreated;
        public DateTime _timeLastUsed;
        protected object teleportObject;


        public Portal()
        {
            // Portal._coords = new Vector3i(0, 0, 0);
            _name = "";
            TimeCreated = DateTime.Now;
            TimeLastUsed = DateTime.Now;
            teleportObject = null;
        }

        public Portal(string _name, Vector3i _coords)
        {
            // LogAnywhere.Log($"coords from v3i constructor: {_coords.x}, {_coords.y}, {_coords.z}");

            Coords = _coords;
            this._name = _name;
            TimeCreated = DateTime.Now;
            TimeLastUsed = DateTime.Now;
        }

        public bool IsValid { get; set; } = true;
        public static int ChargesUsed { get; set; }
        protected int Used { get; set; }
        public PortalConfigData Config { get; set; }


        public string Name
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
            set => _timeLastUsed = value;
        }

        public DateTime TimeCreated
        {
            get => _timeCreated;
            set => _timeCreated = value;
        }


        public TimeSpan GetAge()
        {
            return DateTime.Now - TimeCreated;
        }

        public TimeSpan GetTimeSinceLastUse()
        {
            var tSpan = DateTime.Now - TimeLastUsed;
            return tSpan;
        }


        protected string BuildConsoleCommand(Vector3i _target,
            YRestraint _yRestraint = YRestraint.OnGround)
        {
            return _yRestraint == YRestraint.OnGround
                ? $"teleport {_target.x} {_target.z}"
                : $"teleport {_target.x} {_target.y} {_target.z}";
        }

        public virtual void Teleport(EntityPlayer _entityPlayer,
            YRestraint _yRestraint = YRestraint.OnGround)
        {
            if (CanTeleport(_entityPlayer))
            {
                SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(
                    BuildConsoleCommand(Coords, _yRestraint), null);
                StoreTimeStamp();
                Used++;
                ImposeConsequences(Coords);
            }
        }

        protected virtual bool CanTeleport(EntityPlayer _entityPlayer)
        {
            if (!Config.CanTeleportNearEnemies)
            {
                var scanRange = new Vector3(Config.EnemyScanningRange, Config.EnemyScanningRange,
                    Config.EnemyScanningRange);
                var nearbyEnemies = EnemyActivity.GetTargetingEntities(_entityPlayer, scanRange);

                if (nearbyEnemies.Count == 0)
                {
                    return true;
                }

                KHelper.EasyLog(
                    $"Cannot teleport just now. There are {nearbyEnemies.Count} within {Config.EnemyScanningRange}!");
                return false;
            }

            return true;
        }

        protected virtual void ImposeConsequences(Vector3i playerCoords)
        {
        }

        protected void StoreTimeStamp()
        {
            TimeLastUsed = DateTime.Now;
        }
    }
}