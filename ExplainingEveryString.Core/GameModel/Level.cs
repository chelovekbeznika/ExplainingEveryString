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
        private ActiveGameObjectsStorage activeObjects;
        private CollisionsController collisionsController;
        private GameObjectsFactory factory;

        internal Vector2 PlayerPosition => activeObjects.Player.Position;
        internal event GameLost Lost;

        internal Level(GameObjectsFactory factory)
        {
            this.factory = factory;
            factory.Level = this;
            activeObjects = new ActiveGameObjectsStorage(factory);
            activeObjects.InitializeGameObjects();
            activeObjects.Player.Weapon.Shoot += PlayerShoot;
            collisionsController = new CollisionsController(activeObjects);
        }

        internal void Update(Single elapsedSeconds)
        {
            foreach (IUpdatable updatable in activeObjects.GetObjectsToUpdate())
                updatable.Update(elapsedSeconds);
            collisionsController.CheckCollisions();
            activeObjects.SendDeadToHeaven();
            if (!activeObjects.Player.IsAlive())
                Lost?.Invoke(this, EventArgs.Empty);
        }

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return activeObjects.GetObjectsToDraw();
        }

        private void PlayerShoot(Object sender, PlayerShootEventArgs args)
        {
            PlayerBullet playerBullet = args.PlayerBullet;
            playerBullet.Update(args.FirstUpdateTime);
            activeObjects.PlayerBullets.Add(playerBullet);
        }
    }
}
