using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Music.Model
{
    public class TriangleNote : BpmSoundDirectingEvent, INote
    {
        public Note Note { get; set; }
        [DefaultValue(Accidental.None)]
        public Accidental Accidental { get; set; }
        public NoteLength Length { get; set; }
        [DefaultValue(false)]
        public Boolean PartOfLegato { get; set; }

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundComponentType.Triangle,
                Parameter = SoundChannelParameter.Timer,
                Value = NotesHelper.TriangleTimer(Note, Accidental)
            };
            var endAt = PartOfLegato ? SamplesOffset + NoteLengthInSamples(Length) : SamplesOffset + NoteLengthInSamples(Length) * 19 / 20;
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = endAt,
                SoundComponent = SoundComponentType.Triangle,
                Parameter = SoundChannelParameter.Timer,
                Value = 0
            };
            yield break;
        }
    }
}
