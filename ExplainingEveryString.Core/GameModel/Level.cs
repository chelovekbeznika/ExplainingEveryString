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
        private ActiveActorsStorage activeActors;
        private CollisionsController collisionsController;
        private ActorsFactory factory;
        private List<EpicEventArgs> epicEventsHappened = new List<EpicEventArgs>();

        internal Vector2 PlayerPosition => activeActors.Player.Position;
        internal event GameLost Lost;

        internal Level(ActorsFactory factory, LevelData levelData)
        {
            this.factory = factory;
            factory.Level = this;
            activeActors = new ActiveActorsStorage(factory, levelData);
            activeActors.InitializeActors();
            collisionsController = new CollisionsController(activeActors);
        }

        internal void Update(Single elapsedSeconds)
        {
            foreach (IUpdatable updatable in activeActors.GetObjectsToUpdate())
                updatable.Update(elapsedSeconds);
            collisionsController.CheckCollisions();
            activeActors.SendDeadToHeaven();
            if (!activeActors.Player.IsAlive())
                Lost?.Invoke(this, EventArgs.Empty);
        }

        internal IEnumerable<IDisplayble> GetObjectsToDraw()
        {
            return activeActors.GetObjectsToDraw();
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
            activeActors.PlayerBullets.Add(bullet);
        }

        internal void EnemyShoot(Object sender, ShootEventArgs args)
        {
            Bullet bullet = args.Bullet;
            bullet.Update(args.FirstUpdateTime);
            activeActors.EnemyBullets.Add(bullet);
        }

        internal void EpicEventOccured(Object sender, EpicEventArgs epicEvent)
        {
            epicEventsHappened.Add(epicEvent);
        }
    }
}
