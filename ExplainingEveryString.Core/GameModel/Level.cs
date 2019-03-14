using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel
{
    internal delegate void GameLost(Object sender, EventArgs e);

    internal class Level
    {
        private Player player;
        private List<GameObject> gameObjects; 

        internal List<GameObject> GameObjects => gameObjects;
        internal Vector2 PlayerPosition => player.Position;
        internal event GameLost Lost;

        internal Level()
        {
            InitializeGameObjects();
        }

        internal void Update(Single elapsedSeconds)
        {
            player.Move(elapsedSeconds);
            CheckCollisions();
        }

        private void InitializeGameObjects()
        {
            player = new Player();
            gameObjects = new List<GameObject>() {
                player,
                new Mine(new Vector2(100, 100)),
                new Mine(new Vector2(200, 200)),
                new Mine(new Vector2(-300, -150))
            };
        }

        private void CheckCollisions()
        {
            CollisionsChecker collisionsChecker = new CollisionsChecker();
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject is Mine)
                {
                    if (collisionsChecker.Collides(gameObject.GetHitbox(), player.GetHitbox()))
                    {
                        Lost?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
