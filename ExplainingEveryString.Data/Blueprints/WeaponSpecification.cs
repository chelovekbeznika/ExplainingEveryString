using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ExplainingEveryString.Data.Blueprints
{
    public class WeaponSpecification
    {
        public Single FireRate { get; set; }
        [DefaultValue(null)]
        public BarrelSpecification[] Barrels { get; set; }
        public SpriteSpecification Sprite { get; set; }
        public SpecEffectSpecification ShootingEffect { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AimType AimType { get; set; }

        public IEnumerable<SpriteSpecification> GetSprites()
        {
            return new SpriteSpecification[] { Sprite }.Concat(Barrels.Select(b => b.Bullet.Sprite));
        }
    }

    public enum AimType
    {
        FixedFireDirection, AimAtPlayer, ControlledByPlayer
    }
}
