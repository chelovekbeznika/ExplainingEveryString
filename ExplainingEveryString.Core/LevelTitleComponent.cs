using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core
{
    internal class LevelTitleComponent : CutSceneComponent
    {
        private Texture2D levelTitle;
        private LevelSequence levelSequence;
        private String levelTitleName;
        private readonly Vector2 screenCenter = new Vector2(Displaying.Constants.TargetWidth / 2, Displaying.Constants.TargetHeight / 2);

        internal LevelTitleComponent(EesGame eesGame, LevelSequence levelSequence) : 
            base(eesGame, minFrameTime: 1f / 3, maxFrameTime: 5, frames: 1)
        {
            this.UpdateOrder = ComponentsOrder.Title;
            this.DrawOrder = ComponentsOrder.Title;
            this.levelSequence = levelSequence;
        }

        public override void Initialize()
        {
            this.levelTitleName = levelSequence.GetCurrentLevelTitle();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.levelTitle = Game.Content.Load<Texture2D>(levelTitleName);
        }

        protected override void DrawCutScene(SpriteBatch spriteBatch, Int32 frameNumber)
        {
            var spriteCenter = new Vector2(levelTitle.Width / 2, levelTitle.Height / 2);
            spriteBatch.Draw(levelTitle, screenCenter, null, Color.White, 0, spriteCenter, 1, SpriteEffects.None, 0);
        }
    }
}
