using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal interface IAimer
    {
        Vector2 GetFireDirection();
        Boolean IsFiring();
    }

    internal class FixedAimer : IAimer
    {
        private Vector2 fireDirection;

        public FixedAimer(Single angle)
        {
            fireDirection = AngleConverter.ToVector(angle);
        }

        public Vector2 GetFireDirection()
        {
            return fireDirection;
        }

        public bool IsFiring()
        {
            return true;
        }
    }
}
