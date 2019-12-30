using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameModel
{
    internal class WallsFactory
    {
        private TileWrapper map;
        private WallsOptimizer wallsOptimizer = new WallsOptimizer();

        internal WallsFactory(TileWrapper map)
        {
            this.map = map;
        }

        internal ICollidable[] ConstructWalls()
        {
            var wallsTiles = map.GetWallTiles();
            var walls = wallsOptimizer.GetWalls(wallsTiles);
            var pitTiles = map.GetPitTiles();
            var pits = wallsOptimizer.GetWalls(pitTiles);
            return walls.Select(w => new Wall(map.GetHitbox(w), CollidableMode.Solid))
                .Concat(pits.Select(p => new Wall(map.GetHitbox(p), CollidableMode.Pit)))
                .Cast<ICollidable>().ToArray();
        }
    }
}
