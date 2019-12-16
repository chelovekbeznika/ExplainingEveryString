using System.Collections.Generic;

namespace ExplainingEveryString.Core.Music.Model
{
    internal interface ISoundDirectingSequence
    {
        IEnumerable<RawSoundDirectingEvent> GetEvents();
    }
}
