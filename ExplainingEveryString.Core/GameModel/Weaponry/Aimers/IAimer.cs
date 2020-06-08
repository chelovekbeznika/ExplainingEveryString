using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry.Aimers
{
    internal interface IAimer : IUpdateable
    {
        Vector2 GetFireDirection();
        Boolean IsFiring();
    }
}
