using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core
{
    internal abstract class CutSceneComponent : DrawableGameComponent
    {
        private Texture2D cutSceneSkipTip;
        private Texture2D frameSkipTip;
        private readonly Single maxFrameTime = 5f;
        private readonly Single minFrameTime = 0.333333f;
        private Single frameTime = 0;
        private Int32 frameNumber = 0;
        private Boolean frameSkipped = false;
        private Boolean sceneSkipped = false;
        private Color background;
        private SpriteBatch spriteBatch;

        protected Int32 FramesCount { get; private set; }
        internal Boolean Closed => frameNumber >= FramesCount || sceneSkipped;
        private Boolean FrameCanBeSkipped => frameTime >= minFrameTime;
        private Boolean SceneCanBeSkipped => frameTime >= minFrameTime || frameNumber > 0;

        public CutSceneComponent(Game game, Single minFrameTime, Single maxFrameTime, Int32 framesCount) : base(game)
        {
            this.minFrameTime = minFrameTime;
            this.maxFrameTime = maxFrameTime;
            this.FramesCount = framesCount;
        }

        public override void Initialize()
        {
            var configuration = ConfigurationAccess.GetCurrentConfig();
            var (red, green, blue) = configuration.LevelTitleBackgroundColor;
            this.background = new Color(red, green, blue);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.cutSceneSkipTip = Game.Content.Load<Texture2D>(@"Sprites/Interface/CutSceneSkip");
            this.frameSkipTip = Game.Content.Load<Texture2D>(@"Sprites/Interface/FrameSkip");
        }

        public override void Update(GameTime gameTime)
        {
            frameTime += (Single)gameTime.ElapsedGameTime.TotalSeconds;
            if (FrameCanBeSkipped)
            {
                frameSkipped |= GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A)
                    || Keyboard.GetState().IsKeyDown(Keys.Space);
            }
            if (SceneCanBeSkipped)
            {
                sceneSkipped |= GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start)
                    || Keyboard.GetState().IsKeyDown(Keys.Enter)
                    || Keyboard.GetState().IsKeyDown(Keys.Escape);
            }
            if (frameTime >= maxFrameTime || frameSkipped)
            {
                frameNumber += 1;
                frameTime = 0;
                frameSkipped = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);
            spriteBatch.Begin();
            DrawCutScene(spriteBatch, frameNumber);
            if (SceneCanBeSkipped)
                spriteBatch.Draw(cutSceneSkipTip, new Vector2(Constants.TargetWidth - 64, Constants.TargetHeight - 64), Color.White);
            if (FrameCanBeSkipped)
                spriteBatch.Draw(frameSkipTip, new Vector2(Constants.TargetWidth - 128, Constants.TargetHeight - 64), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected abstract void DrawCutScene(SpriteBatch spriteBatch, Int32 frameNumber);
    }
}
