using ExplainingEveryString.Core.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Interface
{
    internal class AnimationController
    {
        private readonly Dictionary<String, SpriteData> spritesStorage;
        private Single elapsedTime = 0;
        private readonly Int32 desiredFps;

        internal AnimationController(Dictionary<String, SpriteData> spritesStorage, Int32 desiredFps)
        {
            this.spritesStorage = spritesStorage;
        }

        internal void Update(Single elapsedSeconds)
        {
            elapsedTime += elapsedSeconds;
        }

        internal void Draw(SpriteBatch spriteBatch, Color colorMask, String spriteName, Vector2 position)
        {
            SpriteData spriteData = spritesStorage[spriteName];
            Rectangle? framePart = AnimationHelp.GetDrawPart(spriteData, spriteData.DefaultAnimationCycle, elapsedTime);
            spriteBatch.Draw(spriteData.Sprite, position, framePart, colorMask);
        }
    }
}
