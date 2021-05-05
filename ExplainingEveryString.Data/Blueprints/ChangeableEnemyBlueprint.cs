using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Blueprints
{
    public class ChangeableEnemyBlueprint : EnemyBlueprint
    {
        [DefaultValue(null)]
        public Dictionary<String, IModificationSpecification[]> Modifications { get; set; }
        [DefaultValue(null)]
        public ChangingEventSpecification ChangingEventAtDeath { get; set; }
    }
}
