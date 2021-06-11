using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace ExplainingEveryString.Core
{
    public class EesApp : Game
    {
        private GraphicsDeviceManager graphics;

        protected void PreInit()
        {
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
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
            if (!ResolutionSupported(screenConfig.ScreenWidth, screenConfig.ScreenHeight))
            {
                var displayMode = graphics.GraphicsDevice.Adapter.SupportedDisplayModes.First();
                screenConfig.ScreenWidth = displayMode.Width;
                screenConfig.ScreenHeight = displayMode.Height;
            }

            graphics.PreferredBackBufferHeight = screenConfig.ScreenHeight;
            graphics.PreferredBackBufferWidth = screenConfig.ScreenWidth;
            graphics.IsFullScreen = screenConfig.FullScreen;
            graphics.ApplyChanges();
        }

        private Boolean ResolutionSupported(Int32 width, Int32 height)
        {
            return graphics.GraphicsDevice.Adapter.SupportedDisplayModes.Any(dp => dp.Width == width && dp.Height == height);
        }
    }
}
