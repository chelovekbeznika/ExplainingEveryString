using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Blueprints
{
    public class WeaponSpecification
    {
        public Single FireRate { get; set; }
        public Single BarrelLength { get; set; }
        public BulletSpecification BulletSpecification { get; set; }
        [DefaultValue(null)]
        public SpriteSpecification Sprite { get; set; }
        public SpecEffectSpecification ShootingEffect { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AimType AimType { get; set; }
    }

    public enum AimType
    {
        FixedFireDirection, AimAtPlayer, ControlledByPlayer
    }
}
