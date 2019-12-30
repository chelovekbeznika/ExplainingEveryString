﻿using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core
{
    public class EesGame : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private IBlueprintsLoader blueprintsLoader;
        private OuterMenuInputProcessor menuInputProcessor;

        internal GameStateManager GameState { get; private set; }

        internal AssetsStorage AssetsStorage { get; private set; }

        public EesGame()
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
            blueprintsLoader = BlueprintsAccess.GetLoader();
            blueprintsLoader.Load();

            var levelSequenceSpecification = LevelSequenceAccess.LoadLevelSequence();
            var musicTestSpecification = MusicTestSpecificationAccess.Load();

            var componentsManager =  new ComponentsManager(this, levelSequenceSpecification, 
                musicTestSpecification, blueprintsLoader);

            this.GameState = new GameStateManager(this, levelSequenceSpecification, componentsManager);
            this.menuInputProcessor = new OuterMenuInputProcessor(ConfigurationAccess.GetCurrentConfig());
            menuInputProcessor.Pause.ButtonPressed += (sender, e) => GameState.TryPauseSwitch();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            var metadataLoader = AssetsMetadataAccess.GetLoader();
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
            base.Update(gameTime);
            GameState.Update();
            menuInputProcessor.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
