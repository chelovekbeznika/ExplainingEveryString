using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Assets
{
    internal class SpriteData
    {
        internal int Width => Sprite.Width / AnimationFrames;
        internal int Height => Sprite.Height;
        internal Texture2D Sprite { get; set; }
        internal int AnimationFrames { get; set; }
        internal float AnimationCycle { get; set; }
    }
}
