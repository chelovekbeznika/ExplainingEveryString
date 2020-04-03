using ExplainingEveryString.Core.Collisions;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Level
    {
        private const Single defeatDelay = 2;
        private const Single wonDelay = 5;

        internal event EventHandler<CheckpointReachedEventArgs> CheckpointReached;

        private LevelState levelState;
        private CollisionsController collisionsController;
        private List<EpicEventArgs> epicEventsHappened = new List<EpicEventArgs>();
        private LevelProgress levelProgress;
        private Boolean levelEndDelayPassed = false;

        internal PlayerInputFactory PlayerInputFactory { get; private set; }
        internal Player Player => levelState.ActiveActors.Player;
        internal Boolean Lost => levelState.Lost && levelEndDelayPassed;
        internal Boolean Won => levelState.Won && levelEndDelayPassed;


        internal Level(ActorsFactory factory, TileWrapper map, PlayerInputFactory playerInputFactory, 
            LevelData levelData, LevelProgress levelProgress)
        {
            this.levelProgress = levelProgress;
            this.PlayerInputFactory = playerInputFactory;
            factory.Level = this;

            var actorsInitializer = new ActorsInitializer(map, factory, levelData);
            var activeActors = new ActiveActorsStorage();
            var checkpointsManager = new CheckpointsManager(map, levelData);
            this.levelState = new LevelState(activeActors, actorsInitializer, checkpointsManager, 
                levelData.EnemyWaves.Length, levelProgress.CurrentCheckPoint);

            this.collisionsController = new CollisionsController(activeActors);
        }

        internal void Update(Single elapsedSeconds)
        {
            var levelEndedBeforeUpdate = levelState.LevelIsEnded;
            foreach (var updatable in levelState.ActiveActors.GetObjectsToUpdate())
                updatable.Update(elapsedSeconds);
            collisionsController.CheckCollisions();
            levelState.Update(elapsedSeconds);
            UpdateLevelProgress(elapsedSeconds);
            if (levelState.LevelIsEnded != levelEndedBeforeUpdate)
                PlanLevelEndDelay();
        }

        private void UpdateLevelProgress(Single elapsedSeconds)
        {
            if (!levelState.LevelIsEnded)
                levelProgress.GameTime += elapsedSeconds;
            var checkpointReached = levelProgress.CurrentCheckPoint != levelState.CurrentCheckpoint;
            levelProgress.CurrentCheckPoint = levelState.CurrentCheckpoint;
            if (checkpointReached)
                CheckpointReached?.Invoke(this, new CheckpointReachedEventArgs { LevelProgress = levelProgress });
        }

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return levelState.ActiveActors.GetObjectsToDraw();
        }

        internal IEnumerable<EpicEventArgs> CollectEpicEvents()
        {
            var result = epicEventsHappened;
            epicEventsHappened = new List<EpicEventArgs>();
            return result;
        }

        internal void PlayerShoot(Object sender, ShootEventArgs args)
        {
            var bullet = args.Bullet;
            bullet.Update(args.FirstUpdateTime);
            levelState.ActiveActors.PlayerBullets.Add(bullet);
        }

        internal void EnemyShoot(Object sender, ShootEventArgs args)
        {
            var bullet = args.Bullet;
            bullet.Update(args.FirstUpdateTime);
            levelState.ActiveActors.EnemyBullets.Add(bullet);
        }

        internal void EpicEventOccured(Object sender, EpicEventArgs epicEvent)
        {
            epicEventsHappened.Add(epicEvent);
        }

        internal InterfaceInfo GetInterfaceInfo(Camera camera)
        {
            return new InterfaceInfoExtractor().GetInterfaceInfo(camera, levelState.ActiveActors, levelProgress);
        }

        private void PlanLevelEndDelay()
        {
            var levelEndDelay = levelState.Won ? wonDelay : defeatDelay;
            TimersComponent.Instance.ScheduleEvent(levelEndDelay, () => levelEndDelayPassed = true);
        }
    }

    internal class CheckpointReachedEventArgs : EventArgs
    {
        internal LevelProgress LevelProgress { get; set; }
    }
}
