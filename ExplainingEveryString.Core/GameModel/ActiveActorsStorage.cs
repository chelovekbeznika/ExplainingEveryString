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
        internal List<IActor> Enemies => currentWaveEnemies;
        internal List<Bullet> PlayerBullets { get; private set; }
        internal List<Bullet> EnemyBullets { get; private set; }
        internal Hitbox CurrentWaveStartRegion { get; private set; }
        
        private List<IActor> walls;
        private List<Door> doors;
        private ICollidable[] tileWalls;
        private List<IActor> currentWaveEnemies = new List<IActor>();
        private Int32 maxEnemiesAtOnce;
        private Queue<IActor> enemiesQueue = new Queue<IActor>();

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
                currentWaveEnemies.Add(enemiesQueue.Dequeue());
            }
        }

        private void SendDeadToHeaven()
        {
            PlayerBullets = PlayerBullets.Where(bullet => bullet.IsAlive()).ToList();
            EnemyBullets = EnemyBullets.Where(bullet => bullet.IsAlive()).ToList();
            currentWaveEnemies = currentWaveEnemies.Where(enemy => enemy.IsAlive()).ToList();
            doors = doors.Where(door => door.IsAlive()).ToList();
        }

        internal void InitializeActorsOnLevelStart(ActorsInitializer actorsInitializer)
        {
            Player = actorsInitializer.InitializePlayer();
            walls = actorsInitializer.InitializeWalls();
            tileWalls = actorsInitializer.InitializeTileWalls();
            doors = actorsInitializer.InitializeDoors();

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

        internal void GetEnemiesFromWave(ActorsInitializer actorsInitializer, Int32 waveNumber)
        {
            (currentWaveEnemies, enemiesQueue) = actorsInitializer.InitializeEnemies(waveNumber);
            maxEnemiesAtOnce = currentWaveEnemies.Count;
        }
    }
}
