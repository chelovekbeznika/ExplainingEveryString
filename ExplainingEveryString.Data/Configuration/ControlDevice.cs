using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExplainingEveryString.Data.Configuration
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ControlDevice
    {
        GamePad,
        Keyboard
    }
}
