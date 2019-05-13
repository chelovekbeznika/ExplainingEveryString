using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Data;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core
{
    internal class GameplayComponent : DrawableGameComponent
    {
        private IBlueprintsLoader blueprintsLoader;
        private Level level;
        private String levelFileName;
        private EesGame eesGame;
        internal Camera Camera { get; private set; }
        internal EpicEventsProcessor EpicEventsProcessor { get; private set; }

        internal GameplayComponent(IBlueprintsLoader blueprintsLoader, String levelFileName, EesGame game) : base(game)
        {
            this.eesGame = game;
            this.levelFileName = levelFileName;
            this.blueprintsLoader = blueprintsLoader;
        }

        public override void Initialize()
        {
            ActorsFactory factory = new ActorsFactory(blueprintsLoader);
            ILevelLoader levelLoader = LevelDataAccess.GetLevelLoader();
            LevelData levelData = levelLoader.Load(levelFileName);
            level = new Level(factory, new PlayerInputFactory(this), levelData);
            level.Lost += eesGame.GameLost;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            Camera = new Camera(level, eesGame, config);
            EpicEventsProcessor = new EpicEventsProcessor(eesGame.AssetsStorage, level);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Single elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            level.Update(elapsedSeconds);
            Camera.Update();
            EpicEventsProcessor.Update(elapsedSeconds);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin();
            Camera.Draw(level.GetObjectsToDraw());
            EpicEventsProcessor.ProcessEpicEvents();
            Camera.Draw(EpicEventsProcessor.GetSpecEffectsToDraw());
            Camera.End();
            base.Draw(gameTime);
        }
    }
}
