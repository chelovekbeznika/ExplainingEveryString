using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel
{
    internal class PlayerBullet : IDisplayble, IUpdatable
    {
        private String spriteName;
        private Vector2 speed;
        private Vector2 position;
        private Vector2 oldPosition;
        private Single remainingDistance;
        private Boolean alive = true;

        public string CurrentSpriteName => spriteName;
        public Vector2 Position => position;
        internal Vector2 OldPosition => oldPosition;
        
        internal PlayerBullet(String spriteName, Vector2 position, Vector2 speed, Single range)
        {
            this.spriteName = spriteName;
            this.position = position;
            this.oldPosition = position;
            this.speed = speed;
            this.remainingDistance = range;
        }

        public void Update(Single elapsedSeconds)
        {
            oldPosition = position;
            Vector2 positionChange = speed * elapsedSeconds;
            position += positionChange;
            remainingDistance -= positionChange.Length();
            if (remainingDistance < 0)
                alive = false;
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
