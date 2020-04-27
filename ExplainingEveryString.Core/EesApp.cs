using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core
{
    public class EesApp : Game
    {
        private GraphicsDeviceManager graphics;

        protected void PreInit()
        {
            ConfigurationAccess.InitializeConfig();
            var screenConfig = ConfigurationAccess.GetCurrentConfig().Screen;
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferHeight = screenConfig.ScreenHeight,
                PreferredBackBufferWidth = screenConfig.ScreenWidth,
                IsFullScreen = screenConfig.FullScreen
            };
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }
    }
}
