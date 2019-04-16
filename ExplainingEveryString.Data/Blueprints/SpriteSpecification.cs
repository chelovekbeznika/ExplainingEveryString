using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Blueprints
{
    public class SpriteSpecification
    {
        public String Name { get; set; }
        [DefaultValue(1)]
        public Single AnimationCycle { get; set; }
    }
}
