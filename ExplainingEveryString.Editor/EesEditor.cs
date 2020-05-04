using ExplainingEveryString.Core;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.AssetsMetadata;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;

namespace ExplainingEveryString.Editor
{
    public class EesEditor : EesApp
    {
        private readonly String levelToEdit;
        private SpriteBatch spriteBatch;
        private IScreenCoordinatesMaster screenCoordinatesMaster;
        private TiledMapDisplayer mapDisplayer;
        private TileWrapper map;
        private LevelData levelData;
        private KeyboardInputProcessor keyboardInput = new KeyboardInputProcessor();
        private IEditorMode currentMode = null; 

        public EesEditor(string levelToEdit)
        {
            PreInit();
            this.levelToEdit = levelToEdit;
        }

        protected override void Initialize()
        {
            var levelLoader = LevelDataAccess.GetLevelLoader();
            this.levelData = levelLoader.Load(levelToEdit);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.map = new TileWrapper(Content.Load<TiledMap>(levelData.TileMap));
            var config = ConfigurationAccess.GetCurrentConfig().Camera;
            var editorCameraFocus = new EditorInfoForCameraExtractor(map.GetLevelPosition(levelData.PlayerPosition.TilePosition), keyboardInput);
            var levelCoordinatesMaster = new CameraObjectGlass(editorCameraFocus, GraphicsDevice.Viewport, config);
            this.screenCoordinatesMaster = new ScreenCoordinatesMaster(GraphicsDevice.Viewport, levelCoordinatesMaster);
            this.mapDisplayer = new TiledMapDisplayer(map, this, screenCoordinatesMaster);
            this.currentMode = InitEditorModes()[0];
            currentMode.Load(levelData);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            keyboardInput.Update(elapsedSeconds);
            screenCoordinatesMaster.Update(elapsedSeconds);
            mapDisplayer.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            mapDisplayer.Draw();
            currentMode?.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private IEditorMode[] InitEditorModes()
        {
            var blueprintsLoader = BlueprintsAccess.GetLoader(levelData.Blueprints);
            blueprintsLoader.Load();
            var blueprintsDisplayer = new BlueprintDisplayer(Content, blueprintsLoader, AssetsMetadataAccess.GetLoader().Load());
            return new IEditorMode[]
            {
                new ObstaclesEditorMode(screenCoordinatesMaster, map, blueprintsDisplayer)
            };
        }
    }
}
