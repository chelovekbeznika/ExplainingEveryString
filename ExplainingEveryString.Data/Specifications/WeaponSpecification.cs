using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ExplainingEveryString.Data.Specifications
{
    public class WeaponSpecification
    {
        public ReloaderSpecification Reloader { get; set; }
        public BarrelSpecification[] Barrels { get; set; }
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
