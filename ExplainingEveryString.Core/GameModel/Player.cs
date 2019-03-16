using ExplainingEveryString.Core.Blueprints;
using ExplainingEveryString.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal sealed class Player : GameObject<PlayerBlueprint>
    {
        private Vector2 speed = new Vector2(0,0);
        private Single maxSpeed;
        private Single maxAcceleration;
        
        protected override void Construct(PlayerBlueprint blueprint)
        {
            base.Construct(blueprint);
            maxSpeed = blueprint.MaxSpeed;
            maxAcceleration = blueprint.MaxAcceleration;
        }

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
