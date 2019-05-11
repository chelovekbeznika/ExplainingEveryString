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
        internal List<IActor> Walls { get; private set; }
        internal List<Bullet> PlayerBullets { get; private set; }
        internal List<Bullet> EnemyBullets { get; private set; }
        private ActorsFactory factory;
        private LevelData levelData;

        internal ActiveActorsStorage(ActorsFactory factory, LevelData levelData)
        {
            this.factory = factory;
            this.levelData = levelData;
        }

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return Walls
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
                .Concat(Walls.OfType<IUpdatable>());
        }

        internal void SendDeadToHeaven()
        {
            PlayerBullets = PlayerBullets.Where(bullet => bullet.IsAlive()).ToList();
            EnemyBullets = EnemyBullets.Where(bullet => bullet.IsAlive()).ToList();
            Enemies = Enemies.Where(mine => mine.IsAlive()).ToList();
        }

        internal void InitializeActors()
        {
            Player = factory.ConstructPlayer(levelData.PlayerPosition);

            Walls = new List<IActor>();
            foreach (String wallType in levelData.WallsPositions.Keys)
            {
                List<Vector2> wallPositions = levelData.WallsPositions[wallType];
                Walls.AddRange(factory.ConstructWalls(wallType, wallPositions));
            }

            Enemies = new List<IActor>();
            foreach (String enemyType in levelData.EnemiesPositions.Keys)
            {
                List<ActorStartPosition> enemiesPositions = levelData.EnemiesPositions[enemyType];
                Enemies.AddRange(factory.ConstructEnemies(enemyType, enemiesPositions));
            }

            PlayerBullets = new List<Bullet>();
            EnemyBullets = new List<Bullet>();
        }
    }
}
