﻿using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuComponent : DrawableGameComponent
    {
        private Texture2D background;
        private SpriteBatch spriteBatch;
        private MenuVisiblePart visiblePart;
        private MenuItemsContainer CurrentButtonsContainer => visiblePart.CurrentButtonsContainer;
        private EesGame game;
        private InnerMenuInputProcessor menuInputProcessor;
        private LevelSequnceSpecification levelSequenceSpecification;

        internal MenuComponent(EesGame game, LevelSequnceSpecification levelSequenceSpecification) : base(game)
        {
            this.game = game;
            this.levelSequenceSpecification = levelSequenceSpecification;
            this.DrawOrder = ComponentsOrder.Menu;
            this.UpdateOrder = ComponentsOrder.Menu;
            this.menuInputProcessor = new InnerMenuInputProcessor(ConfigurationAccess.GetCurrentConfig());
            InitMenuInput(menuInputProcessor);
        }

        public override void Initialize()
        {
            this.background = Game.Content.Load<Texture2D>(@"Sprites/Menu/Background");
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            this.visiblePart = InitializeVisiblePart();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            menuInputProcessor.Update(gameTime);
            base.Update(gameTime);
        }

        private MenuVisiblePart InitializeVisiblePart()
        {
            Texture2D borderPart = game.Content.Load<Texture2D>(@"Sprites/Menu/SelectedButtonBorder");
            MenuBuilder menuBuild = new MenuBuilder(game, new LevelSelectMenuBuilder(game, levelSequenceSpecification));
            Rectangle screen = game.GraphicsDevice.Viewport.Bounds;
            MenuItemPositionsMapper positionsMapper = new MenuItemPositionsMapper(new Point(screen.Width, screen.Height), 16);
            MenuItemDisplayer menuItemDisplayer = new MenuItemDisplayer(borderPart, spriteBatch);
            return new MenuVisiblePart(menuBuild, positionsMapper, menuItemDisplayer);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, game.GraphicsDevice.Viewport.Bounds, Color.White);
            visiblePart.Draw();
            spriteBatch.End();
            base.Draw(gameTime);
        }

        internal void Accept()
        {
            CurrentButtonsContainer.RequestSelectedCommandExecution();
        }

        internal void ReturnMenuToDefaultStateAtPause()
        {
            CurrentButtonsContainer.SelectDefaultButton();
        }

        internal void InitMenuInput(InnerMenuInputProcessor menuInputProcessor)
        {
            menuInputProcessor.Down.ButtonPressed += (sender, e) => CurrentButtonsContainer.SelectNextButton();
            menuInputProcessor.Up.ButtonPressed += (sender, e) => CurrentButtonsContainer.SelectPreviousButton();
            menuInputProcessor.Accept.ButtonPressed += (sender, e) => Accept();
            menuInputProcessor.Back.ButtonPressed += (sender, e) => visiblePart.TryToGetBack();
        }
    }
}
