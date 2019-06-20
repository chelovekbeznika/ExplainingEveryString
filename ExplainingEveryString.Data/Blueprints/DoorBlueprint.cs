using ExplainingEveryString.Data.Specifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal override IEnumerable<SpriteSpecification> GetSprites()
        {
            return base.GetSprites().Concat(new SpriteSpecification[] { OpeningSprite });
        }

        internal override IEnumerable<SpecEffectSpecification> GetSpecEffects()
        {
            return base.GetSpecEffects()
                .Concat(new SpecEffectSpecification[] { OpeningStartedEffect, CompletelyOpenedEffect });
        }
    }

    public enum DoorOpeningMode { Instant, Up, Down, Left, Right }
}
