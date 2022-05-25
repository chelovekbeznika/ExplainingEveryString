using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Displaying
{
    internal class EmittedSprite : IDisplayble, GameModel.IUpdateable
    {
        private Vector2 speed;
        private Single lifetime;
        public SpriteState SpriteState { get; private set; }

        public Vector2 Position { get; private set; }

        public Boolean IsVisible => lifetime >= 0;

        internal EmittedSprite(SpriteSpecification specification, Vector2 startPosition, Vector2 speed, Single lifetime)
        {
            this.SpriteState = new SpriteState(specification);
            this.Position = startPosition;
            this.lifetime = lifetime;
            this.speed = speed;
        }

        public IEnumerable<IDisplayble> GetParts() => Enumerable.Empty<IDisplayble>();

        public void Update(Single elapsedSeconds)
        {
            SpriteState.Update(elapsedSeconds);
            Position += speed * elapsedSeconds;
            lifetime -= elapsedSeconds;
        }
    }
}
