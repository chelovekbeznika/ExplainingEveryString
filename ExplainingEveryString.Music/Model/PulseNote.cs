using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Music.Model
{
    public class PulseNote : BpmSoundDirectingEvent, INote
    {
        public Note Note { get; set; }
        [DefaultValue(Accidental.None)]
        public Accidental Accidental { get; set; }
        public NoteLength Length { get; set; }
        [DefaultValue(15)]
        public Int32 Volume { get; set; }
        [DefaultValue(true)]
        public Boolean Decaying { get; set; }
        [DefaultValue(null)]
        public Int32? Duty { get; set; }
        [DefaultValue(true)]
        public Boolean FirstChannel { get; set; }

        private SoundComponentType SoundChannel => FirstChannel ? SoundComponentType.Pulse1 : SoundComponentType.Pulse2;

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return GetPulseChannelEvent(SoundChannelParameter.Timer, NotesHelper.PulseTimer(Note, Accidental));
            yield return GetPulseChannelEvent(SoundChannelParameter.Volume, Volume);
            if (Duty.HasValue)
                yield return GetPulseChannelEvent(SoundChannelParameter.Duty, Duty.Value);
            yield return GetPulseChannelEvent(SoundChannelParameter.EnvelopeConstant, Decaying ? 0 : 1);
            yield return GetPulseChannelEvent(SoundChannelParameter.Timer, 0, false);
            yield break;
        }

        private RawSoundDirectingEvent GetPulseChannelEvent(SoundChannelParameter parameter, Int32 value, Boolean inBeginning = true)
        {
            return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = inBeginning ? SamplesOffset : SamplesOffset + NoteLengthInSamples(Length),
                SoundComponent = SoundChannel,
                Parameter = parameter,
                Value = value
            };
        }
    }
}
