using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Data.Blueprints
{
    public class BulletSpecification
    {
        public Dictionary<String, Single> TrajectoryParameters { get; set; }
        [DefaultValue("Linear")]
        public String TrajectoryType { get; set; }
        public Single Damage { get; set; }
        public Single TimeToLive { get; set; }
        /// <summary>
        /// Degrees per second
        /// </summary>
        [DefaultValue(0.0)]
        public Single HomingSpeed { get; set; }

        public SpriteSpecification Sprite { get; set; }
    }
}
