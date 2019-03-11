using ExplainingEveryString.Core.Input;
using System;
using System.IO;

namespace ExplainingEveryString.Core
{
    internal class Configuration
    {
        internal ControlDevice ControlDevice { get; set; }
        internal Int32 PlayerFramePercentageWidth { get; set; }
        internal Int32 PlayerFramePercentageHeigth { get; set; }
    }

    internal static class ConfigurationAccess
    {
        private static Configuration configuration = null;

        internal static Configuration GetCurrentConfig()
        {
            return configuration;
        }

        internal static void InitializeConfig(String fileName)
        {
            try
            {
                String[] lines = File.ReadAllLines(fileName);
                Configuration config = new Configuration();
                config.ControlDevice = (ControlDevice)Enum.Parse(typeof(ControlDevice), lines[0]);
                config.PlayerFramePercentageHeigth = Int32.Parse(lines[1]);
                config.PlayerFramePercentageWidth = Int32.Parse(lines[2]);
                configuration = config;
            }
            catch (Exception ex)
            {
                using (StreamWriter sw = File.AppendText("log.txt"))
                {
                    sw.WriteLine(ex.Message);
                }
                configuration = GetDefaultConfig();
            }
        }

        private static Configuration GetDefaultConfig()
        {
            return new Configuration()
            {
                ControlDevice = ControlDevice.Keyboard,
                PlayerFramePercentageWidth = 60,
                PlayerFramePercentageHeigth = 60
            };
        }
    }
}