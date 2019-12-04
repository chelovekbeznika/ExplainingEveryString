using System;

namespace ExplainingEveryString.Core.Music
{
    internal class SoundDirectingEvent
    {
        internal Int32 Seconds { get; set; }
        internal Int32 SamplesOffset { get; set; }
        internal Int32 Position => Seconds * Constants.SampleRate + SamplesOffset;

        internal SoundComponentType SoundComponent { get; set; }
        internal SoundChannelParameter Parameter { get; set; }
        internal Int32 Value { get; set; }
    }
}
