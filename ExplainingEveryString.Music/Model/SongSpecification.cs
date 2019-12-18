using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Music.Model
{
    public class SongSpecification
    {
        public Single Duration { get; set; }
        public List<ISoundDirectingSequence> Song { get; set; }
    }
}
