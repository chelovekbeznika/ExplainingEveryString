using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Enemies;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
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
        private List<EpicEventArgs> epicEventsHappened = new List<EpicEventArgs>();

        internal Vector2 PlayerPosition => activeObjects.Player.Position;
        internal event GameLost Lost;

        internal Level(GameObjectsFactory factory, LevelData levelData)
        {
            this.factory = factory;
            factory.Level = this;
            activeObjects = new ActiveGameObjectsStorage(factory, levelData);
            activeObjects.InitializeGameObjects();
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

        internal IEnumerable<EpicEventArgs> CollectEpicEvents()
        {
            IEnumerable<EpicEventArgs> result = epicEventsHappened;
            epicEventsHappened = new List<EpicEventArgs>();
            return result;
        }

        internal void PlayerShoot(Object sender, ShootEventArgs args)
        {
            Bullet bullet = args.Bullet;
            bullet.Update(args.FirstUpdateTime);
            activeObjects.PlayerBullets.Add(bullet);
        }

        internal void EpicEventOccured(Object sender, EpicEventArgs epicEvent)
        {
            epicEventsHappened.Add(epicEvent);
        }
    }
}
