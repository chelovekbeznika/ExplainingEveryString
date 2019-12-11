using System;
using System.Collections.Generic;
namespace ExplainingEveryString.Core.Music.Model
{
    internal abstract class SoundDirectingEvent : ISoundDirectingSequence
    {
        internal Int32 Seconds { get; set; }
        internal Int32 SamplesOffset { get; set; }
        public Int32 Position => Seconds * Constants.SampleRate + SamplesOffset;

        public abstract IEnumerable<RawSoundDirectingEvent> GetEvents();
    }
}
