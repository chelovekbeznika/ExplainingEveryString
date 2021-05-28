using ExplainingEveryString.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core.Menu.Settings
{
    internal static class SettingsConfigurationConverter
    {
        internal static CurrentSettings SettingsFromConfiguration(Configuration config)
        {
            return new CurrentSettings
            {
                MusicVolume = (Int32)(config.MusicVolume * CurrentSettings.MaxSoundBars),
            };
        }

        internal static void SettingsIntoConfiguration(Configuration config, CurrentSettings settings)
        {
            config.MusicVolume = (Single)settings.MusicVolume / CurrentSettings.MaxSoundBars;
        }
    }
}
