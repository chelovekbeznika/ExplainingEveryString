using System;
using System.Text.Json.Serialization;

namespace ExplainingEveryString.Data.Configuration
{
    public class ScreenConfiguration
    {
        [JsonIgnore]
        public Int32 TargetWidth => ScreenWidth * TargetHeight / ScreenHeight;
        public Int32 TargetHeight { get; set; }
        public Int32 ScreenWidth { get; set; }
        public Int32 ScreenHeight { get; set; }
        public Boolean FullScreen { get; set; }
    }
}
