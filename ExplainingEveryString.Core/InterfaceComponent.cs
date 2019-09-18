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
        private AnimationController animationController;
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
            SpriteDataBuilder spriteDataBuilder = new SpriteDataBuilder(Game.Content);
            String[] animatedSprites = new String[] { GetTextureFullName("ActiveDash") };
            IAssetsMetadataLoader metadataLoader = AssetsMetadataAccess.GetLoader();
            Dictionary<String, SpriteData> spritesStorage = spriteDataBuilder.Build(animatedSprites, metadataLoader);
            this.animationController = new AnimationController(spritesStorage, Game.GraphicsDevice.RenderTargetCount);

            this.spriteBatch = new SpriteBatch(eesGame.GraphicsDevice);
            Texture2D healthBarSprite = GetTexture("HealthBar");
            Texture2D emptyHealthBarSprite = GetTexture("EmptyHealthBar");
            healthBarDisplayer = new HealthBarDisplayer(healthBarSprite, emptyHealthBarSprite);

            Texture2D availableDash = GetTexture("FullDash");
            Texture2D cooldownDash = GetTexture("EmptyDash");
            Texture2D nonAvailableDash = GetTexture("FullDashNotReady");
            dashStateDisplayer = new DashStateDisplayer(healthBarDisplayer, animationController, 
                availableDash, nonAvailableDash, cooldownDash, GetTextureFullName("ActiveDash"));

            SpriteFont timeFont = eesGame.Content.Load<SpriteFont>(@"TimeFont");           
            gameTimeDisplayer = new GameTimeDisplayer(timeFont);

            Texture2D enemyHealthBar = GetTexture("EnemyHealthBar");
            enemiesInfoDisplayer = new EnemyInfoDisplayer(enemyHealthBar);

            base.LoadContent();
        }

        private Texture2D GetTexture(String name)
        {
            return eesGame.Content.Load<Texture2D>(GetTextureFullName(name));
        }

        private String GetTextureFullName(String name)
        {
            return $@"Sprites/Interface/{name}";
        }

        public override void Update(GameTime gameTime)
        {
            interfaceInfo = gameplayComponent.GetInterfaceInfo();
            this.animationController.Update((Single)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (interfaceInfo != null)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp);
                enemiesInfoDisplayer.Draw(interfaceInfo.Enemies, spriteBatch, alphaMask);
                healthBarDisplayer.Draw(interfaceInfo.Player, spriteBatch, alphaMask);
                dashStateDisplayer.Draw(interfaceInfo.Player, spriteBatch, alphaMask);
                gameTimeDisplayer.Draw(interfaceInfo.GameTime, spriteBatch, alphaMask);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
