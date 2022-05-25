using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameModel
{
    internal class ActorChangingEventsProcessor
    {
        private List<IChangeableActor> changeableActors;
        private HashSet<IChangeableActor> subscribed = new HashSet<IChangeableActor>();
        private ActiveActorsStorage allActiveActors;

        internal ActorChangingEventsProcessor(ActiveActorsStorage activeActorsStorage)
        {
            this.allActiveActors = activeActorsStorage;
        }

        public void Update()
        {
            changeableActors = allActiveActors.ChangeableActors;
            foreach (var actor in changeableActors)
            {
                if (!subscribed.Contains(actor))
                {
                    actor.ChangingEventOccured += ProcessChangingEvent;
                    subscribed.Add(actor);
                }
            }
        }

        private void ProcessChangingEvent(Object sender, ChangingEventArgs e)
        {
            foreach (var actor in changeableActors)
                actor.ReactToChangingEvent(e.Specification.Type);
        }
    }
}
