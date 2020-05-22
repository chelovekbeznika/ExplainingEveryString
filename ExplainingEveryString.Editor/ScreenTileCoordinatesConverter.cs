using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Editor
{
    internal class CoordinatesConverter
    {
        private IScreenCoordinatesMaster screenCoordinatesMaster;
        internal TileWrapper TileWrapper { get; private set; }
        internal Vector2 ScreenBottomRight => new Vector2(screenCoordinatesMaster.ScreenCovers.Width, screenCoordinatesMaster.ScreenCovers.Height);

        internal CoordinatesConverter(IScreenCoordinatesMaster screenCoordinatesMaster, TileWrapper tileWrapper)
        {
            this.screenCoordinatesMaster = screenCoordinatesMaster;
            this.TileWrapper = tileWrapper;
        }

        internal PositionOnTileMap GetLevelPosition(Vector2 screenPosition)
        {
            var levelPosition = screenCoordinatesMaster.ConvertToLevelPosition(screenPosition);
            return TileWrapper.GetTilePosition(levelPosition);
        }

        internal Vector2 GetScreenPosition(PositionOnTileMap tileMapPosition)
        {
            var levelPosition = TileWrapper.GetLevelPosition(tileMapPosition);
            return screenCoordinatesMaster.ConvertToScreenPosition(levelPosition);
        }

        /// <summary>
        /// This is for position on screen
        /// </summary>
        internal Vector2 HalfSpriteOffsetToLeftUpperCorner => 
            new Vector2(-TileWrapper.TiledMap.TileWidth / 2, -TileWrapper.TiledMap.TileHeight / 2);
    }
}
