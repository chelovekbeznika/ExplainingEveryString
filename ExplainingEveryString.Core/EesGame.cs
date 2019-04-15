using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data;
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

        private Dictionary<String, Texture2D> spritesStorage = new Dictionary<String, Texture2D>();
        private Dictionary<String, SoundEffect> soundsStorage = new Dictionary<String, SoundEffect>();
        private IBlueprintsLoader blueprintsLoader;
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
            blueprintsLoader = BlueprintsAccess.GetBlueprintsLoader();
            blueprintsLoader.Load();

            GameObjectsFactory factory = new GameObjectsFactory(blueprintsLoader);
            level = new Level(factory);
            level.Lost += GameLost;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            Camera = new Camera(level, GraphicsDevice, spritesStorage,
                config.PlayerFramePercentageWidth, config.PlayerFramePercentageHeigth);
            EpicEventsProcessor = new EpicEventsProcessor(soundsStorage, level);

            FillAssetsStorages();
        }

        private void FillAssetsStorages()
        {
            foreach (String spriteName in blueprintsLoader.GetNeccessarySprites())
            {
                spritesStorage[spriteName] = Content.Load<Texture2D>(spriteName);
            }
            foreach (String soundName in blueprintsLoader.GetNecessarySounds())
            {
                soundsStorage[soundName] = Content.Load<SoundEffect>(soundName);
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            level.Update((Single)gameTime.ElapsedGameTime.TotalSeconds);
            Camera.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Camera.Draw();
            EpicEventsProcessor.ProcessEpicEvents();
            base.Draw(gameTime);
        }

        private void GameLost(Object sender, EventArgs args)
        {
            Exit();
        }
    }
}
