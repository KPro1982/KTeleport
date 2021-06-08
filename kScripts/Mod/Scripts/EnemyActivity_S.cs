using System;
using System.Collections.Generic;
using UnityEngine;

//adapted from Sphereii's EIA Scripts

namespace kScripts
{
    static class EnemyActivity
    {
        public static List<Entity> GetSurroundingEntities(Entity _theEntity, Vector3 _boundingbox)
        {
            List<Entity> NearbyEntities = new List<Entity>();

            Bounds bb = new Bounds(_theEntity.position, _boundingbox);

            _theEntity.world.GetEntitiesInBounds(typeof(EntityAlive), bb, NearbyEntities);

            NearbyEntities.RemoveAll(x => x.name == _theEntity.name);
            NearbyEntities.RemoveAll(x => x.IsDead() == true);
            kHelper.EasyLog(String.Format($"Nearby Entities: {NearbyEntities.Count}"));

            return NearbyEntities;
        }

        public static List<Entity> GetTargetingEntities(Entity _theEntity, Vector3 _boundingbox)
        {
            List<Entity> TargetingEntities = GetSurroundingEntities(_theEntity, _boundingbox);
            int originalCount = TargetingEntities.Count;

            for (int i = 0; i < TargetingEntities.Count; i++)
            {
                EntityAlive x = (EntityAlive)TargetingEntities[i];

                bool blAttackTarget = x.GetAttackTarget() == null ? false : (x.GetAttackTarget().name == _theEntity.name ? true : false);
                bool blRevengeTarget = x.GetRevengeTarget() == null ? false : (x.GetRevengeTarget().name == _theEntity.name ? true : false);

                if (!blAttackTarget && !blRevengeTarget)
                {
                    TargetingEntities.Remove(x);  // safe to assume that only entities only occur in list once? If not what would the remove all look like?
                }
            }
           kHelper.EasyLog($"Targeting Entities: {TargetingEntities.Count}");
           kHelper.EasyLog((TargetingEntities.Count != originalCount) ? $"{originalCount - TargetingEntities.Count} entities are in the area but are not targetting you...yet." : (TargetingEntities.Count > 0 ? $"Happy now? You pissed everyone off." : ""));
           return TargetingEntities;
        }
    }
}