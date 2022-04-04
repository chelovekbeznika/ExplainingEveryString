using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Displaying.Debug;
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
        private IBlueprintsLoader blueprintsLoader;
        private Level level;
        private readonly String levelFileName;
        private readonly LevelProgress levelStart;
        private LevelData levelData;
        private SpriteEmitter spriteEmitter;
        private EesGame eesGame;
        private TileWrapper map;
        private TiledMapDisplayer mapDisplayer;
        private FogOfWarRuler fogOfWarRuler;
        private SpriteBatch spriteBatch;
#if DEBUG
        private DebugInfoDisplayer debugInfoDisplayer;
#endif

        internal Camera Camera { get; private set; }
        internal EpicEventsProcessor EpicEventsProcessor { get; private set; }
        internal Boolean Lost => level.Lost;
        internal Boolean Won => level.Won;

        internal GameplayComponent(EesGame game, String levelFileName, LevelProgress levelStart) 
            : base(game)
        {
            this.eesGame = game;
            this.levelFileName = levelFileName;
            this.levelStart = levelStart;

            this.DrawOrder = ComponentsOrder.Gameplay;
            this.UpdateOrder = ComponentsOrder.Gameplay;
        }

        public override void Initialize()
        {
            var levelLoader = LevelDataAccess.GetLevelLoader();
            this.levelData = levelLoader.Load(levelFileName);
            this.blueprintsLoader = BlueprintsAccess.GetLoader(levelData.Blueprints);
            blueprintsLoader.Load();
            var factory = new ActorsFactory(blueprintsLoader);
            map = new TileWrapper(levelData.TileMap, eesGame.Content);
            level = new Level(factory, map, new PlayerInputFactory(this), levelData, levelStart);
            level.CheckpointReached += eesGame.GameState.NotableProgressMaid;
            if (levelData.SpriteEmitter != null)
                spriteEmitter = new SpriteEmitter(levelData.SpriteEmitter, map);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            var config = ConfigurationAccess.GetCurrentConfig();
            var levelCoordinatesMaster = new CameraObjectGlass(new PlayerInfoForCameraExtractor(level), config.Camera);
            var screenCoordinatesMaster = new ScreenCoordinatesMaster(levelCoordinatesMaster);
            var assetsStorage = CreateFilledAssetsStorage();
            Camera = new Camera(assetsStorage, screenCoordinatesMaster);
            map.LoadContent(Game.Content);
            this.mapDisplayer = new TiledMapDisplayer(map, eesGame, screenCoordinatesMaster);
            EpicEventsProcessor = new EpicEventsProcessor(assetsStorage, level, config);
            this.fogOfWarRuler = ConstructFogOfWarRuler(levelCoordinatesMaster, screenCoordinatesMaster);
            eesGame.GameState.StartMusicInGame(levelData.MusicName);
#if DEBUG
            this.debugInfoDisplayer = new DebugInfoDisplayer(screenCoordinatesMaster, Game.Content);
#endif
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
            var extractor = new LevelFogOfWarExtractor(map);
            var screenDetector = new ScreenFogOfWarDetector(levelCoordinatesMaster, screenCoordinatesMaster);
            var filler = new FogOfWarFiller();

            var displayer = new FogOfWarDisplayer();
            var spriteDataBuilder = new SpriteDataBuilder(Game.Content, AssetsMetadataAccess.GetLoader());
            displayer.Construct(levelData.FogOfWar, spriteDataBuilder);

            return new FogOfWarRuler(extractor, screenDetector, filler, displayer);
        }

        public override void Update(GameTime gameTime)
        {
            var elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
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
