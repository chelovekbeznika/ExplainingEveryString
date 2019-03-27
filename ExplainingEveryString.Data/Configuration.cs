using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ExplainingEveryString.Data
{
    public class Configuration
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ControlDevice ControlDevice { get; set; }
        public Int32 PlayerFramePercentageWidth { get; set; }
        public Int32 PlayerFramePercentageHeigth { get; set; }
    }
}