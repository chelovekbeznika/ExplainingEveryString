using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies.Bosses
{
    internal class FourthBossPart : Enemy<FourthBossPartBlueprint>
    {
        private FourthBossPartState state = FourthBossPartState.FirstPhase;
        private SpriteState phaseSwitchSprite;
        private SpriteState secondPhaseSprite;
        private Single tillSwitchToSecondPhase = Single.MaxValue;
        private Level level;
        private WeaponSpecification weaponOfRage;

        internal IFourthBossBrain BossBrain { get; private set; }

        public override SpriteState SpriteState
        {
            get
            {
                switch (state)
                {
                    case FourthBossPartState.FirstPhase: 
                        return base.SpriteState;
                    case FourthBossPartState.BetweenPhases: 
                        return phaseSwitchSprite;
                    case FourthBossPartState.SecondPhase:
                    case FourthBossPartState.ThirdPhase:
                        return secondPhaseSprite;
                    default:
                        return base.SpriteState;
                }
            }
        }

        public override CollidableMode CollidableMode => 
            state == FourthBossPartState.BetweenPhases || state == FourthBossPartState.SecondPhase 
                ? CollidableMode.Shadow 
                : base.CollidableMode;

        public override void TakeDamage(Single damage)
        {
            if (state == FourthBossPartState.ThirdPhase)
                BossBrain.TakeDamage(damage);
            else
                base.TakeDamage(damage);
        }

        protected override void Construct(FourthBossPartBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.phaseSwitchSprite = new SpriteState(blueprint.PhaseSwitchSprite) { Looping = false };
            this.secondPhaseSprite = new SpriteState(blueprint.SecondPhaseSprite);
            this.level = level;
            this.weaponOfRage = blueprint.WeaponOfRage;
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
            ManageLifeCycle(elapsedSeconds);
        }

        private void ManageLifeCycle(Single elapsedSeconds)
        {
            if (state == FourthBossPartState.FirstPhase && HitPoints <= 0)
            {
                state = FourthBossPartState.BetweenPhases;
                tillSwitchToSecondPhase = phaseSwitchSprite.AnimationCycle;
                (Behavior as FourthBossPartBehavior).GiveWeapon(null, level);
            }
            else if (state == FourthBossPartState.BetweenPhases)
            {
                tillSwitchToSecondPhase -= elapsedSeconds;
                if (tillSwitchToSecondPhase <= 0)
                {
                    state = FourthBossPartState.SecondPhase;
                    BossBrain.SendAgonySignal();
                    (Behavior as FourthBossPartBehavior).GiveWeapon(weaponOfRage, level);
                }
            }
            else if (state == FourthBossPartState.SecondPhase && BossBrain.InAgony)
            {
                state = FourthBossPartState.ThirdPhase;
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
