using System;
using System.IO;

namespace ExplainingEveryString.Data.Configuration
{
    public static class ConfigurationAccess
    {
        private static Configuration configuration = null;

        public static Configuration GetCurrentConfig()
        {
            return configuration;
        }

        public static void SaveCurrentConfig()
        {
            JsonDataAccessor.Instance.Save(FileNames.Configuration, configuration);
        }

        public static void InitializeConfig()
        {
            try
            {
                if (File.Exists(FileNames.Configuration))
                {
                    configuration = JsonDataAccessor.Instance.Load<Configuration>(FileNames.Configuration);
                }
                else
                {
                    configuration = GetDefaultConfig();
                    SaveCurrentConfig();
                }
            }
            catch (Exception)
            {
                configuration = GetDefaultConfig();
            }
        }

        private static Configuration GetDefaultConfig()
        {
            return new Configuration()
            {
                Input = new InputConfiguration
                {
                    PreferredControlDevice = ControlDevice.Keyboard,
                    TimeToFocusOnKeyboard = 0.25F,
                    TimeToFocusOnGamepad = 0.25F
                },
                Camera = new CameraConfiguration
                {
                    PlayerFramePercentageWidth = 60,
                    PlayerFramePercentageHeight = 60,
                    TimeToReverseFocusDirection = 0.5F
                },
                Screen = new ScreenConfiguration
                {
                    FullScreen = false,
                    ScreenWidth = 1024,
                    ScreenHeight = 768,
                    TargetWidth = 1024,
                    TargetHeight = 768
                },
                InterfaceAlpha = 0.75F,
                SoundFadingOut = 1000,
            };
        }
    }
}
