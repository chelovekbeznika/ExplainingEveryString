using ExplainingEveryString.Core.Math;
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
        private WaveState currentEnemyWaveState = WaveState.Sleeping;
        private CollisionsChecker collisionsChecker = new CollisionsChecker();

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
            if (currentEnemyWaveState == WaveState.Sleeping)
                SleepingWaveCheck();
            if (currentEnemyWaveState == WaveState.Triggered)
                TriggeredWaveCheck();
        }

        private void SleepingWaveCheck()
        {
            if (collisionsChecker.Collides(ActiveActors.Player.GetCurrentHitbox(), ActiveActors.CurrentWaveStartRegion))
            {
                currentEnemyWaveState = WaveState.Triggered;
                ActiveActors.AssignEnemiesFromWave(actorsInitializer, currentEnemyWaveNumber);
            }
        }

        private void TriggeredWaveCheck()
        {
            if (ActiveActors.CurrentEnemyWaveDestroyed && !Won)
            {
                ActiveActors.EndWave(currentEnemyWaveNumber);
                currentEnemyWaveNumber += 1;
                currentEnemyWaveState = WaveState.Sleeping;
                if (!Won)
                    ActiveActors.SwitchStartRegion(actorsInitializer, currentEnemyWaveNumber);
            }
        }

        private enum WaveState { Sleeping, Triggered }
    }
}
