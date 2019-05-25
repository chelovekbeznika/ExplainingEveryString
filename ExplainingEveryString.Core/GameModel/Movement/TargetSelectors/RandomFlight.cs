using Microsoft.Xna.Framework;
using System;
using ExplainingEveryString.Core.Math;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class RandomFlight : IMoveTargetSelector
    {
        private Vector2 range;
        private Vector2 startPosition;
        private Vector2 currentTarget;

        internal RandomFlight(Vector2 startPosition, Vector2 range)
        {
            this.startPosition = startPosition;
            this.range = range;
            SwitchToNextTarget();
        }

        public Vector2 GetTarget()
        {
            return currentTarget;
        }

        public void SwitchToNextTarget()
        {
            currentTarget = new Vector2
            {
                X = startPosition.X + (RandomUtility.Next() - 0.5F) * range.X,
                Y = startPosition.Y + (RandomUtility.Next() - 0.5F) * range.Y
            };
        }
    }
}
