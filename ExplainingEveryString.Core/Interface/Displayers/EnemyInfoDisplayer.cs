using ExplainingEveryString.Core.Assets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class EnemyInfoDisplayer : IDisplayer
    {
        private const String HealthBarTexture = "EnemyHealthBar";
        private const String RecentlyHitHealthBarTexture = "RecentlyHitEnemyHealthBar";

        private const Single RecentHitThreshold = 0.2f;
        private SpriteData healthBar;
        private SpriteData recentlyHitHealthBar;
        private readonly Int32 pixelsBetweenEnemyAndHealthBar = 8;
        private readonly InterfaceDrawController spriteDisplayer;

        internal EnemyInfoDisplayer(InterfaceDrawController spriteDisplayer)
        {
            this.spriteDisplayer = spriteDisplayer;
        }

        public String[] GetSpritesNames() => new[] { HealthBarTexture, RecentlyHitHealthBarTexture };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            this.healthBar = TextureLoadingHelper.GetSprite(sprites, HealthBarTexture);
            this.recentlyHitHealthBar = TextureLoadingHelper.GetSprite(sprites, RecentlyHitHealthBarTexture);
        }

        public void Draw(IEnumerable<EnemyInterfaceInfo> enemiesInterfaceInfo)
        {
            foreach (EnemyInterfaceInfo enemyInterfaceInfo in enemiesInterfaceInfo)
            {
                Draw(enemyInterfaceInfo);
            }
        }

        private void Draw(EnemyInterfaceInfo enemyInterfaceInfo)
        {
            var currentHealthBar = enemyInterfaceInfo.FromLastHit > RecentHitThreshold ? healthBar : recentlyHitHealthBar;
            var healthRemained = enemyInterfaceInfo.Health / enemyInterfaceInfo.MaxHealth;
            var healthBarPosition = GetHealthBarPosition(enemyInterfaceInfo.PositionOnScreen, currentHealthBar);
            spriteDisplayer.Draw(currentHealthBar, healthBarPosition, new CenterPartDisplayer(), healthRemained);
        }

        private Vector2 GetHealthBarPosition(Rectangle positionOnScreen, SpriteData currentHealthBar)
        {
            var healthBarPosition = new Vector2
            {
                X = positionOnScreen.Center.X - currentHealthBar.Width / 2,
                Y = positionOnScreen.Top - currentHealthBar.Height / 2 - pixelsBetweenEnemyAndHealthBar
            };
            if (healthBarPosition.Y <= 0)
            {
                healthBarPosition.Y = positionOnScreen.Bottom + currentHealthBar.Height / 2 + pixelsBetweenEnemyAndHealthBar;
            }
            return healthBarPosition;
        }
    }
}
