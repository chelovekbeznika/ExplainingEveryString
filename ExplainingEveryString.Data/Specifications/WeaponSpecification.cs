using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class WeaponSpecification
    {
        [DefaultValue(null)]
        public String Name { get; set; }
        public ReloaderSpecification Reloader { get; set; }
        public BarrelSpecification[] Barrels { get; set; }
        public SpriteSpecification Sprite { get; set; }
        public SpecEffectSpecification ShootingEffect { get; set; }
        public AimType AimType { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AimType
    {
        FixedFireDirection, SpinningAim, AimAtPlayer, ByMovement, ControlledByPlayer
    }
}
