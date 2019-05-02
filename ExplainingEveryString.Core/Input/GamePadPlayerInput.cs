using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class GamePadPlayerInput : PlayerInput
    {
        private Vector2 lastDirection = new Vector2(1, 0);

        public override Vector2 GetMoveDirection()
        {
            Vector2 direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            return CutDirectionVector(direction);
        }

        public override Boolean IsFiring()
        {
            return GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Length() > 0;
        }

        public override Vector2 GetFireDirection()
        {
            if (IsFiring())
            {
                Vector2 direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                direction = NormalizeDirectionVector(direction);
                lastDirection = direction;
                return direction;
            }
            else
                return lastDirection;
        }
    }
}
