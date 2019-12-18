
using System;

namespace ExplainingEveryString.Data.Configuration
{
    public class Configuration
    {
        public ControlDevice ControlDevice { get; set; }
        public CameraConfiguration Camera { get; set; }
        public ScreenConfiguration Screen { get; set; }
        public Single InterfaceAlpha { get; set; }
        public Single SoundFadingOut { get; set; }
    }
}