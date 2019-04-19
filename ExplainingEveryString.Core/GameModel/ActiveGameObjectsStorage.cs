using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActiveGameObjectsStorage
    {
        internal Player Player { get; private set; }
        internal List<IGameObject> Enemies { get; private set; }
        internal List<IGameObject> Walls { get; private set; }
        internal List<Bullet> PlayerBullets { get; private set; }
        private GameObjectsFactory factory;
        private LevelData levelData;

        internal ActiveGameObjectsStorage(GameObjectsFactory factory, LevelData levelData)
        {
            this.factory = factory;
            this.levelData = levelData;
        }

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return Walls
                .Concat(new List<IDisplayble> { Player })
                .Concat(Enemies)
                .Concat(PlayerBullets);
        }

        internal IEnumerable<IUpdatable> GetObjectsToUpdate()
        {
            return PlayerBullets
                .Concat(new List<IUpdatable> { Player })
                .Concat(Enemies.OfType<IUpdatable>())
                .Concat(Walls.OfType<IUpdatable>());
        }

        internal void SendDeadToHeaven()
        {
            PlayerBullets = PlayerBullets.Where(playerBullet => playerBullet.IsAlive()).ToList();
            Enemies = Enemies.Where(mine => mine.IsAlive()).ToList();
        }

        internal void InitializeGameObjects()
        {
            Player = factory.ConstructPlayer(levelData.PlayerPosition);

            Walls = new List<IGameObject>();
            foreach (String wallType in levelData.WallsPositions.Keys)
            {
                List<Vector2> wallPositions = levelData.WallsPositions[wallType];
                Walls.AddRange(factory.ConstructWalls(wallType, wallPositions));
            }

            Enemies = new List<IGameObject>();
            foreach (String enemyType in levelData.EnemiesPositions.Keys)
            {
                List<Vector2> enemiesPositions = levelData.EnemiesPositions[enemyType];
                Enemies.AddRange(factory.ConstructEnemies(enemyType, enemiesPositions));
            }

            PlayerBullets = new List<Bullet>();
        }
    }
}
