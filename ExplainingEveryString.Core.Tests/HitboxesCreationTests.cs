using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class HitboxesCreationTests
    {
        [Test]
        public void MineHitbox()
        {
            Mine mine = new Mine(new Vector2(-100, 200));
            Hitbox hitbox = mine.GetHitbox();
            Hitbox model = new Hitbox { Bottom = 192, Top = 208, Left = -108, Right = -92 };
            Assert.AreEqual(hitbox, model);
        }
    }
}
