﻿using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Interface.Displayers;
using ExplainingEveryString.Core.Interface.Minimap;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Interface
{
    internal class InterfaceComponent : DrawableGameComponent
    {
        private readonly EesGame eesGame;
        private GameplayComponent gameplayComponent;
        private InterfaceDrawController interfaceSpritesDisplayer;
        private InterfaceInfo interfaceInfo;
        private InterfaceGameTimeInfo gameTimeInfo;
        private SpriteBatch spriteBatch;
        private Color alphaMask;

        private HealthBarDisplayer healthBarDisplayer;
        private DashStateDisplayer dashStateDisplayer;
        private GameTimeDisplayer gameTimeDisplayer;
        private EnemyInfoDisplayer enemiesInfoDisplayer;
        private BossInfoDisplayer bossInfoDisplayer;
        private BossInfoDisplayer leftBossInfoDisplayer;
        private BossInfoDisplayer rightBossInfoDisplayer;
        private BossInfoDisplayer leftOfThreeBossInfoDisplayer;
        private BossInfoDisplayer rightOfThreeBossInfoDisplayer;
        private BossInfoDisplayer centerOfThreeBossInfoDisplayer;
        private HomingTargetDisplayer homingTargetDisplayer;
        private EnemiesBehindScreenDisplayer enemiesBehindScreenDisplayer;
        private RemainedWeaponsDisplayer remainedWeaponsDisplayer;
        private AmmoStockDisplayer ammoStockDisplayer;
        private CursorDisplayer cursorDisplayer;
        private ReloadDisplayer reloadDisplayer;
        private CheckpointDisplayer checkpointDisplayer;
        private Dictionary<string, IWeaponDisplayer> playerWeaponDisplayers;

        private MiniMapDisplayer minimapDisplayer = null;

        internal InterfaceComponent(EesGame eesGame) : base(eesGame)
        {
            var config = ConfigurationAccess.GetCurrentConfig();
            this.eesGame = eesGame;
            alphaMask = new Color(Color.White, config.InterfaceAlpha);
            SetGameplayComponentToDraw(null);
            DrawOrder = ComponentsOrder.Interface;
            UpdateOrder = ComponentsOrder.Interface;
        }

        internal void SetGameplayComponentToDraw(GameplayComponent gameplayComponent)
        {
            this.gameplayComponent = gameplayComponent;
            if (gameplayComponent == null)
            {
                Enabled = false;
                Visible = false;
            }
            else
            {
                gameplayComponent.ContentLoaded += (sender, e) =>
                {
                    minimapDisplayer = new MiniMapDisplayer(interfaceSpritesDisplayer, gameplayComponent.Map, Game);
                    InitDisplayers(new[] { minimapDisplayer });
                };
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(eesGame.GraphicsDevice);
            interfaceSpritesDisplayer = new InterfaceDrawController(spriteBatch, alphaMask);

            var displayers = CreateDisplayers();
            InitDisplayers(displayers);

            base.LoadContent();
        }

        private IEnumerable<IDisplayer> CreateDisplayers()
        {
            healthBarDisplayer = new HealthBarDisplayer(interfaceSpritesDisplayer);
            dashStateDisplayer = new DashStateDisplayer(healthBarDisplayer, interfaceSpritesDisplayer);
            enemiesInfoDisplayer = new EnemyInfoDisplayer(interfaceSpritesDisplayer);
            bossInfoDisplayer = new BossInfoDisplayer(interfaceSpritesDisplayer, BossInfoDisplayer.OneBossPrefix, 0, DrainType.Left);
            leftBossInfoDisplayer = new BossInfoDisplayer(interfaceSpritesDisplayer, BossInfoDisplayer.LeftBossPrefix, -160, DrainType.Left);
            rightBossInfoDisplayer = new BossInfoDisplayer(interfaceSpritesDisplayer, BossInfoDisplayer.RightBossPrefix, 160, DrainType.Right);
            leftOfThreeBossInfoDisplayer = new BossInfoDisplayer(interfaceSpritesDisplayer, BossInfoDisplayer.LeftOfThreeBossPrefix, -224, DrainType.Left);
            rightOfThreeBossInfoDisplayer = new BossInfoDisplayer(interfaceSpritesDisplayer, BossInfoDisplayer.RightOfThreeBossPrefix, 224, DrainType.Right);
            centerOfThreeBossInfoDisplayer = new BossInfoDisplayer(interfaceSpritesDisplayer, BossInfoDisplayer.CenterOfThreeBossPrefix, 0, DrainType.Center);
            enemiesBehindScreenDisplayer = new EnemiesBehindScreenDisplayer(interfaceSpritesDisplayer);
            homingTargetDisplayer = new HomingTargetDisplayer(interfaceSpritesDisplayer);
            remainedWeaponsDisplayer = new RemainedWeaponsDisplayer(interfaceSpritesDisplayer, eesGame.FontsStorage.LevelTime);
            ammoStockDisplayer = new AmmoStockDisplayer(interfaceSpritesDisplayer);
            cursorDisplayer = new CursorDisplayer(interfaceSpritesDisplayer);
            reloadDisplayer = new ReloadDisplayer(interfaceSpritesDisplayer);
            checkpointDisplayer = new CheckpointDisplayer(interfaceSpritesDisplayer);
            gameTimeDisplayer = new GameTimeDisplayer(eesGame.FontsStorage.LevelTime);
            playerWeaponDisplayers = new Dictionary<string, IWeaponDisplayer>
            {
                { "Shotgun", new ShotgunDisplayer(interfaceSpritesDisplayer) },
                { "Homing", new HomingDisplayer(interfaceSpritesDisplayer) },
                { "Cone", new ConeDisplayer(interfaceSpritesDisplayer) },
                { "FiveShot", new FiveShotDisplayer(interfaceSpritesDisplayer) }
            };

            return new IDisplayer[]
            {
                healthBarDisplayer, dashStateDisplayer, remainedWeaponsDisplayer,
                ammoStockDisplayer, cursorDisplayer, reloadDisplayer, checkpointDisplayer,
                enemiesInfoDisplayer, bossInfoDisplayer, leftBossInfoDisplayer,
                rightBossInfoDisplayer, enemiesBehindScreenDisplayer, homingTargetDisplayer,
                leftOfThreeBossInfoDisplayer, rightOfThreeBossInfoDisplayer, centerOfThreeBossInfoDisplayer
            }
            .Concat(playerWeaponDisplayers.Values).ToArray();
        }

        private void InitDisplayers(IEnumerable<IDisplayer> displayers)
        {
            var metadataLoader = AssetsMetadataAccess.GetLoader();
            var spriteDataBuilder = new SpriteDataBuilder(Game.Content, metadataLoader);
            var animatedSprites = displayers
                .SelectMany(displayer => displayer.GetSpritesNames())
                .Select(spriteName => TextureLoadingHelper.GetFullName(spriteName));
            var sprites = spriteDataBuilder.Build(animatedSprites);
            foreach (var displayer in displayers)
                displayer.InitSprites(sprites);
        }

        public override void Update(GameTime gameTime)
        {
            interfaceInfo = gameplayComponent.GetInterfaceInfo();
            gameTimeInfo = new InterfaceInfoExtractor().GetInterfaceGameTimeInfo(eesGame.GameState.GameTimeState);
            interfaceSpritesDisplayer.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            minimapDisplayer.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            if (interfaceInfo != null)
            {
                DrawPlayerElements();
                DrawEnemiesElements();
                minimapDisplayer?.Draw(interfaceInfo);
                if (gameTimeInfo != null)
                    gameTimeDisplayer.Draw(gameTimeInfo, spriteBatch, alphaMask);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawPlayerElements()
        {
            healthBarDisplayer.Draw(interfaceInfo.Player);
            dashStateDisplayer.Draw(interfaceInfo.Player);
            var weaponName = interfaceInfo.Player.Weapon.SelectedWeapon;
            if (weaponName != WeaponNames.Default && playerWeaponDisplayers.ContainsKey(weaponName))
                playerWeaponDisplayers[weaponName].Draw(interfaceInfo.Player.Weapon);
            remainedWeaponsDisplayer.Draw(interfaceInfo.Player.Weapon);
            if (interfaceInfo.Player.Weapon.AmmoStock.HasValue)
                ammoStockDisplayer.Draw(interfaceInfo.Player.Weapon.AmmoStock.Value);
            checkpointDisplayer.Draw(interfaceInfo.Player);
            if (interfaceInfo.Player.HomingTarget != null)
                homingTargetDisplayer.Draw(interfaceInfo.Player.HomingTarget);
            cursorDisplayer.Draw(interfaceInfo.Player);
            if (interfaceInfo.Player.Weapon.ReloadRemained.HasValue)
                reloadDisplayer.Draw(interfaceInfo.Player);
        }

        private void DrawEnemiesElements()
        {
            enemiesInfoDisplayer.Draw(interfaceInfo.Enemies);
            enemiesBehindScreenDisplayer.Draw(interfaceInfo.HiddenEnemies);
            if (interfaceInfo.Bosses != null && interfaceInfo.Bosses.Count > 0)
            {
                if (interfaceInfo.Bosses.Count == 1)
                    bossInfoDisplayer.Draw(interfaceInfo.Bosses[0]);
                else if (interfaceInfo.Bosses.Count == 2)
                {
                    leftBossInfoDisplayer.Draw(interfaceInfo.Bosses[0]);
                    rightBossInfoDisplayer.Draw(interfaceInfo.Bosses[1]);
                }
                else
                {
                    leftOfThreeBossInfoDisplayer.Draw(interfaceInfo.Bosses[0]);
                    centerOfThreeBossInfoDisplayer.Draw(interfaceInfo.Bosses[1]);
                    rightOfThreeBossInfoDisplayer.Draw(interfaceInfo.Bosses[2]);
                }
            }
        }
    }
}
