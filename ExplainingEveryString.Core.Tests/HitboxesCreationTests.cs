using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using ExplainingEveryString.Core.GameModel.Enemies;
using System.Linq;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class HitboxesCreationTests
    {
        [Test]
        public void MineHitbox()
        {
            TestLevel testLevel = new TestLevel();
            Mine mine = testLevel.ActorsStorage.Enemies.OfType<Mine>().First();
            Hitbox hitbox = mine.GetCurrentHitbox();

            Hitbox model = new Hitbox { Bottom = 192, Top = 208, Left = -108, Right = -92 };
            Assert.That(model, Is.EqualTo(hitbox));
        }
    }
}
