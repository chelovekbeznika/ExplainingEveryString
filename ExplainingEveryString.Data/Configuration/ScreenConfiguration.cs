using System;
using System.Text.Json.Serialization;

namespace ExplainingEveryString.Data.Configuration
{
    public class ScreenConfiguration
    {
        //NES aspect ratio is 8:7. We're emulating this.
        [JsonIgnore]
        public Int32 TargetWidth => 1024;
        [JsonIgnore]
        public Int32 TargetHeight => 896;
        public Int32 ScreenWidth { get; set; }
        public Int32 ScreenHeight { get; set; }
        public Boolean FullScreen { get; set; }
    }
}
