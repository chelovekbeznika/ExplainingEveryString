using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Tiles
{
    internal class TileWrapper
    {
        internal TiledMap TiledMap { get; }
        internal Int32 MapHeight => TiledMap.Height * TiledMap.TileHeight;

        internal TileWrapper(TiledMap map)
        {
            this.TiledMap = map;
        }

        internal Vector2 GetPosition(PositionOnTileMap tilePosition)
        {
            Vector2 upperLeftCorner = TileCoordinatesToLevelCoordinates(tilePosition.X, tilePosition.Y);
            Vector2 center = upperLeftCorner + new Vector2 { X = TiledMap.TileWidth / 2, Y = -TiledMap.TileHeight / 2 };
            return center + tilePosition.Offset;
        }

        internal List<Point> GetWallTiles()
        {
            List<Point> result = new List<Point>();
            TiledMapTileLayer wallsLayer = TiledMap.TileLayers.First(tl => tl.Name == "Walls");
            foreach (Int32 row in Enumerable.Range(0, TiledMap.Height))
                foreach (Int32 column in Enumerable.Range(0, TiledMap.Width))
                {
                    if (wallsLayer.TryGetTile(column, row, out TiledMapTile? wallTile))
                    {
                        Int32 tileId = wallTile.Value.GlobalIdentifier;
                        if (IsWall(TiledMap, tileId))
                            result.Add(new Point { X = column, Y = row });
                    }
                }
            return result;
        }

        private Boolean IsWall(TiledMap map, Int32 tileId)
        {
            TiledMapTileset tileset = map.GetTilesetByTileGlobalIdentifier(tileId);
            if (tileset != null)
            {
                TiledMapTilesetTile tilesetTile = tileset.Tiles.FirstOrDefault
                    (tst => tst.LocalTileIdentifier == tileId - tileset.FirstGlobalIdentifier);
                if (tilesetTile != null && tilesetTile.Properties.ContainsKey("Wall"))
                    return Boolean.Parse(tilesetTile.Properties["Wall"]);
                else
                    return false;
            }
            else
                return false;
        }

        internal Hitbox GetHitbox(Rectangle wall)
        {
            Vector2 hitboxUpperLeftCorner = TileCoordinatesToLevelCoordinates(wall.X, wall.Y);
            return new Hitbox
            {
                Top = hitboxUpperLeftCorner.Y,
                Bottom = hitboxUpperLeftCorner.Y - TiledMap.TileHeight * wall.Height,
                Left = hitboxUpperLeftCorner.X,
                Right = hitboxUpperLeftCorner.X + TiledMap.TileWidth * wall.Width
            };
        }

        private Vector2 TileCoordinatesToLevelCoordinates(Int32 x, Int32 y)
        {
            return new Vector2
            {
                X = x * TiledMap.TileWidth,
                Y = MapHeight - y * TiledMap.TileHeight
            };
        }
    }
}
