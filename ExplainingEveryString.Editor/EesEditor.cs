using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Editor
{
    public class EesEditor : Game
    {
        private readonly GraphicsDeviceManager graphics;

        public EesEditor()
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

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
