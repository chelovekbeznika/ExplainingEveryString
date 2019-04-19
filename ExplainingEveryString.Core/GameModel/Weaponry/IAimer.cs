using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal interface IAimer
    {
        Vector2 GetFireDirection();
        Boolean IsFiring();
    }
}
