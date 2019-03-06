using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExplainingEveryString.Core
{
    internal static class Input
    {
        private static readonly Vector2 down = new Vector2(0, 1);
        private static readonly Vector2 up = new Vector2(0, -1);
        private static readonly Vector2 left = new Vector2(-1, 0);
        private static readonly Vector2 right = new Vector2(1, 0);


        internal static Vector2 GetMoveDirection()
        {
            Vector2 direction = GetDpadDirection() + GetKeyboardDirection();
            if (direction.Length() > 0)
                direction.Normalize();
            return direction;
        }

        private static Vector2 GetDpadDirection()
        {
            Vector2 direction = new Vector2(0, 0);
            GamePadDPad dpad = GamePad.GetState(PlayerIndex.One).DPad;
            if (dpad.Down == ButtonState.Pressed)
                direction += down;
            if (dpad.Up == ButtonState.Pressed)
                direction += up;
            if (dpad.Left == ButtonState.Pressed)
                direction += left;
            if (dpad.Right == ButtonState.Pressed)
                direction += right;
            return direction;
        }

        private static Vector2 GetKeyboardDirection()
        {
            Vector2 direction = new Vector2(0, 0);
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.S))
                direction += down;
            if (keyboard.IsKeyDown(Keys.W))
                direction += up;
            if (keyboard.IsKeyDown(Keys.A))
                direction += left;
            if (keyboard.IsKeyDown(Keys.D))
                direction += right;
            return direction;
        }
    }
}
