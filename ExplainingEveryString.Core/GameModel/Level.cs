﻿using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private LevelProgress levelProgress { get; set; }
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

            ActorsInitializer actorsInitializer = new ActorsInitializer(map, factory, levelData);
            ActiveActorsStorage activeActors = new ActiveActorsStorage();
            CheckpointsManager checkpointsManager = new CheckpointsManager(map, levelData);
            this.levelState = new LevelState(activeActors, actorsInitializer, checkpointsManager, 
                levelData.EnemyWaves.Length, levelProgress.CurrentCheckPoint);

            this.collisionsController = new CollisionsController(activeActors);
        }

        internal void Update(Single elapsedSeconds)
        {
            Boolean levelEndedBeforeUpdate = levelState.LevelIsEnded;
            foreach (IUpdatable updatable in levelState.ActiveActors.GetObjectsToUpdate())
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
            Boolean checkpointReached = levelProgress.CurrentCheckPoint != levelState.CurrentCheckpoint;
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
            IEnumerable<EpicEventArgs> result = epicEventsHappened;
            epicEventsHappened = new List<EpicEventArgs>();
            return result;
        }

        internal void PlayerShoot(Object sender, ShootEventArgs args)
        {
            Bullet bullet = args.Bullet;
            bullet.Update(args.FirstUpdateTime);
            levelState.ActiveActors.PlayerBullets.Add(bullet);
        }

        internal void EnemyShoot(Object sender, ShootEventArgs args)
        {
            Bullet bullet = args.Bullet;
            bullet.Update(args.FirstUpdateTime);
            levelState.ActiveActors.EnemyBullets.Add(bullet);
        }

        internal void EpicEventOccured(Object sender, EpicEventArgs epicEvent)
        {
            epicEventsHappened.Add(epicEvent);
        }

        internal InterfaceInfo GetInterfaceInfo(Camera camera)
        {
            return new InterfaceInfo
            {
                Health = levelState.ActiveActors.Player.HitPoints,
                MaxHealth = levelState.ActiveActors.Player.MaxHitPoints,
                GameTime = levelProgress.GameTime,
                Enemies = levelState.ActiveActors.Enemies
                            .Where(e => camera.IsVisibleOnScreen(e)).OfType<IInterfaceAccessable>()
                            .Where(e => e.ShowInterfaceInfo).Select(e => GetInterfaceInfo(e, camera)).ToList()
            };
        }

        private EnemyInterfaceInfo GetInterfaceInfo(IInterfaceAccessable interfaceAccessable, Camera camera)
        {
            return new EnemyInterfaceInfo
            {
                Health = interfaceAccessable.HitPoints,
                MaxHealth = interfaceAccessable.MaxHitPoints,
                PositionOnScreen = camera.PositionOnScreen(interfaceAccessable)
            };
        }

        private void PlanLevelEndDelay()
        {
            Single levelEndDelay = levelState.Won ? wonDelay : defeatDelay;
            TimersComponent.Instance.ScheduleEvent(levelEndDelay, () => levelEndDelayPassed = true);
        }
    }

    internal class CheckpointReachedEventArgs : EventArgs
    {
        internal LevelProgress LevelProgress { get; set; }
    }
}
