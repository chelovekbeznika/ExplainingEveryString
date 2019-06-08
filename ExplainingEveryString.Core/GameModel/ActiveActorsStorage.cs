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
        internal List<IActor> Enemies { get; private set; }
        internal List<Bullet> PlayerBullets { get; private set; }
        internal List<Bullet> EnemyBullets { get; private set; }
        
        private List<IActor> walls;
        private ICollidable[] tileWalls;

        internal ActiveActorsStorage()
        {
        }

        internal Boolean CurrentEnemyWaveDestroyed => !Enemies.Any();

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return walls
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
                .Concat(walls.OfType<IUpdatable>());
        }

        internal IEnumerable<ICollidable> GetWalls()
        {
            return walls.OfType<ICollidable>().Concat(tileWalls);
        }

        internal void SendDeadToHeaven()
        {
            PlayerBullets = PlayerBullets.Where(bullet => bullet.IsAlive()).ToList();
            EnemyBullets = EnemyBullets.Where(bullet => bullet.IsAlive()).ToList();
            Enemies = Enemies.Where(mine => mine.IsAlive()).ToList();
        }

        internal void InitializeActorsOnLevelStart(ActorsInitializer actorsInitializer)
        {
            Player = actorsInitializer.InitializePlayer();
            walls = actorsInitializer.InitializeWalls();
            tileWalls = actorsInitializer.InitializeTileWalls();
            Enemies = actorsInitializer.InitializeEnemies(0);
            PlayerBullets = new List<Bullet>();
            EnemyBullets = new List<Bullet>();
        }

        internal void GetNextEnemyWave(ActorsInitializer actorsInitializer, Int32 waveNumber)
        {
            Enemies = actorsInitializer.InitializeEnemies(waveNumber);
        }
    }
}
