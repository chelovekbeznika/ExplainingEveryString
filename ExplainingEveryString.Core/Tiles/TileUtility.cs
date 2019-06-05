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
    internal class TileUtility
    {
        private TiledMap map;

        internal TileUtility(TiledMap map)
        {
            this.map = map;
        }

        internal Vector2 GetPosition(PositionOnTileMap tilePosition)
        {
            Vector2 upperLeftCorner = new Vector2 { X = tilePosition.X * map.TileWidth, Y = -tilePosition.Y * map.TileWidth };
            Vector2 center = upperLeftCorner + new Vector2 { X = map.TileWidth / 2, Y = -map.TileHeight / 2 };
            return center + tilePosition.Offset;
        }

        internal List<Point> GetWallTiles()
        {
            List<Point> result = new List<Point>();
            TiledMapTileLayer wallsLayer = map.TileLayers.First(tl => tl.Name == "Walls");
            foreach (Int32 row in Enumerable.Range(0, map.Height))
                foreach (Int32 column in Enumerable.Range(0, map.Width))
                {
                    if (wallsLayer.TryGetTile(column, row, out TiledMapTile? wallTile))
                    {
                        Int32 tileId = wallTile.Value.GlobalIdentifier;
                        if (IsWall(map, tileId))
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
            Vector2 hitboxUpperLeftCorner = new Vector2
            {
                X = wall.X * map.TileWidth,
                Y = -wall.Y * map.TileHeight
            };
            return new Hitbox
            {
                Top = hitboxUpperLeftCorner.Y,
                Bottom = hitboxUpperLeftCorner.Y - map.TileHeight * wall.Height,
                Left = hitboxUpperLeftCorner.X,
                Right = hitboxUpperLeftCorner.X + map.TileWidth * wall.Width
            };
        }
    }
}
