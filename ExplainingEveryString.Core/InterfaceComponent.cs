﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core
{
    internal class InterfaceComponent : DrawableGameComponent
    {
        private Texture2D healthBar;
        private EesGame eesGame;
        private SpriteBatch spriteBatch;

        internal InterfaceComponent(EesGame eesGame) : base(eesGame)
        {
            this.eesGame = eesGame;
            this.spriteBatch = new SpriteBatch(eesGame.GraphicsDevice);
        }

        protected override void LoadContent()
        {
            healthBar = eesGame.Content.Load<Texture2D>(@"Sprites/Interface/HealthBar");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 healthBarPosition = CalculatePlaceForHealthBar(spriteBatch.GraphicsDevice.Viewport);
            spriteBatch.Begin();
            spriteBatch.Draw(healthBar, healthBarPosition, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private Vector2 CalculatePlaceForHealthBar(Viewport viewport)
        {
            Int32 x = 32;
            Int32 y = viewport.Height - 32 - healthBar.Height;
            return new Vector2(x, y);
        }
    }
}
