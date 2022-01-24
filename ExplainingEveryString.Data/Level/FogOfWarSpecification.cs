using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class FogOfWarSpecification
    {
        public Int32 ParticleWidth { get; set; }
        public Int32 ParticleHeight { get; set; }
        [DefaultValue(null)]
        public String DefaultSprite { get; set; }
        [DefaultValue(Int32.MinValue)]
        public Int32 PutDefaultSpritesLeftOf { get; set; }
        public String[] Sprites { get; set; }
        public Int32[] Weights { get; set; }
    }
}
