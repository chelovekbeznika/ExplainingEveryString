using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.Tiles
{
    interface ITileCoordinatesMaster
    {
        Rectangle Bounds { get; }
        Vector2 GetLevelPosition(PositionOnTileMap tilePosition);
    }
}
