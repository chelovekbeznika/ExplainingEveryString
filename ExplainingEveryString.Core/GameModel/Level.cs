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
        private GameObjectsFactory factory;

        internal List<IDrawable> ObjectsToDraw => new List<IDrawable> { player }.Concat(mines).ToList();
        internal Vector2 PlayerPosition => player.Position;
        internal event GameLost Lost;

        internal Level(GameObjectsFactory factory)
        {
            this.factory = factory;
            InitializeGameObjects();
        }

        internal void Update(Single elapsedSeconds)
        {
            player.Move(elapsedSeconds);
            CheckCollisions();
        }

        private void InitializeGameObjects()
        {
            player = factory.Construct<Player, PlayerBlueprint>(new Vector2(0, 0));
            Vector2[] minePositions = 
                new Vector2[] { new Vector2(100, 100), new Vector2(200, 200), new Vector2(-300, -150) };

            mines = factory.Construct<Mine, MineBlueprint>(minePositions);
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
        }
    }
}
