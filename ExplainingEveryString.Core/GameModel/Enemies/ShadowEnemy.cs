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
        private event EventHandler<EpicEventArgs> PhaseChanged;
        private Boolean inShadow = false;
        private Single phaseCountingTime = 0;

        private SpriteState shadowSprite;
        private Single activeTime;
        private Single shadowTime;
        private SpecEffectSpecification phaseChangeSpecEffect;

        public override SpriteState SpriteState => !inShadow ? base.SpriteState : shadowSprite;
        public override CollidableMode Mode => !inShadow ? base.Mode : CollidableMode.Ghost;

        protected override void Construct(ShadowEnemyBlueprint blueprint, ActorStartInfo startInfo, Level level)
        {
            base.Construct(blueprint, startInfo, level);
            this.shadowSprite = new SpriteState(blueprint.ShadowSprite);
            this.activeTime = blueprint.ActiveTime;
            this.shadowTime = blueprint.ShadowTime;
            this.phaseChangeSpecEffect = blueprint.PhaseChangeEffect;
            this.PhaseChanged += level.EpicEventOccured;
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
            PhaseChanged?.Invoke(this, StandardEpicEvent(phaseChangeSpecEffect));
        }
    }
}
