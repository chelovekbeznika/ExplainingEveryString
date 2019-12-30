using ExplainingEveryString.Core.GameModel.Weaponry;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class EnemyBehaviorChangedEventArgs : EventArgs
    {
        internal SpawnedActorsController OldSpawner { get; set; }
        internal SpawnedActorsController NewSpawner { get; set; }
    }
}
