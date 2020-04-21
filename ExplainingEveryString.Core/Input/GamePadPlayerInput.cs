using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.Input
{
    internal class GamePadPlayerInput : PlayerInput
    {
        private Vector2 lastDirection = new Vector2(0, 1);

        private readonly Single timeToFocus;
        public override Single Focus => focus;
        private Single focus = 0;


        public GamePadPlayerInput(Single timeToFocus)
        {
            this.timeToFocus = timeToFocus;
        }

        public override void Update(Single elapsedSeconds)
        {
            var desiredFocus = GamePad.GetState(PlayerIndex.One).Triggers.Right;
            var maxFocusChange = elapsedSeconds / timeToFocus;
            if (System.Math.Abs(focus - desiredFocus) < maxFocusChange)
                focus = desiredFocus;
            else if (focus > desiredFocus)
                focus -= maxFocusChange;
            else
                focus += maxFocusChange;
        }

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
