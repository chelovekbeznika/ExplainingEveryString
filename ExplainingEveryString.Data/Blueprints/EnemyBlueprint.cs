using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class EnemyBlueprint : Blueprint
    {
        public Single CollisionDamage { get; set; }
        public Single MaxSpeed { get; set; }
    }
}
