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
        private GameplayComponent gameplayComponent;
        private InterfaceComponent interfaceComponent;
        private MenuComponent menuComponent;
        private MenuInputProcessor menuInputProcessor;
        private GameState GameState = GameState.Menu;

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

            gameplayComponent = new GameplayComponent(this, blueprintsLoader, "level_11.dat");
            interfaceComponent = new InterfaceComponent(this, gameplayComponent);
            menuComponent = new MenuComponent(this);

            menuInputProcessor = new MenuInputProcessor(ConfigurationAccess.GetCurrentConfig());
            menuInputProcessor.OnExit += (sender, e) => Exit();
            menuInputProcessor.OnPause += (sender, e) => SwitchGameState();
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
            if (!Components.Contains(gameplayComponent))
            {
                Components.Add(gameplayComponent);
                Components.Add(interfaceComponent);
                Components.Add(menuComponent);
                SwitchMenuRelatedComponents(true);
                SwitchGameplayRelatedComponents(false);
            }
            menuInputProcessor.Update();
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

        private void SwitchGameState ()
        {
            GameState newState = GameState == GameState.Gameplay ? GameState.Menu : GameState.Gameplay;
            GameState = newState;
            switch (newState)
            {
                case GameState.Menu:
                    SwitchGameplayRelatedComponents(false);
                    SwitchMenuRelatedComponents(true);
                    break;
                case GameState.Gameplay:
                    SwitchGameplayRelatedComponents(true);
                    SwitchMenuRelatedComponents(false);
                    break;
            }
        }

        private void SwitchGameplayRelatedComponents(Boolean active)
        {
            gameplayComponent.Enabled = active;
            interfaceComponent.Enabled = active;
            gameplayComponent.Visible = active;
            interfaceComponent.Visible = active;
        }

        private void SwitchMenuRelatedComponents(Boolean active)
        {
            menuComponent.Enabled = active;
            menuComponent.Visible = active;
        }
    }

    public enum GameState { Gameplay, Menu }
}
