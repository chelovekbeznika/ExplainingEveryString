using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal class EnemyInfoDisplayer
    {
        internal const String HealthBarTexture = "EnemyHealthBar";

        private readonly SpriteData healthBar;
        private readonly Int32 pixelsBetweenEnemyAndHealthBar = 8;
        private readonly InterfaceSpriteDisplayer interfaceSpriteDisplayer;

        internal EnemyInfoDisplayer(InterfaceSpriteDisplayer interfaceSpriteDisplayer, Dictionary<String, SpriteData> sprites)
        {
            this.interfaceSpriteDisplayer = interfaceSpriteDisplayer;
            this.healthBar = TexturesHelper.GetSprite(sprites, HealthBarTexture);
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
            Single healthRemained = enemyInterfaceInfo.Health / enemyInterfaceInfo.MaxHealth;
            Vector2 healthBarPosition = GetHealthBarPosition(enemyInterfaceInfo.PositionOnScreen);
            interfaceSpriteDisplayer.Draw(healthBar, healthBarPosition, new CenterPartDisplayer(), healthRemained);
        }

        private Vector2 GetHealthBarPosition(Rectangle positionOnScreen)
        {
            Vector2 healthBarPosition = new Vector2
            {
                X = positionOnScreen.Center.X - healthBar.Width / 2,
                Y = positionOnScreen.Top - healthBar.Height / 2 - pixelsBetweenEnemyAndHealthBar
            };
            if (healthBarPosition.Y <= 0)
            {
                healthBarPosition.Y = positionOnScreen.Bottom + healthBar.Height / 2 + pixelsBetweenEnemyAndHealthBar;
            }
            return healthBarPosition;
        }
    }
}
