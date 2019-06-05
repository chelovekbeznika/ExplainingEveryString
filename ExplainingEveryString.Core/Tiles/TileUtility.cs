using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Tiles
{
    internal static class TileUtility
    {
        internal static Vector2 GetUpperLeftCorner(TiledMap map)
        {
            return new Vector2
            {
                X = -Single.Parse(map.Properties["Left"]),
                Y = Single.Parse(map.Properties["Upper"])
            };
        }

        internal static List<Point> GetWallTiles(TiledMap map)
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

        private static Boolean IsWall(TiledMap map, Int32 tileId)
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
    }
}
