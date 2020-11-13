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

        internal static Single ClosestArc(Single currentAngle, Single targetAngle)
        {
            currentAngle = AlignAngle(currentAngle);
            targetAngle = AlignAngle(targetAngle);
            Single counterclockArc, clockwiseArc;
            if (currentAngle > targetAngle)
            {
                clockwiseArc = currentAngle - targetAngle;
                counterclockArc = MathHelper.TwoPi - clockwiseArc;
            }
            else
            {
                counterclockArc = targetAngle - currentAngle;
                clockwiseArc = MathHelper.TwoPi - counterclockArc;
            }
            if (counterclockArc < clockwiseArc)
                return counterclockArc;
            else
                return -clockwiseArc;
        }

        private static Single AlignAngle(Single angle) => 
            (Single)(System.Math.IEEERemainder(angle, MathHelper.TwoPi) + MathHelper.Pi);
    }
}
