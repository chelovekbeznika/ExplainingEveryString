using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class ShadowEnemy : Enemy<ShadowEnemyBlueprint>
    {
        private EpicEvent phaseChanged;
        private Boolean inShadow = false;
        private Single phaseCountingTime = 0;

        private SpriteState shadowSprite;
        private Single activeTime;
        private Single shadowTime;

        public override SpriteState SpriteState => !inShadow ? base.SpriteState : shadowSprite;
        public override CollidableMode Mode => !inShadow ? base.Mode : CollidableMode.Shadow;

        protected override void Construct(ShadowEnemyBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.shadowSprite = new SpriteState(blueprint.ShadowSprite);
            this.activeTime = blueprint.ActiveTime;
            this.shadowTime = blueprint.ShadowTime;
            this.phaseChanged = new EpicEvent(level, blueprint.PhaseChangeEffect, false, this, true);
        }

        public override void Update(Single elapsedSeconds)
        {
            if (!IsInAppearancePhase)
                UpdateShadowPhase(elapsedSeconds);
            base.Update(elapsedSeconds);
        }

        private void UpdateShadowPhase(Single elapsedSeconds)
        {
            phaseCountingTime += elapsedSeconds;
            Single currentPhaseTimeToLive = inShadow ? shadowTime : activeTime;
            if (phaseCountingTime > currentPhaseTimeToLive + Math.Constants.Epsilon)
            {
                ChangePhase();
                phaseCountingTime -= currentPhaseTimeToLive;
            }
        }

        private void ChangePhase()
        {
            inShadow = !inShadow;
            phaseChanged.TryHandle();
        }
    }
}
