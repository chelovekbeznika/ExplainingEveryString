using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExplainingEveryString.Core.Input
{
    internal interface IPlayerInput
    {
        Vector2 GetMoveDirection();
    }

    internal abstract class PlayerInput : IPlayerInput
    {
        protected abstract Vector2 GetAxesVectorFromDevice();

        public Vector2 GetMoveDirection()
        {
            Vector2 direction = GetAxesVectorFromDevice();
            if (direction.Length() > 0)
                direction.Normalize();
            return direction;
        }
    }

    internal class KeyBoardPlayerInput : PlayerInput
    {
        private static readonly Vector2 down = new Vector2(0, 1);
        private static readonly Vector2 up = new Vector2(0, -1);
        private static readonly Vector2 left = new Vector2(-1, 0);
        private static readonly Vector2 right = new Vector2(1, 0);

        protected override Vector2 GetAxesVectorFromDevice()
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

    internal class GamePadPlayerInput : PlayerInput
    {
        protected override Vector2 GetAxesVectorFromDevice()
        {
            Vector2 direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            direction.Y *= -1;
            return direction;
        }
    }
}
