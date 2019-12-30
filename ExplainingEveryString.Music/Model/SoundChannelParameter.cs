using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExplainingEveryString.Music.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SoundChannelParameter
    {
        /// <summary>
        /// Controls frequency in pulse (8-2047), triangle (8-2047), noise (0-15) and delta modulation (0-15) channels
        /// </summary>
        Timer,
        /// <summary>
        /// Pulse channel waveform (0-3)
        /// </summary>
        Duty,
        /// <summary>
        /// 0-15 (0-127 for delta modulation). Could be volume value, could be decay speed determinator
        /// </summary>
        Volume,
        /// <summary>
        /// 0-1 flag, 0 - gives more random and pleasant noise
        /// </summary>
        NoiseMode,
        /// <summary>
        /// Used by pulse and triangle. WARNING! Here is difference from actual sound chip. You supply value directly instead of lookup table index.
        /// </summary>
        LengthCounter,
        /// <summary>
        /// If set stops length counter and loops envelope decay
        /// </summary>
        HaltLoopFlag,
        /// <summary>
        /// Constant flag for envelope control in pulse and noise channel. See https://wiki.nesdev.com/w/index.php/APU_Envelope for more info
        /// </summary>
        EnvelopeConstant,
        /// <summary>
        /// If set gives 120/240 HZ. If clear 96/192 HZ.
        /// </summary>
        FrameCounterMode,
        /// <summary>
        /// https://wiki.nesdev.com/w/index.php/APU_Sweep
        /// </summary>
        SweepEnabled,
        /// <summary>
        /// https://wiki.nesdev.com/w/index.php/APU_Sweep
        /// </summary>
        SweepPeriod,
        /// <summary>
        /// https://wiki.nesdev.com/w/index.php/APU_Sweep
        /// </summary>
        SweepAmount,
        /// <summary>
        /// https://wiki.nesdev.com/w/index.php/APU_Sweep
        /// </summary>
        SweepNegate,
        /// <summary>
        /// Number of delta sample to play
        /// </summary>
        CurrentSample,
        /// <summary>
        /// Status controlling parameter
        /// </summary>
        Pulse1Enabled,
        /// <summary>
        /// Status controlling parameter
        /// </summary>
        Pulse2Enabled,
        /// <summary>
        /// Status controlling parameter
        /// </summary>
        TriangleEnabled,
        /// <summary>
        /// Status controlling parameter
        /// </summary>
        NoiseEnabled,
        /// <summary>
        /// Status controlling parameter
        /// </summary>
        DeltaEnabled
    }
}
