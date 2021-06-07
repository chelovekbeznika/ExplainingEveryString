using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core
{
    internal class SpriteData
    {
        internal Int32 Width => Sprite.Width / AnimationFrames;
        internal Int32 Height => Sprite.Height;
        internal Texture2D Sprite { get; set; }
        internal Int32 AnimationFrames { get; set; }
        internal Single AnimationCycle { get; set; }
    }
}
