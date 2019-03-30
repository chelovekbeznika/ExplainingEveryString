using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Hunter : Enemy<EnemyBlueprint>
    {
        public override void Update(Single elapsedSeconds)
        {
            Vector2 playerPosition = PlayerLocator();
            Vector2 moveDirection = playerPosition - this.Position;
            Vector2 oneSecondPositionChange = moveDirection / moveDirection.Length() * MaxSpeed;
            Vector2 positionChange = oneSecondPositionChange * elapsedSeconds;
            Position += positionChange;
        }
    }
}
