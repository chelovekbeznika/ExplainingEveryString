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

        private Player player;
        private List<GameObject> gameObjects;
        private Camera camera;
        private Dictionary<String, Texture2D> spritesStorage = new Dictionary<String, Texture2D>();

        private IEnumerable<GameObject> GameObjects { get => gameObjects; }
       
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
            camera = new Camera(player, GraphicsDevice, spritesStorage);
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

            player.Move(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            camera.Begin();
            foreach (GameObject go in GameObjects)
                camera.Draw(go);
            camera.End();

            base.Draw(gameTime);
        }

        private void InitializeGameObjects()
        {
            player = new Player();
            gameObjects = new List<GameObject>() {
                player,
                new Mine(new Vector2(100, 100)),
                new Mine(new Vector2(200, 200)),
                new Mine(new Vector2(-300, -150))
            };
        }
    }
}
