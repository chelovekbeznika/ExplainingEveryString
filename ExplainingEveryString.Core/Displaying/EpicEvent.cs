using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class EpicEvent
    {
        private event EventHandler<EpicEventArgs> Event;
        internal Boolean Handled { get; private set; } = false;
        private Boolean oneTimeEvent;
        private SpecEffectSpecification specEffect;
        private IDisplayble eventSource;
        private Boolean inheritAngle;
        private Boolean follow;

        internal EpicEvent(Level level, SpecEffectSpecification specEffect, Boolean handleOneTime,
            IDisplayble eventSource, Boolean inheritAngle, Boolean follow = false)
        {
            this.Event += level.EpicEventOccured;
            this.specEffect = specEffect;
            this.oneTimeEvent = handleOneTime;
            this.eventSource = eventSource;
            this.inheritAngle = inheritAngle;
            this.follow = follow;
        }

        internal void TryHandle()
        {
            if (!Handled || !oneTimeEvent)
            {
                var sprite = eventSource.SpriteState;
                var startPosition = eventSource.Position;
                Handled = true;
                if (specEffect != null)
                    Event?.Invoke(eventSource, new EpicEventArgs
                    {
                        PositionLocator = () => follow ? eventSource.Position : startPosition,
                        Angle = inheritAngle  && sprite != null ? sprite.Angle : 0,
                        SpecEffectSpecification = specEffect
                    });
            }
        }
    }
}
