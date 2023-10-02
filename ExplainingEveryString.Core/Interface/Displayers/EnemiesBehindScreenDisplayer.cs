using ExplainingEveryString.Core.Assets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class EnemiesBehindScreenDisplayer : IDisplayer
    {
        private const String DangerSign = "EnemyBehindScreen";

        private readonly InterfaceDrawController spriteDisplayer;
        private SpriteData dangerSign;

        internal EnemiesBehindScreenDisplayer(InterfaceDrawController spriteDisplayer)
        {
            this.spriteDisplayer = spriteDisplayer;
            
        }

        public String[] GetSpritesNames() => new[] { DangerSign };

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            this.dangerSign = TextureLoadingHelper.GetSprite(sprites, DangerSign);
        }

        public void Draw(List<Vector2> hiddenEnemies)
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
