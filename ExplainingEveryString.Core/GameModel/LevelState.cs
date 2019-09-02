﻿using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Level;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class LevelState
    {
        private Int32 currentEnemyWaveNumber;
        private Int32 wavesAmount;
        private ActorsInitializer actorsInitializer;
        private WaveState currentEnemyWaveState = WaveState.Sleeping;
        private CheckpointsManager checkpointsManager;
        private CollisionsChecker collisionsChecker = new CollisionsChecker();

        internal ActiveActorsStorage ActiveActors { get; private set; }
        internal String CurrentCheckpoint { get; private set; }
        internal Single SecondsPassedAfterLevelEnd { get; private set; } = 0;

        internal Boolean Lost => !ActiveActors.Player.IsAlive();
        internal Boolean Won => !Lost && currentEnemyWaveNumber >= wavesAmount;
        internal Boolean LevelEnded => Won || Lost;

        internal LevelState(ActiveActorsStorage activeActors, ActorsInitializer actorsInitializer, 
            CheckpointsManager checkpointsManager, Int32 wavesAmount, String startCheckpoint)
        {
            this.ActiveActors = activeActors;
            this.actorsInitializer = actorsInitializer;
            this.checkpointsManager = checkpointsManager;
            this.wavesAmount = wavesAmount;
            checkpointsManager.InitializeCheckpoints();
            TrySwitchCheckpoint();
            activeActors.InitializeActorsOnLevelStart(actorsInitializer, checkpointsManager, startCheckpoint);
            currentEnemyWaveNumber = checkpointsManager.GetStartWave(startCheckpoint);
        }

        internal void Update(Single elapsedSeconds)
        {
            ActiveActors.Update();
            if (LevelEnded)
                SecondsPassedAfterLevelEnd += elapsedSeconds;
            else
            {
                if (currentEnemyWaveState == WaveState.Sleeping)
                    SleepingWaveCheck();
                if (currentEnemyWaveState == WaveState.Triggered)
                    TriggeredWaveCheck();
            }
        }

        private void SleepingWaveCheck()
        {
            if (collisionsChecker.Collides(ActiveActors.Player.GetCurrentHitbox(), ActiveActors.CurrentWaveStartRegion))
            {
                currentEnemyWaveState = WaveState.Triggered;
                TrySwitchCheckpoint();
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
                if (!LevelEnded)
                    ActiveActors.SwitchStartRegion(actorsInitializer, currentEnemyWaveNumber);
            }
        }

        private void TrySwitchCheckpoint()
        {
            String possibleCheckpoint = checkpointsManager.CheckForCheckpoint(currentEnemyWaveNumber);
            if (possibleCheckpoint != null)
                CurrentCheckpoint = possibleCheckpoint;
        }

        private enum WaveState { Sleeping, Triggered }
    }
}
