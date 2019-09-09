using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
