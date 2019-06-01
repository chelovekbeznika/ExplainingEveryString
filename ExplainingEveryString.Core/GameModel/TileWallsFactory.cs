using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameModel
{
    internal class TileWallsFactory
    {
        private TiledMap map;

        internal TileWallsFactory(TiledMap map)
        {
            this.map = map;
        }

        internal IEnumerable<TileWall> ConstructTileWalls()
        {
            List<Point> wallsTiles = TileUtility.GetWalls(map);
            return wallsTiles.Select(wt => new TileWall(GetHitbox(map, wt)));
        }

        private Hitbox GetHitbox(TiledMap map, Point point)
        {
            Vector2 mapUpperLeftCorner = TileUtility.GetUpperLeftCorner(map);
            Vector2 hitboxUpperLeftCorner = new Vector2
            {
                X = mapUpperLeftCorner.X + point.X * map.TileWidth,
                Y = mapUpperLeftCorner.Y - point.Y * map.TileHeight
            };
            return new Hitbox
            {
                Top = hitboxUpperLeftCorner.Y,
                Bottom = hitboxUpperLeftCorner.Y - map.TileHeight,
                Left = hitboxUpperLeftCorner.X,
                Right = hitboxUpperLeftCorner.X + map.TileWidth
            };
        }
    }
}
