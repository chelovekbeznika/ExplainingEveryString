using ExplainingEveryString.Core.Displaying;
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
            .Concat(activeEnemySpawners.SelectMany(aes => aes.SpawnedEnemies)).ToList();
      
        private List<IActor> obstacles;
        private List<Door> doors;
        private ICollidable[] walls;
        private List<IEnemy> currentWaveEnemies = new List<IEnemy>();
        private List<IEnemy> avengers = new List<IEnemy>();
        private List<SpawnedActorsController> activeEnemySpawners = new List<SpawnedActorsController>();
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
                .Concat(activeEnemySpawners.OfType<IUpdateable>())
                .Concat(Enemies.OfType<IUpdateable>())
                .Concat(obstacles.OfType<IUpdateable>())
                .Concat(doors.OfType<IUpdateable>());
        }

        internal IEnumerable<ICollidable> GetWalls()
        {
            return obstacles.Concat(doors).OfType<ICollidable>().Concat(walls);
        }

        internal void Update()
        {
            SendDeadToHeaven();
            while (enemiesQueue.Count > 0 && currentWaveEnemies.Count < maxEnemiesAtOnce)
            {
                IEnemy enemy = enemiesQueue.Dequeue();
                currentWaveEnemies.Add(enemy);
                if (enemy.SpawnedActors != null)
                    activeEnemySpawners.Add(enemy.SpawnedActors);
            }
        }

        private void SendDeadToHeaven()
        {
            PlayerBullets = PlayerBullets.Where(bullet => bullet.IsAlive()).ToList();
            EnemyBullets = EnemyBullets.Where(bullet => bullet.IsAlive()).ToList();
            doors = doors.Where(door => door.IsAlive()).ToList();

            currentWaveEnemies = EnemyDeathProcessor.SendDeadToHeaven(currentWaveEnemies, avengers);
            foreach (SpawnedActorsController spawnedActorsController in activeEnemySpawners)
                spawnedActorsController.SendDeadToHeaven(avengers);
            avengers = EnemyDeathProcessor.SendDeadToHeaven(avengers, avengers);
        }

        internal void InitializeActorsOnLevelStart(ActorsInitializer actorsInitializer, 
            CheckpointsManager checkpointsManager, String startCheckpoint)
        {
            Int32 startWave = checkpointsManager.GetStartWave(startCheckpoint);

            Player = actorsInitializer.InitializePlayer(checkpointsManager.GetPlayerPosition(startCheckpoint));
            obstacles = actorsInitializer.InitializeObstacles();
            walls = actorsInitializer.InitializeWalls();
            doors = actorsInitializer.InitializeCommonDoors(startWave);

            SwitchStartRegion(actorsInitializer, startWave);
            PlayerBullets = new List<Bullet>();
            EnemyBullets = new List<Bullet>();
        }

        internal void EndWave(Int32 waveNumber)
        {
            foreach (Door door in doors.Where(d => d.OpeningWaveNumber == waveNumber))
                door.Open();
        }

        internal void SwitchStartRegion(ActorsInitializer actorsInitializer, Int32 waveNumber)
        {
            CurrentWaveStartRegion = actorsInitializer.InitializeStartRegion(waveNumber);
        }

        internal void StartEnemyWave(ActorsInitializer actorsInitializer, Int32 waveNumber)
        {
            (currentWaveEnemies, enemiesQueue) = actorsInitializer.InitializeEnemies(waveNumber);
            activeEnemySpawners = currentWaveEnemies
                .Where(e => e.SpawnedActors != null)
                .Select(e => e.SpawnedActors).ToList();
            maxEnemiesAtOnce = currentWaveEnemies.Count;
            doors.AddRange(actorsInitializer.InitializeClosingDoors(waveNumber));
        }
    }
}
