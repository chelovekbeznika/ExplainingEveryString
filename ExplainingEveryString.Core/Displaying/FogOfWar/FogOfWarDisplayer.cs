using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal class FogOfWarDisplayer : IFogOfWarDisplayer
    {
        private Single timeElapsed = 0;
        private SpriteData[] fogParticles;

        public Int32 SpriteWidth { get; private set; }
        public Int32 SpriteHeight { get; private set; }
        public Int32 SpritesNumber => fogParticles.Length;

        public void Construct(FogOfWarSpecification specification, SpriteDataBuilder spriteDataBuilder)
        {
            this.SpriteWidth = specification.ParticleWidth;
            this.SpriteHeight = specification.ParticleHeight;
            this.fogParticles = spriteDataBuilder.Build(specification.Sprites).Values.ToArray();
        }

        public void Update(Single elapsedSeconds)
        {
            timeElapsed += elapsedSeconds;
        }

        public void Draw(SpriteBatch spriteBatch, FogOfWarSpriteEntry[] fogOfWarSpriteEntries)
        {
            foreach (FogOfWarSpriteEntry spriteEntry in fogOfWarSpriteEntries)
            {
                SpriteData sprite = fogParticles[spriteEntry.SpriteNumber];
                Rectangle? drawPart = AnimationHelp.GetDrawPart(sprite, timeElapsed);
                Vector2 origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
                Vector2 position = new Vector2(spriteEntry.ScreenPosition.X, spriteEntry.ScreenPosition.Y);
                spriteBatch.Draw(sprite.Sprite, position, drawPart, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
