using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Music.Model
{
    internal class RawSoundDirectingEvent : SoundDirectingEvent
    {
        internal SoundComponentType SoundComponent { get; set; }
        internal SoundChannelParameter Parameter { get; set; }
        internal Int32 Value { get; set; }

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return this;
            yield break;
        }
    }
}
