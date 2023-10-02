using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Math
{
    internal static class ScreenCoordinatesHelper
    {
        public static Vector2 ConvertToInnerScreenCoordinates(Single x, Single y)
        {
            var screenConfig = ConfigurationAccess.GetCurrentConfig().Screen;
            var widthWithoutVerticalStripes = (Int32)System.Math.Round((Double)(screenConfig.ScreenHeight * 8.0 / 7.0));
            var stripWidth = (screenConfig.ScreenWidth - widthWithoutVerticalStripes) / 2;
            return new Vector2(
                x: (x - stripWidth) / widthWithoutVerticalStripes * Displaying.Constants.TargetWidth,
                y: y / screenConfig.ScreenHeight * Displaying.Constants.TargetHeight);
        }

        public static Vector2 GetScreenBorderDangerDirection(Rectangle screenBorders, Vector2 playerPosition, Vector2 enemyPosition)
        {
            var candidates = new List<Vector2>(2);
            if (enemyPosition.X <= screenBorders.Left)
                candidates.Add(new Vector2
                {
                    X = screenBorders.Left,
                    Y = GeometryHelper.GetLineCrossingWithVerticalFringe(screenBorders.Left, playerPosition, enemyPosition).Value
                });
            if (enemyPosition.X >= screenBorders.Right)
                candidates.Add(new Vector2
                {
                    X = screenBorders.Right,
                    Y = GeometryHelper.GetLineCrossingWithVerticalFringe(screenBorders.Right, playerPosition, enemyPosition).Value
                });
            if (enemyPosition.Y <= screenBorders.Top)
                candidates.Add(new Vector2
                {
                    X = GeometryHelper.GetLineCrossingWithHorizontalsFringe(screenBorders.Top, playerPosition, enemyPosition).Value,
                    Y = screenBorders.Top
                });
            if (enemyPosition.Y >= screenBorders.Bottom)
                candidates.Add(new Vector2
                {
                    X = GeometryHelper.GetLineCrossingWithHorizontalsFringe(screenBorders.Bottom, playerPosition, enemyPosition).Value,
                    Y = screenBorders.Bottom
                });
            return candidates.First(candidate => LiesWithinScreenBorders(screenBorders, candidate));
        }

        private static Boolean LiesWithinScreenBorders(Rectangle screenBorders, Vector2 value)
        {
            //Cause Rectangle.Contains doesn't count points lying on right and bottom borders of rectangle.
            return (screenBorders.X <= value.X) && (value.X <= (screenBorders.X + screenBorders.Width))
                && (screenBorders.Y <= value.Y) && (value.Y <= (screenBorders.Y + screenBorders.Height));
        }
    }
}
