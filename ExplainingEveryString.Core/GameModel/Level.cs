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
        private readonly Single delayBeforeLevelEnd = 2.0F;
        private LevelState levelState;
        private CollisionsController collisionsController;
        private List<EpicEventArgs> epicEventsHappened = new List<EpicEventArgs>();
        private Single gameTime = 0;

        internal PlayerInputFactory PlayerInputFactory { get; private set; }
        internal Player Player => levelState.ActiveActors.Player;
        internal String CurrentCheckpoint => levelState.CurrentCheckpoint;
        internal Boolean Lost => levelState.Lost && levelState.SecondsPassedAfterLevelEnd > delayBeforeLevelEnd;
        internal Boolean Won => levelState.Won && levelState.SecondsPassedAfterLevelEnd > delayBeforeLevelEnd;

        internal Level(ActorsFactory factory, TileWrapper map, PlayerInputFactory playerInputFactory, 
            LevelData levelData, String startCheckpoint)
        {
            this.PlayerInputFactory = playerInputFactory;
            factory.Level = this;
            ActorsInitializer actorsInitializer = new ActorsInitializer(map, factory, levelData);
            ActiveActorsStorage activeActors = new ActiveActorsStorage();
            CheckpointsManager checkpointsManager = new CheckpointsManager(map, levelData);
            this.levelState = new LevelState(activeActors, actorsInitializer, checkpointsManager, 
                levelData.EnemyWaves.Length, startCheckpoint);
            collisionsController = new CollisionsController(activeActors);
        }

        internal void Update(Single elapsedSeconds)
        {
            foreach (IUpdatable updatable in levelState.ActiveActors.GetObjectsToUpdate())
                updatable.Update(elapsedSeconds);
            collisionsController.CheckCollisions();
            levelState.Update(elapsedSeconds);
            if (!levelState.LevelEnded)
                gameTime += elapsedSeconds;
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
                GameTime = gameTime,
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
    }
}
