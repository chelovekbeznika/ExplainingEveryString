using ExplainingEveryString.Data.RandomVariables;
using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Data.Blueprints
{
    public class LastBossBlueprint : EnemyBlueprint
    {

        public Int32 SimultaneouslyInUse { get; set; }
        public GaussRandomVariable WeaponsChangeInterval { get; set; }
        public SpecEffectSpecification WeaponsChangeEffect { get; set; }
        public LastBossPhaseWeaponSpecification[] Weapons { get; set; }
    }
}
