using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal class BossInfoDisplayer
    {
        internal const String HealthBarTexture = "BossHealthBar";
        internal const String EmptyHealthBarTexture = "EmptyBossHealthBar";

        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;
        private readonly SpriteData healthBar;
        private readonly SpriteData emptyHealthBar;

        private readonly Int32 pixelsFromTop = 16;

        public BossInfoDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, Dictionary<String, SpriteData> sprites)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.healthBar = TexturesHelper.GetSprite(sprites, HealthBarTexture);
            this.emptyHealthBar = TexturesHelper.GetSprite(sprites, EmptyHealthBarTexture);
        }

        public void Draw(EnemyInterfaceInfo bossInfo)
        {
            var healthRemained = bossInfo.Health / bossInfo.MaxHealth;
            var healthBarPosition = new Vector2
            {
                X = (interfaceSpriteDisplayer.ScreenWidth - healthBar.Width) / 2,
                Y = pixelsFromTop
            };
            interfaceSpriteDisplayer.Draw(healthBar, healthBarPosition, new LeftPartDisplayer(), healthRemained);
            interfaceSpriteDisplayer.Draw(emptyHealthBar, healthBarPosition, new RightPartDisplayer(), 1 - healthRemained);
        }
    }
}
