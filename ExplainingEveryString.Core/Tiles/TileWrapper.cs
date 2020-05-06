using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Tiles
{
    internal class TileWrapper : ITileCoordinatesMaster
    {
        internal TiledMap TiledMap { get; }
        public Rectangle Bounds => new Rectangle
        {
            X = 0,
            Y = 0,
            Width = TiledMap.Width * TiledMap.TileWidth,
            Height = TiledMap.Height * TiledMap.TileHeight
        };

        internal TileWrapper(TiledMap map)
        {
            this.TiledMap = map;
        }

        public Vector2 GetLevelPosition(PositionOnTileMap tilePosition)
        {
            var tileUpperLeftCorner = TileCoordinatesToLevelCoordinates(tilePosition.X, tilePosition.Y);
            var center = tileUpperLeftCorner + new Vector2 { X = TiledMap.TileWidth / 2, Y = -TiledMap.TileHeight / 2 };
            return center + tilePosition.Offset;
        }

        public PositionOnTileMap GetTilePosition(Vector2 levelPosition)
        {
            var x = (Int32)(levelPosition.X / TiledMap.TileWidth);
            var y = TiledMap.Height - (Int32)(levelPosition.Y / TiledMap.TileHeight) - 1;
            var tileUpperLeftCorner = TileCoordinatesToLevelCoordinates(x, y);
            var center = tileUpperLeftCorner + new Vector2 { X = TiledMap.TileWidth / 2, Y = -TiledMap.TileHeight / 2 };
            var offset = levelPosition - center;
            return new PositionOnTileMap { X = x, Y = y, Offset = offset };
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
            var result = new List<Point>();
            var layer = TiledMap.TileLayers.FirstOrDefault(tl => tl.Name == layerName);
            if (layer != null)
                foreach (UInt16 row in Enumerable.Range(0, TiledMap.Height))
                    foreach (UInt16 column in Enumerable.Range(0, TiledMap.Width))
                    {
                        if (layer.TryGetTile(column, row, out TiledMapTile? wallTile))
                        {
                            var tileId = wallTile.Value.GlobalIdentifier;
                            if (ContainsProperty(TiledMap, tileId, property))
                                result.Add(new Point { X = column, Y = row });
                        }
                    }
            return result;
        }

        private Boolean ContainsProperty(TiledMap map, Int32 tileId, String property)
        {
            var tileset = map.GetTilesetByTileGlobalIdentifier(tileId);
            if (tileset != null)
            {
                var firstGlobalIdentifier = map.GetTilesetFirstGlobalIdentifier(tileset);
                var tilesetTile = tileset.Tiles.FirstOrDefault
                    (tst => tst.LocalTileIdentifier == tileId - firstGlobalIdentifier);
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
            var hitboxUpperLeftCorner = TileCoordinatesToLevelCoordinates(wall.X, wall.Y);
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
                Y = Bounds.Height - y * TiledMap.TileHeight
            };
        }
    }
}
