using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Data;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
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
        private LevelData levelData;
        private EesGame eesGame;
        private TiledMapDisplayer mapDisplayer;
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
            this.levelData = levelLoader.Load(levelFileName);
            level = new Level(factory, new PlayerInputFactory(this), levelData);
            level.Lost += eesGame.GameLost;

            base.Initialize();
        }

        protected override void LoadContent()
        {         
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            Camera = new Camera(level, eesGame, config);
            EpicEventsProcessor = new EpicEventsProcessor(eesGame.AssetsStorage, level, config);
            this.mapDisplayer = new TiledMapDisplayer(levelData, eesGame, Camera);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Single elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            level.Update(elapsedSeconds);
            Camera.Update();
            mapDisplayer.Update(gameTime);
            EpicEventsProcessor.Update(elapsedSeconds);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Camera.Begin();
            mapDisplayer.Draw();
            Camera.Draw(level.GetObjectsToDraw());
            EpicEventsProcessor.ProcessEpicEvents();
            Camera.Draw(EpicEventsProcessor.GetSpecEffectsToDraw());
            Camera.End();
            base.Draw(gameTime);
        }

        internal InterfaceInfo GetInterfaceInfo()
        {
            return level.GetInterfaceInfo(Camera);
        }
    }
}
