using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Displaying
{
    internal interface IMultiPartDisplayble : IDisplayble
    {
        IEnumerable<IDisplayble> GetParts();
    }

    internal interface IDisplayble
    {
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
