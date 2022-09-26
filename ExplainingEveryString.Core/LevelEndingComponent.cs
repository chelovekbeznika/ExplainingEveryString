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
        private Texture2D map;
        private List<Texture2D> layers;
        private LevelSequence levelSequence;

        public LevelEndingComponent(Game game, LevelSequence levelSequence) : 
            base(game, minLifeTime: 3, maxLifeTime: 10)
        {
            this.UpdateOrder = ComponentsOrder.Ending;
            this.DrawOrder = ComponentsOrder.Ending;
            this.levelSequence = levelSequence;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.map = Game.Content.Load<Texture2D>(@"Sprites/LevelEndings/Map");
            this.layers = levelSequence.GetCurrentLevelEndingLayers()
                .Select(spriteName => Game.Content.Load<Texture2D>(spriteName)).ToList();
        }

        protected override void DrawCutScene(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(map, Vector2.Zero, Color.White);
            foreach (var layer in layers)
                spriteBatch.Draw(layer, Vector2.Zero, Color.White);
        }
    }
}
