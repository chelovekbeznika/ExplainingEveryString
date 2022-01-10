using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossPart : Enemy<FourthBossPartBlueprint>
    {
        internal IFourthBossBrain BossBrain { get; private set; }

        internal Vector2 Offset { get; private set; }

        protected override void Construct(FourthBossPartBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            this.Offset = blueprint.Offset;
            base.Construct(blueprint, startInfo, level, factory);
        }

        protected override void PlaceOnLevel(ActorStartInfo info)
        {
            base.PlaceOnLevel(info);
            BossBrain = (IFourthBossBrain)info.AdditionalParameters[0];
        }

        public override bool IsAlive() => BossBrain.IsAlive();

        protected override IEnemyBehavior CreateBehaviorObject(FourthBossPartBlueprint blueprint, Player player, 
            ActorStartInfo actorStartInfo, ActorsFactory factory)
        {
            return new FourthBossPartBehavior(this);
        }
    }
}
