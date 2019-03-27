using System;
using System.IO;

namespace ExplainingEveryString.Data
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
            String fileName = FileNames.Configuration;
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
