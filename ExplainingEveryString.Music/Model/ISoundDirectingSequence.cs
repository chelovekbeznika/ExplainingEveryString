using System.Collections.Generic;

namespace ExplainingEveryString.Music.Model
{
    internal interface ISoundDirectingSequence
    {
        IEnumerable<RawSoundDirectingEvent> GetEvents();
    }
}
