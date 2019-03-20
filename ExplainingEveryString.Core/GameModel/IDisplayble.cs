using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal interface IDisplayble
    {
        String CurrentSpriteName { get; }
        Vector2 Position { get; }
    }
}
