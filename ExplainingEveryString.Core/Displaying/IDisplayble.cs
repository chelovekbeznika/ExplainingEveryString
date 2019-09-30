using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Displaying
{
    internal interface IDisplayble
    {
        IEnumerable<IDisplayble> GetParts();
        SpriteState SpriteState { get; }
        Vector2 Position { get; }
        Boolean IsVisible { get; }
    }

    internal interface IInterfaceAccessable : IDisplayble
    {
        Single HitPoints { get; }
        Single MaxHitPoints { get; }
        Boolean ShowInterfaceInfo { get; }
    }
}
