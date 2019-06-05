using ExplainingEveryString.Core.Tiles;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class WallOptimizationTests
    {
        [Test]
        public void EmptyLevelTest()
        {
            List<Point> points = new List<Point>();
            List<Rectangle> walls = new WallsOptimizer().GetWalls(points);
            Assert.That(walls.Count == 0);
        }

        [Test]
        public void OneRectangleTest()
        {
            List<Point> wallTiles = new List<Point>
            {
                new Point(1, 1), new Point(2, 1), new Point(3, 1),
                new Point(1, 2), new Point(2, 2), new Point(3, 2)
            };
            AssertWallsOptimizedSuccessfully(wallTiles,  1);
        }

        [Test]
        public void TwoRectanglesTest()
        {
            List<Point> wallTiles = Enumerable.Range(0, 100)
                .Select(y => Enumerable.Range(0, 100).Select(x => new Point(x, y))).SelectMany(p => p)
                .Concat(new Point[] { new Point(100, 0) }).ToList();
            AssertWallsOptimizedSuccessfully(wallTiles, 2);
        }

        [Test]
        public void FewPointsTest()
        {
            List<Point> wallTiles = new List<Point>
            {
                new Point(2, 0), new Point(6, 0), new Point(5, 1), new Point(10, 1),
                new Point(3, 3), new Point(5, 3), new Point(10, 10)
            };
            AssertWallsOptimizedSuccessfully(wallTiles, 7);
        }

        [Test]
        public void RealDataAlikeTest()
        {
            List<Point> wallTiles = new List<Point>
            {
                new Point(1, 1), new Point(4, 1), new Point(5, 1), new Point(6, 1),
                    new Point(7, 1), new Point(8, 1), new Point(9, 1),
                new Point(1, 2), new Point(6, 2), new Point(7, 2),
                new Point(1, 3), new Point(6, 3), new Point(7, 3),
                new Point(1, 4), new Point(6, 4), new Point(7, 4),
                new Point(1, 5), new Point(6, 5), new Point(7, 5),
                new Point(1, 6), new Point(6, 6), new Point(7, 6),
                new Point(1, 7), new Point(2, 7), new Point(3, 7), new Point(4, 7),
                    new Point(5, 7), new Point(6, 7), new Point(7, 7), new Point(8, 7)
            };
            AssertWallsOptimizedSuccessfully(wallTiles, 4);
        }

        private void AssertWallsOptimizedSuccessfully(List<Point> wallTiles, Int32 optimumWallsCount)
        {
            List<Rectangle> walls = new WallsOptimizer().GetWalls(wallTiles);
            Assert.That(RectanglesCoversEveryTile(wallTiles, walls));
            Assert.That(walls.Count, Is.LessThanOrEqualTo(optimumWallsCount));
        }

        [Test]
        public void TestingTest()
        {
            List<Point> wallTiles = new List<Point>
            {
                new Point(1, 1), new Point(2, 1), new Point(3, 1),
                new Point(1, 2), new Point(2, 2), new Point(3, 2),
                new Point(6, 3), new Point(3, 6), new Point(4, 6)
            };
            List<Rectangle> walls = new List<Rectangle>
            {
                new Rectangle(1, 1, 3, 2), new Rectangle(6, 3, 1, 1), new Rectangle(3, 6, 2, 1)
            };
            Assert.That(RectanglesCoversEveryTile(wallTiles, walls));
            Assert.That(!RectanglesCoversEveryTile(wallTiles.Take(wallTiles.Count() - 1), walls));
            Assert.That(!RectanglesCoversEveryTile(wallTiles, walls.Take(walls.Count() - 1)));
        }

        private Boolean RectanglesCoversEveryTile(IEnumerable<Point> wallTiles, IEnumerable<Rectangle> walls)
        {
            List<Point> tilesFromWallsArray = new List<Point>();
            foreach (Rectangle wall in walls)
            {
                foreach (Int32 x in Enumerable.Range(wall.X, wall.Width))
                {
                    foreach (Int32 y in Enumerable.Range(wall.Y, wall.Height))
                    {
                        tilesFromWallsArray.Add(new Point(x, y));
                    }
                }
            }
            tilesFromWallsArray = tilesFromWallsArray.Distinct().ToList();
            return !(wallTiles.Except(tilesFromWallsArray).Any() || tilesFromWallsArray.Except(wallTiles).Any());
        }
    }
}
