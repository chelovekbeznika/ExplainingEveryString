using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Core
{
    internal abstract class CutSceneComponent : DrawableGameComponent
    {
        private readonly Single maxLifeTime = 5f;
        private readonly Single minLifeTime = 0.333333f;
        private Single lifeTime = 0;
        private Boolean somethingPressed = false;
        private Color background;
        private SpriteBatch spriteBatch;

        internal Boolean Closed => lifeTime >= maxLifeTime || somethingPressed;

        public CutSceneComponent(Game game, Single minLifeTime, Single maxLifeTime) : base(game)
        {
            this.minLifeTime = minLifeTime;
            this.maxLifeTime = maxLifeTime;
        }

        public override void Initialize()
        {
            var configuration = ConfigurationAccess.GetCurrentConfig();
            var (red, green, blue) = configuration.LevelTitleBackgroundColor;
            this.background = new Color(red, green, blue);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
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
            DrawCutScene(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected abstract void DrawCutScene(SpriteBatch spriteBatch);
    }
}
