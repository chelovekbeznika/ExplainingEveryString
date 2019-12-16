using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Music.Model
{
    internal class BpmSequence : ISoundDirectingSequence
    {
        public Int32 Seconds { get; set; }
        public Int32 SamplesOffset { get; set; }

        public Int32 BeatsPerMinute { get; set; }
        public IEnumerable<BpmSoundDirectingEvent> BpmNotes { get; set; } 

        public IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            foreach (BpmSoundDirectingEvent Note in BpmNotes)
            {
                Note.BeatsPerMinute = BeatsPerMinute;
                foreach (RawSoundDirectingEvent rawSoundDirectingEvent in Note.GetEvents())
                {
                    rawSoundDirectingEvent.Seconds += Seconds;
                    rawSoundDirectingEvent.SamplesOffset += SamplesOffset;
                    yield return rawSoundDirectingEvent;
                }
            }
            yield break;
        }
    }
}
