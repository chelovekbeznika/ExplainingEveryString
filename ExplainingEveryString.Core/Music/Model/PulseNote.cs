using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Music.Model
{
    internal class PulseNote : SoundDirectingEvent
    {
        internal Note Note { get; set; }
        [DefaultValue(Alteration.None)]
        internal Alteration Alteration { get; set; } = Alteration.None;
        internal NoteLength Length { get; set; }
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
                Value = 15
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
