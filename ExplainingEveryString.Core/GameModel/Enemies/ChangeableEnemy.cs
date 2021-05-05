using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class ChangeableEnemy : Enemy<ChangeableEnemyBlueprint>, IChangeableActor
    {
        private Dictionary<ValueTuple<String, Int32>, IModificationSpecification[]> modifications;
        private Dictionary<String, Int32> occuredEvents = new Dictionary<String, Int32>();
        private Level level;

        public event EventHandler<ChangingEventArgs> ChangingEventOccured;

        protected override void Construct(ChangeableEnemyBlueprint blueprint, ActorStartInfo startInfo, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, startInfo, level, factory);
            this.modifications = blueprint.Modifications?
                .ToDictionary(
                    kvp =>
                    {
                        var parts = kvp.Key.Split(':');
                        return (parts[0], Int32.Parse(parts[1]));
                    },
                    kvp => kvp.Value
                );
            this.level = level;

            this.Died += (sender, e) =>
            {
                ChangingEventOccured?.Invoke(this, new ChangingEventArgs { Specification = blueprint.ChangingEventAtDeath });
            };
        }

        public void ReactToChangingEvent(String enemyChangingEvent)
        {
            if (!occuredEvents.ContainsKey(enemyChangingEvent))
                occuredEvents.Add(enemyChangingEvent, 0);
            occuredEvents[enemyChangingEvent] += 1;

            var currentEvent = (enemyChangingEvent, occuredEvents[enemyChangingEvent]);
            if (modifications != null && modifications.ContainsKey(currentEvent))
                foreach (var modification in modifications[currentEvent])
                    ApplyModification(modification);
        }

        private void ApplyModification(IModificationSpecification modification)
        {
            switch (modification.ModificationType)
            {
                case "Weapon":
                    Behavior.ChangeWeapon(modification as WeaponSpecification, level);
                    break;
                case "DefaultSprite":
                    SpriteState = new SpriteState(modification as SpriteSpecification);
                    break;
            }
        }
    }
}
