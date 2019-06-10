using ExplainingEveryString.Core.Displaying;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal interface IUpdatable
    {
        void Update(Single elapsedSeconds);
    }

    internal interface IActor : ICollidable, IDisplayble, IUpdatable
    {
        Boolean IsAlive();
    }
}
