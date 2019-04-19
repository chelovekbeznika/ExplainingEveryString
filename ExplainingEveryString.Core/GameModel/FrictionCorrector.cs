using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal static class FrictionCorrector
    {
        internal static Single FrictionCoefficient { get; set; } = 0.9F;

        internal static Vector2 Correct(Vector2 beforeFriction, Single elapsedSeconds)
        {
            Vector2 afterFriction = beforeFriction * (Single)System.Math.Pow(1 - FrictionCoefficient, elapsedSeconds);
            if (afterFriction.Length() < 5)
                afterFriction = new Vector2(0, 0);
            return afterFriction;
        }
    }
}
