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
        private GraphicsDeviceManager graphics;

        private Camera camera;
        private Dictionary<String, Texture2D> spritesStorage = new Dictionary<String, Texture2D>();
        private Level level;

        public EesGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            String fileName = "config.dat";
            ConfigurationAccess.InitializeConfig(fileName);
            level = new Level();
            level.Lost += GameLost;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            camera = new Camera(level, GraphicsDevice, spritesStorage,
                config.PlayerFramePercentageWidth, config.PlayerFramePercentageHeigth);

            spritesStorage[Player.CommonSpriteName] = Content.Load<Texture2D>(@"Sprites/Rectangle");
            spritesStorage[Mine.CommonSpriteName] = Content.Load<Texture2D>(@"Sprites/Mine");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            level.Update((Single)gameTime.ElapsedGameTime.TotalSeconds);
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
