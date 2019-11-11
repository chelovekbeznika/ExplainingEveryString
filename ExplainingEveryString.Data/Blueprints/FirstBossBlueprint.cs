using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Data.Blueprints
{
    public class FirstBossBlueprint : EnemyBlueprint
    {
        public Single TimeBetweenPhases { get; set; }
        public Single MinPhaseDuration { get; set; }
        public Single MaxPhaseDuration { get; set; }
        public SpecEffectSpecification PhaseOnEffect { get; set; }
        public SpecEffectSpecification PhaseOffEffect { get; set; }
        public FirstBossPhaseSpecification[] Phases { get; set; }
    }
}
