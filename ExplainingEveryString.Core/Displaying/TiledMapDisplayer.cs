using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Displaying
{
    internal class TiledMapDisplayer
    {
        private TileWrapper map;
        private TiledMapRenderer renderer;
        private IScreenCoordinatesMaster screenCoordinatesMaster;

        internal TiledMapDisplayer(TileWrapper map, EesGame eesGame, IScreenCoordinatesMaster camera)
        {
            this.map = map;
            this.renderer = new TiledMapRenderer(eesGame.GraphicsDevice);
            this.screenCoordinatesMaster = camera;
        }

        internal void Update(GameTime gameTime)
        {
            renderer.Update(map.TiledMap, gameTime);
        }

        internal void Draw()
        {
            Vector2 renderPosition = screenCoordinatesMaster.ConvertToScreenPosition(new Vector2(0, map.Bounds.Height));
            Matrix viewMatrix = Matrix.CreateTranslation(renderPosition.X, renderPosition.Y, 0);
            renderer.Draw(map.TiledMap, viewMatrix, null);
        }
    }
}
