using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal interface IDisplayble
    {
        String CurrentSpriteName { get; }
        Vector2 Position { get; }
    }
}
