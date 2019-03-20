using ExplainingEveryString.Core.Blueprints;
using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
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
        private IBlueprintsLoader blueprintsLoader = new HardCodeBlueprintsLoader();
        private Level level;
        private Camera camera;

        internal Camera Camera => camera;

        public EesGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            CurrentlyRunnedGame = this;
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            String fileName = "config.dat";
            ConfigurationAccess.InitializeConfig(fileName);

            blueprintsLoader = new HardCodeBlueprintsLoader();
            blueprintsLoader.Load();

            GameObjectsFactory factory = new GameObjectsFactory(blueprintsLoader);
            level = new Level(factory);
            level.Lost += GameLost;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            camera = new Camera(level, GraphicsDevice, spritesStorage,
                config.PlayerFramePercentageWidth, config.PlayerFramePercentageHeigth);

            FillSpritesStorage();
        }

        private void FillSpritesStorage()
        {
            foreach (String spriteName in blueprintsLoader.GetNeccessarySprites())
            {
                spritesStorage[spriteName] = Content.Load<Texture2D>(spriteName);
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
            camera.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            camera.Draw();
            base.Draw(gameTime);
        }

        private void GameLost(Object sender, EventArgs args)
        {
            Exit();
        }
    }
}
