using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ExplainingEveryString.Music.Model
{
    public class SwitchChannel : ISoundDirectingSequence
    {
        public Int32 Seconds { get; set; }
        [DefaultValue(0)]
        public Int32 SamplesOffset { get; set; }
        public SoundComponentType Channel { get; set; }
        public Boolean TurnOn { get; set; }

        public IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            SoundChannelParameter channelParameter;
            switch (Channel)
            {
                case SoundComponentType.Pulse1: channelParameter = SoundChannelParameter.Pulse1Enabled; break;
                case SoundComponentType.Pulse2: channelParameter = SoundChannelParameter.Pulse2Enabled; break;
                case SoundComponentType.Triangle: channelParameter = SoundChannelParameter.TriangleEnabled; break;
                case SoundComponentType.Noise: channelParameter = SoundChannelParameter.NoiseEnabled; break;
                case SoundComponentType.DeltaModulation: channelParameter = SoundChannelParameter.DeltaEnabled; break;
                default: throw new ArgumentException(nameof(Channel));
            }
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = SoundComponentType.Status,
                Parameter = channelParameter,
                Value = TurnOn ? 1 : 0
            };
            yield break;
        }
    }
}
