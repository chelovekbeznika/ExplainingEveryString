using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Music.Model
{
    public class PulseFluteNote : BpmSoundDirectingEvent, INote
    {
        public Note Note { get; set; }
        [DefaultValue(Accidental.None)]
        public Accidental Accidental { get; set; }
        public NoteLength Length { get; set; }
        [DefaultValue(15)]
        public Boolean Stressed { get; set; }
        [DefaultValue(Articulation.NonLegato)]
        public Articulation Articulation { get; set; }
        [DefaultValue(null)]
        public Int32? Duty { get; set; }
        [DefaultValue(true)]
        public Boolean FirstChannel { get; set; }

        private SoundComponentType SoundChannel => FirstChannel ? SoundComponentType.Pulse1 : SoundComponentType.Pulse2;

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return GetPulseChannelEvent(SoundChannelParameter.Timer, NotesHelper.PulseTimer(Note, Accidental));
            if (Duty.HasValue)
                yield return GetPulseChannelEvent(SoundChannelParameter.Duty, Duty.Value);
            yield return GetPulseChannelEvent(SoundChannelParameter.EnvelopeConstant, 1);

            yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 3, 1, 15);
            yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 6, 2, 15);
            yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 9, 1, 5);
            yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 11, 4, 15);
            yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 13, 1, 3);
            yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 15, 2, 5);
            switch (Articulation)
            {
                case Articulation.Legato:
                    yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 10, 4, 5);
                    yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 7, 13, 15);
                    yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 3, 14, 15);
                    break;
                case Articulation.NonLegato:
                    yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 10, 4, 5);
                    yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 7, 13, 15);
                    yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 3, 14, 15);
                    yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 0, 1, 1);
                    break;
                case Articulation.Stacatto:
                    yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 8, 3, 5);
                    yield return GetPulseChannelEvent(SoundChannelParameter.Volume, 0, 10, 15);
                    break;
            }
            yield break;
        }

        private RawSoundDirectingEvent GetPulseChannelEvent(SoundChannelParameter parameter, 
            Int32 value, Int32 numenator = 0, Int32 denominator = 1)
        {
            if (parameter == SoundChannelParameter.Volume && !Stressed && value > 2)
                value -= 2;
            return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset + NoteLengthInSamples(Length) * numenator / denominator,
                SoundComponent = SoundChannel,
                Parameter = parameter,
                Value = value
            };
        }
    }
}
