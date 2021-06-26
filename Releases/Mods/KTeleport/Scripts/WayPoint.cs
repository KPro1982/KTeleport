using UnityEngine;
using Random = System.Random;

namespace KTeleport
{
    public class WayPoint : Portal
    {
        public WayPoint()
        {
        }

        public WayPoint(string _name, Vector3i _coords) : base(_name, _coords)
        {
        }

        public override void Teleport(EntityPlayer _entityPlayer,
            YRestraint _yRestraint = YRestraint.OnGround)
        {
            if (CanTeleport(_entityPlayer))
            {
                int _distance = 0;
                var fuzzyCoords = MakeFuzzy(Coords, Config.AverageDisplacementDistance, out _distance);
                SingletonMonoBehaviour<SdtdConsole>.Instance.ExecuteSync(
                    BuildConsoleCommand(fuzzyCoords, _yRestraint), null);
                ChargesUsed++;
                StoreTimeStamp();
                ImposeConsequences(fuzzyCoords);
                KHelper.EasyLog($"You were displaced {_distance} meters");
            }
        }

        protected override bool CanTeleport(EntityPlayer _entityPlayer)
        {
            return base.CanTeleport(_entityPlayer);
        }

        protected override void ImposeConsequences(Vector3i playerCoords)
        {
            if (DidConsequenceTrigger())
            {
                var rand = new Random();
                var roll = rand.Next(1, calcNumZeds());
                SpawnHere(Config.ZombieTypeAtWaypoint, playerCoords, calcFuzzy(), roll);
            }
        }

        protected int calcFuzzy()
        {
            float resultFuzzy = (float)Config.AverageSpawnDistance * CalcEnchancement();
            return (int) resultFuzzy;
        }

        protected int calcNumZeds()
        {
            WeightedRandom rand = new WeightedRandom();
            rand.Weight = CalcWeight();
            int resultZeds = rand.Next(Config.MAXZeds);
            return resultZeds;
        }
        private static float CalcEnchancement()
        {
            float enchancement = (float) (ChargesUsed + 100) / 100f;
            return enchancement;
        }

        public Vector3i MakeFuzzy(Vector3i _target, Vector3i _fuzzy, out int _distance)
        {
            int dx, dz;
            var rand = new WeightedRandom();
            rand.Weight = CalcWeight();
            dx = rand.Next(_fuzzy.x) - _fuzzy.x/2 ;
            dz = rand.Next(_fuzzy.y) - _fuzzy.y/2;
            Vector3i _result = _target + new Vector3i(dx, 0, dz);
            _distance = (int)Vector3.Distance(_target.ToVector3(), _result.ToVector3());
            KHelper.EasyLog($"Dx:{dx}, Dy:{dz}, Wt:{rand.Weight}, Charges:{ChargesUsed}");

            return _result;
        }

        private float CalcWeight()
        {
            const int freeCharges = 15;
            const int maxCharges = 40;
            const decimal minWeight = 0.05M;
            float _wt = 0;
            
            if (ChargesUsed <= freeCharges)
            {
                _wt = (float)Map(ChargesUsed, 0, freeCharges, 5, 1);
            }
            else if (ChargesUsed <=maxCharges)
            {
                _wt = (float)Map(ChargesUsed, freeCharges, maxCharges, 1, minWeight);
            }
            else
            {
                _wt = (float)minWeight;
            }
            
            return (float)_wt;
        }

        public static decimal Map (decimal value, decimal fromSource, decimal toSource, decimal fromTarget, decimal toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
        public Vector3i MakeFuzzy(Vector3i _target, int _dxz, out int _distance)
        {
            return MakeFuzzy(_target, new Vector3i(_dxz, 0, _dxz), out _distance);
        }

        public void SpawnHere(string _zombieType, Vector3i playerCoords, int _dxz, int _num = 1)
        {
            var classId = EntityClass.FromString(_zombieType);

            for (var i = 0; i < _num; i++)
            {
                var fuzzyCoords = MakeFuzzy(playerCoords, _dxz, out int _distance).ToVector3();
                var entity = EntityFactory.CreateEntity(classId, fuzzyCoords);
                if (entity != null)
                {

                    GameManager.Instance.World.SpawnEntityInWorld(entity);
                }
            }
            KHelper.EasyLog($"Spawned {_num} {_zombieType}...WATCHOUT!", LogLevel.Chat);
        }

        protected bool DidConsequenceTrigger()
        {
            var chance = 100 - Config.BasePercentChanceOfConsequence * ChargesUsed;
            WeightedRandom rand = new WeightedRandom();
            rand.Weight = CalcWeight();
            var roll = rand.Next(100);
            KHelper.EasyLog(
                $"ChargesUsed: {ChargesUsed}.  You have a {chance}% chance of a safe trip. You rolled a {roll}.",
                LogLevel.Chat);
            return roll >= chance;
        }
    }
}