using ExplainingEveryString.Core.GameModel.Weaponry.Aimers;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal interface IPlayerInput : IAimer, GameModel.IUpdateable
    {
        Vector2 GetMoveDirection();
        Vector2 GetCursorPosition();
        Boolean IsTryingToDash();
        Boolean IsTryingToReload();
        Int32 WeaponSwitchMeasure();
        String DirectlySelectedWeapon();
        Single Focus { get; }
    }

    internal abstract class PlayerInput : IPlayerInput
    {
        private Func<Vector2> playerPositionOnScreen;

        protected Vector2 PlayerPositionOnScreen => playerPositionOnScreen();

        public abstract Single Focus { get; }

        protected PlayerInput(Func<Vector2> playerPositionOnScreen)
        {
            this.playerPositionOnScreen = playerPositionOnScreen;
        }

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
        public abstract Vector2 GetCursorPosition();
        public abstract Vector2 GetFireDirection(Vector2 currentMuzzlePosition);
        public abstract Boolean IsTryingToDash();
        public abstract Boolean IsTryingToReload();
        public abstract Int32 WeaponSwitchMeasure();
        public abstract String DirectlySelectedWeapon();

        public virtual void Update(Single elapsedSeconds)
        {
        }

    }
}
