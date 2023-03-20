using ExplainingEveryString.Data.RandomVariables;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

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
