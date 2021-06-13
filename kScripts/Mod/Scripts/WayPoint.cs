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
            SpawnHere("zombieStripper", 3);
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


                Vector3i myCoords = _coords;
                myCoords.y = 60;
                KHelper.EasyLog(_coords, LogLevel.Both);
                Vector3 spawnLocation = myCoords.ToVector3();
                Entity entity = EntityFactory.CreateEntity(classId, spawnLocation);
                if (entity != null)
                {
                    GameManager.Instance.World.SpawnEntityInWorld(entity);
                }
            
        }
    }
}