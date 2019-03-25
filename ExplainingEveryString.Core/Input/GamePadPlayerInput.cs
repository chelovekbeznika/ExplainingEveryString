using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class GamePadPlayerInput : PlayerInput
    {
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
            Vector2 direction = GamePad.GetState(PlayerIndex.One).ThumbSticks.Right;
            return NormalizeDirectionVector(direction);
        }
    }
}
