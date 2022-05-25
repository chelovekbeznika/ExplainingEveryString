using ExplainingEveryString.Core.Collisions;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using System;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class SpatialPartioningHelperTests
    {
        [Test]
        [TestCase(15, 20, 40, 40)]
        [TestCase(40, 15, 10, 40)]
        public void GetSectors_TwoPointsInOneSector_ShouldReturnOneSector(Single ax, Single ay, Single bx, Single by)
        {
            var result = SpatialPartioningHelper.GetSectors(new Vector2(ax, ay), new Vector2(bx, by));
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo("0:0"));
        }

        [Test]
        public void GetSectors_TwoPointsInOneRow_ShouldReturnSectorsFromThatRow()
        {
            var result = SpatialPartioningHelper.GetSectors(new Vector2(168, 25), new Vector2(400, 100));
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result, Is.SubsetOf(new[] { "1:0", "2:0", "3:0" }));
        }

        [Test]
        public void GetSectors_TwoPointsInOneColumn_ShouldReturnSectorsFromThatColumn()
        {
            var result = SpatialPartioningHelper.GetSectors(new Vector2(64, 64), new Vector2(64, 145));
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Is.SubsetOf(new[] { "0:0", "0:1" }));
        }

        [Test]
        public void GetSectors_TwoPointsInOneLine_ShouldReturnSectorsFromThatColumn()
        {
            var result = SpatialPartioningHelper.GetSectors(new Vector2(64, 64), new Vector2(64, 192));
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Is.SubsetOf(new[] { "0:0", "0:1" }));
        }

        [Test]
        [TestCase(64, 96, 192, 192, "0:0", "0:1", "1:1")]
        [TestCase(416, 479, 193, 65, "1:0", "1:1", "2:1", "2:2", "2:3", "3:3")]
        [TestCase(96, 448, 480, 224, "0:3", "1:3", "1:2", "2:2", "3:2", "3:1")]
        public void GetSectors_TwoPointsInDifferentRowsAndColumns_ShouldReturnCorrectSectors
            (Single ax, Single ay, Single bx, Single by, params String[] exptectedSectors)
        {
            var result = SpatialPartioningHelper.GetSectors(new Vector2(ax, ay), new Vector2(bx, by));
            Assert.That(result.Count, Is.EqualTo(exptectedSectors.Length));
            Assert.That(result, Is.SubsetOf(exptectedSectors));
        }
    }
}
