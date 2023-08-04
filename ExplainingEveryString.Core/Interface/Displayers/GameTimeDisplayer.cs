using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class GameTimeDisplayer
    {
        private readonly CustomFont timeFont;
        private const Int32 pixelsFromLeft = 32;
        private const Int32 pixelsFromTop = 32;
        private const Int32 pixelsFromTopSecondLine = 64;
        private const Int32 pixelsBetween = 16;

        internal GameTimeDisplayer(CustomFont timeFont)
        {
            this.timeFont = timeFont;
        }

        internal void Draw(InterfaceGameTimeInfo gameTimeInfo, SpriteBatch spriteBatch, Color colorMask)
        {
            DrawLine(spriteBatch, gameTimeInfo.LevelRecord, gameTimeInfo.LevelTime, pixelsFromTop, colorMask);

            if (gameTimeInfo.RunTime.HasValue)
            {
                var gameRecord = gameTimeInfo.PersonalBest;
                var runTime = gameTimeInfo.RunTime.Value;
                DrawLine(spriteBatch, gameRecord, runTime, pixelsFromTopSecondLine, colorMask);
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Single? record, Single currentTime, Int32 fromTop, Color colorMask)
        {
            if (record != null)
            {
                var recordString = GameTimeHelper.ToTimeString(record.Value);
                var recordPosition = CurrentTimePositionOnScreen(fromTop);
                timeFont.Draw(spriteBatch, recordPosition, recordString, new Color(Constants.NintendoGold, colorMask.A));

                var timeString = GameTimeHelper.ToTimeString(currentTime);
                var timePosition = RecordTimePositionOnScreen(fromTop, recordString);
                timeFont.Draw(spriteBatch, timePosition, timeString, colorMask);
            }
            else
            {
                var timeString = GameTimeHelper.ToTimeString(currentTime);
                var timePosition = CurrentTimePositionOnScreen(fromTop);
                timeFont.Draw(spriteBatch, timePosition, timeString, colorMask);
            }
        }

        private Vector2 CurrentTimePositionOnScreen(Int32 fromTop)
        {
            return new Vector2 { X = pixelsFromLeft, Y = fromTop };
        }

        private Vector2 RecordTimePositionOnScreen(Int32 fromTop, String firstString)
        {
            var firstTimeSize = timeFont.GetSize(firstString);
            return new Vector2 { X = pixelsFromLeft + firstTimeSize.X + pixelsBetween, Y = fromTop };
        }
    }
}
