using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class HealthBarDisplayer
    {
        internal const String HealthBarTexture = "HealthBar";
        internal const String EmptyHealthBarTexture = "EmptyHealthBar";

        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;
        private readonly SpriteData healthBar;
        private readonly SpriteData emptyHealthBar;
        private readonly Int32 pixelsFromLeft = 32;
        private readonly Int32 pixelsFromBottom = 32;
        private const Single ShakeCooldown = 0.5F;
        private const Single ShakeAmplitudeHorizontal = 8;
        private const Single ShakeAmplitudeVertical = 4;

        internal Int32 MarginOfLeftEdge => pixelsFromLeft;
        internal Int32 HeightOfTopEdge => pixelsFromBottom + healthBar.Height;

        internal HealthBarDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, Dictionary<String, SpriteData> sprites)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.healthBar = TexturesHelper.GetSprite(sprites, HealthBarTexture);
            this.emptyHealthBar = TexturesHelper.GetSprite(sprites, EmptyHealthBarTexture);
        }

        internal void Draw(PlayerInterfaceInfo interfaceInfo)
        {
            var healthRemained = interfaceInfo.Health / interfaceInfo.MaxHealth;
            var basePosition = new Vector2(pixelsFromLeft, interfaceSpriteDisplayer.ScreenHeight - pixelsFromBottom - healthBar.Height);
            var position = basePosition + CalculateRecentHitShake(interfaceInfo.FromLastHit);
            interfaceSpriteDisplayer.Draw(healthBar, position, new LeftPartDisplayer(), healthRemained);
            interfaceSpriteDisplayer.Draw(emptyHealthBar, position, new RightPartDisplayer(), 1 - healthRemained);
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
