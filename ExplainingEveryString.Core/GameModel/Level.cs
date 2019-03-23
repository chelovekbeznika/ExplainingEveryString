using ExplainingEveryString.Core.Blueprints;
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
        private List<Mine> mines;
        private List<PlayerBullet> playerBullets = new List<PlayerBullet>();
        private GameObjectsFactory factory;

        internal Vector2 PlayerPosition => player.Position;
        internal event GameLost Lost;

        internal Level(GameObjectsFactory factory)
        {
            this.factory = factory;
            InitializeGameObjects();
        }

        internal void Update(Single elapsedSeconds)
        {
            foreach (PlayerBullet playerBullet in playerBullets)
            {
                playerBullet.Update(elapsedSeconds);
            }
            player.Update(elapsedSeconds);
            CheckCollisions();
            SendDeadToHeaven();
        }

        private void CheckCollisions()
        {
            CollisionsChecker collisionsChecker = new CollisionsChecker();
            foreach (Mine mine in mines)
            {
                if (collisionsChecker.Collides(mine.GetHitbox(), player.GetHitbox()))
                {
                    Lost?.Invoke(this, EventArgs.Empty);
                }
            }
            foreach (PlayerBullet playerBullet in playerBullets)
            {
                foreach (Mine mine in mines)
                {
                    if (collisionsChecker.Collides(mine.GetHitbox(), playerBullet.Position))
                    {
                        mine.TakeDamage();
                        playerBullet.RegisterCollision();
                    }
                }
            }
        }

        private void SendDeadToHeaven()
        {
            playerBullets = playerBullets.Where(playerBullet => playerBullet.IsAlive()).ToList();
            mines = mines.Where(mine => mine.IsAlive()).ToList();
        }

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return new List<IDisplayble> { player }.Concat(mines).Concat(playerBullets);
        }

        private void InitializeGameObjects()
        {
            player = factory.Construct<Player, PlayerBlueprint>(new Vector2(0, 0));
            player.Weapon.Shoot += PlayerShoot;
            Vector2[] minePositions = 
                new Vector2[] { new Vector2(100, 100), new Vector2(200, 200), new Vector2(-300, -150) };

            mines = factory.Construct<Mine, MineBlueprint>(minePositions);
        }

        private void PlayerShoot(Object sender, PlayerShootEventArgs args)
        {
            PlayerBullet playerBullet = args.PlayerBullet;
            playerBullet.Update(args.FirstUpdateTime);
            playerBullets.Add(playerBullet);
        }
    }
}
