using Newtonsoft.Json;
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

        [JsonIgnore]
        internal Int32 TimeToRepeat { get; set; }
        [JsonIgnore]
        internal Single StartingBeat { get; set; }
        [JsonIgnore]
        internal Single OneRepeatBeats { get; set; }
        [JsonIgnore]
        internal Single ActualBeatNumber => StartingBeat + TimeToRepeat * OneRepeatBeats + BeatNumber;

        protected Int32 SamplesPerBeat => Constants.SampleRate * 60 / BeatsPerMinute;
        private Int32 Position => (Int32)Math.Round(SamplesPerBeat * ActualBeatNumber);
        internal Int32 Seconds => Position / Constants.SampleRate;
        internal Int32 SamplesOffset => Position % Constants.SampleRate;

        protected Int32 NoteLengthInSamples(NoteLength noteLength)
        {
            Int32 result;
            if (noteLength.HasFlag(NoteLength.Whole)) result = SamplesPerBeat * 4;
            else if (noteLength.HasFlag(NoteLength.Half)) result = SamplesPerBeat * 2;
            else if (noteLength.HasFlag(NoteLength.Quarter)) result = SamplesPerBeat;
            else if (noteLength.HasFlag(NoteLength.Eigth)) result = SamplesPerBeat / 2;
            else if (noteLength.HasFlag(NoteLength.Sixteenth)) result = SamplesPerBeat / 4;
            else throw new ArgumentException(nameof(noteLength));

            if (noteLength.HasFlag(NoteLength.Dotted)) result = result * 3 / 2;
            if (noteLength.HasFlag(NoteLength.DoubleDotted)) result = result * 7 / 4;
            if (noteLength.HasFlag(NoteLength.TripleDotted)) result = result * 15 / 8;

            return result;
        }

        public abstract IEnumerable<RawSoundDirectingEvent> GetEvents();
    }
}
