﻿using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Core.Text;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core
{
    public class EesGame : EesApp
    {
        private OuterMenuInputProcessor menuInputProcessor;
        private SpriteBatch targetBatch;
        private RenderTarget2D unscaledRenderTarget;

        internal FontsStorage FontsStorage { get; private set; }
        internal GameStateManager GameState { get; private set; }

        public EesGame()
        {
            PreInit();
        }

        protected override void Initialize()
        {
            GraphicsPreInit();
            var levelSequenceSpecification = LevelSequenceAccess.LoadLevelSequence();
            var cutscenesMetadata = CutscenesMetadataAccess.LoadCutscenesMetadata();
            var musicTestSpecification = MusicTestSpecificationAccess.Load();

            var componentsManager = new ComponentsManager(this, levelSequenceSpecification,
                cutscenesMetadata, musicTestSpecification);

            FontsStorage = new FontsStorage();
            var config = ConfigurationAccess.GetCurrentConfig();
            GameState = new GameStateManager(levelSequenceSpecification, componentsManager, config.SaveProfile);
            menuInputProcessor = new OuterMenuInputProcessor();
            menuInputProcessor.Pause.ButtonPressed += (sender, e) => GameState.TryPauseSwitch();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            targetBatch = new SpriteBatch(GraphicsDevice);
            unscaledRenderTarget = new RenderTarget2D(GraphicsDevice, Constants.TargetWidth, Constants.TargetHeight);
            IsMouseVisible = false;
            base.LoadContent();
            FontsStorage.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (Components.Count == 0)
            {
                GameState.InitComponents();
            }
            base.Update(gameTime);
            menuInputProcessor.Update(gameTime);
            GameState.Update((Single)gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(unscaledRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);

            var config = ConfigurationAccess.GetCurrentConfig().Screen;
            GraphicsDevice.SetRenderTarget(null);
            targetBatch.Begin();
            targetBatch.Draw(unscaledRenderTarget, FitWithNesAspectRatio(config), Color.White);
            targetBatch.End();
        }

        internal void ConfigChanged()
        {
            var newConfig = ConfigurationAccess.GetCurrentConfig();
            GameState.ConfigChanged(newConfig);
            ChangeScreenResolution(newConfig.Screen);
            unscaledRenderTarget = new RenderTarget2D(GraphicsDevice, Constants.TargetWidth, Constants.TargetHeight);
        }

        /// <summary>
        /// Which is 8:7
        /// </summary>
        /// <returns></returns>
        private Rectangle FitWithNesAspectRatio(ScreenConfiguration screenConfiguration)
        {
            var widthWithoutVerticalStripes = (int)System.Math.Round((double)(screenConfiguration.ScreenHeight * 8.0 / 7.0));
            var stripWidth = (screenConfiguration.ScreenWidth - widthWithoutVerticalStripes) / 2;
            return new Rectangle(stripWidth, 0, widthWithoutVerticalStripes, screenConfiguration.ScreenHeight);
        }
    }
}
