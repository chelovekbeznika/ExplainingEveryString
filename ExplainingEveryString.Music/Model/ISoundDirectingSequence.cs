using System.Collections.Generic;

namespace ExplainingEveryString.Music.Model
{
    public interface ISoundDirectingSequence
    {
        IEnumerable<RawSoundDirectingEvent> GetEvents();
    }
}
