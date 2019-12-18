using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Music.Model
{
    public abstract class BpmSoundDirectingEvent : ISoundDirectingSequence
    {
        [DefaultValue(90)]
        public Int32 BeatsPerMinute { get; set; }
        public Single BeatNumber { get; set; }

        protected Int32 SamplesPerBeat => Constants.SampleRate * 60 / BeatsPerMinute;
        private Int32 Position => (Int32)Math.Round(SamplesPerBeat * BeatNumber);
        internal Int32 Seconds => Position / Constants.SampleRate;
        internal Int32 SamplesOffset => Position % Constants.SampleRate;

        protected Int32 NoteLengthInSamples(NoteLength noteLength)
        {
            switch (noteLength)
            {
                case NoteLength.Whole: return SamplesPerBeat * 4;
                case NoteLength.Half: return SamplesPerBeat * 2;
                case NoteLength.Quarter: return SamplesPerBeat;
                case NoteLength.Eigth: return SamplesPerBeat / 2;
                case NoteLength.Sixteenth: return SamplesPerBeat / 4;
                default: throw new ArgumentException(nameof(noteLength));
            }
        }

        public abstract IEnumerable<RawSoundDirectingEvent> GetEvents();
    }
}
