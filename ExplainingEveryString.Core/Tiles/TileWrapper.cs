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
    internal class TileWrapper : ITilePositionConverter
    {
        internal TiledMap TiledMap { get; }
        internal Int32 MapHeight => TiledMap.Height * TiledMap.TileHeight;

        internal TileWrapper(TiledMap map)
        {
            this.TiledMap = map;
        }

        public Vector2 GetPosition(PositionOnTileMap tilePosition)
        {
            Vector2 upperLeftCorner = TileCoordinatesToLevelCoordinates(tilePosition.X, tilePosition.Y);
            Vector2 center = upperLeftCorner + new Vector2 { X = TiledMap.TileWidth / 2, Y = -TiledMap.TileHeight / 2 };
            return center + tilePosition.Offset;
        }

        internal List<Point> GetPitTiles()
        {
            return GetSpecificTiles("Floor", "Pit");
        }

        internal List<Point> GetWallTiles()
        {
            return GetSpecificTiles("Walls", "Wall");
        }

        private List<Point> GetSpecificTiles(String layerName, String property)
        {
            List<Point> result = new List<Point>();
            TiledMapTileLayer layer = TiledMap.TileLayers.FirstOrDefault(tl => tl.Name == layerName);
            if (layer != null)
                foreach (Int32 row in Enumerable.Range(0, TiledMap.Height))
                    foreach (Int32 column in Enumerable.Range(0, TiledMap.Width))
                    {
                        if (layer.TryGetTile(column, row, out TiledMapTile? wallTile))
                        {
                            Int32 tileId = wallTile.Value.GlobalIdentifier;
                            if (ContainsProperty(TiledMap, tileId, property))
                                result.Add(new Point { X = column, Y = row });
                        }
                    }
            return result;
        }

        private Boolean ContainsProperty(TiledMap map, Int32 tileId, String property)
        {
            TiledMapTileset tileset = map.GetTilesetByTileGlobalIdentifier(tileId);
            if (tileset != null)
            {
                TiledMapTilesetTile tilesetTile = tileset.Tiles.FirstOrDefault
                    (tst => tst.LocalTileIdentifier == tileId - tileset.FirstGlobalIdentifier);
                if (tilesetTile != null && tilesetTile.Properties.ContainsKey(property))
                    return Boolean.Parse(tilesetTile.Properties[property]);
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
