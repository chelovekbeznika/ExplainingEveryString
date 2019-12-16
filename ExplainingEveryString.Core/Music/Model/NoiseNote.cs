using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Music.Model
{
    internal class NoiseNote : BpmSoundDirectingEvent
    {
        public Int32 NoiseType { get; set; }
        public Boolean LoopedNoise { get; set; }
        public Int32 Volume { get; set; }
        public NoteLength Length { get; set; }

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundComponentType.Noise,
                Parameter = SoundChannelParameter.Timer,
                Value = NoiseType
            };
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundComponentType.Noise,
                Parameter = SoundChannelParameter.NoiseMode,
                Value = LoopedNoise ? 1 : 0
            };
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundComponentType.Noise,
                Parameter = SoundChannelParameter.Volume,
                Value = Volume
            };
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset + NoteLengthInSamples(Length),
                SoundComponent = SoundComponentType.Noise,
                Parameter = SoundChannelParameter.Volume,
                Value = 0
            };
        }
    }
}
