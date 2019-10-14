using Microsoft.Xna.Framework;
using ExplainingEveryString.Core.Math;
using System;

namespace ExplainingEveryString.Core.GameModel.Movement.TargetSelectors
{
    internal class RandomFlight : IMoveTargetSelector
    {
        private ICollidable actor;
        private Single tillBottom;
        private Single tillTop;
        private Single tillRight;
        private Single tillLeft;
        private Vector2 startPosition;
        private Vector2 currentTarget;

        internal RandomFlight(ICollidable actor, Vector2 topLeftBorder, Vector2 bottomRightBorder)
        {
            this.actor = actor;
            this.startPosition = actor.Position;
            this.tillBottom = bottomRightBorder.Y;
            this.tillTop = topLeftBorder.Y;
            this.tillRight = bottomRightBorder.X;
            this.tillLeft = topLeftBorder.X;
            SwitchToNextTarget();
        }

        public Vector2 GetTarget()
        {
            return currentTarget;
        }

        public void SwitchToNextTarget()
        {
            currentTarget = new Vector2(GetNextTargetX(), GetNextTargetY());
        }

        private Single GetNextTargetX()
        {
            Hitbox hitbox = actor.GetCurrentHitbox();
            Single width = hitbox.Right - hitbox.Left;
            return startPosition.X + tillLeft + width / 2 + RandomUtility.Next() * (tillRight - tillLeft - width);
        }

        private Single GetNextTargetY()
        {
            Hitbox hitbox = actor.GetCurrentHitbox();
            Single height = hitbox.Top - hitbox.Bottom;
            return startPosition.Y + tillBottom + height / 2 + RandomUtility.Next() * (tillTop - tillBottom - height);
        }
    }
}
