using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Core.Music.Model
{
    internal class TriangleNote : SoundDirectingEvent
    {
        internal Note Note { get; set; }
        [DefaultValue(Alteration.None)]
        internal Alteration Alteration { get; set; } = Alteration.None;
        internal NoteLength Length { get; set; }

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundComponentType.Triangle,
                Parameter = SoundChannelParameter.Timer,
                Value = NotesHelper.TriangleTimer(Note, Alteration)
            };
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset + Constants.SampleRate * 4 / 3 / (Int32)Length,
                SoundComponent = SoundComponentType.Triangle,
                Parameter = SoundChannelParameter.Timer,
                Value = 0
            };
            yield break;
        }
    }
}
