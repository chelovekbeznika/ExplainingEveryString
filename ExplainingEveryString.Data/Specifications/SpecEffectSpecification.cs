using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class SpecEffectSpecification
    {
        public String Sound { get; set; }
        public Single Volume { get; set; }
        [DefaultValue(null)]
        public SpriteSpecification Sprite { get; set; }
    }
}
