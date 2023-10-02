using ExplainingEveryString.Core.Extensions;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using System.Linq;

namespace ExplainingEveryString.Core
{
    public class EesApp : Game
    {
        private GraphicsDeviceManager graphics;

        protected void PreInit()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
        }

        //This method exists only cause of bug in Monogame 3.8.0. Should be merged with PreInit after update to 3.8.1
        protected void GraphicsPreInit()
        {
            ConfigurationAccess.InitializeConfig();
            var screenConfig = ConfigurationAccess.GetCurrentConfig().Screen;
            ChangeScreenResolution(screenConfig);
        }

        protected void ChangeScreenResolution(ScreenConfiguration screenConfig)
        {
            if (!ResolutionSupported(screenConfig.ScreenWidth, screenConfig.ScreenHeight, screenConfig.FullScreen))
            {
                var displayMode = graphics.GraphicsDevice.Adapter.AllowedResolutions(screenConfig.FullScreen).Last();
                screenConfig.ScreenWidth = displayMode.Width;
                screenConfig.ScreenHeight = displayMode.Height;
            }

            graphics.PreferredBackBufferHeight = screenConfig.ScreenHeight;
            graphics.PreferredBackBufferWidth = screenConfig.ScreenWidth;
            graphics.IsFullScreen = screenConfig.FullScreen;
            graphics.ApplyChanges();
        }

        private bool ResolutionSupported(int width, int height, bool fullscreen)
        {
            return graphics.GraphicsDevice.Adapter.AllowedResolutions(fullscreen)
                .Any(r => r.Width == width && r.Height == height);
        }
    }
}
