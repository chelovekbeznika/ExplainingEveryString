using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class BossInfoDisplayer
    {
        internal const String HealthBarTexture = "BossHealthBar";
        internal const String EmptyHealthBarTexture = "EmptyBossHealthBar";
        internal const String RecentlyHitHealthBarTexture = "RecentlyHitBossHealthBar";
        internal const String RecentlyHitEmptyHealthBarTexture = "RecentlyHitEmptyBossHealthBar";

        private const Single RecentHitThreshold = 0.33333F;
        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;
        private readonly SpriteData healthBar;
        private readonly SpriteData emptyHealthBar;
        private readonly SpriteData recentlyHitHealthBar;
        private readonly SpriteData recentlyHitEmptyHealthBar;

        private readonly Int32 pixelsFromTop = 16;

        public BossInfoDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, Dictionary<String, SpriteData> sprites)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.healthBar = TexturesHelper.GetSprite(sprites, HealthBarTexture);
            this.emptyHealthBar = TexturesHelper.GetSprite(sprites, EmptyHealthBarTexture);
            this.recentlyHitHealthBar = TexturesHelper.GetSprite(sprites, RecentlyHitHealthBarTexture);
            this.recentlyHitEmptyHealthBar = TexturesHelper.GetSprite(sprites, RecentlyHitEmptyHealthBarTexture);
        }

        public void Draw(EnemyInterfaceInfo bossInfo)
        {
            var currentHealthBar = bossInfo.FromLastHit > RecentHitThreshold ? healthBar : recentlyHitHealthBar;
            var currentEmptyHealthBar = bossInfo.FromLastHit > RecentHitThreshold ? emptyHealthBar : recentlyHitEmptyHealthBar;
            var healthRemained = bossInfo.Health / bossInfo.MaxHealth;
            var healthBarPosition = new Vector2
            {
                X = (interfaceSpriteDisplayer.ScreenWidth - currentHealthBar.Width) / 2,
                Y = pixelsFromTop
            };
            interfaceSpriteDisplayer.Draw(currentHealthBar, healthBarPosition, new LeftPartDisplayer(), healthRemained);
            interfaceSpriteDisplayer.Draw(currentEmptyHealthBar, healthBarPosition, new RightPartDisplayer(), 1 - healthRemained);
        }
    }
}
