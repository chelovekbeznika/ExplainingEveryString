using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Data.Blueprints
{
    public class SecondBossBlueprint : EnemyBlueprint
    {
        public Single DeathEllipseX { get; set; }
        public Single DeathEllipseY { get; set; }
        public Single DeathZoneDamage { get; set; }
        public SpawnerSpecification DeathZoneBorderSpawner { get; set; }
        public Single DeathZonePatrolCycleTime { get; set; }
        public OneTimeSpawnerSpecification PowerKeepersSpawner { get; set; }
        public SecondBossPowerKeepersSpecification PowerKeepersMovement { get; set; }
    }
}
