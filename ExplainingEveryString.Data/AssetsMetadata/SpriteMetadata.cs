using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
