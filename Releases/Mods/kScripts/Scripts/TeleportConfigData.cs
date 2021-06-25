using System.Globalization;

namespace kScripts
{
    public class TeleportConfigData
    {
        public int MAXZeds = 1;
        public string ZombieTypeAtWaypoint = "zombieStripper";
        public int BasePercentChanceOfConsequence = 1;
        public int AverageSpawnDistance = 15;
        public int AverageDisplacementDistance = 100;
        public bool CanTeleportNearEnemies = false;
        public float EnemyScanningRange = 50f;

        public TeleportConfigData()
        {
            GetXmlProperties();
        }

        private void GetXmlProperties()
        {
            LogLevel log = LogLevel.None;
            if (KHelper.GetXmlProperty("KTeleport", "MaxZedsAtWaypoint") != "")
            {
                MAXZeds = int.Parse(KHelper.GetXmlProperty("KTeleport", "MaxZedsAtWaypoint"));
                KHelper.EasyLog($"MaxZedsAtWaypoint: {MAXZeds}", log);
            }

            if (KHelper.GetXmlProperty("KTeleport", "ZombieTypeAtWaypoint") != "")
            {
                ZombieTypeAtWaypoint = KHelper.GetXmlProperty("KTeleport", "ZombieTypeAtWaypoint");
                KHelper.EasyLog($"ZombieTypeAtWaypoint: {ZombieTypeAtWaypoint}", log);
            }

            if (KHelper.GetXmlProperty("KTeleport", "BasePercentChanceOfConsequence") != "")
            {
                BasePercentChanceOfConsequence = int.Parse(KHelper.GetXmlProperty("KTeleport", "BasePercentChanceOfConsequence"));
                KHelper.EasyLog($"BasePercentChanceOfConsequence: {BasePercentChanceOfConsequence}", log);
            }
            if (KHelper.GetXmlProperty("KTeleport", "AverageSpawnDistance") != "")
            {
                AverageSpawnDistance =
                    int.Parse(KHelper.GetXmlProperty("KTeleport", "AverageSpawnDistance"));
                KHelper.EasyLog($"AverageSpawnDistance: {AverageSpawnDistance}", log);
            }
            if (KHelper.GetXmlProperty("KTeleport", "AverageDisplacementDistance") != "")
            {
                AverageDisplacementDistance =
                    int.Parse(KHelper.GetXmlProperty("KTeleport", "AverageDisplacementDistance"));
                KHelper.EasyLog($"AverageDisplacementDistance: {AverageDisplacementDistance}", log);
            }
            if (KHelper.GetXmlProperty("KTeleport", "CanTeleportNearEnemies") != "")
            {
                CanTeleportNearEnemies = bool.Parse(KHelper.GetXmlProperty("KTeleport", "CanTeleportNearEnemies"));
                KHelper.EasyLog($"MaxZedsAtWaypoint: {CanTeleportNearEnemies}", log);
            }
            if (KHelper.GetXmlProperty("KTeleport", "EnemyScanningRange") != "")
            {
                EnemyScanningRange = float.Parse(KHelper.GetXmlProperty("KTeleport", "EnemyScanningRange"));
                KHelper.EasyLog($"EnemyScanningRange: {CanTeleportNearEnemies}", log);
            }
            
            
        }
    }
}