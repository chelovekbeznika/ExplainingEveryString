using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class MenuItemsPositioningTests
    {
        private MenuItemPositionsMapper mapper = new MenuItemPositionsMapper(() => new Point(800, 600), 16);

        [Test]
        public void NoMappingTest()
        {
            var empty = new Point[] { };
            Assert.That(mapper.GetItemsPositions(empty), Is.EquivalentTo(new Point[] { }));
        }

        [Test]
        public void OneItemTest()
        {
            var oneItem = new Point[] { new Point(600, 400) };
            Assert.That(mapper.GetItemsPositions(oneItem), Is.EquivalentTo(new Point[] { new Point(100, 100) }));
        }

        [Test]
        public void FewItemsSameSize()
        {
            var items = new Point[] 
            {
                new Point(64, 32), new Point(64, 32), new Point(64, 32), new Point(64, 32)
            };
            var positions = new Point[] 
            {
                new Point(368, 212), new Point(368, 260), new Point(368, 308), new Point(368, 356)
            };
            Assert.That(mapper.GetItemsPositions(items), Is.EquivalentTo(positions));
        }

        [Test]
        public void FewItemsDifferentSizes()
        {
            var items = new Point[] { new Point(128, 24), new Point(96, 16), new Point(256, 32) };
            var positions = new Point[] { new Point(336, 248), new Point(352, 288), new Point(272, 320) };
            Assert.That(mapper.GetItemsPositions(items), Is.EquivalentTo(positions));
        }
    }
}
