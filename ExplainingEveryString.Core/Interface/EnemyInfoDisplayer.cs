using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class EnemyInfoDisplayer
    {
        
        internal const String HealthBarTexture = "EnemyHealthBar";
        internal const String RecentlyHitHealthBarTexture = "RecentlyHitEnemyHealthBar";

        private const Single RecentHitThreshold = 0.2f;
        private readonly SpriteData healthBar;
        private readonly SpriteData recentlyHitHealthBar;
        private readonly Int32 pixelsBetweenEnemyAndHealthBar = 8;
        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;

        internal EnemyInfoDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, Dictionary<String, SpriteData> sprites)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.healthBar = TexturesHelper.GetSprite(sprites, HealthBarTexture);
            this.recentlyHitHealthBar = TexturesHelper.GetSprite(sprites, RecentlyHitHealthBarTexture);
        }

        internal void Draw(List<EnemyInterfaceInfo> enemiesInterfaceInfo)
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
            interfaceSpriteDisplayer.Draw(currentHealthBar, healthBarPosition, new CenterPartDisplayer(), healthRemained);
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
