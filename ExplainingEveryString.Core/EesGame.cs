using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ExplainingEveryString.Core
{
    public class EesGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player player;
        private List<GameObject> gameObjects;
        private Dictionary<String, Texture2D> spritesStorage = new Dictionary<String, Texture2D>();

        private IEnumerable<GameObject> GameObjects { get => gameObjects; }

        internal Dictionary<String, Texture2D> SpritesStorage { get => spritesStorage; }
       
        public EesGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            String fileName = "config.dat";
            ConfigurationAccess.InitializeConfig(fileName);
            InitializeGameObjects();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spritesStorage["player"] = Content.Load<Texture2D>(@"Sprites/Rectangle");
            spritesStorage["mine"] = Content.Load<Texture2D>(@"Sprites/Mine");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Move(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach (GameObject go in GameObjects)
                go.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void InitializeGameObjects()
        {
            player = new Player(this);
            gameObjects = new List<GameObject>() {
                player,
                new Mine(this, new Vector2(100, 100)),
                new Mine(this, new Vector2(200, 200))
            };
        }
    }
}
