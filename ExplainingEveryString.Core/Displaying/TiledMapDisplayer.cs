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
        private TiledMap map;
        private TiledMapRenderer renderer;
        private Camera camera;
        private Vector2 upperLeftCorner;

        internal TiledMapDisplayer(TiledMap map, EesGame eesGame, Camera camera)
        {
            this.map = map;
            this.upperLeftCorner = TileUtility.GetUpperLeftCorner(map);
            this.renderer = new TiledMapRenderer(eesGame.GraphicsDevice);
            this.camera = camera;
        }

        internal void Update(GameTime gameTime)
        {
            renderer.Update(map, gameTime);
        }

        internal void Draw()
        {
            Vector2 renderPosition = camera.ConvertToScreenPosition(upperLeftCorner);
            Matrix viewMatrix = Matrix.CreateTranslation(renderPosition.X, renderPosition.Y, 0);
            renderer.Draw(map, viewMatrix, null);
        }
    }
}
