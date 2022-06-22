using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class CompositeSpawnedActorsController : ISpawnedActorsController
    {
        private List<ISpawnedActorsController> controllers = new List<ISpawnedActorsController>();
        private Boolean active = false;

        public event EventHandler<EnemySpawnedEventArgs> EnemySpawned
        {
            add
            {
                foreach (var controller in controllers)
                    controller.EnemySpawned += value;
            }
            remove
            {
                foreach (var controllers in controllers)
                    controllers.EnemySpawned -= value;
            }
        }

        public List<IEnemy> SpawnedEnemies => controllers.SelectMany(controller => controller.SpawnedEnemies).ToList();

        public Int32 MaxSpawned => controllers.Select(controller => controller.MaxSpawned).Sum();

        internal CompositeSpawnedActorsController(params ISpawnedActorsController[] controllers)
        {
            foreach (var controller in controllers)
                AddController(controller);
        }

        internal void AddController(ISpawnedActorsController controller)
        {
            if (controller == null)
                return;
            if (active)
                controller.TurnOn();
            else
                controller.TurnOff();
            controllers.Add(controller);
        }

        public void DivideAliveAndDead(List<IEnemy> avengers)
        {
            foreach (var controller in controllers)
                controller.DivideAliveAndDead(avengers);
        }

        public void TurnOff()
        {
            active = false;
            foreach (var controller in controllers)
                controller.TurnOff();
        }

        public void TurnOn()
        {
            active = true;
            foreach (var controller in controllers)
                controller.TurnOn();
        }

        public void Update(Single elapsedSeconds)
        {
            foreach (var controller in controllers)
                controller.Update(elapsedSeconds);
        }

        public void DespawnRoutine()
        {
            foreach (var controller in controllers)
                controller.DespawnRoutine();
        }
    }
}
