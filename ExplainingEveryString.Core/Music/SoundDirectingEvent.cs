using System;

namespace ExplainingEveryString.Core.Music
{
    internal class SoundDirectingEvent
    {
        internal Int32 Position { get; set; }
        internal Int32 Value { get; set; }
        internal SoundChannelParameter Parameter { get; set; }
    }
}
