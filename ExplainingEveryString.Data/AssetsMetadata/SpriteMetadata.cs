using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.AssetsMetadata
{
    public class SpriteMetadata
    {
        public String Name { get; set; }
        public Int32 AnimationFrames { get; set; }
        [DefaultValue(1)]
        public Single DefaultAnimationCycle { get; set; }
    }
}
