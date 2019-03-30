using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class HunterBlueprint : EnemyBlueprint
    {
        public Single Acceleration { get; set; }
        public Single StartSpeed { get; set; }
        public Single PlayerDetectionRange { get; set; }
    }
}
