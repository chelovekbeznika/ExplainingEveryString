using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
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

        private ActorsFactory actorsFactory;
        private TileWallsFactory tileWallsFactory;
        private List<IActor> walls;
        private List<TileWall> tileWalls;
        private LevelData levelData;

        internal ActiveActorsStorage(ActorsFactory factory, TileWallsFactory tileWallsFactory, LevelData levelData)
        {
            this.actorsFactory = factory;
            this.tileWallsFactory = tileWallsFactory;
            this.levelData = levelData;
        }

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
            return walls.OfType<ICollidable>().Concat(tileWalls.Cast<ICollidable>());
        }

        internal void SendDeadToHeaven()
        {
            PlayerBullets = PlayerBullets.Where(bullet => bullet.IsAlive()).ToList();
            EnemyBullets = EnemyBullets.Where(bullet => bullet.IsAlive()).ToList();
            Enemies = Enemies.Where(mine => mine.IsAlive()).ToList();
        }

        internal void InitializeActors()
        {
            Player = actorsFactory.ConstructPlayer(levelData.PlayerPosition);
            InitializeWalls();
            InitializeTileWalls();
            InitializeEnemies();
            PlayerBullets = new List<Bullet>();
            EnemyBullets = new List<Bullet>();
        }

        private void InitializeWalls()
        {
            walls = new List<IActor>();
            foreach (String wallType in levelData.WallsPositions.Keys)
            {
                List<Vector2> wallPositions = levelData.WallsPositions[wallType];
                walls.AddRange(actorsFactory.ConstructWalls(wallType, wallPositions));
            }
        }

        private void InitializeTileWalls()
        {
            tileWalls = tileWallsFactory.ConstructTileWalls().ToList();
        }

        private void InitializeEnemies()
        {
            Enemies = new List<IActor>();
            foreach (String enemyType in levelData.EnemiesPositions.Keys)
            {
                List<ActorStartInfo> enemiesPositions = levelData.EnemiesPositions[enemyType];
                Enemies.AddRange(actorsFactory.ConstructEnemies(enemyType, enemiesPositions));
            }
        }
    }
}
