using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Core.Menu.Settings;
using ExplainingEveryString.Core.Notifications;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuComponent : DrawableGameComponent
    {
        private Texture2D background;
        private SpriteBatch spriteBatch;
        private MenuVisiblePart visiblePart;
        private ScreenConfiguration screenConfig;
        private MenuItemsContainer CurrentButtonsContainer => visiblePart.CurrentButtonsContainer;
        private EesGame game;
        private InnerMenuInputProcessor menuInputProcessor;
        private LevelSequnceSpecification levelSequenceSpecification;
        private MusicTestButtonSpecification[] musicTestSpecification;

        internal MenuComponent(EesGame game, LevelSequnceSpecification levelSequenceSpecification, 
            MusicTestButtonSpecification[] musicTestSpecification) : base(game)
        {
            var config = ConfigurationAccess.GetCurrentConfig();
            this.game = game;
            this.levelSequenceSpecification = levelSequenceSpecification;
            this.musicTestSpecification = musicTestSpecification;
            this.DrawOrder = ComponentsOrder.Menu;
            this.UpdateOrder = ComponentsOrder.Menu;
            this.menuInputProcessor = new InnerMenuInputProcessor();
            SettingsAccess.InitSettingsFromConfiguration(config);
            InitMenuInput(menuInputProcessor);
        }

        protected override void LoadContent()
        {
            this.background = Game.Content.Load<Texture2D>(@"Sprites/Menu/Background");
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            this.visiblePart = InitializeVisiblePart();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            menuInputProcessor.Update(gameTime);
            base.Update(gameTime);
        }

        private MenuVisiblePart InitializeVisiblePart()
        {
            var content = game.Content;
            var menuBuild = new MenuBuilder(game, 
                new LevelSelectMenuBuilder(game, levelSequenceSpecification),
                new MusicTestMenuBuilder(game, musicTestSpecification),
                new SettingsMenuBuilder(game, SaveSettingsHandler));
            this.screenConfig = ConfigurationAccess.GetCurrentConfig().Screen;
            Point screenSizeAccessor() => new Point(screenConfig.TargetWidth, screenConfig.TargetHeight);
            var positionsMapper = new MenuItemPositionsMapper(screenSizeAccessor, 16);
            var menuItemDisplayer = new MenuItemDisplayer(spriteBatch, 
                borderPart: content.Load<Texture2D>(@"Sprites/Menu/SelectedButtonBorder"),
                left: content.Load<Texture2D>(@"Sprites/Menu/Settings/Left"),
                right: content.Load<Texture2D>(@"Sprites/Menu/Settings/Right"));
            return new MenuVisiblePart(menuBuild, positionsMapper, menuItemDisplayer);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, screenConfig.TargetWidth, screenConfig.TargetHeight), Color.White);
            visiblePart.Draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        internal void ReturnMenuToDefaultStateAtPause()
        {
            visiblePart.ReturnToRoot();
            CurrentButtonsContainer.SelectDefaultButton();
        }

        internal void InitMenuInput(InnerMenuInputProcessor menuInputProcessor)
        {
            menuInputProcessor.Down.ButtonPressed += (sender, e) => CurrentButtonsContainer.SelectNextButton();
            menuInputProcessor.Up.ButtonPressed += (sender, e) => CurrentButtonsContainer.SelectPreviousButton();
            menuInputProcessor.Left.ButtonPressed += (sender, e) => CurrentButtonsContainer.Decrement();
            menuInputProcessor.Right.ButtonPressed += (sender, e) => CurrentButtonsContainer.Increment();
            menuInputProcessor.Accept.ButtonPressed += (sender, e) => CurrentButtonsContainer.RequestSelectedCommandExecution();
            menuInputProcessor.Back.ButtonPressed += (sender, e) => visiblePart.TryToGetBack();
        }

        private void SaveSettingsHandler(object sender, EventArgs e)
        {
            var config = ConfigurationAccess.GetCurrentConfig();
            SettingsAccess.SettingsIntoConfiguration(config);
            ConfigurationAccess.SaveCurrentConfig();
            game.ConfigChanged();
            game.GameState.SendGlobalNotification(NotificationType.SettingsApplied);
            visiblePart.TryToGetBack();
        }
    }
}
