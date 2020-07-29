using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
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
        [DefaultValue(false)]
        public Boolean ConsiderAngle { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification HitEffect { get; set; }

        public SpriteSpecification Sprite { get; set; }
    }
}
