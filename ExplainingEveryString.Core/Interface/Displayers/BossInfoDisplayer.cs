using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class BossInfoDisplayer : IDisplayer
    {
        internal const String OneBossPrefix = "";
        internal const String LeftBossPrefix = "LeftDouble";
        internal const String RightBossPrefix = "RightDouble";
        internal const String LeftOfThreeBossPrefix = "LeftTriple";
        internal const String RightOfThreeBossPrefix = "RightTriple";
        internal const String CenterOfThreeBossPrefix = "CenterTriple";

        private const String HealthBarTexture = "{0}BossHealthBar";
        private const String EmptyHealthBarTexture = "Empty{0}BossHealthBar";
        private const String RecentlyHitHealthBarTexture = "RecentlyHit{0}BossHealthBar";
        private const String RecentlyHitEmptyHealthBarTexture = "RecentlyHitEmpty{0}BossHealthBar";

        private const Single RecentHitThreshold = 0.33333F;
        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;
        private SpriteData healthBar;
        private SpriteData emptyHealthBar;
        private SpriteData recentlyHitHealthBar;
        private SpriteData recentlyHitEmptyHealthBar;

        private readonly Int32 pixelsFromTop = 16;
        private readonly Int32 offset;
        private readonly String spritesPrefix;
        private readonly DrainType drainType;

        internal BossInfoDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, String spritesPrefix, Int32 offset, DrainType drainType)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.drainType = drainType;
            this.spritesPrefix = spritesPrefix;
            this.offset = offset;
        }

        public String[] GetSpritesNames() => new[]
        {
            String.Format(HealthBarTexture, spritesPrefix), String.Format(EmptyHealthBarTexture, spritesPrefix),
            String.Format(RecentlyHitHealthBarTexture, spritesPrefix), String.Format(RecentlyHitEmptyHealthBarTexture, spritesPrefix)
        };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            this.healthBar = TextureLoadingHelper.GetSprite(sprites, String.Format(HealthBarTexture, spritesPrefix));
            this.emptyHealthBar = TextureLoadingHelper.GetSprite(sprites, String.Format(EmptyHealthBarTexture, spritesPrefix));
            this.recentlyHitHealthBar = TextureLoadingHelper.GetSprite(sprites, String.Format(RecentlyHitHealthBarTexture, spritesPrefix));
            this.recentlyHitEmptyHealthBar = TextureLoadingHelper.GetSprite(sprites, String.Format(RecentlyHitEmptyHealthBarTexture, spritesPrefix));
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
            switch (drainType)
            {
                case DrainType.Left:
                    interfaceSpriteDisplayer.Draw(currentHealthBar, healthBarPosition, new LeftPartDisplayer(), healthRemained);
                    interfaceSpriteDisplayer.Draw(currentEmptyHealthBar, healthBarPosition, new RightPartDisplayer(), 1 - healthRemained);
                    break;
                case DrainType.Right:
                    interfaceSpriteDisplayer.Draw(currentHealthBar, healthBarPosition, new RightPartDisplayer(), healthRemained);
                    interfaceSpriteDisplayer.Draw(currentEmptyHealthBar, healthBarPosition, new LeftPartDisplayer(), 1 - healthRemained);
                    break;
                case DrainType.Center:
                    var edgeCoeff = (1 - healthRemained) / 2;
                    interfaceSpriteDisplayer.Draw(currentEmptyHealthBar, healthBarPosition, new RightPartDisplayer(), edgeCoeff);
                    interfaceSpriteDisplayer.Draw(currentHealthBar, healthBarPosition, new VerticalCenterPartDisplayer(), healthRemained);
                    interfaceSpriteDisplayer.Draw(currentEmptyHealthBar, healthBarPosition, new LeftPartDisplayer(), edgeCoeff);
                    break;
            }
        }
    }

    internal enum DrainType { Left, Right, Center }
}
