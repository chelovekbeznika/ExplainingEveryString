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
                    TimeToFocusOnGamepad = 0.25F,
                    BetweenPlayerAndCursor = 80,
                    GamepadCameraSpeed = 384
                },
                Camera = new CameraConfiguration
                {
                    PlayerFramePercentageWidth = 80,
                    PlayerFramePercentageHeight = 80,
                    TimeToReverseFocusDirection = 0.5F
                },
                Screen = new ScreenConfiguration
                {
                    FullScreen = false,
                    ScreenWidth = 1024,
                    ScreenHeight = 768
                },
                PersonalBestCelebration = new RecordFireworkConfiguration
                {
                    Volume = 0.25f,
                    SoundCooldown = new RandomVariables.GaussRandomVariable
                    {
                        ExpectedValue = 0.5f,
                        Sigma = 0.1f
                    },
                    BetweenSpawns = new RandomVariables.GaussRandomVariable
                    {
                        ExpectedValue = 0.066667f,
                        Sigma = 0.05f
                    },
                    LifeTime = new RandomVariables.GaussRandomVariable
                    {
                        ExpectedValue = 0.5f,
                        Sigma = 0.1f
                    }
                },
                InterfaceAlpha = 0.75F,
                SoundFadingOut = 1000,
                SaveProfile = 0,
                LevelTitleBackgroundColor = (64, 146, 195)
            };
        }
    }
}
