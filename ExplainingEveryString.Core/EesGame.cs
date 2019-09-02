using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core
{
    public class EesGame : Game
    {
        private GraphicsDeviceManager graphics;
        private IBlueprintsLoader blueprintsLoader;

        private MenuInputProcessor menuInputProcessor;
        internal GameStateManager GameState { get; private set; }

        internal AssetsStorage AssetsStorage { get; private set; }

        public EesGame()
        {
            ConfigurationAccess.InitializeConfig();
            ScreenConfiguration screenConfig = ConfigurationAccess.GetCurrentConfig().Screen;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = screenConfig.ScreenHeight;
            graphics.PreferredBackBufferWidth = screenConfig.ScreenWidth;
            graphics.IsFullScreen = screenConfig.FullScreen;
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            blueprintsLoader = BlueprintsAccess.GetLoader();
            blueprintsLoader.Load();

            ComponentsManager componentsManager = new ComponentsManager(this, blueprintsLoader);

            this.GameState = new GameStateManager(this, componentsManager);
            this.menuInputProcessor = new MenuInputProcessor(ConfigurationAccess.GetCurrentConfig());
            GameState.InitMenuInput(menuInputProcessor);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            IAssetsMetadataLoader metadataLoader = AssetsMetadataAccess.GetLoader();
            AssetsStorage = new AssetsStorage();
            AssetsStorage.FillAssetsStorages(blueprintsLoader, metadataLoader, Content);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Components.Count == 0)
            {
                GameState.InitComponents();
            }
            menuInputProcessor.Update((Single)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
            GameState.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
