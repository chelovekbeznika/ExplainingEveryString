using ExplainingEveryString.Core.GameState;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Core
{
    internal class LevelEndingComponent : CutSceneComponent
    {
        private enum BlinkPhase { Shown = 0, Rotated = 1, NotShown = 2, RotatedAgain = 3 }

        private BlinkPhase blinkPhase = BlinkPhase.Shown;
        private Dictionary<BlinkPhase, Single> phaseLayout = new Dictionary<BlinkPhase, Single>()
        {
            { BlinkPhase.Shown, 0.375f },
            { BlinkPhase.Rotated, 0.125f },
            { BlinkPhase.NotShown, 0.125f },
            { BlinkPhase.RotatedAgain, 0.125f }
        };
        private Single inPhase = 0;
        private Texture2D map;
        private Texture2D pathMark;
        private Texture2D nextMark;
        private List<Texture2D> layers;
        private LevelSequence levelSequence;

        public LevelEndingComponent(Game game, LevelSequence levelSequence) : 
            base(game, minFrameTime: 3, maxFrameTime: 10, frames: 1)
        {
            this.UpdateOrder = ComponentsOrder.Ending;
            this.DrawOrder = ComponentsOrder.Ending;
            this.levelSequence = levelSequence;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.map = Game.Content.Load<Texture2D>(@"Sprites/LevelEndings/Map");
            this.pathMark = Game.Content.Load<Texture2D>(@"Sprites/LevelEndings/MapMark");
            this.nextMark = Game.Content.Load<Texture2D>(@"Sprites/LevelEndings/NextMapMark");
            this.layers = levelSequence.GetCurrentLevelEndingLayers()
                .Select(spriteName => Game.Content.Load<Texture2D>(spriteName)).ToList();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            inPhase += (Single)gameTime.ElapsedGameTime.TotalSeconds;
            while (inPhase >= phaseLayout[blinkPhase])
            {
                inPhase -= phaseLayout[blinkPhase];
                blinkPhase = (int)blinkPhase < 3 ? blinkPhase + 1 : 0;
            }
        }

        protected override void DrawCutScene(SpriteBatch spriteBatch, Int32 frameNumber)
        {
            spriteBatch.Draw(map, Vector2.Zero, Color.White);
            foreach (var layer in layers)
                spriteBatch.Draw(layer, Vector2.Zero, Color.White);
            var nextMarkPosition = levelSequence.GetNextLevelMapMark();
            if (nextMarkPosition != null)
            {
                var spriteCenter = new Vector2(nextMark.Width / 2, nextMark.Height / 2);
                var centerPosition = nextMarkPosition.Value - spriteCenter;
                switch (blinkPhase)
                {
                    case BlinkPhase.Shown:
                        spriteBatch.Draw(nextMark, centerPosition, Color.White);
                        break;
                    case BlinkPhase.NotShown:
                        break;
                    case BlinkPhase.Rotated:
                    case BlinkPhase.RotatedAgain:
                        spriteBatch.Draw(nextMark, nextMarkPosition.Value, null, Color.White, (Single)System.Math.PI / 4, spriteCenter, 1, SpriteEffects.None, 0);
                        break;
                }
            }
            foreach (var markPosition in levelSequence.GetPath())
                spriteBatch.Draw(pathMark, markPosition - new Vector2(pathMark.Width / 2, pathMark.Height / 2), Color.White);
        }
    }
}
