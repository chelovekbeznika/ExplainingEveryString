using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Data.Blueprints
{
    public class FifthBossBlueprint : EnemyBlueprint
    {
        public FifthBossLimbSpecification LeftEye { get; set; }
        public FifthBossLimbSpecification RightEye { get; set; }
        public FifthBossLimbSpecification Tentacle { get; set; }
        public Single HealthThresholdToUseTentacles { get; set; }
        public Vector2[] TentaclesOffsets { get; set; }
        public Single[] AngleOffsets { get; set; }
        public Single[] HealthThresholdToSpawnHelper { get; set; }
        public SpawnerSpecification HelperSpawn { get; set; }
    }
}
