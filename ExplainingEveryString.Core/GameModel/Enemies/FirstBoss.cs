using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class FirstBoss : Enemy<FirstBossBlueprint>
    {
        private enum BossState { BetweenPhases, TurningOnPhase, InPhase, TurningOffPhase }
        private enum PhaseType { Start, Shoot, Spawn }

        private Int32 currentPhase;
        private BossState state = BossState.BetweenPhases;
        private PhaseType phaseType = PhaseType.Start;
        private Single betweenPhasesDuration;
        private Single minPhaseDuration;
        private Single maxPhaseDuration;
        private EpicEvent phaseOn;
        private EpicEvent phaseOff;

        private Dictionary<PhaseType, FirstBossPhase[]> phases;

        private FirstBossPhase CurrentPhase => phaseType != PhaseType.Start 
            ? phases[phaseType][currentPhase] 
            : throw new InvalidOperationException("There is not phase in this state!");

        protected override void Construct(FirstBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.betweenPhasesDuration = blueprint.TimeBetweenPhases;
            this.minPhaseDuration = blueprint.MinPhaseDuration;
            this.maxPhaseDuration = blueprint.MaxPhaseDuration;
            this.phaseOn = new EpicEvent(level, blueprint.PhaseOnEffect, false, this, true);
            this.phaseOff = new EpicEvent(level, blueprint.PhaseOffEffect, false, this, true);
            this.phases = new Dictionary<PhaseType, FirstBossPhase[]>
            {
                {
                    PhaseType.Shoot,
                    blueprint.Phases.Where(phaseSpec => phaseSpec.Behavior.Spawner == null)
                        .Select(phase => ConstructPhase(phase, startInfo, level, factory)).ToArray()
                },
                {
                    PhaseType.Spawn,
                    blueprint.Phases.Where(phase => phase.Behavior.Spawner != null)
                        .Select(phase => ConstructPhase(phase, startInfo, level, factory)).ToArray()
                }
            };
            var tillFirstPhaseSwitch = betweenPhasesDuration + blueprint.DefaultAppearancePhaseDuration;
            TimersComponent.Instance.ScheduleEvent(betweenPhasesDuration, () => TurningOnPhase(), this);
            SwitchToNextPhase();
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
            get => state == BossState.InPhase ? CurrentPhase.Behavior : base.Behavior;
            set => base.Behavior = value;
        }

        public override SpriteState SpriteState
        {
            get
            {
                switch (state)
                {
                    case BossState.BetweenPhases: return base.SpriteState;
                    case BossState.TurningOnPhase: return CurrentPhase.OnSprite;
                    case BossState.InPhase: return CurrentPhase.Sprite;
                    case BossState.TurningOffPhase: return CurrentPhase.OffSprite;
                    default: return base.SpriteState;
                }
            }
        }
        private void TurningOnPhase()
        {
            state = BossState.TurningOnPhase;
            SpriteState.StartOver();

            TimersComponent.Instance.ScheduleEvent(CurrentPhase.TurningOnTime, () => InPhase(), this);
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
            TimersComponent.Instance.ScheduleEvent(CurrentPhase.TurningOffTime, () => BetweenPhase(), this);
        }

        private void BetweenPhase()
        {                    
            state = BossState.BetweenPhases;
            SpriteState.StartOver();

            var tillNextState = betweenPhasesDuration - CurrentPhase.TurningOffTime - CurrentPhase.TurningOnTime;
            SwitchToNextPhase();
            TimersComponent.Instance.ScheduleEvent(tillNextState, () => TurningOnPhase(), this);
        }

        private void SwitchToNextPhase()
        {
            switch (phaseType)
            {
                case PhaseType.Start: phaseType = PhaseType.Shoot; break;
                case PhaseType.Shoot: phaseType = PhaseType.Spawn; break;
                case PhaseType.Spawn: phaseType = PhaseType.Shoot; break;
            }
            currentPhase = RandomUtility.NextInt(phases[phaseType].Length);
        }
    }
}
