using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Collections;
using System;
using System.Linq;

namespace ExplainingEveryString.Core.Input
{
    internal class KeyBoardMousePlayerInput : PlayerInput
    {
        private static readonly Vector2 down = new Vector2(0, -1);
        private static readonly Vector2 up = new Vector2(0, 1);
        private static readonly Vector2 left = new Vector2(-1, 0);
        private static readonly Vector2 right = new Vector2(1, 0);
        private Int32 afterLastWeaponCheckScroll;
        private KeyboardState afterLastWeaponCheckKeyboard;

        private Single focused = 0;
        private Single timeToFocus = 0.25F;
        public override Single Focus => focused * focused;

        internal KeyBoardMousePlayerInput(Func<Vector2> playerPositionOnScreen, Single timeToFocus) 
            : base(playerPositionOnScreen)
        {
            this.timeToFocus = timeToFocus;
            this.afterLastWeaponCheckScroll = Mouse.GetState().ScrollWheelValue;
            this.afterLastWeaponCheckKeyboard = Keyboard.GetState();
        }

        public override Vector2 GetMoveDirection()
        {
            var direction = new Vector2(0, 0);
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.S))
                direction += down;
            if (keyboard.IsKeyDown(Keys.W))
                direction += up;
            if (keyboard.IsKeyDown(Keys.A))
                direction += left;
            if (keyboard.IsKeyDown(Keys.D))
                direction += right;
            return CutDirectionVector(direction);
        }

        public override Boolean IsFiring()
        {
            return Mouse.GetState().LeftButton == ButtonState.Pressed;
        }

        public override Vector2 GetFireDirection(Vector2 currentMuzzlePosition)
        {
            var mousePosition = ScreenCoordinatesHelper.ConvertToInnerScreenCoordinates(Mouse.GetState().X, Mouse.GetState().Y);
            var fireDirectionOnScreen = mousePosition - PlayerPositionOnScreen;
            var fireDirectionOnLevel = new Vector2(fireDirectionOnScreen.X, -fireDirectionOnScreen.Y);
            return NormalizeDirectionVector(fireDirectionOnLevel);
        }

        public override Vector2 GetCursorPosition()
        {
            var mousePoint = Mouse.GetState().Position;
            return ScreenCoordinatesHelper.ConvertToInnerScreenCoordinates(mousePoint.X, mousePoint.Y);
        }

        public override Boolean IsTryingToDash() => Keyboard.GetState().IsKeyDown(Keys.Space);

        public override Boolean IsTryingToReload() => Keyboard.GetState().IsKeyDown(Keys.R);

        public override void Update(Single elapsedSeconds)
        {
            var focusButtonPressed = Keyboard.GetState().IsKeyDown(Keys.LeftShift);
            var focusChange = elapsedSeconds / timeToFocus;
            focused += focusButtonPressed ? focusChange : -focusChange;
            if (focused < 0)
                focused = 0;
            if (focused > 1)
                focused = 1;
        }

        public override Int32 WeaponSwitchMeasure()
        {
            var scrollDifference = Mouse.GetState().ScrollWheelValue - afterLastWeaponCheckScroll;
            afterLastWeaponCheckScroll = Mouse.GetState().ScrollWheelValue;

            var currentKeyboard = Keyboard.GetState();
            var keyboardResult = 0;
            if (currentKeyboard.IsKeyDown(Keys.E) && !afterLastWeaponCheckKeyboard.IsKeyDown(Keys.E))
                keyboardResult += 1;
            if (currentKeyboard.IsKeyDown(Keys.Q) && !afterLastWeaponCheckKeyboard.IsKeyDown(Keys.Q))
                keyboardResult -= 1;
            afterLastWeaponCheckKeyboard = currentKeyboard;

            return scrollDifference / 120 + keyboardResult;
        }

        public override String DirectlySelectedWeapon()
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            return keys switch
            {
                _ when keys.Contains(Keys.D1) => WeaponNames.AllExisting[0],
                _ when keys.Contains(Keys.D2) => WeaponNames.AllExisting[1],
                _ when keys.Contains(Keys.D3) => WeaponNames.AllExisting[2],
                _ when keys.Contains(Keys.D4) => WeaponNames.AllExisting[3],
                _ when keys.Contains(Keys.D5) => WeaponNames.AllExisting[4],
                _ when keys.Contains(Keys.D6) => WeaponNames.AllExisting[5],
                _ => null
            };
        }
    }
}
