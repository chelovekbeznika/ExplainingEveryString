using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel
{
    internal class PlayerBullet : IDisplayble, IUpdatable
    {
        private Vector2 speed;
        private Single remainingDistance;
        private Boolean alive = true;

        public SpriteState SpriteState { get; private set; }
        public Vector2 Position { get; private set; }
        internal Vector2 OldPosition { get; private set; }
        internal Single Damage { get; private set; }


        internal PlayerBullet(SpriteSpecification sprite, Vector2 position, Vector2 speed, Single damage, Single range)
        {
            this.SpriteState = new SpriteState(sprite);
            this.Position = position;
            this.OldPosition = position;
            this.speed = speed;
            this.Damage = damage;
            this.remainingDistance = range;
        }

        public void Update(Single elapsedSeconds)
        {
            OldPosition = Position;
            Vector2 positionChange = speed * elapsedSeconds;
            Position += positionChange;
            remainingDistance -= positionChange.Length();
            if (remainingDistance < 0)
                alive = false;

            SpriteState.Update(elapsedSeconds);
        }

        public void RegisterCollision()
        {
            alive = false;
        }

        public Boolean IsAlive()
        {
            return alive;
        }
    }
}
