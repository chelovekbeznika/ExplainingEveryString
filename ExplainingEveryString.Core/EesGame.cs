using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
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
        private GameplayComponent gameplayComponent;
        private InterfaceComponent interfaceComponent;

        internal AssetsStorage AssetsStorage { get; private set; }

        public EesGame()
        {
            ConfigurationAccess.InitializeConfig();
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = config.ScreenHeight;
            graphics.PreferredBackBufferWidth = config.ScreenWidth;
            graphics.IsFullScreen = config.FullScreen;
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            blueprintsLoader = BlueprintsAccess.GetLoader();
            blueprintsLoader.Load();

            gameplayComponent = new GameplayComponent(blueprintsLoader, "level_01.dat", this);
            interfaceComponent = new InterfaceComponent(this);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!Components.Contains(gameplayComponent))
            {
                Components.Add(gameplayComponent);
                Components.Add(interfaceComponent);
            }
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
