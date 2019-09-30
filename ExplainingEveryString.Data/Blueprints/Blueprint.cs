using ExplainingEveryString.Data.Specifications;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Blueprints
{
    public class Blueprint
    {
        public SpriteSpecification DefaultSprite { get; set; }
        public Single Height { get; set; }
        public Single Width { get; set; }
        [DefaultValue(1)]
        public Single Hitpoints { get; set; }
    }
}
