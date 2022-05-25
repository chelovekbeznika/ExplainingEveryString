using ExplainingEveryString.Core.Assets;
using ExplainingEveryString.Core.Displaying;
#if DEBUG
using ExplainingEveryString.Core.Displaying.Debug;
#endif
using ExplainingEveryString.Core.Displaying.FogOfWar;
using ExplainingEveryString.Core.GameState;
using ExplainingEveryString.Core.Input;
using ExplainingEveryString.Core.Interface;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExplainingEveryString.Core.GameModel
{
    internal class GameplayComponent : DrawableGameComponent
    {
        private IBlueprintsLoader blueprintsLoader;
        private Level level;
        private readonly string levelFileName;
        private readonly LevelProgress levelStart;
        private LevelData levelData;
        private SpriteEmitter spriteEmitter;
        private EesGame eesGame;
        private TiledMapDisplayer mapDisplayer;
        private FogOfWarRuler fogOfWarRuler;
        private SpriteBatch spriteBatch;
#if DEBUG
        private DebugInfoDisplayer debugInfoDisplayer;
#endif

        internal event EventHandler ContentLoaded;
        internal TileWrapper Map { get; private set; }
        internal Camera Camera { get; private set; }
        internal EpicEventsProcessor EpicEventsProcessor { get; private set; }
        internal bool Lost => level.Lost;
        internal bool Won => level.Won;

        internal GameplayComponent(EesGame game, string levelFileName, LevelProgress levelStart)
            : base(game)
        {
            eesGame = game;
            this.levelFileName = levelFileName;
            this.levelStart = levelStart;

            DrawOrder = ComponentsOrder.Gameplay;
            UpdateOrder = ComponentsOrder.Gameplay;
        }

        public override void Initialize()
        {
            var levelLoader = LevelDataAccess.GetLevelLoader();
            levelData = levelLoader.Load(levelFileName);
            blueprintsLoader = BlueprintsAccess.GetLoader(levelData.Blueprints);
            blueprintsLoader.Load();
            var factory = new ActorsFactory(blueprintsLoader);
            Map = new TileWrapper(levelData.TileMap, eesGame.Content);
            level = new Level(factory, Map, new PlayerInputFactory(this), levelData, levelStart);
            level.CheckpointReached += eesGame.GameState.NotableProgressMaid;
            if (levelData.SpriteEmitter != null)
                spriteEmitter = new SpriteEmitter(levelData.SpriteEmitter, Map);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            var config = ConfigurationAccess.GetCurrentConfig();
            var levelCoordinatesMaster = new CameraObjectGlass(new PlayerInfoForCameraExtractor(level), config.Camera);
            var screenCoordinatesMaster = new ScreenCoordinatesMaster(levelCoordinatesMaster);
            var assetsStorage = CreateFilledAssetsStorage();
            Camera = new Camera(assetsStorage, screenCoordinatesMaster);
            Map.LoadContent(Game.Content);
            mapDisplayer = new TiledMapDisplayer(Map, eesGame, screenCoordinatesMaster);
            EpicEventsProcessor = new EpicEventsProcessor(assetsStorage, level, config);
            fogOfWarRuler = ConstructFogOfWarRuler(levelCoordinatesMaster, screenCoordinatesMaster);
            eesGame.GameState.StartMusicInGame(levelData.MusicName);
#if DEBUG
            debugInfoDisplayer = new DebugInfoDisplayer(screenCoordinatesMaster, Game.Content);
#endif
            ContentLoaded?.Invoke(this, EventArgs.Empty);
            base.LoadContent();
        }

        private IAssetsStorage CreateFilledAssetsStorage()
        {
            var metadataLoader = AssetsMetadataAccess.GetLoader();
            var assetsStorage = new AssetsStorage();
            assetsStorage.FillAssetsStorages(blueprintsLoader, levelData.SpriteEmitter, metadataLoader, eesGame.Content);
            return assetsStorage;
        }

        private FogOfWarRuler ConstructFogOfWarRuler(
            ILevelCoordinatesMaster levelCoordinatesMaster, IScreenCoordinatesMaster screenCoordinatesMaster)
        {
            var extractor = new LevelFogOfWarExtractor(Map);
            var screenDetector = new ScreenFogOfWarDetector(levelCoordinatesMaster, screenCoordinatesMaster);
            var filler = new FogOfWarFiller();

            var displayer = new FogOfWarDisplayer();
            var spriteDataBuilder = new SpriteDataBuilder(Game.Content, AssetsMetadataAccess.GetLoader());
            displayer.Construct(levelData.FogOfWar, spriteDataBuilder);

            return new FogOfWarRuler(extractor, screenDetector, filler, displayer);
        }

        public override void Update(GameTime gameTime)
        {
            var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            level.Update(elapsedSeconds);
            Camera.Update(elapsedSeconds);
            spriteEmitter?.Update(elapsedSeconds);
            mapDisplayer.Update(gameTime);
            EpicEventsProcessor.Update(elapsedSeconds);
            fogOfWarRuler.Update(elapsedSeconds);
#if DEBUG
            debugInfoDisplayer.Update(elapsedSeconds);
#endif
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            mapDisplayer.Draw();
            Camera.Draw(spriteBatch, level.GetObjectsToDraw());
            EpicEventsProcessor.ProcessEpicEvents();
            Camera.Draw(spriteBatch, EpicEventsProcessor.GetSpecEffectsToDraw());
            fogOfWarRuler.DrawFogOfWar(spriteBatch);
            if (spriteEmitter != null)
                Camera.Draw(spriteBatch, spriteEmitter.EmittedSprites);
#if DEBUG
            debugInfoDisplayer.Draw(spriteBatch);
#endif
            spriteBatch.End();
            base.Draw(gameTime);
        }

        internal InterfaceInfo GetInterfaceInfo()
        {
            return level.GetInterfaceInfo(Camera);
        }
    }
}
