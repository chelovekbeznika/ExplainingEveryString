using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Displaying
{
    internal class SpecEffect : IDisplayble, GameModel.IUpdateable
    {
        public SpriteState SpriteState { get; private set; }
        public IEnumerable<IDisplayble> GetParts() => Enumerable.Empty<IDisplayble>();
        private Func<Vector2> positionLocator;
        public Vector2 Position => positionLocator();

        public Boolean IsVisible
        {
            get { return IsAlive(); }
        }

        internal SpecEffect(Func<Vector2> positionLocator, Single angle, SpriteSpecification sprite)
        {
            this.SpriteState = new SpriteState(sprite) { Angle = angle, Looping = false };
            this.positionLocator = positionLocator;
        }

        public void Update(Single elapsedSeconds)
        {
            SpriteState.Update(elapsedSeconds);
        }

        public Boolean IsAlive()
        {
            return SpriteState.ElapsedTime < SpriteState.AnimationCycle - Constants.Epsilon;
        }
    }
}
