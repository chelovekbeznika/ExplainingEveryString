using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Core.Music.Model
{
    internal class PulseNote : BpmSoundDirectingEvent
    {
        internal Note Note { get; set; }
        [DefaultValue(Alteration.None)]
        internal Alteration Alteration { get; set; } = Alteration.None;
        internal NoteLength Length { get; set; }
        [DefaultValue(15)]
        internal Int32 Volume { get; set; } = 15;
        [DefaultValue(false)]
        internal Boolean Decaying { get; set; } = false;
        [DefaultValue(true)]
        internal Boolean FirstChannel { get; set; } = true;

        private SoundComponentType SoundChannel => FirstChannel ? SoundComponentType.Pulse1 : SoundComponentType.Pulse2;

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundChannel,
                Parameter = SoundChannelParameter.Timer,
                Value = NotesHelper.PulseTimer(Note, Alteration)
            };
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundChannel,
                Parameter = SoundChannelParameter.Volume,
                Value = Volume
            };
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundChannel,
                Parameter = SoundChannelParameter.EnvelopeConstant,
                Value = Decaying ? 0 : 1,
            };
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset + NoteLengthInSamples(Length),
                SoundComponent = SoundChannel,
                Parameter = SoundChannelParameter.Timer,
                Value = 0
            };
            yield break;
        }
    }
}
