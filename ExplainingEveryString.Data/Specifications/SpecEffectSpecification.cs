using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class SpecEffectSpecification
    {
        public SoundSpecification Sound { get; set; }
        [DefaultValue(null)]
        public SpriteSpecification Sprite { get; set; }
    }
}
