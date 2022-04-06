using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled.Renderers;
using System;

namespace ExplainingEveryString.Core.Interface.Minimap
{
    internal class MiniMapDisplayer
    {
        private const Int32 MinimapSize = 256;
        private readonly TiledMapRenderer renderer;
        private readonly Matrix minimapPositioning;

        internal MiniMapDisplayer(TileWrapper map, Game gameApp)
        {
            this.renderer = new TiledMapRenderer(gameApp.GraphicsDevice, map.TiledMap);
            var scale = System.Math.Max(map.Bounds.Width / MinimapSize, map.Bounds.Height / MinimapSize);
            var mapHeight = map.Bounds.Height / scale;
            this.minimapPositioning = Matrix.CreateScale(1F / scale) * 
                Matrix.CreateTranslation(Constants.TargetWidth / 2 - MinimapSize / 2, Constants.TargetHeight - mapHeight, 0);
        }

        internal void Update(GameTime gameTime)
        {
            renderer.Update(gameTime);
        }

        internal void Draw()
        {
            renderer.Draw(minimapPositioning);
        }
    }
}
