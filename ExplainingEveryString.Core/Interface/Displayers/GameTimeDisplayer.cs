using ExplainingEveryString.Core.Displaying;
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
        private readonly Int32 pixelsBetween = 16;

        internal GameTimeDisplayer(CustomFont timeFont)
        {
            this.timeFont = timeFont;
        }

        internal void Draw(GameTimeInfo gameTimeInfo, SpriteBatch spriteBatch, Color colorMask)
        {
            if (gameTimeInfo.CurrentLevelRecord != null)
            {
                var record = gameTimeInfo.CurrentLevelRecord.Value;
                var recordString = GameTimeHelper.ToTimeString(record);
                var recordPosition = FirstTimePositionOnScreen();
                timeFont.Draw(spriteBatch, recordPosition, recordString, new Color(Constants.NintendoGold, colorMask.A));

                var timeString = GameTimeHelper.ToTimeString(gameTimeInfo.CurrentTime);
                var timePosition = SecondTimePositionOnScreen(recordString);
                timeFont.Draw(spriteBatch, timePosition, timeString, colorMask);
            }
            else
            {
                var timeString = GameTimeHelper.ToTimeString(gameTimeInfo.CurrentTime);
                var timePosition = FirstTimePositionOnScreen();
                timeFont.Draw(spriteBatch, timePosition, timeString, colorMask);
            }
        }

        private Vector2 FirstTimePositionOnScreen()
        {
            return new Vector2 { X = pixelsFromLeft, Y = pixelsFromTop };
        }

        private Vector2 SecondTimePositionOnScreen(String firstString)
        {
            var firstTimeSize = timeFont.GetSize(firstString);
            return new Vector2 { X = pixelsFromLeft + firstTimeSize.X + pixelsBetween, Y = pixelsFromTop };
        }
    }
}
