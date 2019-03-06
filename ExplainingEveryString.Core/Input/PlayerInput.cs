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
