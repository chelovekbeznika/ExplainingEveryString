using ExplainingEveryString.Core.Displaying;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class GamePadPlayerInput : PlayerInput
    {
        private enum CameraMode { Direction, CursorPosition }

        private readonly Single betweenPlayerAndCursor;
        private readonly Single cameraSpeed;
        private Vector2 lastDirection = new Vector2(0, 1);
        private CameraMode cameraMode = CameraMode.Direction;
        private Vector2 cursorPosition;

        private readonly Single timeToFocus;
        public override Single Focus => focus;
        private Single focus = 0;
        private GamePadState afterLastWeaponCheck;

        public GamePadPlayerInput(Func<Vector2> playerPositionOnScreen, Single timeToFocus, 
            Single betweenPlayerAndCursor, Single cameraSpeed) 
            : base(playerPositionOnScreen)
        {
            this.timeToFocus = timeToFocus;
            this.cameraSpeed = cameraSpeed;
            this.afterLastWeaponCheck = GetState();
            this.betweenPlayerAndCursor = betweenPlayerAndCursor;
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

            cameraMode = focus >= 0.01 ? CameraMode.CursorPosition : CameraMode.Direction;
            switch (cameraMode)
            {
                case CameraMode.CursorPosition:
                    var rightStickDirection = GetState().ThumbSticks.Right;
                    rightStickDirection.Y *= -1;
                    cursorPosition += rightStickDirection * cameraSpeed * elapsedSeconds;
                    cursorPosition = FitOnScreen(cursorPosition);
                    break;
                case CameraMode.Direction:
                    var direction = GetFireDirection(Vector2.Zero);
                    direction.Y *= -1;
                    cursorPosition = PlayerPositionOnScreen + direction * betweenPlayerAndCursor;
                    break;
            }
        }

        public override Vector2 GetMoveDirection()
        {
            var direction = GetState().ThumbSticks.Left;
            return CutDirectionVector(direction);
        }

        public override Boolean IsFiring() => GetState().ThumbSticks.Right.Length() > 0.25 || cameraMode == CameraMode.CursorPosition;

        public override Vector2 GetFireDirection(Vector2 _)
        {
            if (IsFiring())
            {
                var direction = lastDirection;
                switch (cameraMode)
                {
                    case CameraMode.Direction: 
                        direction = NormalizeDirectionVector(GetState().ThumbSticks.Right); 
                        break;
                    case CameraMode.CursorPosition:
                        direction = (cursorPosition - PlayerPositionOnScreen);
                        direction /= (cursorPosition - PlayerPositionOnScreen).Length(); 
                        direction.Y *= -1;
                        break;
                };
                lastDirection = direction;
                return direction;
            }
            else
                return lastDirection;
        }

        public override Vector2 GetCursorPosition() => cursorPosition;

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

        private Vector2 FitOnScreen(Vector2 cursorPosition)
        {
            if (cursorPosition.X < 0)
                cursorPosition.X = 0;
            if (cursorPosition.X > Constants.TargetWidth)
                cursorPosition.X = Constants.TargetWidth;
            if (cursorPosition.Y < 0)
                cursorPosition.Y = 0;
            if (cursorPosition.Y > Constants.TargetHeight)
                cursorPosition.Y = Constants.TargetHeight;
            return cursorPosition;
        }

        private GamePadState GetState() => GamePad.GetState(PlayerIndex.One);

        public override String DirectlySelectedWeapon() => null;
    }
}
