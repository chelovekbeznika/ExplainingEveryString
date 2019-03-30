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
        private List<Enemy<EnemyBlueprint>> enemies;
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
            foreach (Enemy<EnemyBlueprint> enemy in enemies)
            {
                enemy.Update(elapsedSeconds);
            }
            CheckCollisions();
            SendDeadToHeaven();
        }

        private void CheckCollisions()
        {
            CollisionsChecker collisionsChecker = new CollisionsChecker();
            foreach (Enemy<EnemyBlueprint> enemy in enemies)
            {
                if (collisionsChecker.Collides(enemy.GetHitbox(), player.GetHitbox()))
                {
                    enemy.Destroy();
                    player.TakeDamage(enemy.CollisionDamage);
                }
            }
            foreach (PlayerBullet playerBullet in playerBullets)
            {
                foreach (Enemy<EnemyBlueprint> enemy in enemies)
                {
                    if (collisionsChecker.Collides(enemy.GetHitbox(), playerBullet.OldPosition, playerBullet.Position))
                    {
                        enemy.TakeDamage(playerBullet.Damage);
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
            return new List<IDisplayble> { player }.Concat(enemies).Concat(playerBullets);
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

            enemies = new List<Enemy<EnemyBlueprint>>();
            enemies.AddRange(factory.Construct<Mine, EnemyBlueprint>(minePositions));
            enemies.AddRange(factory.Construct<Hunter, EnemyBlueprint>(huntersPositions));
        }

        private void PlayerShoot(Object sender, PlayerShootEventArgs args)
        {
            PlayerBullet playerBullet = args.PlayerBullet;
            playerBullet.Update(args.FirstUpdateTime);
            playerBullets.Add(playerBullet);
        }
    }
}
