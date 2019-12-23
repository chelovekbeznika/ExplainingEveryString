﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Music.Model
{
    public class TriangleNote : BpmSoundDirectingEvent, INote
    {
        public Note Note { get; set; }
        [DefaultValue(Alteration.None)]
        public Alteration Alteration { get; set; }
        public NoteLength Length { get; set; }

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
                SamplesOffset = SamplesOffset + NoteLengthInSamples(Length),
                SoundComponent = SoundComponentType.Triangle,
                Parameter = SoundChannelParameter.Timer,
                Value = 0
            };
            yield break;
        }
    }
}
