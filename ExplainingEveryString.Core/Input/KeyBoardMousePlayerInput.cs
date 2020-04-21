using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class KeyBoardMousePlayerInput : PlayerInput
    {
        private static readonly Vector2 down = new Vector2(0, -1);
        private static readonly Vector2 up = new Vector2(0, 1);
        private static readonly Vector2 left = new Vector2(-1, 0);
        private static readonly Vector2 right = new Vector2(1, 0);
        private readonly Func<Vector2> playerPositionOnScreen;

        private Single focused = 0;
        private Single timeToFocus = 0.25F;
        public override Single Focus => focused * focused;

        internal KeyBoardMousePlayerInput(Func<Vector2> playerPositionOnScreen, Single timeToFocus)
        {
            this.playerPositionOnScreen = playerPositionOnScreen;
            this.timeToFocus = timeToFocus;
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

        public override Vector2 GetFireDirection()
        {
            var mousePoint = Mouse.GetState().Position;
            var mousePosition = new Vector2(mousePoint.X, mousePoint.Y);
            var fireDirectionOnScreen = mousePosition - playerPositionOnScreen();
            var fireDirectionOnLevel = new Vector2(fireDirectionOnScreen.X, -fireDirectionOnScreen.Y);
            return NormalizeDirectionVector(fireDirectionOnLevel);
        }

        public override Boolean IsTryingToDash()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Space);
        }

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
    }
}
