using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Displaying
{
    internal class OneTimeEpicEvent
    {
        private event EventHandler<EpicEventArgs> Event;
        private Boolean handled = false;
        private SpecEffectSpecification specEffect;
        private IDisplayble eventSource;

        internal OneTimeEpicEvent(Level level, SpecEffectSpecification specEffect, IDisplayble eventSource)
        {
            this.Event += level.EpicEventOccured;
            this.specEffect = specEffect;
            this.eventSource = eventSource;
        }

        internal void TryHandle()
        {
            if (!handled)
            {
                handled = true;
                if (specEffect != null)
                    Event?.Invoke(eventSource, new EpicEventArgs
                    {
                        Position = eventSource.Position,
                        SpecEffectSpecification = specEffect
                    });
            }
        }
    }
}
