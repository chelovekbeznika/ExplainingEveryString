using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Math
{
    internal static class GeometryHelper
    {
        internal static Single? GetLineCrossingWithVerticalFringe
            (Single fringeXPosition, Vector2 lineBegin, Vector2 lineEnd)
        {
            if (lineBegin.X == lineEnd.X)
            {
                return null;
            }

            var deltaX = lineBegin.X - lineEnd.X;
            var deltaY = lineBegin.Y - lineEnd.Y;
            var deltaXTillFringe = fringeXPosition - lineEnd.X;
            var deltaYTillFringe = deltaY * deltaXTillFringe / deltaX;
            return lineEnd.Y + deltaYTillFringe;
        }

        internal static Single? GetLineCrossingWithHorizontalsFringe
            (Single fringeYPosition, Vector2 lineBegin, Vector2 lineEnd)
        {
            if (lineBegin.Y == lineEnd.Y)
            {
                return null;
            }

            var deltaX = lineBegin.X - lineEnd.X;
            var deltaY = lineBegin.Y - lineEnd.Y;
            var deltaYTillFringe = fringeYPosition - lineEnd.Y;
            var deltaXTillFringe = deltaX * deltaYTillFringe / deltaY;
            return lineEnd.X + deltaXTillFringe;
        }

        internal static Vector2 RotateVector(Vector2 rawPosition, Single sinus, Single cosinus)
        {
            return new Vector2
            {
                X = rawPosition.X * cosinus - rawPosition.Y * sinus,
                Y = rawPosition.X * sinus + rawPosition.Y * cosinus
            };
        }

        internal static Vector2 RotateVector(Vector2 rawPosition, Single radians)
        {
            return RotateVector(rawPosition, (Single)System.Math.Sin(radians), (Single)System.Math.Cos(radians));
        }
    }
}
