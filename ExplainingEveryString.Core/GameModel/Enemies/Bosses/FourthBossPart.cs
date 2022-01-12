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

        protected override void Construct(FourthBossPartBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
        }

        protected override void PlaceOnLevel(ActorStartInfo info)
        {
            base.PlaceOnLevel(info);
            BossBrain = (IFourthBossBrain)info.AdditionalParameters[0];
        }

        public override void Update(Single elapsedSeconds)
        {
            base.Update(elapsedSeconds);
            if (IsInAppearancePhase)
            {
                (Behavior as FourthBossPartBehavior).UpdatePosition();
                SpriteState.Angle = Behavior.EnemyAngle ?? 0;
            }
        }

        public override bool IsAlive() => BossBrain.IsAlive();

        protected override IEnemyBehavior CreateBehaviorObject(FourthBossPartBlueprint blueprint, Player player, 
            ActorStartInfo actorStartInfo, ActorsFactory factory)
        {
            return new FourthBossPartBehavior(this, blueprint);
        }
    }
}
