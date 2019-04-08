using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal delegate void GameLost(Object sender, EventArgs e);

    internal class Level
    {
        private Player player;
        private List<IGameObject> enemies;
        private List<IGameObject> walls;
        private List<PlayerBullet> playerBullets = new List<PlayerBullet>();
        private GameObjectsFactory factory;

        internal Vector2 PlayerPosition => player.Position;
        internal event GameLost Lost;

        internal Level(GameObjectsFactory factory)
        {
            this.factory = factory;
            factory.Level = this;
            InitializeGameObjects();
        }

        internal void Update(Single elapsedSeconds)
        {
            foreach (PlayerBullet playerBullet in playerBullets)
            {
                playerBullet.Update(elapsedSeconds);
            }
            player.Update(elapsedSeconds);
            foreach (IUpdatable enemy in enemies.OfType<IUpdatable>())
            {
                enemy.Update(elapsedSeconds);
            }
            CheckCollisions();
            SendDeadToHeaven();
        }

        private void CheckCollisions()
        {
            CollisionsChecker collisionsChecker = new CollisionsChecker();
            foreach (ICrashable crashable in enemies.OfType<ICrashable>())
            {
                if (collisionsChecker.Collides(crashable.GetCurrentHitbox(), player.GetCurrentHitbox()))
                {
                    crashable.Destroy();
                    player.TakeDamage(crashable.CollisionDamage);
                }
            }

            Hitbox oldHitbox = player.GetOldHitbox();
            foreach (ICollidable wall in walls.Cast<ICollidable>())
            {
                Vector2? wallCorrection = null;
                collisionsChecker.TryToBypassWall(oldHitbox, player.GetCurrentHitbox(),
                    wall.GetCurrentHitbox(), out wallCorrection);
                if (wallCorrection != null)
                    player.Position = wallCorrection.Value;
            }

            foreach (PlayerBullet playerBullet in playerBullets)
            {
                foreach (ICollidable collidable in enemies.Concat(walls).OfType<ICollidable>())
                {
                    if (collisionsChecker.Collides(collidable.GetCurrentHitbox(), playerBullet.OldPosition, playerBullet.Position))
                    {
                        if (collidable is ITouchableByBullets)
                            (collidable as ITouchableByBullets).TakeDamage(playerBullet.Damage);
                        playerBullet.RegisterCollision();
                    }
                }
            }
        }

        private void SendDeadToHeaven()
        {
            if (!player.IsAlive())
                Lost?.Invoke(this, EventArgs.Empty);
            playerBullets = playerBullets.Where(playerBullet => playerBullet.IsAlive()).ToList();
            enemies = enemies.Where(mine => mine.IsAlive()).ToList();
        }

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return walls.Concat(new List<IDisplayble> { player }).Concat(enemies).Concat(playerBullets);
        }

        private void InitializeGameObjects()
        {
            player = factory.Construct<Player, PlayerBlueprint>(new Vector2(0, 0));
            player.Weapon.Shoot += PlayerShoot;
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
                new Vector2(-256, -288),
                new Vector2(-128, -256),
                new Vector2(-96, -256),
                new Vector2(-64, -256)
            };

            enemies = new List<IGameObject>();
            enemies.AddRange(factory.Construct<Mine, EnemyBlueprint>(minePositions));
            enemies.AddRange(factory.Construct<Hunter, HunterBlueprint>(huntersPositions));
            walls = new List<IGameObject>();
            walls.AddRange(factory.Construct<Wall, Blueprint>("LittleWall", littleWallsPositions));
            walls.AddRange(factory.Construct<Wall, Blueprint>("MiddleWall", middleWallsPositions));
        }

        private void PlayerShoot(Object sender, PlayerShootEventArgs args)
        {
            PlayerBullet playerBullet = args.PlayerBullet;
            playerBullet.Update(args.FirstUpdateTime);
            playerBullets.Add(playerBullet);
        }
    }
}
