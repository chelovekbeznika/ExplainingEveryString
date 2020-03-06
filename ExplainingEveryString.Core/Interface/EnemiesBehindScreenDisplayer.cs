using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface
{
    internal class EnemiesBehindScreenDisplayer
    {
        internal const String DangerSign = "EnemyBehindScreen";

        private readonly InterfaceSpriteDisplayer spriteDisplayer;
        private readonly SpriteData dangerSign;

        internal EnemiesBehindScreenDisplayer(InterfaceSpriteDisplayer spriteDisplayer, Dictionary<String, SpriteData> sprites)
        {
            this.spriteDisplayer = spriteDisplayer;
            this.dangerSign = TexturesHelper.GetSprite(sprites, DangerSign);
        }

        internal void Draw(List<Vector2> hiddenEnemies)
        {
            foreach (var hiddenEnemyPosition in hiddenEnemies)
            {
                var dangerSignPosition = GetDangerSignPosition(hiddenEnemyPosition);
                spriteDisplayer.Draw(dangerSign, dangerSignPosition);
            }
        }

        private Vector2 GetDangerSignPosition(Vector2 hiddenEnemyPosition)
        {
            var dangerSignPosition = hiddenEnemyPosition;
            if (dangerSignPosition.X > spriteDisplayer.ScreenWidth - dangerSign.Width)
                dangerSignPosition.X = spriteDisplayer.ScreenWidth - dangerSign.Width;
            if (dangerSignPosition.Y > spriteDisplayer.ScreenHeight - dangerSign.Height)
                dangerSignPosition.Y = spriteDisplayer.ScreenHeight - dangerSign.Height;
            return dangerSignPosition;
        }
    }
}
