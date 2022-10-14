using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Music.Model
{
    public class SpecialPulseNote : BpmSoundDirectingEvent, INote
    {
        public Note Note { get; set; }
        public Accidental Accidental { get; set; }
        public NoteLength Length { get; set; }
        [DefaultValue(Articulation.NonLegato)]
        public Articulation Articulation { get; set; }
        public FourForFourBeats Beat { get; set; }
        public Int32? Duty { get; set; }
        [DefaultValue(true)]
        public Boolean FirstChannel { get; set; }
        private SoundComponentType SoundChannel => FirstChannel ? SoundComponentType.Pulse1 : SoundComponentType.Pulse2;

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return GetPulseChannelEvent(SoundChannelParameter.Timer, NotesHelper.PulseTimer(Note, Accidental));
            if (Duty.HasValue)
                yield return GetPulseChannelEvent(SoundChannelParameter.Duty, Duty.Value);
            switch (Articulation)
            {
                case Articulation.NonLegato:
                    yield return GetVolumeEvent(11, 0, 10);
                    yield return GetVolumeEvent(13, 1, 20);
                    yield return GetVolumeEvent(15, 1, 10);
                    yield return GetVolumeEvent(14, 2, 10);
                    yield return GetVolumeEvent(13, 3, 10);
                    yield return GetVolumeEvent(12, 4, 10);
                    yield return GetVolumeEvent(13, 6, 10);
                    yield return GetVolumeEvent(14, 7, 10);
                    yield return GetVolumeEvent(15, 8, 10);
                    yield return GetVolumeEvent(13, 9, 10);
                    yield return GetVolumeEvent(11, 19, 20);
                    yield return GetVolumeEvent(0, 39, 40);
                    break;
                case Articulation.Legato:
                    yield return GetVolumeEvent(13, 0, 10);
                    yield return GetVolumeEvent(14, 1, 10);
                    yield return GetVolumeEvent(15, 2, 10);
                    yield return GetVolumeEvent(14, 4, 10);
                    yield return GetVolumeEvent(15, 6, 10);
                    yield return GetVolumeEvent(14, 8, 10);
                    yield return GetVolumeEvent(13, 9, 10);
                    yield return GetVolumeEvent(0, 1, 1);
                    break;
                case Articulation.Stacatto:
                    yield return GetVolumeEvent(11, 0, 20);
                    yield return GetVolumeEvent(15, 2, 20);
                    yield return GetVolumeEvent(13, 3, 20);
                    yield return GetVolumeEvent(15, 4, 20);
                    yield return GetVolumeEvent(13, 5, 20);
                    yield return GetVolumeEvent(15, 6, 20);
                    yield return GetVolumeEvent(13, 7, 20);
                    yield return GetVolumeEvent(15, 8, 20);
                    yield return GetVolumeEvent(13, 9, 20);
                    yield return GetVolumeEvent(15, 10, 20);
                    yield return GetVolumeEvent(13, 11, 20);
                    yield return GetVolumeEvent(15, 12, 20);
                    yield return GetVolumeEvent(13, 13, 20);
                    yield return GetVolumeEvent(15, 14, 20);
                    yield return GetVolumeEvent(0, 15, 20);
                    break;
                case Articulation.Portato:
                    yield return GetVolumeEvent(13, 0, 10);
                    yield return GetVolumeEvent(14, 1, 10);
                    yield return GetVolumeEvent(15, 2, 10);
                    yield return GetVolumeEvent(14, 3, 10);
                    yield return GetVolumeEvent(13, 4, 10);
                    yield return GetVolumeEvent(14, 5, 10);
                    yield return GetVolumeEvent(15, 6, 10);
                    yield return GetVolumeEvent(0, 9, 10);
                    break;
            }
        }

        private RawSoundDirectingEvent GetVolumeEvent(Int32 volume, Int32 numenator, Int32 denominator) =>
            GetPulseChannelEvent(SoundChannelParameter.Volume, volume, numenator, denominator);

        private RawSoundDirectingEvent GetPulseChannelEvent(SoundChannelParameter parameter,
            Int32 value, Int32 numenator = 0, Int32 denominator = 1)
        {
            if (parameter == SoundChannelParameter.Volume)
            {
                value = Beat switch
                {
                    FourForFourBeats.Strong => value,
                    FourForFourBeats.Weak => value -= 3,
                    FourForFourBeats.Middle => value -= 1,
                    _ => value,
                };
                if (value < 0)
                    value = 0;
            }
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
