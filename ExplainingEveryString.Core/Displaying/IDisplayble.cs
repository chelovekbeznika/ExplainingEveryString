using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal interface IDisplayble
    {
        SpriteState SpriteState { get; }
        Vector2 Position { get; }
    }
}
