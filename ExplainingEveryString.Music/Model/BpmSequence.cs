using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Music.Model
{
    internal class BpmSequence : ISoundDirectingSequence
    {
        public Int32 Seconds { get; set; }
        [DefaultValue(0)]
        public Int32 SamplesOffset { get; set; }
        [DefaultValue(90)]
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
