using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Data.Blueprints
{
    public class FourthBossBlueprint : EnemyBlueprint
    {
        public String[] PartsList { get; set; }
        public FourthBossMovementSpecification MovementSpecification { get; set; }
    }
}
