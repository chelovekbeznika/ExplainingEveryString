﻿using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Core.GameModel;
using Microsoft.Xna.Framework;
using NUnit.Framework;
using ExplainingEveryString.Core.GameModel.Enemies;

namespace ExplainingEveryString.Core.Tests
{
    [TestFixture]
    public class HitboxesCreationTests
    {
        [Test]
        public void MineHitbox()
        {
            EnemyBlueprint mineBlueprint = new EnemyBlueprint()
            {
                DefaultSpriteName = "mine",
                Width = 16,
                Height = 16
            };
            Mine mine = new Mine();
            mine.Initialize(mineBlueprint, null, new Vector2(-100, 200));
            Hitbox hitbox = mine.GetHitbox();

            Hitbox model = new Hitbox { Bottom = 192, Top = 208, Left = -108, Right = -92 };
            Assert.That(model, Is.EqualTo(hitbox));
        }
    }
}
