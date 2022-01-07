using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    interface ISpawnedActorsController : IUpdateable
    {
        List<IEnemy> SpawnedEnemies { get; }
        Int32 MaxSpawned { get; }
        void DivideAliveAndDead(List<IEnemy> avengers);
        void TurnOn();
        void TurnOff();
    }
}
