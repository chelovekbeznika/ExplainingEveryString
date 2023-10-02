using ExplainingEveryString.Core.Assets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class DashStateDisplayer : IDisplayer
    {
        private const String AvailableTexture = "FullDash";
        private const String NonAvailableTexture = "FullDashNotReady";
        private const String CooldownTexture = "EmptyDash";
        private const String ActiveTexture = "ActiveDash";

        private SpriteData available;
        private SpriteData nonAvailable;
        private SpriteData cooldown;
        private SpriteData active;
        private readonly InterfaceDrawController spriteDisplayer;
        private readonly HealthBarDisplayer healthBarDisplayer;
        private Int32 PixelsFromLeft => healthBarDisplayer.MarginOfLeftEdge - 16;
        private Int32 PixelsFromBottom => healthBarDisplayer.HeightOfTopEdge;

        internal DashStateDisplayer(HealthBarDisplayer healthBarDisplayer, InterfaceDrawController spriteDisplayer)
        {
            this.healthBarDisplayer = healthBarDisplayer;
            this.spriteDisplayer = spriteDisplayer;
        }

        public String[] GetSpritesNames() => new[] { AvailableTexture, NonAvailableTexture, CooldownTexture, ActiveTexture };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            this.available = TextureLoadingHelper.GetSprite(sprites, AvailableTexture);
            this.nonAvailable = TextureLoadingHelper.GetSprite(sprites, NonAvailableTexture);
            this.cooldown = TextureLoadingHelper.GetSprite(sprites, CooldownTexture);
            this.active = TextureLoadingHelper.GetSprite(sprites, ActiveTexture);
        }
        public void Draw(PlayerInterfaceInfo interfaceInfo)
        {
            var barPosition = new Vector2
            {
                X = PixelsFromLeft,
                Y = spriteDisplayer.ScreenHeight - PixelsFromBottom - available.Height
            };
            var cooldownRemained = interfaceInfo.TillDashRecharge / interfaceInfo.DashCooldown;
            switch (interfaceInfo.DashState)
            {
                case DashState.Active:
                    spriteDisplayer.Draw(active, barPosition);
                    break;
                case DashState.Nonavailable:
                    spriteDisplayer.Draw(cooldown, barPosition, new LeftPartDisplayer(), cooldownRemained);
                    spriteDisplayer.Draw(nonAvailable, barPosition, new RightPartDisplayer(), 1 - cooldownRemained);
                    break;
                case DashState.Available:
                    spriteDisplayer.Draw(cooldown, barPosition, new LeftPartDisplayer(), cooldownRemained);
                    spriteDisplayer.Draw(available, barPosition, new RightPartDisplayer(), 1 - cooldownRemained);
                    break;
            }
        }
    }
}
