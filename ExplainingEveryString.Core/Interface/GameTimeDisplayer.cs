using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal class GameTimeDisplayer
    {
        private SpriteFont timeFont;

        internal GameTimeDisplayer(SpriteFont timeFont)
        {
            this.timeFont = timeFont;
        }

        internal void Draw(Single time, SpriteBatch spriteBatch, Color colorMask)
        {
            String timeString = $"{time:f1}";
            Vector2 positionOnScreen = CalculatePositionOnScreen(timeString);
            spriteBatch.DrawString(timeFont, timeString, positionOnScreen, colorMask);
        }

        private Vector2 CalculatePositionOnScreen(String timeString)
        {
            Vector2 stringSize = timeFont.MeasureString(timeString);
            return new Vector2 { X = 64 - stringSize.X, Y = 32 };
        }
    }
}
