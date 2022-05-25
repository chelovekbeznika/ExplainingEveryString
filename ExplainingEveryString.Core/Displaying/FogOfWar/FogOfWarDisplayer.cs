using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace ExplainingEveryString.Core.Displaying.FogOfWar
{
    internal class FogOfWarDisplayer : IFogOfWarDisplayer
    {
        private Single timeElapsed = 0;
        private SpriteData[] fogParticles;

        public FogOfWarSpecification Specification { get; private set; }

        public void Construct(FogOfWarSpecification specification, SpriteDataBuilder spriteDataBuilder)
        {
            this.Specification = specification;
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
                var sprite = fogParticles[spriteEntry.SpriteNumber];
                var drawPart = AnimationHelper.GetDrawPart(sprite, timeElapsed);
                var origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
                var position = new Vector2(spriteEntry.ScreenPosition.X, spriteEntry.ScreenPosition.Y);
                spriteBatch.Draw(sprite.Sprite, position, drawPart, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
