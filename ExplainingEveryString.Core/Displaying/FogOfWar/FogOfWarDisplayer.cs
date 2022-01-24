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
        private SpriteData defaultSprite = null;
        private IScreenCoordinatesMaster screenCoordinatesMaster;

        public FogOfWarSpecification Specification { get; private set; }

        public FogOfWarDisplayer(IScreenCoordinatesMaster screenCoordinatesMaster)
        {
            this.screenCoordinatesMaster = screenCoordinatesMaster;
        }

        public void Construct(FogOfWarSpecification specification, SpriteDataBuilder spriteDataBuilder)
        {
            this.Specification = specification;
            this.fogParticles = spriteDataBuilder.Build(specification.Sprites).Values.ToArray();
            if (specification.DefaultSprite != null)
            {
                defaultSprite = spriteDataBuilder.Build(new[] { specification.DefaultSprite }).Values.First();
            }
        }

        public void Update(Single elapsedSeconds)
        {
            timeElapsed += elapsedSeconds;
        }

        public void Draw(SpriteBatch spriteBatch, FogOfWarSpriteEntry[] fogOfWarSpriteEntries)
        {
            var currentVerticalBorder = screenCoordinatesMaster.ConvertToScreenPosition(new Vector2(Specification.PutDefaultSpritesLeftOf, 0)).X;
            foreach (FogOfWarSpriteEntry spriteEntry in fogOfWarSpriteEntries)
            {
                var sprite = (defaultSprite != null && spriteEntry.ScreenPosition.X < currentVerticalBorder)
                        ? defaultSprite
                        : fogParticles[spriteEntry.SpriteNumber];
                var drawPart = AnimationHelper.GetDrawPart(sprite, timeElapsed);
                var origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
                var position = new Vector2(spriteEntry.ScreenPosition.X, spriteEntry.ScreenPosition.Y);
                spriteBatch.Draw(sprite.Sprite, position, drawPart, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            }
        }
    }
}
