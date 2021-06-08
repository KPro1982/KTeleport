using System.Collections.Generic;
using UnityEngine;

//adapted from Sphereii's EIA Scripts

namespace kScripts
{
    static class EnemyActivity
    {
        public static List<Entity> GetSurroundingEntities(Entity _theEntity, Vector3 _boundingBox)
        {
            List<Entity> nearbyEntities = new List<Entity>();

            Bounds bb = new Bounds(_theEntity.position, _boundingBox);

            _theEntity.world.GetEntitiesInBounds(typeof(EntityAlive), bb, nearbyEntities);

            nearbyEntities.RemoveAll(x => x.name == _theEntity.name);
            nearbyEntities.RemoveAll(x => x.IsDead());
            kHelper.EasyLog($"Nearby Entities: {nearbyEntities.Count}");

            return nearbyEntities;
        }

        public static List<Entity> GetTargetingEntities(Entity _theEntity, Vector3 _boundingBox)
        {
            List<Entity> targetingEntities = GetSurroundingEntities(_theEntity, _boundingBox);
            int originalCount = targetingEntities.Count;

            for (int i = 0; i < targetingEntities.Count; i++)
            {
                EntityAlive x = (EntityAlive)targetingEntities[i];

                bool blAttackTarget = x.GetAttackTarget() != null && (x.GetAttackTarget().name == _theEntity.name);
                bool blRevengeTarget = x.GetRevengeTarget() != null && (x.GetRevengeTarget().name == _theEntity.name);

                if (!blAttackTarget && !blRevengeTarget)
                {
                    targetingEntities.Remove(x);  // safe to assume that only entities only occur in list once? If not what would the remove all look like?
                }
            }
            kHelper.EasyLog($"Targeting Entities: {targetingEntities.Count}");
            kHelper.EasyLog((targetingEntities.Count != originalCount) ? $"{originalCount - targetingEntities.Count} entities are in the area but are not targeting you...yet." : (targetingEntities.Count > 0 ? $"Happy now? You pissed everyone off." : ""));
            return targetingEntities;
        }
    }
}