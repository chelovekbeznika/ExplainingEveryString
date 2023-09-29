using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class WeaponSpecification : IModificationSpecification
    {
        [DefaultValue(null)]
        public String Name { get; set; }
        [DefaultValue("0.0, 0.0")]
        public Vector2 Offset { get; set; }
        public ReloaderSpecification Reloader { get; set; }
        public BarrelSpecification[] Barrels { get; set; }
        public SpriteSpecification Sprite { get; set; }
        public SpriteSpecification CooldownSprite { get; set; }
        public SpecEffectSpecification ShootingEffect { get; set; }
        public AimType AimType { get; set; }
        [DefaultValue("Weapon")]
        public String ModificationType { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AimType
    {
        FixedFireDirection, RecalibratableFireDirection, SpinningAim, AimAtPlayer, ByMovement, ControlledByPlayer, Rotating
    }
}
