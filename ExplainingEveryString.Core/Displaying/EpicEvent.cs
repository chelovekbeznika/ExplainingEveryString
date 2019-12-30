﻿using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Specifications;
using System;

namespace ExplainingEveryString.Core.Displaying
{
    internal class EpicEvent
    {
        private event EventHandler<EpicEventArgs> Event;
        private Boolean handled = false;
        private Boolean oneTimeEvent;
        private SpecEffectSpecification specEffect;
        private IDisplayble eventSource;
        private Boolean inheritAngle;

        internal EpicEvent(Level level, SpecEffectSpecification specEffect, Boolean handleOneTime,
            IDisplayble eventSource, Boolean inheritAngle)
        {
            this.Event += level.EpicEventOccured;
            this.specEffect = specEffect;
            this.oneTimeEvent = handleOneTime;
            this.eventSource = eventSource;
            this.inheritAngle = inheritAngle;
        }

        internal void TryHandle()
        {
            if (!handled || !oneTimeEvent)
            {
                var sprite = eventSource.SpriteState;
                handled = true;
                if (specEffect != null)
                    Event?.Invoke(eventSource, new EpicEventArgs
                    {
                        Position = eventSource.Position,
                        Angle = inheritAngle  && sprite != null ? sprite.Angle : 0,
                        SpecEffectSpecification = specEffect
                    });
            }
        }
    }
}
