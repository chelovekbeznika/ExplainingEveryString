﻿using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel
{
    internal interface IUpdateable
    {
        void Update(Single elapsedSeconds);
    }

    internal interface IActor : ICollidable, IDisplayble, IUpdateable
    {
        Boolean IsAlive();
    }

    internal interface IEnemy : IActor, IInterfaceAccessable, IMovableCollidable, ICrashable, ITouchableByBullets, IDisplayble
    {
        SpawnedActorsController SpawnedActors { get; }
        List<IEnemy> Avengers { get; }
    }
}
