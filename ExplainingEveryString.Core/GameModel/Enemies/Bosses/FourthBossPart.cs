using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossPart : Enemy<FourthBossPartBlueprint>
    {
        internal IFourthBossBrain BossBrain { get; set; }

        internal Vector2 Offset { get; private set; }

        protected override void Construct(FourthBossPartBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.Offset = blueprint.Offset;
        }

        public override bool IsAlive() => BossBrain.IsAlive();

        protected override IEnemyBehavior CreateBehaviorObject(IEnemy enemy, Player player, BehaviorParameters behaviorParameters)
        {
            return new FourthBossPartBehavior(this);
        }
    }
}
