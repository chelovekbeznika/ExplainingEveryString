using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Movement.Movers
{
    internal class NonMover : IMover
    {
        public Vector2 GetPositionChange(Vector2 lineToTarget, Single elapsedSeconds, out Boolean goalReached)
        {
            goalReached = lineToTarget.Length() >= Math.Constants.Epsilon;
            return new Vector2(0, 0);
        }
    }
}
