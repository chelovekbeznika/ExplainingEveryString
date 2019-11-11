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
        private Int32 currentPhase;
        private bool betweenPhases = true;
        private Single betweenPhasesDuration;
        private Single minPhaseDuration;
        private Single maxPhaseDuration;
        private EpicEvent phaseOn;
        private EpicEvent phaseOff;

        private SpriteState[] phaseSprite;
        private EnemyBehavior[] phaseBehavior;

        protected override void Construct(FirstBossBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.betweenPhasesDuration = blueprint.TimeBetweenPhases;
            this.minPhaseDuration = blueprint.MinPhaseDuration;
            this.maxPhaseDuration = blueprint.MaxPhaseDuration;
            this.phaseOn = new EpicEvent(level, blueprint.PhaseOnEffect, false, this, true);
            this.phaseOff = new EpicEvent(level, blueprint.PhaseOffEffect, false, this, true);
            this.phaseSprite = blueprint.Phases.Select(phase => new SpriteState(phase.Phase)).ToArray();
            this.phaseBehavior = blueprint.Phases.Select(phase => ConstructBehavior(phase, startInfo, level, factory)).ToArray();
            Single tillFirstPhaseSwitch = betweenPhasesDuration + blueprint.DefaultAppearancePhaseDuration;
            TimersComponent.Instance.ScheduleEvent(betweenPhasesDuration, () => PhaseOn());
        }

        private EnemyBehavior ConstructBehavior(FirstBossPhaseSpecification phase,
            ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            EnemyBehavior result = new EnemyBehavior(this, () => level.Player.Position);
            BehaviorParameters behaviorParameters = new BehaviorParameters
            {
                LevelSpawnPoints = startInfo.BehaviorParameters.LevelSpawnPoints,
                TrajectoryParameters = phase.TrajectoryParameters
            };
            result.Construct(phase.Behavior, behaviorParameters, level, factory);
            return result;
        }

        protected override EnemyBehavior Behavior
        {
            get => betweenPhases ? base.Behavior : phaseBehavior[currentPhase];
            set => base.Behavior = value;
        }

        public override SpriteState SpriteState => betweenPhases ? base.SpriteState : phaseSprite[currentPhase];

        private void PhaseOn()
        {
            SpawnedActorsController oldSpawner = Behavior.SpawnedActors;
            betweenPhases = false;
            currentPhase = RandomUtility.NextInt(phaseBehavior.Length);
            SpawnedActorsController newSpawner = Behavior.SpawnedActors;

            phaseOn.TryHandle();
            OnBehaviorChanged(oldSpawner, newSpawner);
            Single phaseDuration = minPhaseDuration + RandomUtility.Next() * (maxPhaseDuration - minPhaseDuration);
            TimersComponent.Instance.ScheduleEvent(phaseDuration, () => PhaseOff());
        }

        private void PhaseOff()
        {
            SpawnedActorsController oldSpawner = Behavior.SpawnedActors;
            betweenPhases = true;
            SpawnedActorsController newSpawner = Behavior.SpawnedActors;

            phaseOff.TryHandle();
            OnBehaviorChanged(oldSpawner, newSpawner);
            TimersComponent.Instance.ScheduleEvent(betweenPhasesDuration, () => PhaseOn());
        }
    }
}
