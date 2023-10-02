using ExplainingEveryString.Core.Assets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.Interface.Displayers
{
    internal class CheckpointDisplayer : IDisplayer
    {
        private const Single ShowCheckpointFlag = 5.0F;
        private const Int32 PixelsFromRight = 16;
        private SpriteData checkpoint;
        private InterfaceDrawController spriteDisplayer;

        public String[] GetSpritesNames() => new[] { "Checkpoint" };

        internal CheckpointDisplayer(InterfaceDrawController spriteDisplayer)
        {
            this.spriteDisplayer = spriteDisplayer;
        }

        public void InitSprites(Dictionary<String, SpriteData> sprites)
        {
            checkpoint = TextureLoadingHelper.GetSprite(sprites, "Checkpoint");
        }

        public void Draw(PlayerInterfaceInfo playerInfo)
        {
            if (playerInfo.FromLastCheckpoint <= ShowCheckpointFlag)
            {
                var flagPosition = new Vector2(
                    x: spriteDisplayer.ScreenWidth - PixelsFromRight - checkpoint.Width,
                    y: (spriteDisplayer.ScreenHeight - checkpoint.Height) / 2);
                spriteDisplayer.Draw(checkpoint, flagPosition);
            }
        }

    }
}
