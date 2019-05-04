using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel.Weaponry.Trajectories;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Core.GameModel.Weaponry
{
    internal class Bullet : IDisplayble, IUpdatable
    {
        private static TrajectoryFactory trajectoryFactory = new TrajectoryFactory();

        private Single timeToLive;
        private Single bulletAge = 0;
        private Boolean alive = true;
        private BulletTrajectory trajectory;

        public SpriteState SpriteState { get; private set; }
        public Vector2 Position { get; private set; }
        internal Vector2 OldPosition { get; private set; }
        internal Single Damage { get; private set; }

        public Boolean IsVisible => IsAlive();

        internal Bullet(Vector2 position, Vector2 fireDirection, BulletSpecification bulletSpecification)
        {
            this.SpriteState = new SpriteState(bulletSpecification.Sprite);
            this.Position = position;
            this.OldPosition = position;
            this.Damage = bulletSpecification.Damage;
            this.timeToLive = bulletSpecification.TimeToLive;
            this.trajectory = trajectoryFactory.GetTrajectory(bulletSpecification.TrajectoryType,
                position, fireDirection, bulletSpecification.TrajectoryParameters);
        }

        public void Update(Single elapsedSeconds)
        {
            OldPosition = Position;
            bulletAge += elapsedSeconds;
            Position = trajectory.GetBulletPosition(bulletAge);
            SpriteState.Update(elapsedSeconds);
        }

        public void RegisterCollision()
        {
            alive = false;
        }

        public Boolean IsAlive()
        {
            return alive && bulletAge <= timeToLive;
        }
    }
}
