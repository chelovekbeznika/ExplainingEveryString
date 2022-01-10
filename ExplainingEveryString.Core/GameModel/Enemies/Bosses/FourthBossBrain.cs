using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Data.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossBrain : Enemy<FourthBossBlueprint>, IFourthBossBrain
    {
        public override Single HitPoints
        {
            get => SpawnedActorsController.SpawnedEnemies.Sum(p => System.Math.Max(p.HitPoints, 0));
            set {;}
        }

        public override CollidableMode CollidableMode => CollidableMode.Ghost;

        public Single Angle => Behavior?.EnemyAngle ?? 0;


        protected override void Construct(FourthBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
        }

        protected override IEnemyBehavior CreateBehaviorObject(FourthBossBlueprint blueprint, Player player, ActorStartInfo actorStartInfo, ActorsFactory factory)
        {
            var movementController = new FourthBossMovementController(blueprint.MovementSpecification, actorStartInfo.Position);
            Position = movementController.Position;
            var partsSpawner = new FourthBossPartsSpawner(this, factory, blueprint.PartsList);
            return new FourthBossBrainBehavior(movementController, partsSpawner, this);
        }
    }
}
