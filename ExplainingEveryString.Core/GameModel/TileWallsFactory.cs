using ExplainingEveryString.Core.Tiles;
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
        private WallsOptimizer wallsOptimizer = new WallsOptimizer();

        internal TileWallsFactory(TiledMap map)
        {
            this.map = map;
        }

        internal IEnumerable<TileWall> ConstructTileWalls()
        {
            List<Point> wallsTiles = TileUtility.GetWallTiles(map);
            List<Rectangle> walls = wallsOptimizer.GetWalls(wallsTiles);
            return walls.Select(w => new TileWall(GetHitbox(map, w)));
        }

        private Hitbox GetHitbox(TiledMap map, Rectangle wall)
        {
            Vector2 mapUpperLeftCorner = TileUtility.GetUpperLeftCorner(map);
            Vector2 hitboxUpperLeftCorner = new Vector2
            {
                X = mapUpperLeftCorner.X + wall.X * map.TileWidth,
                Y = mapUpperLeftCorner.Y - wall.Y * map.TileHeight
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
