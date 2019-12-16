using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Music.Model
{
    internal class SwitchChannel : ISoundDirectingSequence
    {
        internal Int32 Seconds { get; set; }
        internal Int32 SamplesOffset { get; set; }
        internal SoundComponentType ChannelToTurn { get; set; }
        internal Boolean TurnOn { get; set; }

        public IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            SoundChannelParameter channelParameter;
            switch (ChannelToTurn)
            {
                case SoundComponentType.Pulse1: channelParameter = SoundChannelParameter.Pulse1Enabled; break;
                case SoundComponentType.Pulse2: channelParameter = SoundChannelParameter.Pulse2Enabled; break;
                case SoundComponentType.Triangle: channelParameter = SoundChannelParameter.TriangleEnabled; break;
                case SoundComponentType.Noise: channelParameter = SoundChannelParameter.NoiseEnabled; break;
                case SoundComponentType.DeltaModulation: channelParameter = SoundChannelParameter.DeltaEnabled; break;
                default: throw new ArgumentException(nameof(ChannelToTurn));
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
