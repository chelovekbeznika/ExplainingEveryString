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
        private TileUtility utility;
        private WallsOptimizer wallsOptimizer = new WallsOptimizer();

        internal TileWallsFactory(TiledMap map)
        {
            this.utility = new TileUtility(map);
        }

        internal IEnumerable<TileWall> ConstructTileWalls()
        {
            List<Point> wallsTiles = utility.GetWallTiles();
            List<Rectangle> walls = wallsOptimizer.GetWalls(wallsTiles);
            return walls.Select(w => new TileWall(utility.GetHitbox(w)));
        }
    }
}
