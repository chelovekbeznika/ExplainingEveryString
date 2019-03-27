using ExplainingEveryString.Data.Blueprints;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class BlueprintsTests
    {
        [Test]
        public void GetSpritesTest()
        {
            HardCodeBlueprintsLoader loader = new HardCodeBlueprintsLoader();
            loader.Load();
            List<String> expectedSprites = new List<String>()
            {
                @"Sprites/Mine",
                @"Sprites/Rectangle",
                @"Sprites/PlayerBullet"
            };
            Assert.That(loader.GetNeccessarySprites(), Is.EquivalentTo(expectedSprites));
        }
    }
}
