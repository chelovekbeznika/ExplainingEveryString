using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class LevelState
    {
        private Int32 currentEnemyWaveNumber = 0;
        private ActorsInitializer actorsInitializer;
        private LevelData levelData;

        internal ActiveActorsStorage ActiveActors { get; private set; }

        internal Boolean Lost => !ActiveActors.Player.IsAlive();
        internal Boolean Won => currentEnemyWaveNumber >= levelData.EnemyWaves.Length;

        internal LevelState(ActiveActorsStorage activeActors, ActorsInitializer actorsInitializer, LevelData levelData)
        {
            this.ActiveActors = activeActors;
            this.actorsInitializer = actorsInitializer;
            this.levelData = levelData;
        }

        internal void Update()
        {
            ActiveActors.Update();
            if (ActiveActors.CurrentEnemyWaveDestroyed && !Won)
            {
                currentEnemyWaveNumber += 1;
                if (!Won)
                    ActiveActors.GetNextEnemyWave(actorsInitializer, currentEnemyWaveNumber);
            }
        }
    }
}
