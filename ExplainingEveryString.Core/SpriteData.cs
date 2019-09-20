using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core
{
    internal class SpriteData
    {
        internal Int32 Width => Sprite.Width / AnimationFrames;
        internal Int32 Height => Sprite.Height;
        internal Texture2D Sprite { get; set; }
        internal Int32 AnimationFrames { get; set; }
        internal Single DefaultAnimationCycle { get; set; }
    }
}
