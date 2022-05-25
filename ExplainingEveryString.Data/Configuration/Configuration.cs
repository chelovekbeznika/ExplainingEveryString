
using System;

namespace ExplainingEveryString.Data.Configuration
{
    public class Configuration
    {
        public InputConfiguration Input { get; set; }
        public CameraConfiguration Camera { get; set; }
        public ScreenConfiguration Screen { get; set; }
        public Single InterfaceAlpha { get; set; }
        public Single SoundFadingOut { get; set; }
        public Single MusicVolume { get; set; }
        public Single SoundVolume { get; set; }
        public (Int32, Int32, Int32) LevelTitleBackgroundColor { get; set; }
    }
}