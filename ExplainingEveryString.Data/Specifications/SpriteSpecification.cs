using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Specifications
{
    public class SpriteSpecification : IModificationSpecification
    {
        public String Name { get; set; }
        [DefaultValue(1)]
        public Single AnimationCycle { get; set; }
        [DefaultValue("DefaultSprite")]
        public String ModificationType { get; set; }
    }
}
