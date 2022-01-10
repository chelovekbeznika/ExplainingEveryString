using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossBrainBehavior : IEnemyBehavior
    {
        private FourthBossPartsSpawner partsSpawner;
        private FourthBossMovementController movementController;
        private IFourthBossBrain bossBrain;

        public EventHandler MoveGoalReached { get; set; }

        public Weapon Weapon => null;

        public ISpawnedActorsController SpawnedActors => partsSpawner;

        public PostMortemSurprise PostMortemSurprise => null;

        public Boolean IsTeleporter => false;

        public Single? EnemyAngle => null;

        internal FourthBossBrainBehavior(FourthBossMovementController movementController, FourthBossPartsSpawner partsSpawner, IFourthBossBrain bossBrain)
        {
            this.bossBrain = bossBrain;
            this.movementController = movementController;
            this.partsSpawner = partsSpawner;
        }

        public void Construct(EnemyBehaviorSpecification specification, Level level, ActorsFactory factory)
        {
        }

        public IEnumerable<IDisplayble> GetPartsToDisplay() => new IDisplayble[0];

        public void Update(Single elapsedSeconds)
        {
            movementController.Update(elapsedSeconds);
            bossBrain.Position = movementController.Position;
        }
    }
}
