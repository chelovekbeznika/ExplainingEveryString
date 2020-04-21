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

        public static void InitializeConfig()
        {
            var fileName = FileNames.Configuration;
            try
            {
                if (File.Exists(fileName))
                {
                    configuration = JsonDataAccessor.Instance.Load<Configuration>(fileName);
                }
                else
                {
                    configuration = GetDefaultConfig();
                    JsonDataAccessor.Instance.Save(fileName, configuration);
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
                    ControlDevice = ControlDevice.Keyboard,
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
                    ScreenHeight = 768
                },
                InterfaceAlpha = 0.75F,
                SoundFadingOut = 1000,
            };
        }
    }
}
