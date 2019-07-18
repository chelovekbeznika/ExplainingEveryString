using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core
{
    public class EesGame : Game
    {
        private GraphicsDeviceManager graphics;
        private IBlueprintsLoader blueprintsLoader;

        private MenuInputProcessor menuInputProcessor;
        private GameStateManager gameState;

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

            GameplayComponent gameplayComponent = new GameplayComponent(this, blueprintsLoader, "level_11.dat");
            InterfaceComponent interfaceComponent = new InterfaceComponent(this, gameplayComponent);
            MenuComponent menuComponent = new MenuComponent(this);

            this.gameState = new GameStateManager(this, gameplayComponent, interfaceComponent, menuComponent);
            this.menuInputProcessor = new MenuInputProcessor(ConfigurationAccess.GetCurrentConfig());
            gameState.InitMenuInput(menuInputProcessor);
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
                gameState.InitComponents();
            }
            menuInputProcessor.Update((Single)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }

        internal void GameLost(Object sender, EventArgs args)
        {
            Exit();
        }
    }


}
