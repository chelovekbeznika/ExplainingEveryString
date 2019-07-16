using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuComponent : DrawableGameComponent
    {
        private Texture2D background;
        private SpriteBatch spriteBatch;
        private MenuItem[] menu;
        private MenuItemPositionsMapper positionsMapper;

        internal MenuComponent(EesGame game) : base(game)
        {
            Rectangle screen = game.GraphicsDevice.Viewport.Bounds;
            this.positionsMapper = new MenuItemPositionsMapper(new Point(screen.Width, screen.Height), 16);
        }

        public override void Initialize()
        {
            this.background = Game.Content.Load<Texture2D>(@"Sprites/Menu/Background");
            MenuBuilder menuBuild = new MenuBuilder();
            menu = menuBuild.BuildMenu(Game.Content);
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Game.GraphicsDevice.Viewport.Bounds, Color.White);
            DrawMenu(spriteBatch, menu);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawMenu(SpriteBatch spriteBatch, MenuItem[] menu)
        {
            Point[] positions = positionsMapper.GetItemsPositions(menu.Select(item => item.GetSize()).ToArray());
            foreach (var item in menu.Zip(positions, (rawItem, position) => new { rawItem.Sprite, Position = position }))
            {
                Vector2 position = new Vector2(item.Position.X, item.Position.Y);
                spriteBatch.Draw(item.Sprite, position, Color.White);
            }
        }
    }
}
