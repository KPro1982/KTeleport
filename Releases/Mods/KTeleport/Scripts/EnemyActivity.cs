using System.Collections.Generic;
using UnityEngine;

//adapted from Sphereii's EIA Scripts

namespace KTeleport
{
    internal static class EnemyActivity
    {
        private static readonly LogLevel _log = LogLevel.None;

        public static List<Entity> GetSurroundingEntities(Entity _theEntity, Vector3 _boundingBox)
        {
            var log = LogLevel.None;
            var nearbyEntities = new List<Entity>();

            var bb = new Bounds(_theEntity.position, _boundingBox);

            _theEntity.world.GetEntitiesInBounds(typeof(EntityAlive), bb, nearbyEntities);

            nearbyEntities.RemoveAll(x => x.name == _theEntity.name);
            nearbyEntities.RemoveAll(x => x.IsDead());
            KHelper.EasyLog($"Nearby Entities: {nearbyEntities.Count}", log);

            return nearbyEntities;
        }

        public static List<Entity> GetTargetingEntities(Entity _theEntity, Vector3 _boundingBox)
        {
            var targetingEntities = GetSurroundingEntities(_theEntity, _boundingBox);
            var originalCount = targetingEntities.Count;

            for (var i = 0; i < targetingEntities.Count; i++)
            {
                var x = (EntityAlive) targetingEntities[i];

                var blAttackTarget = x.GetAttackTarget() != null &&
                                     x.GetAttackTarget().name == _theEntity.name;
                var blRevengeTarget = x.GetRevengeTarget() != null &&
                                      x.GetRevengeTarget().name == _theEntity.name;

                if (!blAttackTarget && !blRevengeTarget)
                    targetingEntities
                        .Remove(x); // safe to assume that only entities only occur in list once? If not what would the remove all look like?
            }

            KHelper.EasyLog(
                targetingEntities.Count != originalCount
                    ?
                    $"{originalCount} entities are in the area. {targetingEntities.Count} targeting you...yet."
                    : targetingEntities.Count > 0
                        ? "Happy now? You pissed everyone off."
                        : "", _log);
            return targetingEntities;
        }
    }
}