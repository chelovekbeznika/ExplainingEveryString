using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Blueprints
{
    public class SpecEffectSpecification
    {
        public String Sound { get; set; }
        public Single Volume { get; set; }
        [DefaultValue(null)]
        public SpriteSpecification Sprite { get; set; }
    }
}
