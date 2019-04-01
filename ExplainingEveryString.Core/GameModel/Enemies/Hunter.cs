using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel.Enemies
{
    internal class Hunter : Enemy<HunterBlueprint>, IUpdatable, ITouchableByBullets, ICrashable
    {
        private Single acceleration;
        private Single startSpeed;
        private Single playerDetectionRange;
        private Boolean turnedOn;
        private Vector2 currentSpeed;

        protected override void Construct(HunterBlueprint blueprint, Level level)
        {
            base.Construct(blueprint, level);
            this.acceleration = blueprint.Acceleration;
            this.startSpeed = blueprint.StartSpeed;
            this.playerDetectionRange = blueprint.PlayerDetectionRange;
            this.turnedOn = false;
        }

        public void Update(Single elapsedSeconds)
        {
            Vector2 playerPosition = PlayerPosition;
            Vector2 vectorToPlayer = playerPosition - this.Position;
            if (!turnedOn)
            {
                turnedOn = vectorToPlayer.Length() <= playerDetectionRange;
                currentSpeed = vectorToPlayer / vectorToPlayer.Length() * startSpeed;
            }
            if (turnedOn)
            {
                Vector2 oneSecondSpeedChange = vectorToPlayer / vectorToPlayer.Length() * acceleration;
                Vector2 speedChange = oneSecondSpeedChange * elapsedSeconds;
                currentSpeed += speedChange;
                if (currentSpeed.Length() > MaxSpeed)
                    currentSpeed = currentSpeed / currentSpeed.Length() * MaxSpeed;
                Position += currentSpeed * elapsedSeconds;
            }
        }
    }
}
