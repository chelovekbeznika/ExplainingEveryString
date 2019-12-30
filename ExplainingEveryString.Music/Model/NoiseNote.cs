using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Music.Model
{
    public class NoiseNote : BpmSoundDirectingEvent
    {
        public Int32 NoiseType { get; set; }
        public Boolean LoopedNoise { get; set; }
        public Int32 Volume { get; set; }
        public NoteLength Length { get; set; }

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return GetNoiseChannelEvent(SoundChannelParameter.Timer, NoiseType);
            yield return GetNoiseChannelEvent(SoundChannelParameter.NoiseMode, LoopedNoise ? 1 : 0);
            yield return GetNoiseChannelEvent(SoundChannelParameter.Volume, Volume);
            yield return GetNoiseChannelEvent(SoundChannelParameter.Volume, 0, false);
            yield break;
        }

        private RawSoundDirectingEvent GetNoiseChannelEvent(SoundChannelParameter parameter, Int32 value, Boolean inBeginning = true)
        {
            return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = inBeginning ? SamplesOffset : SamplesOffset + NoteLengthInSamples(Length),
                SoundComponent = SoundComponentType.Noise,
                Parameter = parameter,
                Value = value
            };
        }
    }
}
