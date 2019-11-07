using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Specifications
{
    public class FirstBossPhaseSpecification
    {
        public EnemyBehaviorSpecification Behavior { get; set; }
        public SpriteSpecification Phase { get; set; }
        public SpriteSpecification On { get; set; }
        public SpriteSpecification Off { get; set; }
        [DefaultValue(null)]
        public Vector2[] TrajectoryParameters { get; set; }
    }
}
