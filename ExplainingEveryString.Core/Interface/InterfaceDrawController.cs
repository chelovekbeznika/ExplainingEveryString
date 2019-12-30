using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceSpriteDisplayer
    {
        private readonly SpriteBatch spriteBatch;
        private readonly Color colorMask;
        private Single elapsedTime = 0;

        internal Int32 ScreenWidth => spriteBatch.GraphicsDevice.Viewport.Width;
        internal Int32 ScreenHeight => spriteBatch.GraphicsDevice.Viewport.Height;

        internal InterfaceSpriteDisplayer(SpriteBatch spriteBatch, Color colorMask)
        {
            this.spriteBatch = spriteBatch;
            this.colorMask = colorMask;
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
            var animationFrame = AnimationHelp.GetDrawPart(spriteData, elapsedTime);
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
