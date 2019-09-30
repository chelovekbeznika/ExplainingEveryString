using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Data.Blueprints
{
    public class ShadowEnemyBlueprint : EnemyBlueprint
    {
        public SpriteSpecification ShadowSprite { get; set; }
        public SpecEffectSpecification PhaseChangeEffect { get; set; }
        public Single ActiveTime { get; set; }
        public Single ShadowTime { get; set; }
    }
}
