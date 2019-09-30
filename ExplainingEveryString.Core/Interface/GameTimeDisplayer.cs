﻿using Microsoft.Xna.Framework;
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
            String timeString = $"{time:f1}";
            Vector2 positionOnScreen = CalculatePositionOnScreen(timeString);
            spriteBatch.DrawString(timeFont, timeString, positionOnScreen, colorMask);
        }

        private Vector2 CalculatePositionOnScreen(String timeString)
        {
            Vector2 stringSize = timeFont.MeasureString(timeString);
            return new Vector2 { X = pixelsFromLeft - stringSize.X, Y = pixelsFromTop };
        }
    }
}
