using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Menu;
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
        private MusicTestButtonSpecification[] musicTestSpecification;

        internal MenuComponent(EesGame game, LevelSequnceSpecification levelSequenceSpecification, 
            MusicTestButtonSpecification[] musicTestSpecification) : base(game)
        {
            this.game = game;
            this.levelSequenceSpecification = levelSequenceSpecification;
            this.musicTestSpecification = musicTestSpecification;
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
            var borderPart = game.Content.Load<Texture2D>(@"Sprites/Menu/SelectedButtonBorder");
            var menuBuild = new MenuBuilder(game, 
                new LevelSelectMenuBuilder(game, levelSequenceSpecification),
                new MusicTestMenuBuilder(game, musicTestSpecification));
            var screen = game.GraphicsDevice.Viewport.Bounds;
            var positionsMapper = new MenuItemPositionsMapper(new Point(screen.Width, screen.Height), 16);
            var menuItemDisplayer = new MenuItemDisplayer(borderPart, spriteBatch);
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
            visiblePart.ReturnToRoot();
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
