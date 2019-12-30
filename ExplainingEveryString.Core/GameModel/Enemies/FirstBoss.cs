using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class FirstBoss : Enemy<FirstBossBlueprint>
    {
        private enum BossState { BetweenPhases, TurningOnPhase, InPhase, TurningOffPhase }

        private Int32 currentPhase;
        private Int32 nextPhase;
        private BossState state = BossState.BetweenPhases;
        private Single betweenPhasesDuration;
        private Single minPhaseDuration;
        private Single maxPhaseDuration;
        private EpicEvent phaseOn;
        private EpicEvent phaseOff;

        private FirstBossPhase[] phases;

        protected override void Construct(FirstBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.betweenPhasesDuration = blueprint.TimeBetweenPhases;
            this.minPhaseDuration = blueprint.MinPhaseDuration;
            this.maxPhaseDuration = blueprint.MaxPhaseDuration;
            this.phaseOn = new EpicEvent(level, blueprint.PhaseOnEffect, false, this, true);
            this.phaseOff = new EpicEvent(level, blueprint.PhaseOffEffect, false, this, true);
            this.phases = blueprint.Phases.Select(phase => ConstructPhase(phase, startInfo, level, factory)).ToArray();
            var tillFirstPhaseSwitch = betweenPhasesDuration + blueprint.DefaultAppearancePhaseDuration;
            TimersComponent.Instance.ScheduleEvent(betweenPhasesDuration, () => TurningOnPhase(), this);
            nextPhase = SelectNextPhase();
        }

        private FirstBossPhase ConstructPhase(FirstBossPhaseSpecification phase,
            ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            var behavior = new EnemyBehavior(this, () => level.Player.Position);
            var behaviorParameters = new BehaviorParameters
            {
                LevelSpawnPoints = startInfo.BehaviorParameters.LevelSpawnPoints,
                TrajectoryParameters = phase.TrajectoryParameters
            };
            behavior.Construct(phase.Behavior, behaviorParameters, level, factory);
            return new FirstBossPhase
            {
                Sprite = new SpriteState(phase.Phase),
                OnSprite = new SpriteState(phase.On) { Looping = false },
                OffSprite = new SpriteState(phase.Off) { Looping = false },
                Behavior = behavior
            };
        }

        protected override EnemyBehavior Behavior
        {
            get => state == BossState.InPhase ? phases[currentPhase].Behavior : base.Behavior;
            set => base.Behavior = value;
        }

        public override SpriteState SpriteState
        {
            get
            {
                switch (state)
                {
                    case BossState.BetweenPhases: return base.SpriteState;
                    case BossState.TurningOnPhase: return phases[currentPhase].OnSprite;
                    case BossState.InPhase: return phases[currentPhase].Sprite;
                    case BossState.TurningOffPhase: return phases[currentPhase].OffSprite;
                    default: return base.SpriteState;
                }
            }
        }
        private void TurningOnPhase()
        {
            currentPhase = nextPhase;
            state = BossState.TurningOnPhase;
            SpriteState.StartOver();

            TimersComponent.Instance.ScheduleEvent(phases[currentPhase].TurningOnTime, () => InPhase(), this);
        }

        private void InPhase()
        {
            var oldSpawner = Behavior.SpawnedActors;
            state = BossState.InPhase;
            SpriteState.StartOver();
            var newSpawner = Behavior.SpawnedActors;

            phaseOn.TryHandle();
            OnBehaviorChanged(oldSpawner, newSpawner);
            var phaseDuration = minPhaseDuration + RandomUtility.Next() * (maxPhaseDuration - minPhaseDuration);
            TimersComponent.Instance.ScheduleEvent(phaseDuration, () => TurningOffPhase(), this);
        }

        private void TurningOffPhase()
        {
            var oldSpawner = Behavior.SpawnedActors;
            state = BossState.TurningOffPhase;
            SpriteState.StartOver();
            var newSpawner = Behavior.SpawnedActors;

            phaseOff.TryHandle();
            OnBehaviorChanged(oldSpawner, newSpawner);
            TimersComponent.Instance.ScheduleEvent(phases[currentPhase].TurningOffTime, () => BetweenPhase(), this);
        }

        private void BetweenPhase()
        {                    
            state = BossState.BetweenPhases;
            SpriteState.StartOver();

            nextPhase = SelectNextPhase();
            var tillNextState = betweenPhasesDuration 
                - phases[currentPhase].TurningOffTime
                - phases[nextPhase].TurningOnTime;
            TimersComponent.Instance.ScheduleEvent(tillNextState, () => TurningOnPhase(), this);
        }

        private Int32 SelectNextPhase()
        {
            return RandomUtility.NextInt(phases.Length);
        }
    }
}
