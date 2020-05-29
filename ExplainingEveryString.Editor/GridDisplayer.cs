using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Editor
{
    internal class GridDisplayer
    {
        private const Int32 GridStepInTiles = 4;
        private TileWrapper map;
        private CoordinatesConverter coordinatesConverter;
        private Texture2D gridCorner;

        internal GridDisplayer(TileWrapper map, CoordinatesConverter coordinatesConverter, ContentManager content)
        {
            this.map = map;
            this.coordinatesConverter = coordinatesConverter;
            this.gridCorner = content.Load<Texture2D>(@"Sprites/Editor/GridCorner");
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var oneTileOffset = new Vector2(0, map.TiledMap.TileHeight);
            var upperLeftScreenTile = coordinatesConverter.ScreenToTile(Vector2.Zero + oneTileOffset);
            var bottomRigthScreenTile = coordinatesConverter.ScreenToTile(coordinatesConverter.ScreenBottomRight + oneTileOffset);
            var textureCenter = new Vector2(gridCorner.Width / 2, gridCorner.Height / 2);

            for (var yTile = upperLeftScreenTile.Y; yTile <= bottomRigthScreenTile.Y; yTile += GridStepInTiles)
                for (var xTile = upperLeftScreenTile.X; xTile <= bottomRigthScreenTile.X; xTile += GridStepInTiles)
                {
                    var screenPosition = coordinatesConverter.TileToScreen(new PositionOnTileMap { X = xTile, Y = yTile }) 
                        + coordinatesConverter.HalfSpriteOffsetToLeftUpperCorner;
                    spriteBatch.Draw(gridCorner, screenPosition, null, Color.White, 0, textureCenter, Vector2.One, SpriteEffects.None, 0);
                }
        }
    }
}
