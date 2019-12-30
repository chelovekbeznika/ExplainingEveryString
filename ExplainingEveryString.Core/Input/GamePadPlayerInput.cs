using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class GamePadPlayerInput : PlayerInput
    {
        private Vector2 lastDirection = new Vector2(0, 1);

        public override Vector2 GetMoveDirection()
        {
            var direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
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
                var direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
                direction = NormalizeDirectionVector(direction);
                lastDirection = direction;
                return direction;
            }
            else
                return lastDirection;
        }

        public override Boolean IsTryingToDash()
        {
            return GamePad.GetState(PlayerIndex.One).Triggers.Left >= 0.5;
        }
    }
}
