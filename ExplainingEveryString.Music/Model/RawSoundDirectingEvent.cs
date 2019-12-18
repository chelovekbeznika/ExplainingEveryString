using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Music.Model
{
    internal class RawSoundDirectingEvent : ISoundDirectingSequence
    {
        internal Int32 Seconds { get; set; }
        internal Int32 SamplesOffset { get; set; }
        public Int32 Position => Seconds * Constants.SampleRate + SamplesOffset;

        internal SoundComponentType SoundComponent { get; set; }
        internal SoundChannelParameter Parameter { get; set; }
        internal Int32 Value { get; set; }

        public IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return this;
            yield break;
        }
    }
}
