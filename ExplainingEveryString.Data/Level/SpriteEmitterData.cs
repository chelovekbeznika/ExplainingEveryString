using ExplainingEveryString.Data.RandomVariables;
using ExplainingEveryString.Data.Specifications;
using Microsoft.Xna.Framework;

namespace ExplainingEveryString.Data.Level
{
    public class SpriteEmitterData
    {
        public Rectangle Region { get; set; }
        public GaussRandomVariable SpriteLifetime { get; set; }
        public GaussRandomVariable BetweenSpawns { get; set; }
        public Proportions<SpriteSpecification> RandomSprites { get; set; }
        public GaussRandomVariable SpriteSpeedMovement { get; set; }
        public Proportions<Vector2> RandomDirections { get; set; }
    }
}
