using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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

    internal interface IEnemy : IActor, IInterfaceAccessable, ICrashable, ITouchableByBullets, IMultiPartDisplayble
    {
        SpawnedActorsController SpawnedActors { get; }
        List<IEnemy> Avengers { get; }
    }
}
