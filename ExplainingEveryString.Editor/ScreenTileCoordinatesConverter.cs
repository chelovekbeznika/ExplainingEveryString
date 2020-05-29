using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Editor
{
    internal class CoordinatesConverter
    {
        private IScreenCoordinatesMaster screenCoordinatesMaster;
        private TileWrapper tileWrapper;
        internal Vector2 ScreenBottomRight => new Vector2(screenCoordinatesMaster.ScreenCovers.Width, screenCoordinatesMaster.ScreenCovers.Height);

        internal CoordinatesConverter(IScreenCoordinatesMaster screenCoordinatesMaster, TileWrapper tileWrapper)
        {
            this.screenCoordinatesMaster = screenCoordinatesMaster;
            this.tileWrapper = tileWrapper;
        }

        internal PositionOnTileMap ScreenToTile(Vector2 screenPosition)
        {
            var levelPosition = screenCoordinatesMaster.ConvertToLevelPosition(screenPosition);
            return tileWrapper.GetTilePosition(levelPosition);
        }

        internal Vector2 TileToScreen(PositionOnTileMap tileMapPosition)
        {
            var levelPosition = tileWrapper.GetLevelPosition(tileMapPosition);
            return screenCoordinatesMaster.ConvertToScreenPosition(levelPosition);
        }

        internal Vector2 ScreenToLevel(Vector2 screenPosition) => screenCoordinatesMaster.ConvertToLevelPosition(screenPosition);

        internal Vector2 LevelToScreen(Vector2 levelPoisition) => screenCoordinatesMaster.ConvertToScreenPosition(levelPoisition);

        internal PositionOnTileMap LevelToTile(Vector2 levelPosition) => tileWrapper.GetTilePosition(levelPosition);

        internal Vector2 TileToLevel(PositionOnTileMap positionOnTileMap) => tileWrapper.GetLevelPosition(positionOnTileMap);

        /// <summary>
        /// This is for position on screen
        /// </summary>
        internal Vector2 HalfSpriteOffsetToLeftUpperCorner => 
            new Vector2(-tileWrapper.TiledMap.TileWidth / 2, -tileWrapper.TiledMap.TileHeight / 2);
    }
}
