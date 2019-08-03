using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Level;
using System;

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
        internal Single SecondsPassedAfterLevelEnd { get; private set; } = 0;

        internal Boolean Lost => !ActiveActors.Player.IsAlive();
        internal Boolean Won => !Lost && currentEnemyWaveNumber >= levelData.EnemyWaves.Length;
        internal Boolean LevelEnded => Won || Lost;

        internal LevelState(ActiveActorsStorage activeActors, ActorsInitializer actorsInitializer, LevelData levelData)
        {
            this.ActiveActors = activeActors;
            this.actorsInitializer = actorsInitializer;
            this.levelData = levelData;
            activeActors.InitializeActorsOnLevelStart(actorsInitializer);
        }

        internal void Update(Single elapsedSeconds)
        {
            ActiveActors.Update();
            if (!Won)
            {
                if (currentEnemyWaveState == WaveState.Sleeping)
                    SleepingWaveCheck();
                if (currentEnemyWaveState == WaveState.Triggered)
                    TriggeredWaveCheck();
            }
            if (LevelEnded)
                SecondsPassedAfterLevelEnd += elapsedSeconds;
        }

        private void SleepingWaveCheck()
        {
            if (collisionsChecker.Collides(ActiveActors.Player.GetCurrentHitbox(), ActiveActors.CurrentWaveStartRegion))
            {
                currentEnemyWaveState = WaveState.Triggered;
                ActiveActors.StartEnemyWave(actorsInitializer, currentEnemyWaveNumber);
            }
        }

        private void TriggeredWaveCheck()
        {
            if (ActiveActors.CurrentEnemyWaveDestroyed)
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
