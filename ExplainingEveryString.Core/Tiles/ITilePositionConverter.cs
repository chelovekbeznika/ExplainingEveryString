using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.Tiles
{
    interface ITilePositionConverter
    {
        Vector2 GetPosition(PositionOnTileMap tilePosition);
    }
}
