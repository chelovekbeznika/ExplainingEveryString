using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActiveGameObjectsStorage
    {
        internal Player Player { get; private set; }
        internal List<IGameObject> Enemies { get; private set; }
        internal List<IGameObject> Walls { get; private set; }
        internal List<PlayerBullet> PlayerBullets { get; private set; }
        private GameObjectsFactory factory;

        internal ActiveGameObjectsStorage(GameObjectsFactory factory)
        {
            this.factory = factory;
        }

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return Walls
                .Concat(new List<IDisplayble> { Player })
                .Concat(Enemies).Concat(PlayerBullets);
        }

        internal IEnumerable<IUpdatable> GetObjectsToUpdate()
        {
            return PlayerBullets
                .Concat(new List<IUpdatable> { Player })
                .Concat(Enemies.OfType<IUpdatable>());
        }

        internal void SendDeadToHeaven()
        {
            PlayerBullets = PlayerBullets.Where(playerBullet => playerBullet.IsAlive()).ToList();
            Enemies = Enemies.Where(mine => mine.IsAlive()).ToList();
        }

        internal void InitializeGameObjects()
        {
            Vector2 playerPosition = new Vector2(0, 0);
            Vector2[] minePositions = new Vector2[]
            {
                new Vector2(100, 100),
                new Vector2(200, 200),
                new Vector2(-300, -150),
                new Vector2(500, 250)
            };
            Vector2[] huntersPositions = new Vector2[]
            {
                new Vector2(300, 300),
                new Vector2(400, 300),
                new Vector2(500, 300)
            };
            Vector2[] littleWallsPositions = new Vector2[]
            {
                new Vector2(0, -200),
                new Vector2(0, -232),
                new Vector2(8, -264),
                new Vector2(8, -312)
            };
            Vector2[] middleWallsPositions = new Vector2[]
            {
                new Vector2(-256, -256),
                new Vector2(-288, -256),
                new Vector2(-256, -288),
                new Vector2(-320, -256),
                new Vector2(-320, -288),
                new Vector2(-128, -256),
                new Vector2(-96, -256),
                new Vector2(-64, -256)
            };

            Player = factory.Construct<Player, PlayerBlueprint>(playerPosition);
            Enemies = new List<IGameObject>();
            Enemies.AddRange(factory.Construct<Mine, EnemyBlueprint>(minePositions));
            Enemies.AddRange(factory.Construct<Hunter, HunterBlueprint>(huntersPositions));
            Walls = new List<IGameObject>();
            Walls.AddRange(factory.Construct<Wall, Blueprint>("LittleWall", littleWallsPositions));
            Walls.AddRange(factory.Construct<Wall, Blueprint>("MiddleWall", middleWallsPositions));
            PlayerBullets = new List<PlayerBullet>();
        }
    }
}
