using System;
using UnityEngine;
using Random = System.Random;

namespace kScripts
{
    public class WayPoint : Portal
    {
        
        public WayPoint()
        {
        }

        public WayPoint(string _name, Vector3i _coords) : base(_name, _coords)
        {
        }
        public override void Teleport(EntityPlayer _entityPlayer, YRestraint _yRestraint = YRestraint.OnGround)
        {
            if (CanTeleport(_entityPlayer))
            {
                Vector3i fuzzyCoords = MakeFuzzy(Coords, Config.AverageDisplacementDistance);
                SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(
                        BuildConsoleCommand(fuzzyCoords, _yRestraint), null);
                ChargesUsed++;
                base.StoreTimeStamp();
                ImposeConsequences(GameManager.Instance.World.GetPrimaryPlayer());    
            }
            
        }

        protected override bool CanTeleport(EntityPlayer _entityPlayer)
        {
            return base.CanTeleport(_entityPlayer);
        }

        protected override void ImposeConsequences(EntityPlayer _entityPlayer)
        {
            if (DidConsequenceTrigger())
            {
                var rand = new Random();
                var roll = rand.Next(1, Config.MAXZeds);
                SpawnHere(Config.ZombieTypeAtWaypoint,Config.AverageSpawnDistance, roll);
            }
    
        }
        public Vector3i MakeFuzzy(Vector3i _target, Vector3i _fuzzy)
        {
            int dx, dz;
            var rand = new Random();
            dx = rand.Next(-_fuzzy.x, +_fuzzy.x);
            dz = rand.Next(-_fuzzy.z, +_fuzzy.z);

            return _target + new Vector3i(dx, 0, dz);

        }

        public Vector3i MakeFuzzy(Vector3i _target, int _dxz)
        {
            return MakeFuzzy(_target, new Vector3i(_dxz, 0, _dxz));
        }

        public void SpawnHere(String _zombieType, int _dxz, int _num = 1)
        {
            int classId = EntityClass.FromString(_zombieType);

            for (int i=0; i < _num; i++)
            {
                Vector3 fuzzyCoords = MakeFuzzy(Coords, _dxz).ToVector3();
                Entity entity = EntityFactory.CreateEntity(classId, fuzzyCoords);
                if (entity != null)
                {
                    KHelper.EasyLog($"Spawning {_zombieType}...WATCHOUT!", LogLevel.Chat);
                    GameManager.Instance.World.SpawnEntityInWorld(entity);
                }
            }
                
            
        }

        protected bool DidConsequenceTrigger()
        {
            int chance = (int) (Config.BasePercentChanceOfConsequence * ChargesUsed);
            var rand = new Random();
            var roll = rand.Next(0, 100);
            KHelper.EasyLog($"ChargesUsed: {ChargesUsed}.  You have a {chance}% chance of a negative consequence each time you use a waypont. You rolled a {roll}.", LogLevel.Chat );
            return ( roll <= chance);
        }
    }
}