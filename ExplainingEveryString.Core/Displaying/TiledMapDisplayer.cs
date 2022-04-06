using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled.Renderers;

namespace ExplainingEveryString.Core.Displaying
{
    internal class TiledMapDisplayer
    {
        private readonly TileWrapper map;
        private readonly TiledMapRenderer renderer;
        private readonly IScreenCoordinatesMaster screenCoordinatesMaster;

        public TiledMapDisplayer(TileWrapper map, Game gameApp, IScreenCoordinatesMaster screenCoordinatesMaster)
        {
            this.map = map;
            this.renderer = new TiledMapRenderer(gameApp.GraphicsDevice, map.TiledMap);
            this.screenCoordinatesMaster = screenCoordinatesMaster;
        }

        internal void Update(GameTime gameTime)
        {
            renderer.Update(gameTime);
        }

        internal void Draw()
        {
            var renderPosition = screenCoordinatesMaster.ConvertToScreenPosition(new Vector2(0, map.Bounds.Height));
            var viewMatrix = Matrix.CreateTranslation(renderPosition.X, renderPosition.Y, 0);
            renderer.Draw(viewMatrix, null);
        }
    }
}
