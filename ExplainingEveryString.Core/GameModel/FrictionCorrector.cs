using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal static class FrictionCorrector
    {
        private const Single frictionCoefficient = 0.5F;

        internal static Vector2 Correct(Vector2 beforeFriction, Single elapsedSeconds)
        {
            Vector2 afterFriction = beforeFriction * (Single)Math.Pow(frictionCoefficient, elapsedSeconds);
            if (afterFriction.Length() < 5)
                afterFriction = new Vector2(0, 0);
            return afterFriction;
        }
    }
}
