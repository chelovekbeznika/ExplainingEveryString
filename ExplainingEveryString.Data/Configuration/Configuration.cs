
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Configuration
{
    public class Configuration
    {
        public InputConfiguration Input { get; set; }
        public CameraConfiguration Camera { get; set; }
        public ScreenConfiguration Screen { get; set; }
        public Single InterfaceAlpha { get; set; }
        public Single SoundFadingOut { get; set; }
        [DefaultValue(0.25)]
        public Single MusicVolume { get; set; }
    }
}