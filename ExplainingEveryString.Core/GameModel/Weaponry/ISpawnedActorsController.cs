using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    interface ISpawnedActorsController : IUpdateable
    {
        event EventHandler<EnemySpawnedEventArgs> EnemySpawned;
        List<IEnemy> SpawnedEnemies { get; }
        Int32 MaxSpawned { get; }
        void DivideAliveAndDead(List<IEnemy> avengers);
        void DespawnRoutine();
        void TurnOn();
        void TurnOff();
    }

    internal sealed class EnemySpawnedEventArgs : EventArgs
    {
        internal IEnemy Enemy { get; set; }
    }
}
