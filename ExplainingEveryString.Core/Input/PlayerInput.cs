using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal interface IPlayerInput : IAimer, GameModel.IUpdateable
    {
        Vector2 GetMoveDirection();
        Boolean IsTryingToDash();
        Int32 WeaponSwitchMeasure();
        Single Focus { get; }
    }

    internal abstract class PlayerInput : IPlayerInput
    {
        public abstract Single Focus { get; }

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
        public abstract Boolean IsTryingToDash();
        public abstract Int32 WeaponSwitchMeasure();

        public virtual void Update(Single elapsedSeconds)
        {
        }
    }
}
