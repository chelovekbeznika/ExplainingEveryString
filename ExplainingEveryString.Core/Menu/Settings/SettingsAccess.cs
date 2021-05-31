using ExplainingEveryString.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal static class SettingsAccess
    {
        private static CurrentSettings settings = null;

        internal static CurrentSettings GetCurrentSettings() => settings;

        internal static void InitSettingsFromConfiguration(Configuration config)
        {
            settings = new CurrentSettings
            {
                MusicVolume = (Int32)(config.MusicVolume * CurrentSettings.MaxSoundBars),
                SoundVolume = (Int32)(config.SoundVolume * CurrentSettings.MaxSoundBars)
            };
        }

        internal static void SettingsIntoConfiguration(Configuration config)
        {
            config.MusicVolume = (Single)settings.MusicVolume / CurrentSettings.MaxSoundBars;
            config.SoundVolume = (Single)settings.SoundVolume / CurrentSettings.MaxSoundBars;
        }
    }
}
