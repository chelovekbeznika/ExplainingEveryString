using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Data.Blueprints
{
    public class FourthBossBlueprint : EnemyBlueprint
    {
        public String[] PartsList { get; set; }
        public FourthBossMovementSpecification MovementSpecification { get; set; }
    }
}
