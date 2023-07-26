using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Data.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ExplainingEveryString.Core
{
    internal abstract class StaticImagesSequenceComponent : DrawableGameComponent
    {
        private Texture2D cutSceneSkipTip;
        private Texture2D frameSkipTip;
        private readonly Single maxFrameTime = 5f;
        private readonly Single minFrameTime = 0.333333f;
        
        private Boolean frameSkipped = false;
        private Boolean sceneSkipped = false;
        private Color background;
        private SpriteBatch spriteBatch;

        protected Int32 FrameNumber { get; private set; } = 0;
        protected Single FrameTime { get; private set; } = 0;
        protected Int32 FramesCount { get; private set; }
        internal Boolean Closed => FrameNumber >= FramesCount || sceneSkipped;
        private Boolean FrameCanBeSkipped => FrameTime >= minFrameTime;
        private Boolean SceneCanBeSkipped => FrameTime >= minFrameTime || FrameNumber > 0;

        public StaticImagesSequenceComponent(Game game, Single minFrameTime, Single maxFrameTime, Int32 framesCount) : base(game)
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
            FrameTime += (Single)gameTime.ElapsedGameTime.TotalSeconds;
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
            if (FrameTime >= maxFrameTime || frameSkipped)
            {
                FrameNumber += 1;
                FrameTime = 0;
                frameSkipped = false;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);
            spriteBatch.Begin();
            DrawImage(spriteBatch, FrameNumber);
            if (SceneCanBeSkipped)
                spriteBatch.Draw(cutSceneSkipTip, new Vector2(Constants.TargetWidth - 64, Constants.TargetHeight - 64), Color.White);
            if (FrameCanBeSkipped)
                spriteBatch.Draw(frameSkipTip, new Vector2(Constants.TargetWidth - 128, Constants.TargetHeight - 64), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected abstract void DrawImage(SpriteBatch spriteBatch, Int32 frameNumber);
    }
}
