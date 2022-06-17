using ExplainingEveryString.Data.Blueprints;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class Obstacle : Actor<ObstacleBlueprint>
    {
        private Boolean justDecoration;

        public override CollidableMode CollidableMode => justDecoration ? CollidableMode.Ghost : base.CollidableMode;

        protected override void Construct(ObstacleBlueprint blueprint, ActorStartInfo info, Level level, ActorsFactory factory)
        {
            base.Construct(blueprint, info, level, factory);
            this.justDecoration = blueprint.JustDecoration;
        }
    }
}
