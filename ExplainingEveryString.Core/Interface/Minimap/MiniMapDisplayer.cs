using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Minimap
{
    internal class MiniMapDisplayer
    {
        private const Int32 MinimapSize = 256;
        private readonly TiledMapRenderer renderer;
        private readonly Matrix minimapPositioning;
        private readonly GraphicsDevice graphicsDevice;
        private readonly TiledMap tiledMap;

        internal MiniMapDisplayer(TileWrapper map, Game gameApp)
        {
            this.graphicsDevice = gameApp.GraphicsDevice;
            this.renderer = new TiledMapRenderer(graphicsDevice, map.TiledMap);
            this.tiledMap = map.TiledMap;
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
            var savedOpacity = new Dictionary<String, Single>();
            foreach (var layer in tiledMap.Layers)
            {
                savedOpacity.Add(layer.Name, layer.Opacity);
                layer.Opacity = 0.5F;
            }
                
            renderer.Draw(minimapPositioning);

            foreach(var layer in tiledMap.Layers)
                layer.Opacity = savedOpacity[layer.Name];
        }
    }
}
