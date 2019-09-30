using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Math
{
    internal static class AngleConverter
    {
        internal static Single ToRadians(Single degrees)
        {
            return degrees * MathHelper.Pi / 180;
        }

        internal static Single ToRadians(Vector2 directionVector)
        {
            return (Single)System.Math.Atan2(directionVector.Y, directionVector.X);
        }

        internal static Vector2 ToVector(Single radians)
        {
            return new Vector2 { X = (Single)System.Math.Cos(radians), Y = (Single)System.Math.Sin(radians) };
        }

        internal static Single ClosestArc(Single a, Single b)
        {
            Single counterclockArc, clockwiseArc;
            if (a > b)
            {
                clockwiseArc = a - b;
                counterclockArc = MathHelper.TwoPi - clockwiseArc;
            }
            else
            {
                counterclockArc = b - a;
                clockwiseArc = MathHelper.TwoPi - counterclockArc;
            }
            if (counterclockArc < clockwiseArc)
                return counterclockArc;
            else
                return -clockwiseArc;
        }
    }
}
