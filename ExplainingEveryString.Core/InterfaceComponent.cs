﻿using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Data;
using Microsoft.Xna.Framework;
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
        private GameplayComponent gameplayComponent;
        private InterfaceInfo interfaceInfo;
        private EesGame eesGame;
        private SpriteBatch spriteBatch;
        private HealthBarDisplayer healthBarDisplayer;
        private GameTimeDisplayer gameTimeDisplayer;
        private Color alphaMask;

        internal InterfaceComponent(EesGame eesGame, GameplayComponent gameplayComponent) : base(eesGame)
        {
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            this.eesGame = eesGame;
            this.gameplayComponent = gameplayComponent;
            this.alphaMask = new Color(Color.White, config.InterfaceAlpha);
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(eesGame.GraphicsDevice);
            Texture2D healthBarSprite = eesGame.Content.Load<Texture2D>(@"Sprites/Interface/HealthBar");
            Texture2D emptyHealthBarSprite = eesGame.Content.Load<Texture2D>(@"Sprites/Interface/EmptyHealthBar");
            SpriteFont timeFont = eesGame.Content.Load<SpriteFont>(@"TimeFont");
            healthBarDisplayer = new HealthBarDisplayer(healthBarSprite, emptyHealthBarSprite);
            gameTimeDisplayer = new GameTimeDisplayer(timeFont);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            interfaceInfo = gameplayComponent.GetInterfaceInfo();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            healthBarDisplayer.Draw(interfaceInfo.Health, interfaceInfo.MaxHealth, spriteBatch, alphaMask);
            gameTimeDisplayer.Draw(interfaceInfo.GameTime, spriteBatch, alphaMask);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
