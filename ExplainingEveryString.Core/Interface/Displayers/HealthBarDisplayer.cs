using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class HealthBarDisplayer : IDisplayer
    {
        private const String HealthBarTexture = "HealthBar";
        private const String EmptyHealthBarTexture = "EmptyHealthBar";

        private readonly Int32 pixelsFromLeft = 32;
        private readonly Int32 pixelsFromBottom = 32;
        private const Single ShakeCooldown = 0.5F;
        private const Single ShakeAmplitudeHorizontal = 8;
        private const Single ShakeAmplitudeVertical = 4;

        private readonly InterfaceSpriteDisplayer spriteDisplayer;
        private SpriteData healthBar;
        private SpriteData emptyHealthBar;

        internal Int32 MarginOfLeftEdge => pixelsFromLeft;
        internal Int32 HeightOfTopEdge => pixelsFromBottom + healthBar.Height;

        internal HealthBarDisplayer(InterfaceSpriteDisplayer spriteDisplayer)
        {
            this.spriteDisplayer = spriteDisplayer;
        }

        public String[] GetSpritesNames() => new[] { HealthBarTexture, EmptyHealthBarTexture };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            this.healthBar = TextureLoadingHelper.GetSprite(sprites, HealthBarTexture);
            this.emptyHealthBar = TextureLoadingHelper.GetSprite(sprites, EmptyHealthBarTexture);
        }

        public void Draw(PlayerInterfaceInfo interfaceInfo)
        {
            var healthRemained = interfaceInfo.Health / interfaceInfo.MaxHealth;
            var basePosition = new Vector2(pixelsFromLeft, spriteDisplayer.ScreenHeight - pixelsFromBottom - healthBar.Height);
            var position = basePosition + CalculateRecentHitShake(interfaceInfo.FromLastHit);
            spriteDisplayer.Draw(healthBar, position, new LeftPartDisplayer(), healthRemained);
            spriteDisplayer.Draw(emptyHealthBar, position, new RightPartDisplayer(), 1 - healthRemained);
        }

        private Vector2 CalculateRecentHitShake(Single fromLastHit)
        {
            if (fromLastHit < ShakeCooldown)
            {
                var coeff = (ShakeCooldown - fromLastHit) / ShakeCooldown;
                return new Vector2
                {
                    X = (RandomUtility.Next() - 0.5F) * coeff * ShakeAmplitudeHorizontal * 2,
                    Y = RandomUtility.Next() * coeff * ShakeAmplitudeVertical,
                };
            }
            else
                return Vector2.Zero;
        }
    }
}
