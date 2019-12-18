using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Music.Model
{
    public class RawSoundDirectingEvent : ISoundDirectingSequence
    {
        public Int32 Seconds { get; set; }
        public Int32 SamplesOffset { get; set; }
        [JsonIgnore]
        public Int32 Position => Seconds * Constants.SampleRate + SamplesOffset;

        public SoundComponentType SoundComponent { get; set; }
        public SoundChannelParameter Parameter { get; set; }
        public Int32 Value { get; set; }

        public IEnumerable<RawSoundDirectingEvent> GetEvents()
        {
            yield return this;
            yield break;
        }
    }
}
