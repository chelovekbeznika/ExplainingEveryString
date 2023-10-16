using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Math;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core
{
    internal class RecordCelebrationGenerator : GameModel.IUpdateable
    {
        private struct Firework
        {
            public Vector2 Position;
            public Single TimeToLive;
            public Single LiveTime;
        }

        private SpriteData recordFireworkSprite;
        private SoundEffect recordFireworkSound;
        private RecordFireworkConfiguration config;

        private List<Firework> fireworks = new List<Firework>();
        private Single tillNextFirework;
        private Single tillNextFireworkSound = 0;

        public RecordCelebrationGenerator(SpriteData recordFireworkSprite, SoundEffect recordFireworkSound, 
            RecordFireworkConfiguration config)
        {
            this.recordFireworkSprite = recordFireworkSprite;
            this.recordFireworkSound = recordFireworkSound;
            this.config = config;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            foreach (var firework in fireworks)
            {
                var drawPart = AnimationHelper.GetDrawPart(recordFireworkSprite, firework.TimeToLive, firework.LiveTime);
                spriteBatch.Draw(recordFireworkSprite.Sprite, firework.Position, drawPart, Color.White);
            }
        }

        public void Update(Single elapsedSeconds)
        {
            fireworks = fireworks.Select(f => { f.LiveTime += elapsedSeconds; return f; }).ToList();
            var remainedTimeInFrame = elapsedSeconds;
            while (remainedTimeInFrame > 0)
            {
                if (tillNextFirework <= remainedTimeInFrame)
                {
                    remainedTimeInFrame -= tillNextFirework;
                    tillNextFireworkSound -= tillNextFirework;
                    FireUp(remainedTimeInFrame);
                    tillNextFirework = RandomUtility.NextGauss(config.BetweenSpawns);
                }
                else
                {
                    tillNextFirework -= remainedTimeInFrame;
                    tillNextFireworkSound -= remainedTimeInFrame;
                    remainedTimeInFrame = 0;
                }
            }
            fireworks = fireworks.Where(f => f.LiveTime < f.TimeToLive).ToList();
        }

        private void FireUp(Single remainedTimeInFrame)
        {
            var position = new Vector2(
                x: RandomUtility.NextInt(Displaying.Constants.TargetWidth - recordFireworkSprite.Width),
                y: RandomUtility.NextInt(Displaying.Constants.TargetHeight - recordFireworkSprite.Height));
            var firework = new Firework
            { 
                Position = position, 
                LiveTime = remainedTimeInFrame, 
                TimeToLive = RandomUtility.NextGauss(config.LifeTime) 
            };
            fireworks.Add(firework);

            if (tillNextFireworkSound <= 0)
            {
                recordFireworkSound.Play(config.Volume, 0, 0);
                tillNextFireworkSound = RandomUtility.NextGauss(config.SoundCooldown);
            }
        }
    }
}
