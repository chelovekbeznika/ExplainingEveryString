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
        [DefaultValue(0F)]
        public Single BlastWaveRadius { get; set; }
        [DefaultValue(0F)]
        public Single PrematureBlastInterval { get; set; }
        public Single TimeToLive { get; set; }
        /// <summary>
        /// Degrees per second
        /// </summary>
        [DefaultValue(0F)]
        public Single HomingSpeed { get; set; }
        [DefaultValue(false)]
        public Boolean ConsiderAngle { get; set; }

        [DefaultValue(null)]
        public SpecEffectSpecification HitEffect { get; set; }

        public SpriteSpecification Sprite { get; set; }
    }
}
