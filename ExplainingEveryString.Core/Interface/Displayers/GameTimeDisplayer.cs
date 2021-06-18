using ExplainingEveryString.Core.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class GameTimeDisplayer
    {
        private CustomFont timeFont;
        private readonly Int32 pixelsFromLeft = 32;
        private readonly Int32 pixelsFromTop = 32;

        internal GameTimeDisplayer(CustomFont timeFont)
        {
            this.timeFont = timeFont;
        }

        internal void Draw(Single time, SpriteBatch spriteBatch, Color colorMask)
        {
            var timeSpan = TimeSpan.FromSeconds(time);
            var timeString = $"{timeSpan:h\\:mm\\:ss\\.ff}";
            var positionOnScreen = CalculatePositionOnScreen(timeString);
            timeFont.Draw(spriteBatch, positionOnScreen, timeString, colorMask);
        }

        private Vector2 CalculatePositionOnScreen(String timeString)
        {
            return new Vector2 { X = pixelsFromLeft, Y = pixelsFromTop };
        }
    }
}
