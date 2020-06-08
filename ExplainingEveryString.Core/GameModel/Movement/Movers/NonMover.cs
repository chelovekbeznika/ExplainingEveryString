using System;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Movement.Movers
{
    internal class NonMover : IMover
    {
        public Boolean IsTeleporting => false;
        public Vector2 GetPositionChange(Vector2 lineToTarget, ref Single timeRemained)
        {
            timeRemained = 0;
            return new Vector2(0, 0);
        }
    }
}
