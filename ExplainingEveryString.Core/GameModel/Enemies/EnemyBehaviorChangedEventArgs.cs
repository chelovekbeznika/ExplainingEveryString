using ExplainingEveryString.Core.GameModel.Weaponry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class EnemyBehaviorChangedEventArgs : EventArgs
    {
        internal SpawnedActorsController OldSpawner { get; set; }
        internal SpawnedActorsController NewSpawner { get; set; }
    }
}
