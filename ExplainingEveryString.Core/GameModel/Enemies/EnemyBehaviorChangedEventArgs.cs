using ExplainingEveryString.Core.GameModel.Weaponry;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class EnemyBehaviorChangedEventArgs : EventArgs
    {
        internal ISpawnedActorsController OldSpawner { get; set; }
        internal ISpawnedActorsController NewSpawner { get; set; }
    }
}
