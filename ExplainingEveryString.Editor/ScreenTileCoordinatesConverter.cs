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

        internal CoordinatesConverter(IScreenCoordinatesMaster screenCoordinatesMaster, TileWrapper tileWrapper)
        {
            this.screenCoordinatesMaster = screenCoordinatesMaster;
            this.tileWrapper = tileWrapper;
        }

        internal PositionOnTileMap GetLevelPosition(Vector2 screenPosition)
        {
            var levelPosition = screenCoordinatesMaster.ConvertToLevelPosition(screenPosition);
            return tileWrapper.GetTilePosition(levelPosition);
        }

        internal Vector2 GetScreenPosition(PositionOnTileMap tileMapPosition)
        {
            var levelPosition = tileWrapper.GetLevelPosition(tileMapPosition);
            return screenCoordinatesMaster.ConvertToScreenPosition(levelPosition);
        }

        /// <summary>
        /// This is for position on screen
        /// </summary>
        internal Vector2 HalfSpriteOffsetToLeftUpperCorner => 
            new Vector2(-tileWrapper.TiledMap.TileWidth / 2, -tileWrapper.TiledMap.TileHeight / 2);
    }
}
