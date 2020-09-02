using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Core.Interface.Displayers;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

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
        private BossInfoDisplayer bossInfoDisplayer;
        private BossInfoDisplayer leftBossInfoDisplayer;
        private BossInfoDisplayer rightBossInfoDisplayer;
        private EnemiesBehindScreenDisplayer enemiesBehindScreenDisplayer;
        private RemainedWeaponsDisplayer remainedWeaponsDisplayer;
        private AmmoStockDisplayer ammoStockDisplayer;
        private ReloadDisplayer reloadDisplayer;
        private Dictionary<String, IWeaponDisplayer> playerWeaponDisplayers;

        internal InterfaceComponent(EesGame eesGame) : base(eesGame)
        {
            var config = ConfigurationAccess.GetCurrentConfig();
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
            spriteBatch = new SpriteBatch(eesGame.GraphicsDevice);
            interfaceSpritesDisplayer = new InterfaceSpriteDisplayer(spriteBatch, alphaMask);

            var displayers = InitDisplayers();

            var metadataLoader = AssetsMetadataAccess.GetLoader();
            var spriteDataBuilder = new SpriteDataBuilder(Game.Content, metadataLoader);
            var animatedSprites = displayers
                .SelectMany(displayer => displayer.GetSpritesNames())
                .Select(spriteName => TextureLoadingHelper.GetFullName(spriteName));
            var sprites = spriteDataBuilder.Build(animatedSprites);
            foreach (var displayer in displayers)
                displayer.InitSprites(sprites);

            base.LoadContent();
        }

        private IEnumerable<IDisplayer> InitDisplayers()
        {
            healthBarDisplayer = new HealthBarDisplayer(interfaceSpritesDisplayer);
            dashStateDisplayer = new DashStateDisplayer(healthBarDisplayer, interfaceSpritesDisplayer);
            enemiesInfoDisplayer = new EnemyInfoDisplayer(interfaceSpritesDisplayer);
            bossInfoDisplayer = new BossInfoDisplayer(interfaceSpritesDisplayer, BossInfoDisplayer.OneBossPrefix, 0);
            leftBossInfoDisplayer = new BossInfoDisplayer(interfaceSpritesDisplayer, BossInfoDisplayer.LeftBossPrefix, -160);
            rightBossInfoDisplayer = new BossInfoDisplayer(interfaceSpritesDisplayer, BossInfoDisplayer.RightBossPrefix, 160);
            enemiesBehindScreenDisplayer = new EnemiesBehindScreenDisplayer(interfaceSpritesDisplayer);
            remainedWeaponsDisplayer = new RemainedWeaponsDisplayer(interfaceSpritesDisplayer);
            ammoStockDisplayer = new AmmoStockDisplayer(interfaceSpritesDisplayer);
            reloadDisplayer = new ReloadDisplayer(interfaceSpritesDisplayer);
            var timeFont = eesGame.Content.Load<SpriteFont>(@"TimeFont");
            gameTimeDisplayer = new GameTimeDisplayer(timeFont);
            playerWeaponDisplayers = new Dictionary<string, IWeaponDisplayer>
            {
                { "Shotgun", new ShotgunDisplayer(interfaceSpritesDisplayer) },
                { "Cone", new ConeDisplayer(interfaceSpritesDisplayer) }
            };

            return new IDisplayer[]
            {
                healthBarDisplayer, dashStateDisplayer, remainedWeaponsDisplayer, ammoStockDisplayer, reloadDisplayer,
                enemiesInfoDisplayer, bossInfoDisplayer, leftBossInfoDisplayer, 
                rightBossInfoDisplayer, enemiesBehindScreenDisplayer
            }
            .Concat(playerWeaponDisplayers.Values).ToArray();
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
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

                //Player
                healthBarDisplayer.Draw(interfaceInfo.Player);
                dashStateDisplayer.Draw(interfaceInfo.Player);
                var weaponName = interfaceInfo.Player.Weapon.SelectedWeapon;
                if (weaponName != GameModel.Constants.DefaultPlayerWeapon && playerWeaponDisplayers.ContainsKey(weaponName))
                    playerWeaponDisplayers[weaponName].Draw(interfaceInfo.Player.Weapon);
                remainedWeaponsDisplayer.Draw(interfaceInfo.Player.Weapon);                if (interfaceInfo.Player.Weapon.AmmoStock.HasValue)
                    ammoStockDisplayer.Draw(interfaceInfo.Player.Weapon.AmmoStock.Value);
                if (interfaceInfo.Player.Weapon.ReloadRemained.HasValue) 
                    reloadDisplayer.Draw(interfaceInfo.Player.Weapon.ReloadRemained.Value);


                //Enemies
                enemiesInfoDisplayer.Draw(interfaceInfo.Enemies);
                enemiesBehindScreenDisplayer.Draw(interfaceInfo.HiddenEnemies);
                if (interfaceInfo.Bosses != null && interfaceInfo.Bosses.Count > 0)
                {
                    if (interfaceInfo.Bosses.Count == 1)
                        bossInfoDisplayer.Draw(interfaceInfo.Bosses[0]);
                    else
                    {
                        leftBossInfoDisplayer.Draw(interfaceInfo.Bosses[0]);
                        rightBossInfoDisplayer.Draw(interfaceInfo.Bosses[1]);
                    }
                }

                gameTimeDisplayer.Draw(interfaceInfo.GameTime, spriteBatch, alphaMask);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
