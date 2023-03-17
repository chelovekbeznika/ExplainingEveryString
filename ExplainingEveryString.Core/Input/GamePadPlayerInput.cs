using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class GamePadPlayerInput : PlayerInput
    {
        private Vector2 lastDirection = new Vector2(0, 1);

        private readonly Single timeToFocus;
        public override Single Focus => focus;
        private Single focus = 0;
        private GamePadState afterLastWeaponCheck;

        public GamePadPlayerInput(Single timeToFocus)
        {
            this.timeToFocus = timeToFocus;
            this.afterLastWeaponCheck = GetState();
        }

        public override void Update(Single elapsedSeconds)
        {
            var desiredFocus = GetState().Triggers.Right;
            var maxFocusChange = elapsedSeconds / timeToFocus;
            if (System.Math.Abs(focus - desiredFocus) < maxFocusChange)
                focus = desiredFocus;
            else if (focus > desiredFocus)
                focus -= maxFocusChange;
            else
                focus += maxFocusChange;
        }

        public override Vector2 GetMoveDirection()
        {
            var direction = GetState().ThumbSticks.Left;
            return CutDirectionVector(direction);
        }

        public override Boolean IsFiring() => GetState().ThumbSticks.Right.Length() > 0;

        public override Vector2 GetFireDirection(Vector2 currentMuzzlePosition)
        {
            if (IsFiring())
            {
                var direction = GetState().ThumbSticks.Right;
                direction = NormalizeDirectionVector(direction);
                lastDirection = direction;
                return direction;
            }
            else
                return lastDirection;
        }

        public override Boolean IsTryingToDash() => GetState().Triggers.Left >= 0.5;

        public override Boolean IsTryingToReload() => GetState().Buttons.A == ButtonState.Pressed;

        public override Int32 WeaponSwitchMeasure()
        {
            var currentState = GetState();
            var result = 0;
            if (currentState.Buttons.RightShoulder == ButtonState.Pressed && afterLastWeaponCheck.Buttons.RightShoulder == ButtonState.Released)
                result += 1;
            if (currentState.Buttons.LeftShoulder == ButtonState.Pressed && afterLastWeaponCheck.Buttons.LeftShoulder == ButtonState.Released)
                result -= 1;
            afterLastWeaponCheck = currentState;
            return result;
        }

        private GamePadState GetState() => GamePad.GetState(PlayerIndex.One);

        public override String DirectlySelectedWeapon() => null;
    }
}
