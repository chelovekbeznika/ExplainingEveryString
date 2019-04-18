using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
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
        internal static EesGame CurrentlyRunnedGame { get; private set; }

        private GraphicsDeviceManager graphics;

        private IBlueprintsLoader blueprintsLoader;
        private AssetsStorage assetsStorage;
        private Level level;

        internal Camera Camera { get; private set; }
        internal EpicEventsProcessor EpicEventsProcessor { get; private set; }

        public EesGame()
        {
            ConfigurationAccess.InitializeConfig();
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = config.ScreenHeight;
            graphics.PreferredBackBufferWidth = config.ScreenWidth;
            graphics.IsFullScreen = config.FullScreen;
            Content.RootDirectory = "Content";
            CurrentlyRunnedGame = this;
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            blueprintsLoader = BlueprintsAccess.GetLoader();
            blueprintsLoader.Load();

            GameObjectsFactory factory = new GameObjectsFactory(blueprintsLoader);
            level = new Level(factory);
            level.Lost += GameLost;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Configuration config = ConfigurationAccess.GetCurrentConfig();

            IAssetsMetadataLoader metadataLoader = AssetsMetadataAccess.GetLoader();
            assetsStorage = new AssetsStorage();
            assetsStorage.FillAssetsStorages(blueprintsLoader, metadataLoader, Content);

            Camera = new Camera(level, GraphicsDevice, assetsStorage,
                config.PlayerFramePercentageWidth, config.PlayerFramePercentageHeigth);
            EpicEventsProcessor = new EpicEventsProcessor(assetsStorage, level);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Single elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            level.Update(elapsedSeconds);
            Camera.Update();
            EpicEventsProcessor.Update(elapsedSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Camera.Begin();
            Camera.Draw(level.GetObjectsToDraw());
            EpicEventsProcessor.ProcessEpicEvents();
            Camera.Draw(EpicEventsProcessor.GetSpecEffectsToDraw());
            Camera.End();
            base.Draw(gameTime);
        }

        private void GameLost(Object sender, EventArgs args)
        {
            Exit();
        }
    }
}
