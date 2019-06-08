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
        private TileWrapper map;
        private WallsOptimizer wallsOptimizer = new WallsOptimizer();

        internal TileWallsFactory(TileWrapper map)
        {
            this.map = map;
        }

        internal ICollidable[] ConstructTileWalls()
        {
            List<Point> wallsTiles = map.GetWallTiles();
            List<Rectangle> walls = wallsOptimizer.GetWalls(wallsTiles);
            return walls.Select(w => new TileWall(map.GetHitbox(w))).Cast<ICollidable>().ToArray();
        }
    }
}
