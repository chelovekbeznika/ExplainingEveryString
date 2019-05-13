using ExplainingEveryString.Core.Interface;
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
        private Texture2D healthBar;
        private Texture2D emptyHealthBar;
        private EesGame eesGame;
        private SpriteBatch spriteBatch;
        private Single alpha;

        internal InterfaceComponent(EesGame eesGame, GameplayComponent gameplayComponent) : base(eesGame)
        {
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            this.eesGame = eesGame;
            this.gameplayComponent = gameplayComponent;
            this.alpha = config.InterfaceAlpha;
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(eesGame.GraphicsDevice);
            healthBar = eesGame.Content.Load<Texture2D>(@"Sprites/Interface/HealthBar");
            emptyHealthBar = eesGame.Content.Load<Texture2D>(@"Sprites/Interface/EmptyHealthBar");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            interfaceInfo = gameplayComponent.GetInterfaceInfo();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 healthBarPosition = CalculatePlaceForHealthBar(spriteBatch.GraphicsDevice.Viewport);
            Rectangle healthBarPart = CalculateHealthBarDrawPart();
            Vector2 emptyHealthBarPosition = healthBarPosition;
            emptyHealthBarPosition.X += healthBarPart.Width;
            Rectangle emptyHealthBarPart = new Rectangle
            {
                X = healthBarPart.Width,
                Y = 0,
                Height = healthBarPart.Height,
                Width = healthBar.Width - healthBarPart.Width
            };

            Color interfaceMask = new Color(Color.White, alpha);
            spriteBatch.Begin();
            spriteBatch.Draw(healthBar, healthBarPosition, healthBarPart, interfaceMask);
            spriteBatch.Draw(emptyHealthBar, emptyHealthBarPosition, emptyHealthBarPart, interfaceMask);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private Vector2 CalculatePlaceForHealthBar(Viewport viewport)
        {
            Int32 x = 32;
            Int32 y = viewport.Height - 32 - healthBar.Height;
            return new Vector2(x, y);
        }

        private Rectangle CalculateHealthBarDrawPart()
        {
            Single healthRemained = interfaceInfo.Health / interfaceInfo.MaxHealth;
            return new Rectangle
            {
                X = 0,
                Y = 0,
                Height = healthBar.Height,
                Width = (Int32)(healthBar.Width * healthRemained)
            };
        }
    }
}
