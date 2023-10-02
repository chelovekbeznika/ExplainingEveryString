using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Core.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceDrawController
    {
        private readonly SpriteBatch spriteBatch;
        private readonly Color colorMask;
        private Single elapsedTime = 0;

        internal Int32 ScreenWidth => Displaying.Constants.TargetWidth;
        internal Int32 ScreenHeight => Displaying.Constants.TargetHeight;

        internal InterfaceDrawController(SpriteBatch spriteBatch, Color colorMask)
        {
            this.spriteBatch = spriteBatch;
            this.colorMask = colorMask;
        }

        internal void Update(Single elapsedSeconds)
        {
            elapsedTime += elapsedSeconds;
        }

        internal void DrawText(String text, Vector2 position, CustomFont customFont)
        {
            customFont.Draw(spriteBatch, position, text);
        }

        internal void Draw(SpriteData spriteData, Vector2 position, Boolean opaque = false)
        {
            Draw(spriteData, position, new WholeSpriteDisplayer(), 1, opaque);
        }

        internal void Draw(SpriteData spriteData, Vector2 position, ISpritePartDisplayer partDisplayer, Single coeff, Boolean opaque = false)
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
            spriteBatch.Draw(spriteData.Sprite, positionOfSpritePart, drawPart, opaque ? Color.White : colorMask);
        }
    }
}
