using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Core.Music.Model
{
    internal class ConstantPulseNote : PulseNote
    {
        [DefaultValue(15)]
        internal Int32 Volume { get; set; }
        internal override Boolean EnvelopeConstant => true;
        internal override Int32 GetVolume() => Volume;
    }

    internal class DecayingPulseNote : PulseNote
    {
        internal override Boolean EnvelopeConstant => false;
        internal override Int32 GetVolume()
        {
            switch(Length)
            {
                case NoteLength.Whole: return 15;
                case NoteLength.Half: return 8;
                case NoteLength.Quarter: return 4;
                case NoteLength.Eigth: return 2;
                case NoteLength.Sixteenth: return 1;
                default: return 15;
            }
        }
    }

    internal abstract class PulseNote : SoundDirectingEvent
    {
        internal Note Note { get; set; }
        [DefaultValue(Alteration.None)]
        internal Alteration Alteration { get; set; } = Alteration.None;
        internal NoteLength Length { get; set; }
        internal abstract Int32 GetVolume();
        [JsonIgnore]
        internal abstract Boolean EnvelopeConstant { get; }
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
                Value = GetVolume()
            };
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundChannel,
                Parameter = SoundChannelParameter.EnvelopeConstant,
                Value = EnvelopeConstant ? 1 : 0,
            };
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundChannel,
                Parameter = SoundChannelParameter.LengthCounter,
                Value = 160 / (Int32)Length
            };
            yield break;
        }
    }
}
