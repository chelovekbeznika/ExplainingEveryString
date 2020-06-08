using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Movement
{
    interface IMover
    {
        Vector2 GetPositionChange(Vector2 lineToTarget, ref Single timeRemained);
        Boolean IsTeleporting { get; }
    }
}
