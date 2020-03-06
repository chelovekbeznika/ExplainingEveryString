using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
