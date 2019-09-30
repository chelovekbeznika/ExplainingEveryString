using ExplainingEveryString.Data.Specifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Blueprints
{
    public class DoorBlueprint : Blueprint
    {
        public SpriteSpecification OpeningSprite { get; set; }
        [DefaultValue(DoorOpeningMode.Instant)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DoorOpeningMode OpeningMode { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification OpeningStartedEffect { get; set; }
        [DefaultValue(null)]
        public SpecEffectSpecification CompletelyOpenedEffect { get; set; }
    }

    public enum DoorOpeningMode { Instant, Up, Down, Left, Right }
}
