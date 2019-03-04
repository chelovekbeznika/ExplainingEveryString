using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExplainingEveryString.Core
{
    internal static class Input
    {
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
                direction += new Vector2(0, 1);
            if (dpad.Up == ButtonState.Pressed)
                direction += new Vector2(0, -1);
            if (dpad.Left == ButtonState.Pressed)
                direction += new Vector2(-1, 0);
            if (dpad.Right == ButtonState.Pressed)
                direction += new Vector2(1, 0);
            return direction;
        }

        private static Vector2 GetKeyboardDirection()
        {
            Vector2 direction = new Vector2(0, 0);
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.S))
                direction += new Vector2(0, 1);
            if (keyboard.IsKeyDown(Keys.W))
                direction += new Vector2(0, -1);
            if (keyboard.IsKeyDown(Keys.A))
                direction += new Vector2(-1, 0);
            if (keyboard.IsKeyDown(Keys.D))
                direction += new Vector2(1, 0);
            return direction;
        }
    }
}
