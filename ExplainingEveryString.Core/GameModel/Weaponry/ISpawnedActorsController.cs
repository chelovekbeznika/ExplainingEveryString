using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    interface ISpawnedActorsController : IUpdateable
    {
        List<IEnemy> SpawnedEnemies { get; }
        Int32 MaxSpawned { get; }
        void SendDeadToHeaven(List<IEnemy> avengers);
        void TurnOn();
        void TurnOff();
    }
}
