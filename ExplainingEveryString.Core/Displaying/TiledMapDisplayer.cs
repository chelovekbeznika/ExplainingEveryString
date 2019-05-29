using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
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

        internal TiledMapDisplayer(LevelData levelData, EesGame eesGame, Camera camera)
        {
            this.map = eesGame.Content.Load<TiledMap>(levelData.TileMap);
            this.upperLeftCorner = levelData.TileMapUpperLeftCorner;
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
