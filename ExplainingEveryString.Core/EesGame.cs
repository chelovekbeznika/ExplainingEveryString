using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Core.Text;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            var musicTestSpecification = MusicTestSpecificationAccess.Load();

            var componentsManager =  new ComponentsManager(this, levelSequenceSpecification, 
                musicTestSpecification);

            this.FontsStorage = new FontsStorage();
            this.GameState = new GameStateManager(levelSequenceSpecification, componentsManager);
            this.menuInputProcessor = new OuterMenuInputProcessor();
            menuInputProcessor.Pause.ButtonPressed += (sender, e) => GameState.TryPauseSwitch();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            var configuration = ConfigurationAccess.GetCurrentConfig().Screen;
            targetBatch = new SpriteBatch(GraphicsDevice);
            unscaledRenderTarget = new RenderTarget2D(GraphicsDevice, configuration.TargetWidth, configuration.TargetHeight);
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
            GameState.Update();
            menuInputProcessor.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(unscaledRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);

            var config = ConfigurationAccess.GetCurrentConfig().Screen;
            GraphicsDevice.SetRenderTarget(null);
            targetBatch.Begin();
            targetBatch.Draw(unscaledRenderTarget, new Rectangle(0, 0, config.ScreenWidth, config.ScreenHeight), Color.White);
            targetBatch.End();
        }

        internal void ConfigChanged()
        {
            var newConfig = ConfigurationAccess.GetCurrentConfig();
            GameState.ConfigChanged(newConfig);
            ChangeScreenResolution(newConfig.Screen);
        }
    }
}
