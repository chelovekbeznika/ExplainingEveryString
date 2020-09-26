using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Menu;
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

            this.GameState = new GameStateManager(this, levelSequenceSpecification, componentsManager);
            this.menuInputProcessor = new OuterMenuInputProcessor(ConfigurationAccess.GetCurrentConfig());
            menuInputProcessor.Pause.ButtonPressed += (sender, e) => GameState.TryPauseSwitch();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}
