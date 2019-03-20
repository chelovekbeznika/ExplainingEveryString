using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal interface IDrawable
    {
        String CurrentSpriteName { get; }
        Vector2 Position { get; }
    }
}
