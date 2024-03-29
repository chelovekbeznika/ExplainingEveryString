﻿using ExplainingEveryString.Core.Collisions;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Core.GameModel.Weaponry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActiveActorsStorage
    {
        internal Player Player { get; private set; }
        
        internal List<Bullet> PlayerBullets { get; private set; }
        internal List<Bullet> EnemyBullets { get; private set; }
        internal Hitbox CurrentWaveStartRegion { get; private set; }
        internal List<IEnemy> Enemies => currentWaveEnemies
            .Concat(avengers)
            .Concat(enemySpawners.SelectMany(aes => aes.SpawnedEnemies))
            .Distinct().ToList();
        internal List<IEnemy> ShowAsBossesInInterface { get; private set; } = null;
        internal List<IChangeableActor> ChangeableActors => Enemies.OfType<IChangeableActor>().ToList();
      
        private List<IActor> obstacles;
        private List<Door> doors;
        private ICollidable[] walls;
        private Dictionary<String, List<ICollidable>> wallsPerSectors = new Dictionary<String, List<ICollidable>>();
        private List<IEnemy> currentWaveEnemies = new List<IEnemy>();
        private List<IEnemy> avengers = new List<IEnemy>();
        private List<ISpawnedActorsController> enemySpawners = new List<ISpawnedActorsController>();
        private List<ISpawnedActorsController> freshEnemySpawners = new List<ISpawnedActorsController>();
        private Int32 maxEnemiesAtOnce;
        private Queue<IEnemy> enemiesQueue = new Queue<IEnemy>();

        internal ActiveActorsStorage()
        {
        }

        internal Boolean CurrentEnemyWaveDestroyed => !Enemies.Any() && enemiesQueue.Count == 0;

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return obstacles
                .Concat(doors)
                .Concat(Player.IsAlive() ? new List<IDisplayble> { Player } : Enumerable.Empty<IDisplayble>())
                .Concat(Enemies)
                .Concat(EnemyBullets)
                .Concat(PlayerBullets);
        }

        internal IEnumerable<IUpdateable> GetObjectsToUpdate()
        {
            return PlayerBullets
                .Concat(EnemyBullets)
                .Concat(Player.IsAlive() ? new List<IUpdateable> { Player } : Enumerable.Empty<IUpdateable>())
                .Concat(enemySpawners.OfType<IUpdateable>())
                .Concat(Enemies.OfType<IUpdateable>())
                .Concat(obstacles.OfType<IUpdateable>())
                .Concat(doors.OfType<IUpdateable>());
        }

        internal IEnumerable<ICollidable> GetWalls() => obstacles.Concat(doors).OfType<ICollidable>().Concat(walls);

        internal IEnumerable<ICollidable> GetWalls(String sector) => wallsPerSectors.ContainsKey(sector) ? wallsPerSectors[sector] : new List<ICollidable>();

        internal IEnumerable<ICollidable> GetWalls(IEnumerable<String> sectors) => sectors.SelectMany(s => GetWalls(s)).Distinct();

        internal void Update()
        {
            enemySpawners.AddRange(freshEnemySpawners);
            freshEnemySpawners.Clear();
            SendDeadToHeaven();
            while (enemiesQueue.Count > 0 && currentWaveEnemies.Count < maxEnemiesAtOnce)
            {
                var enemy = enemiesQueue.Dequeue();
                currentWaveEnemies.Add(enemy);
                ProcessEnemyArrival(enemy);
            }
        }

        private void ProcessEnemyArrival(IEnemy enemy)
        {
            enemy.EnemyBehaviorChanged += EnemyBehaviorChanged;
            if (enemy.SpawnedActorsController != null)
            {
                freshEnemySpawners.Add(enemy.SpawnedActorsController);
                enemy.SpawnedActorsController.EnemySpawned += (sender, args) => ProcessEnemyArrival(args.Enemy);
            } 
        }

        private void EnemyBehaviorChanged(Object sender, EnemyBehaviorChangedEventArgs args)
        {
            var oldSpawner = args.OldSpawner;
            var newSpawner = args.NewSpawner;
            if (oldSpawner != null)
                oldSpawner.TurnOff();
            if (newSpawner != null)
            {
                if (!enemySpawners.Contains(newSpawner))
                    enemySpawners.Add(newSpawner);
                newSpawner.TurnOn();
            }
        }

        private void SendDeadToHeaven()
        {
            PlayerBullets = PlayerBullets.Where(bullet => bullet.IsAlive()).ToList();
            EnemyBullets = EnemyBullets.Where(bullet => bullet.IsAlive()).ToList();
            var sortedDoors = doors.ToLookup(door => door.IsAlive());
            doors = sortedDoors[true].ToList();
            foreach (var openedDoor in sortedDoors[false])
                foreach (var sector in wallsPerSectors.Keys)
                    wallsPerSectors[sector].Remove(openedDoor);

            currentWaveEnemies = EnemiesDeathProcessor.DivideAliveAndDead(currentWaveEnemies, avengers);
            var bossAvengers = new List<IEnemy>();
            ShowAsBossesInInterface = EnemiesDeathProcessor.DivideAliveAndDead(ShowAsBossesInInterface, bossAvengers);
            bossAvengers.ForEach(boss => enemiesQueue.Enqueue(boss));
            ShowAsBossesInInterface?.Concat(bossAvengers);
            foreach (var spawnedActorsController in enemySpawners)
                spawnedActorsController.DivideAliveAndDead(avengers);
            avengers = EnemiesDeathProcessor.DivideAliveAndDead(avengers, avengers);
        }

        internal void InitializeActorsOnLevelStart(ActorsInitializer actorsInitializer, 
            CheckpointsManager checkpointsManager, String startCheckpoint)
        {
            var startWave = checkpointsManager.GetStartWave(startCheckpoint);

            Player = actorsInitializer.InitializePlayer(this, checkpointsManager, startCheckpoint);
            obstacles = actorsInitializer.InitializeObstacles();
            walls = actorsInitializer.InitializeWalls();
            doors = actorsInitializer.InitializeCommonDoors(startWave);
            SortWallsBySectors(GetWalls());

            SwitchStartRegion(actorsInitializer, startWave);
            PlayerBullets = new List<Bullet>();
            EnemyBullets = new List<Bullet>();
        }

        internal void EndWave(Int32 waveNumber)
        {
            foreach (var door in doors.Where(d => d.OpeningWaveNumber == waveNumber))
                door.Open();
        }

        internal void SwitchStartRegion(ActorsInitializer actorsInitializer, Int32 waveNumber)
        {
            CurrentWaveStartRegion = actorsInitializer.InitializeStartRegion(waveNumber);
        }

        internal void StartEnemyWave(ActorsInitializer actorsInitializer, Int32 waveNumber)
        {
            enemiesQueue = actorsInitializer.InitializeEnemies(waveNumber);
            ShowAsBossesInInterface = actorsInitializer.InitializeBosses(waveNumber)?.ToList();
            if (ShowAsBossesInInterface != null)
                ShowAsBossesInInterface.ForEach((boss) => enemiesQueue.Enqueue(boss));
            maxEnemiesAtOnce = actorsInitializer.MaxEnemiesAtOnce(waveNumber);
            var newDoors = actorsInitializer.InitializeClosingDoors(waveNumber);
            doors.AddRange(newDoors);
            SortWallsBySectors(newDoors);
        }

        private void SortWallsBySectors(IEnumerable<ICollidable> walls)
        {
            foreach (var wall in walls)
            {
                foreach (var sector in SpatialPartioningHelper.GetSectors(wall.GetCurrentHitbox()))
                {
                    if (!wallsPerSectors.ContainsKey(sector))
                        wallsPerSectors.Add(sector, new List<ICollidable>());
                    wallsPerSectors[sector].Add(wall);
                }
            }
        }
    }
}
