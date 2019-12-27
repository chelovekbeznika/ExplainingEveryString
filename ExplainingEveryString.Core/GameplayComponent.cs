using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Displaying.FogOfWar;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;

namespace ExplainingEveryString.Core
{
    internal class GameplayComponent : DrawableGameComponent
    {
        private readonly IBlueprintsLoader blueprintsLoader;
        private Level level;
        private readonly String levelFileName;
        private readonly LevelProgress levelStart;
        private LevelData levelData;
        private EesGame eesGame;
        private TileWrapper map;
        private TiledMapDisplayer mapDisplayer;
        private FogOfWarRuler fogOfWarRuler;
        private SpriteBatch spriteBatch;

        internal Camera Camera { get; private set; }
        internal EpicEventsProcessor EpicEventsProcessor { get; private set; }
        internal Boolean Lost => level.Lost;
        internal Boolean Won => level.Won;

        internal GameplayComponent(EesGame game, IBlueprintsLoader blueprintsLoader, String levelFileName, LevelProgress levelStart) 
            : base(game)
        {
            this.eesGame = game;
            this.blueprintsLoader = blueprintsLoader;
            this.levelFileName = levelFileName;
            this.levelStart = levelStart;

            this.DrawOrder = ComponentsOrder.Gameplay;
            this.UpdateOrder = ComponentsOrder.Gameplay;
        }

        public override void Initialize()
        {
            ActorsFactory factory = new ActorsFactory(blueprintsLoader);
            ILevelLoader levelLoader = LevelDataAccess.GetLevelLoader();
            this.levelData = levelLoader.Load(levelFileName);
            map = new TileWrapper(eesGame.Content.Load<TiledMap>(levelData.TileMap));
            level = new Level(factory, map, new PlayerInputFactory(this), levelData, levelStart);
            level.CheckpointReached += eesGame.GameState.NotableProgressMaid;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            Configuration config = ConfigurationAccess.GetCurrentConfig();
            Viewport viewport = Game.GraphicsDevice.Viewport;
            ILevelCoordinatesMaster levelCoordinatesMaster = new CameraObjectGlass(level, viewport, config.Camera);
            IScreenCoordinatesMaster screenCoordinatesMaster = new ScreenCoordinatesMaster(viewport, levelCoordinatesMaster);
            Camera = new Camera(level, eesGame, screenCoordinatesMaster);   
            this.mapDisplayer = new TiledMapDisplayer(map, eesGame, screenCoordinatesMaster);
            EpicEventsProcessor = new EpicEventsProcessor(eesGame.AssetsStorage, level, config);
            this.fogOfWarRuler = ConstructFogOfWarRuler(levelCoordinatesMaster, screenCoordinatesMaster);
            eesGame.GameState.StartMusicInGame(levelData.MusicName);
            base.LoadContent();
        }

        private FogOfWarRuler ConstructFogOfWarRuler(
            ILevelCoordinatesMaster levelCoordinatesMaster, IScreenCoordinatesMaster screenCoordinatesMaster)
        {
            ILevelFogOfWarExtractor extractor = new LevelFogOfWarExtractor(map);
            IScreenFogOfWarDetector screenDetector = new ScreenFogOfWarDetector(levelCoordinatesMaster, screenCoordinatesMaster);
            IFogOfWarFiller filler = new FogOfWarFiller();

            IFogOfWarDisplayer displayer = new FogOfWarDisplayer();
            SpriteDataBuilder spriteDataBuilder = new SpriteDataBuilder(Game.Content, AssetsMetadataAccess.GetLoader());
            displayer.Construct(levelData.FogOfWar, spriteDataBuilder);

            return new FogOfWarRuler(extractor, screenDetector, filler, displayer);
        }

        public override void Update(GameTime gameTime)
        {
            Single elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            level.Update(elapsedSeconds);
            Camera.Update(elapsedSeconds);
            mapDisplayer.Update(gameTime);
            EpicEventsProcessor.Update(elapsedSeconds);
            fogOfWarRuler.Update(elapsedSeconds);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            mapDisplayer.Draw();
            Camera.Draw(spriteBatch, level.GetObjectsToDraw());
            EpicEventsProcessor.ProcessEpicEvents();
            Camera.Draw(spriteBatch, EpicEventsProcessor.GetSpecEffectsToDraw());
            fogOfWarRuler.DrawFogOfWar(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        internal InterfaceInfo GetInterfaceInfo()
        {
            return level.GetInterfaceInfo(Camera);
        }
    }
}
