using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal class SpinningAimer : IAimer
    {
        private Single currentAngle;
        private Single angularVelocity;
        private Vector2 fireDirection;

        public SpinningAimer(Single angle)
        {
            this.angularVelocity = angle;
        }

        public Vector2 GetFireDirection(Vector2 currentMuzzlePosition)
        {
            return fireDirection;
        }

        public Boolean IsFiring()
        {
            return true;
        }

        public void Update(Single elapsedSeconds)
        {
            currentAngle += angularVelocity * elapsedSeconds;
            fireDirection = AngleConverter.ToVector(currentAngle);
        }
    }
}
