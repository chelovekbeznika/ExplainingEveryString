using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
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
            .Concat(activeEnemySpawners.SelectMany(aes => aes.SpawnedEnemies)).ToList();
      
        private List<IActor> walls;
        private List<Door> doors;
        private ICollidable[] tileWalls;
        private List<IEnemy> currentWaveEnemies = new List<IEnemy>();
        private List<SpawnedActorsController> activeEnemySpawners = new List<SpawnedActorsController>();
        private Int32 maxEnemiesAtOnce;
        private Queue<IEnemy> enemiesQueue = new Queue<IEnemy>();

        internal ActiveActorsStorage()
        {
        }

        internal Boolean CurrentEnemyWaveDestroyed => !Enemies.Any() && enemiesQueue.Count == 0;

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return walls
                .Concat(doors)
                .Concat(new List<IDisplayble> { Player })
                .Concat(Enemies)
                .Concat(EnemyBullets)
                .Concat(PlayerBullets);
        }

        internal IEnumerable<IUpdatable> GetObjectsToUpdate()
        {
            return PlayerBullets
                .Concat(EnemyBullets)
                .Concat(new List<IUpdatable> { Player })
                .Concat(activeEnemySpawners.OfType<IUpdatable>())
                .Concat(Enemies.OfType<IUpdatable>())
                .Concat(walls.OfType<IUpdatable>())
                .Concat(doors.OfType<IUpdatable>());
        }

        internal IEnumerable<ICollidable> GetWalls()
        {
            return walls.Concat(doors).OfType<ICollidable>().Concat(tileWalls);
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
            currentWaveEnemies = currentWaveEnemies.Where(enemy => enemy.IsAlive()).ToList();
            doors = doors.Where(door => door.IsAlive()).ToList();
            foreach (SpawnedActorsController spawnedActorsController in activeEnemySpawners)
                spawnedActorsController.SendDeadToHeaven();
        }

        internal void InitializeActorsOnLevelStart(ActorsInitializer actorsInitializer)
        {
            Player = actorsInitializer.InitializePlayer();
            walls = actorsInitializer.InitializeWalls();
            tileWalls = actorsInitializer.InitializeTileWalls();
            doors = actorsInitializer.InitializeCommonDoors();

            SwitchStartRegion(actorsInitializer, 0);
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
