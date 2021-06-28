using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core
{
    internal class LevelTitleComponent : DrawableGameComponent
    {
        private const Single maxLifeTime = 5.0f;
        private const Single minLifeTime = 0.333333f;

        private SpriteBatch spriteBatch;
        private Texture2D levelTitle;
        private Single lifeTime = 0;
        private Boolean somethingPressed = false;
        private String levelFileName;
        private String levelTitleName;
        private Color background;
        private readonly Vector2 screenCenter = new Vector2(Displaying.Constants.TargetWidth / 2, Displaying.Constants.TargetHeight / 2);


        internal Boolean Closed => lifeTime >= maxLifeTime || somethingPressed;

        internal LevelTitleComponent(EesGame eesGame, String levelFileName) : base(eesGame)
        {
            this.UpdateOrder = ComponentsOrder.Title;
            this.DrawOrder = ComponentsOrder.Title;
            this.levelFileName = levelFileName;
        }

        public override void Initialize()
        {
            var configuration = ConfigurationAccess.GetCurrentConfig();
            var (red, green, blue) = configuration.LevelTitleBackgroundColor;
            this.background = new Color(red, green, blue);
            var levelLoader = LevelDataAccess.GetLevelLoader();
            var levelData = levelLoader.Load(levelFileName);
            this.levelTitleName = levelData.TitleSpriteName;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.levelTitle = Game.Content.Load<Texture2D>(levelTitleName);
        }

        public override void Update(GameTime gameTime)
        {
            lifeTime += (Single)gameTime.ElapsedGameTime.TotalSeconds;
            if (lifeTime >= minLifeTime)
            {
                somethingPressed |= GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A)
                    || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start)
                    || Keyboard.GetState().IsKeyDown(Keys.Space)
                    || Keyboard.GetState().IsKeyDown(Keys.Enter)
                    || Keyboard.GetState().IsKeyDown(Keys.Escape);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);
            spriteBatch.Begin();
            var spriteCenter = new Vector2(levelTitle.Width / 2, levelTitle.Height / 2);
            spriteBatch.Draw(levelTitle, screenCenter, null, Color.White, 0, spriteCenter, 1, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
