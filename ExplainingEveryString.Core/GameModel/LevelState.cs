using ExplainingEveryString.Core.Collisions;
using ExplainingEveryString.Core.GameModel.Movement;
using ExplainingEveryString.Core.Timers;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class LevelState : IUpdateable
    {
        private const Single waveDelay = 2;

        private Int32 currentEnemyWaveNumber;
        private readonly Int32 wavesAmount;
        private readonly ActorsInitializer actorsInitializer;
        private readonly CheckpointsManager checkpointsManager;
        private readonly ActorChangingEventsProcessor actorChangingEventsProcessor;
        private WaveState currentEnemyWaveState = WaveState.Sleeping;
        private MoveTargetSelectorFactory moveTargetSelectorFactory;
        private CollisionsChecker collisionsChecker = new CollisionsChecker();

        internal ActiveActorsStorage ActiveActors { get; private set; }

        internal String CurrentCheckpoint { get; private set; }

        internal Boolean Lost => !ActiveActors.Player.IsAlive();
        internal Boolean Won => !Lost && currentEnemyWaveNumber >= wavesAmount;
        internal Boolean LevelIsEnded => Won || Lost;

        internal LevelState(ActiveActorsStorage activeActors, ActorsInitializer actorsInitializer, CheckpointsManager checkpointsManager, 
            Int32 wavesAmount, String startCheckpoint, MoveTargetSelectorFactory moveTargetSelectorFactory)
        {
            this.ActiveActors = activeActors;
            this.actorsInitializer = actorsInitializer;
            this.checkpointsManager = checkpointsManager;
            this.wavesAmount = wavesAmount;
            this.actorChangingEventsProcessor = new ActorChangingEventsProcessor(activeActors);
            this.moveTargetSelectorFactory = moveTargetSelectorFactory;
            checkpointsManager.InitializeCheckpoints();
            activeActors.InitializeActorsOnLevelStart(actorsInitializer, checkpointsManager, startCheckpoint);
            currentEnemyWaveNumber = checkpointsManager.GetStartWave(startCheckpoint);
            TrySwitchCheckpoint();
        }

        public void Update(Single elapsedSeconds)
        {
            ActiveActors.Update();
            actorChangingEventsProcessor.Update();
            if (!LevelIsEnded)
            {
                if (currentEnemyWaveState == WaveState.Sleeping)
                    SleepingWaveCheck();
                if (currentEnemyWaveState == WaveState.Triggered)
                    TriggeredWaveCheck();
            }
        }

        internal void RechargePlayer(Object sender, CheckpointReachedEventArgs e)
        {
            var playerArsenal = checkpointsManager.GetPlayerArsenal(e.LevelProgress.CurrentCheckPoint);
            var player = ActiveActors.Player;
            player.CheckpointRefresh(playerArsenal);
        }

        private void SleepingWaveCheck()
        {
            if (collisionsChecker.Collides(ActiveActors.Player.GetCurrentHitbox(), ActiveActors.CurrentWaveStartRegion))
            {
                currentEnemyWaveState = WaveState.Triggered;
                TrySwitchCheckpoint();
                moveTargetSelectorFactory.SwitchRoom(currentEnemyWaveNumber);
                ActiveActors.StartEnemyWave(actorsInitializer, currentEnemyWaveNumber);
            }
        }

        private void TriggeredWaveCheck()
        {
            if (ActiveActors.CurrentEnemyWaveDestroyed)
            {
                ActiveActors.EndWave(currentEnemyWaveNumber);
                currentEnemyWaveNumber += 1;
                currentEnemyWaveState = WaveState.Delay;
                if (!LevelIsEnded)
                {
                    ActiveActors.SwitchStartRegion(actorsInitializer, currentEnemyWaveNumber);
                    TimersComponent.Instance.ScheduleEvent(waveDelay, () => currentEnemyWaveState = WaveState.Sleeping);
                }
            }
        }

        private void TrySwitchCheckpoint()
        {
            var possibleCheckpoint = checkpointsManager.CheckForCheckpoint(currentEnemyWaveNumber);
            if (possibleCheckpoint != null)
                CurrentCheckpoint = possibleCheckpoint;
        }

        private enum WaveState { Delay, Sleeping, Triggered }
    }
}
