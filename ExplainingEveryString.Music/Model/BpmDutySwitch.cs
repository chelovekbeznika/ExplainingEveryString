using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Music.Model
{
    public class BpmDutySwitch : BpmSoundDirectingEvent
    {
        public Boolean FirstChannel { get; set; }
        public Int32 Duty { get; set; }

        public override IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return new RawSoundDirectingEvent
            {
                Seconds = Seconds,
                SamplesOffset = SamplesOffset,
                SoundComponent = FirstChannel ? SoundComponentType.Pulse1 : SoundComponentType.Pulse2,
                Parameter = SoundChannelParameter.Duty,
                Value = Duty
            };
            yield break;
        }
    }
}
