using System;
using UnityEngine;
using Random = System.Random;

namespace kScripts
{
    public class WayPoint : Portal
    {
        public int BasePercentChanceOfConsequence = 5;
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
                
                SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(BuildConsoleCommand(MakeFuzzy(_coords, new Vector3i(10,0,10))), null);
                base.IncrementUsed();
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
                var roll = rand.Next(1, 3);
                SpawnHere("zombieStripper", 3);
            }
    
        }
        public Vector3i MakeFuzzy(Vector3i _target, Vector3i _fuzzy)
        {
            int _dx, _dz;
            var rand = new Random();
            _dx = rand.Next(-_fuzzy.x, +_fuzzy.x);
            _dz = rand.Next(-_fuzzy.z, +_fuzzy.z);

            return _target + new Vector3i(_dx, 0, _dz);

        }

        public Vector3i MakeFuzzy(Vector3i _target, int _dxz)
        {
            return MakeFuzzy(_target, new Vector3i(_dxz, 0, _dxz));
        }

        public void SpawnHere(String _zombieType, int _num = 1, int _dxz = 10)
        {
            int classId = EntityClass.FromString(_zombieType);

            for (int i=0; i < _num; i++)
            {
                KHelper.EasyLog(_coords, LogLevel.Both);
                Entity entity = EntityFactory.CreateEntity(classId, MakeFuzzy(_coords, _dxz).ToVector3());
                if (entity != null)
                {
                    GameManager.Instance.World.SpawnEntityInWorld(entity);
                }
            }
                
            
        }

        protected bool DidConsequenceTrigger()
        {
            int chance = BasePercentChanceOfConsequence * _used;
            var rand = new Random();
            var roll = rand.Next(0, 100);
            KHelper.EasyLog($"You have a {chance}% of a negative consequence. You rolled a {roll}.", LogLevel.Both );
            return ( roll <= chance);
        }
    }
}