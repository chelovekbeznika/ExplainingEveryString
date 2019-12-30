﻿using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled.Graphics;

namespace ExplainingEveryString.Core.Displaying
{
    internal class TiledMapDisplayer
    {
        private TileWrapper map;
        private TiledMapRenderer renderer;
        private IScreenCoordinatesMaster screenCoordinatesMaster;

        internal TiledMapDisplayer(TileWrapper map, EesGame eesGame, IScreenCoordinatesMaster screenCoordinatesMaster)
        {
            this.map = map;
            this.renderer = new TiledMapRenderer(eesGame.GraphicsDevice);
            this.screenCoordinatesMaster = screenCoordinatesMaster;
        }

        internal void Update(GameTime gameTime)
        {
            renderer.Update(map.TiledMap, gameTime);
        }

        internal void Draw()
        {
            var renderPosition = screenCoordinatesMaster.ConvertToScreenPosition(new Vector2(0, map.Bounds.Height));
            var viewMatrix = Matrix.CreateTranslation(renderPosition.X, renderPosition.Y, 0);
            renderer.Draw(map.TiledMap, viewMatrix, null);
        }
    }
}
