using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class BossInfoDisplayer
    {
        internal const String OneBossPrefix = "";
        internal const String LeftBossPrefix = "LeftDouble";
        internal const String RightBossPrefix = "RightDouble";

        internal const String HealthBarTexture = "{0}BossHealthBar";
        internal const String EmptyHealthBarTexture = "Empty{0}BossHealthBar";
        internal const String RecentlyHitHealthBarTexture = "RecentlyHit{0}BossHealthBar";
        internal const String RecentlyHitEmptyHealthBarTexture = "RecentlyHitEmpty{0}BossHealthBar";

        private const Single RecentHitThreshold = 0.33333F;
        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;
        private readonly SpriteData healthBar;
        private readonly SpriteData emptyHealthBar;
        private readonly SpriteData recentlyHitHealthBar;
        private readonly SpriteData recentlyHitEmptyHealthBar;

        private readonly Int32 pixelsFromTop = 16;
        private readonly Int32 offset;
        private readonly Boolean reversedDrain;

        public BossInfoDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, Dictionary<String, SpriteData> sprites,
            String spritesPrefix, Int32 offset)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.healthBar = TexturesHelper.GetSprite(sprites, String.Format(HealthBarTexture, spritesPrefix));
            this.emptyHealthBar = TexturesHelper.GetSprite(sprites, String.Format(EmptyHealthBarTexture, spritesPrefix));
            this.recentlyHitHealthBar = TexturesHelper.GetSprite(sprites, String.Format(RecentlyHitHealthBarTexture, spritesPrefix));
            this.recentlyHitEmptyHealthBar = TexturesHelper.GetSprite(sprites, String.Format(RecentlyHitEmptyHealthBarTexture, spritesPrefix));
            this.reversedDrain = spritesPrefix == RightBossPrefix;
            this.offset = offset;
        }

        public void Draw(EnemyInterfaceInfo bossInfo)
        {
            var currentHealthBar = bossInfo.FromLastHit > RecentHitThreshold ? healthBar : recentlyHitHealthBar;
            var currentEmptyHealthBar = bossInfo.FromLastHit > RecentHitThreshold ? emptyHealthBar : recentlyHitEmptyHealthBar;
            var healthRemained = bossInfo.Health / bossInfo.MaxHealth;
            var healthBarPosition = new Vector2
            {
                X = (interfaceSpriteDisplayer.ScreenWidth - currentHealthBar.Width) / 2 + offset,
                Y = pixelsFromTop
            };
            if (!reversedDrain)
            {
                interfaceSpriteDisplayer.Draw(currentHealthBar, healthBarPosition, new LeftPartDisplayer(), healthRemained);
                interfaceSpriteDisplayer.Draw(currentEmptyHealthBar, healthBarPosition, new RightPartDisplayer(), 1 - healthRemained);
            }
            else
            {
                interfaceSpriteDisplayer.Draw(currentHealthBar, healthBarPosition, new RightPartDisplayer(), healthRemained);
                interfaceSpriteDisplayer.Draw(currentEmptyHealthBar, healthBarPosition, new LeftPartDisplayer(), 1 - healthRemained);
            }
        }
    }
}
