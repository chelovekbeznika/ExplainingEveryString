using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExplainingEveryString.Music.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SoundChannelParameter
    {
        Timer,
        Duty,
        Volume,
        NoiseMode,
        LengthCounter,
        HaltLoopFlag,
        EnvelopeConstant,
        FrameCounterMode,
        SweepEnabled,
        SweepPeriod,
        SweepAmount,
        SweepNegate,
        CurrentSample,
        Pulse1Enabled,
        Pulse2Enabled,
        TriangleEnabled,
        NoiseEnabled,
        DeltaEnabled
    }
}
