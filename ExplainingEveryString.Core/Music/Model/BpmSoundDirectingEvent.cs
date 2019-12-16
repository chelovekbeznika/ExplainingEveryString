using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Core.Music.Model
{
    internal abstract class BpmSoundDirectingEvent : ISoundDirectingSequence
    {
        [DefaultValue(90)]
        internal Int32 BeatsPerMinute { get; set; } = 90;
        internal Single BeatNumber { get; set; }

        protected Int32 SamplesPerBeat => Constants.SampleRate * 60 / BeatsPerMinute;
        private Int32 Position => (Int32)System.Math.Round(SamplesPerBeat * BeatNumber);
        internal Int32 Seconds => Position / Constants.SampleRate;
        internal Int32 SamplesOffset => Position % Constants.SampleRate;
        public abstract IEnumerable<RawSoundDirectingEvent> GetEvents();

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
    }
}
