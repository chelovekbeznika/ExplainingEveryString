using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Interface
{
    internal class GameTimeDisplayer
    {
        private SpriteFont timeFont;
        private readonly Int32 pixelsFromLeft = 64;
        private readonly Int32 pixelsFromTop = 32;

        internal GameTimeDisplayer(SpriteFont timeFont)
        {
            this.timeFont = timeFont;
        }

        internal void Draw(Single time, SpriteBatch spriteBatch, Color colorMask)
        {
            var timeString = $"{time:f1}";
            var positionOnScreen = CalculatePositionOnScreen(timeString);
            spriteBatch.DrawString(timeFont, timeString, positionOnScreen, colorMask);
        }

        private Vector2 CalculatePositionOnScreen(String timeString)
        {
            var stringSize = timeFont.MeasureString(timeString);
            return new Vector2 { X = pixelsFromLeft - stringSize.X, Y = pixelsFromTop };
        }
    }
}
