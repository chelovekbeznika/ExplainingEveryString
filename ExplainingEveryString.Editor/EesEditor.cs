using ExplainingEveryString.Core;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System;

namespace ExplainingEveryString.Editor
{
    public class EesEditor : EesApp
    {
        private readonly String levelToEdit;
        private IScreenCoordinatesMaster screenCoordinatesMaster;
        private TiledMapDisplayer mapDisplayer;
        private LevelData levelData;
        private KeyboardInputProcessor keyboardInput = new KeyboardInputProcessor();

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
            var map = new TileWrapper(Content.Load<TiledMap>(levelData.TileMap));
            var config = ConfigurationAccess.GetCurrentConfig().Camera;
            var editorCameraFocus = new EditorInfoForCameraExtractor(map.GetPosition(levelData.PlayerPosition.TilePosition), keyboardInput);
            var levelCoordinatesMaster = new CameraObjectGlass(editorCameraFocus, GraphicsDevice.Viewport, config);
            this.screenCoordinatesMaster = new ScreenCoordinatesMaster(GraphicsDevice.Viewport, levelCoordinatesMaster);
            this.mapDisplayer = new TiledMapDisplayer(map, this, screenCoordinatesMaster);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            mapDisplayer.Draw();
            base.Draw(gameTime);
        }
    }
}
