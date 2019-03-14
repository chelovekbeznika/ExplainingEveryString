using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal sealed class Player : GameObject
    {
        internal const String CommonSpriteName = "player";

        private Vector2 speed = new Vector2(0,0);
        private readonly Single maxSpeed = 300;
        private readonly Single maxAcceleration = 100;

        protected override Single Height => 32;
        protected override Single Width => 32;

        internal Player() : base(CommonSpriteName, new Vector2(0,0)) { }

        internal void Move(Single elapsedSeconds)
        {
            Vector2 speed = GetCurrentSpeed(elapsedSeconds);
            Vector2 positionChange = speed * elapsedSeconds;
            Position += positionChange;
        }

        private Vector2 GetCurrentSpeed(Single elapsedSeconds)
        {
            Vector2 acceleration = GetAcceleration();
            speed += acceleration;
            if (speed.Length() > maxSpeed)
            {
                Single overcharge = speed.Length() / maxSpeed;
                speed /= overcharge;
            }
            if (acceleration.Length() == 0)
            {
                speed = FrictionCorrector.Correct(speed, elapsedSeconds);
            }

            return speed;
        }

        private Vector2 GetAcceleration()
        {
            IPlayerInput playerInput = PlayerInputFactory.Create();
            Vector2 direction = playerInput.GetMoveDirection();
            return direction * maxAcceleration;
        }
    }
}
