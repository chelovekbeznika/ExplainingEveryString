using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExplainingEveryString.Data.Specifications
{
    public class WeaponSpecification
    {
        public ReloaderSpecification Reloader { get; set; }
        public BarrelSpecification[] Barrels { get; set; }
        public SpriteSpecification Sprite { get; set; }
        public SpecEffectSpecification ShootingEffect { get; set; }
        public AimType AimType { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AimType
    {
        FixedFireDirection, SpinningAim, AimAtPlayer, ControlledByPlayer
    }
}
