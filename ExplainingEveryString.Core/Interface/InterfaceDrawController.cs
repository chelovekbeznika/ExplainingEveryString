using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceSpriteDisplayer
    {
        private readonly SpriteBatch spriteBatch;
        private readonly ScreenConfiguration screenConfig;
        private readonly Color colorMask;
        private Single elapsedTime = 0;

        internal Int32 ScreenWidth => screenConfig.TargetWidth;
        internal Int32 ScreenHeight => screenConfig.TargetHeight;

        internal InterfaceSpriteDisplayer(SpriteBatch spriteBatch, Color colorMask, Configuration config)
        {
            this.spriteBatch = spriteBatch;
            this.colorMask = colorMask;
            this.screenConfig = config.Screen;
        }

        internal void Update(Single elapsedSeconds)
        {
            elapsedTime += elapsedSeconds;
        }

        internal void Draw(SpriteData spriteData, Vector2 position)
        {
            Draw(spriteData, position, new WholeSpriteDisplayer(), 1);
        }

        internal void Draw(SpriteData spriteData, Vector2 position, ISpritePartDisplayer partDisplayer, Single coeff)
        {
            var animationFrame = AnimationHelper.GetDrawPart(spriteData, elapsedTime);
            var drawPart = partDisplayer.GetDrawPart(spriteData, coeff);
            if (animationFrame != null)
                drawPart = new Rectangle
                {
                    X = animationFrame.Value.X + drawPart.X,
                    Y = animationFrame.Value.Y + drawPart.Y,
                    Width = drawPart.Width,
                    Height = drawPart.Height
                };
            var positionOfSpritePart = partDisplayer.GetDrawPosition(spriteData, coeff, position);
            spriteBatch.Draw(spriteData.Sprite, positionOfSpritePart, drawPart, colorMask);
        }
    }
}
