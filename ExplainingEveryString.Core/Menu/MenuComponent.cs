using ExplainingEveryString.Core.Math;
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
        private EesGame game;

        internal Int32 SelectedItemIndex { get => visiblePart.SelectedIndex; set => visiblePart.SelectedIndex = value; }

        internal MenuComponent(EesGame game) : base(game)
        {
            this.game = game;
            this.DrawOrder = ComponentsOrder.Menu;
            this.UpdateOrder = ComponentsOrder.Menu;
        }

        public override void Initialize()
        {
            this.background = Game.Content.Load<Texture2D>(@"Sprites/Menu/Background");
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            this.visiblePart = InitializeVisiblePart();
            base.Initialize();
        }

        private MenuVisiblePart InitializeVisiblePart()
        {
            Texture2D borderPart = game.Content.Load<Texture2D>(@"Sprites/Menu/SelectedButtonBorder");
            MenuBuilder menuBuild = new MenuBuilder(game);
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
            visiblePart.RequestSelectedCommandExecution();
        }
    }
}
