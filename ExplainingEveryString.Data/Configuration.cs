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
        public Single InterfaceAlpha { get; set; }
        public Int32 ScreenWidth { get; set; }
        public Int32 ScreenHeight { get; set; }
        public Boolean FullScreen { get; set; }
    }
}