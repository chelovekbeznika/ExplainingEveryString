using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal interface IPlayerInput
    {
        Vector2 GetMoveDirection();
        Vector2 GetFireDirection();
        Boolean IsFiring();
    }

    internal abstract class PlayerInput : IPlayerInput
    {
        protected Vector2 CutDirectionVector(Vector2 direction)
        {
            if (direction.Length() > 1)
                direction /= direction.Length();
            return direction;
        }

        protected Vector2 NormalizeDirectionVector(Vector2 direction)
        {
            if (direction.Length() > 0)
                direction.Normalize();
            if (direction.Length() == 0)
                return new Vector2(1, 0);
            return direction;
        }

        public abstract Boolean IsFiring();
        public abstract Vector2 GetMoveDirection();
        public abstract Vector2 GetFireDirection();
    }
}
