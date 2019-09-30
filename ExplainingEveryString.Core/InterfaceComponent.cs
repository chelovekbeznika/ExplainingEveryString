using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Data;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Configuration;
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
        private InterfaceSpriteDisplayer interfaceSpritesDisplayer;
        private InterfaceInfo interfaceInfo;
        private EesGame eesGame;
        private SpriteBatch spriteBatch;
        private Color alphaMask;

        private HealthBarDisplayer healthBarDisplayer;
        private DashStateDisplayer dashStateDisplayer;
        private GameTimeDisplayer gameTimeDisplayer;
        private EnemyInfoDisplayer enemiesInfoDisplayer;

        internal InterfaceComponent(EesGame eesGame) : base(eesGame)
        {
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            this.eesGame = eesGame;
            this.alphaMask = new Color(Color.White, config.InterfaceAlpha);
            this.SetGameplayComponentToDraw(null);
            this.DrawOrder = ComponentsOrder.Interface;
            this.UpdateOrder = ComponentsOrder.Interface;
        }

        internal void SetGameplayComponentToDraw(GameplayComponent gameplayComponent)
        {
            this.gameplayComponent = gameplayComponent;
            if (gameplayComponent == null)
            {
                this.Enabled = false;
                this.Visible = false;
            }
        }

        protected override void LoadContent()
        {
            Dictionary<String, SpriteData> sprites = GetSprites();
            spriteBatch = new SpriteBatch(eesGame.GraphicsDevice);
            interfaceSpritesDisplayer = new InterfaceSpriteDisplayer(spriteBatch, alphaMask);

            healthBarDisplayer = new HealthBarDisplayer(interfaceSpritesDisplayer, sprites);
            dashStateDisplayer = new DashStateDisplayer(healthBarDisplayer, interfaceSpritesDisplayer, sprites);
            enemiesInfoDisplayer = new EnemyInfoDisplayer(interfaceSpritesDisplayer, sprites);
            SpriteFont timeFont = eesGame.Content.Load<SpriteFont>(@"TimeFont");           
            gameTimeDisplayer = new GameTimeDisplayer(timeFont);

            base.LoadContent();
        }

        private Dictionary<String, SpriteData> GetSprites()
        {
            IAssetsMetadataLoader metadataLoader = AssetsMetadataAccess.GetLoader();
            SpriteDataBuilder spriteDataBuilder = new SpriteDataBuilder(Game.Content, metadataLoader);
            String[] animatedSprites = new String[] 
            {
                HealthBarDisplayer.HealthBarTexture, HealthBarDisplayer.EmptyHealthBarTexture,
                EnemyInfoDisplayer.HealthBarTexture,
                DashStateDisplayer.ActiveTexture, DashStateDisplayer.AvailableTexture,
                DashStateDisplayer.CooldownTexture, DashStateDisplayer.NonAvailableTexture
            }.Select(textureName => TexturesHelper.GetFullName(textureName)).ToArray();           
            return spriteDataBuilder.Build(animatedSprites);
        }

        public override void Update(GameTime gameTime)
        {
            interfaceInfo = gameplayComponent.GetInterfaceInfo();
            this.interfaceSpritesDisplayer.Update((Single)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (interfaceInfo != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
                enemiesInfoDisplayer.Draw(interfaceInfo.Enemies);
                healthBarDisplayer.Draw(interfaceInfo.Player);
                dashStateDisplayer.Draw(interfaceInfo.Player);
                gameTimeDisplayer.Draw(interfaceInfo.GameTime, spriteBatch, alphaMask);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
