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
        private FourthBossPartsSpawner partsSpawner;

        public override Single HitPoints
        {
            get => partsSpawner.SpawnedEnemies.Sum(p => System.Math.Max(p.HitPoints, 0));
            set {;}
        }

        public override CollidableMode CollidableMode => CollidableMode.Ghost;

        public Single Angle => Behavior.EnemyAngle ?? 0;

        public override ISpawnedActorsController SpawnedActors => partsSpawner;

        protected override void Construct(FourthBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.partsSpawner = new FourthBossPartsSpawner(this, factory, blueprint.PartsList);
        }
    }
}
