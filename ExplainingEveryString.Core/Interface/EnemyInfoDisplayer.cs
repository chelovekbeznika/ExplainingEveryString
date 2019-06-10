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
        private Texture2D healthBar;
        private readonly Int32 pixelsBetweenEnemyAndHealthBar = 8;

        internal EnemyInfoDisplayer(Texture2D healthBar)
        {
            this.healthBar = healthBar;
        }

        internal void Draw(List<EnemyInterfaceInfo> enemiesInterfaceInfo, SpriteBatch spriteBatch, Color mask)
        {
            foreach (EnemyInterfaceInfo enemyInterfaceInfo in enemiesInterfaceInfo)
            {
                Draw(enemyInterfaceInfo, spriteBatch, mask);
            }
        }

        private void Draw(EnemyInterfaceInfo enemyInterfaceInfo, SpriteBatch spriteBatch, Color mask)
        {
            Single healthRemained = enemyInterfaceInfo.Health / enemyInterfaceInfo.MaxHealth;
            Int32 pixelsToCutX = (Int32)(healthBar.Width / 2 * (1 - healthRemained));
            Int32 pixelsToCutY = (Int32)(healthBar.Height / 2 * (1 - healthRemained));

            Vector2 healthBarPosition = 
                GetHealthBarPosition(pixelsToCutX, pixelsToCutY, enemyInterfaceInfo.PositionOnScreen);
            Vector2 healthBarCenter = new Vector2(healthBar.Bounds.Center.X, healthBar.Bounds.Center.Y);
            Rectangle healthBarVisiblePart = new Rectangle
            {
                X = pixelsToCutX,
                Y = pixelsToCutY,
                Width = healthBar.Width - pixelsToCutX * 2,
                Height = healthBar.Height - pixelsToCutY
            };

            spriteBatch.Draw(healthBar, healthBarPosition, healthBarVisiblePart, 
                mask, 0, healthBarCenter, 1, SpriteEffects.None, 0);
        }

        private Vector2 GetHealthBarPosition(Int32 pixelsToCutX, Int32 pixelsToCutY, Rectangle PositionOnScreen)
        {
            Vector2 healthBarPosition = new Vector2
            {
                X = PositionOnScreen.Center.X + pixelsToCutX,
                Y = PositionOnScreen.Top - YHealthBarGap(pixelsToCutY)
            };
            if (healthBarPosition.Y <= 0)
            {
                healthBarPosition.Y = PositionOnScreen.Bottom + YHealthBarGap(pixelsToCutY);
            }
            return healthBarPosition;
        }

        private Int32 YHealthBarGap(Int32 pixelsToCutY)
        {
            return pixelsBetweenEnemyAndHealthBar + healthBar.Height / 2 - pixelsToCutY;
        }
    }
}
