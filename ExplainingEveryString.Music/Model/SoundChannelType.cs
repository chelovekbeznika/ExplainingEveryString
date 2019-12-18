using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExplainingEveryString.Music.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SoundComponentType { FrameCounter, Pulse1, Pulse2, Triangle, Noise, DeltaModulation, Status }
}
