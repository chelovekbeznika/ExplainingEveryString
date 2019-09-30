using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    interface IMover
    {
        Vector2 GetPositionChange(Vector2 lineToTarget, Single elapsedSeconds, out Boolean goalReached);
    }
}
