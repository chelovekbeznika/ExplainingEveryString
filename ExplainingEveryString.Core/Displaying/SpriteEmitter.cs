using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.Displaying
{
    internal class SpriteEmitter : GameModel.IUpdateable
    {
        private SpriteEmitterData data;
        private Single tillNextEmittedSprite;
        private Hitbox spawnRegion;

        internal List<EmittedSprite> EmittedSprites { get; private set; } = new List<EmittedSprite>();

        internal SpriteEmitter(SpriteEmitterData data, TileWrapper map)
        {
            this.data = data;
            this.spawnRegion = map.GetHitbox(data.Region);
        }

        public void Update(Single elapsedSeconds)
        {
            var remainedTimeInFrame = elapsedSeconds;
            while (remainedTimeInFrame > 0)
            {
                if (tillNextEmittedSprite <= remainedTimeInFrame)
                {
                    remainedTimeInFrame -= tillNextEmittedSprite;
                    EmitSprite();
                    tillNextEmittedSprite = RandomUtility.NextGauss(data.BetweenSpawns);
                }
                else
                {
                    tillNextEmittedSprite -= remainedTimeInFrame;
                    remainedTimeInFrame = 0;
                }
            }
            EmittedSprites = EmittedSprites.Where(es => es.IsVisible).ToList();

            foreach (var sprite in EmittedSprites)
                sprite.Update(elapsedSeconds);
        }

        private void EmitSprite()
        {
            var startPosition = new Vector2(
                x: RandomUtility.Next(spawnRegion.Left, spawnRegion.Right), 
                y: RandomUtility.Next(spawnRegion.Bottom, spawnRegion.Top));
            var sprite = RandomUtility.SelectFromProportions(data.RandomSprites);
            var lifetime = RandomUtility.NextGauss(data.SpriteLifetime);
            var speed = RandomUtility.SelectFromProportions(data.RandomDirections) * RandomUtility.NextGauss(data.SpriteSpeedMovement);
            EmittedSprites.Add(new EmittedSprite(sprite, startPosition, speed, lifetime));
        }
    }
}
