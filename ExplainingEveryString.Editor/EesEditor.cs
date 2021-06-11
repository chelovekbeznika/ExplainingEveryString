using ExplainingEveryString.Core;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
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
        private GridDisplayer gridDisplayer;
        private Editor editor;

        public EesEditor(string levelToEdit)
        {
            PreInit();
            this.levelToEdit = levelToEdit;
            this.IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            GraphicsPreInit();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            var levelLoader = LevelDataAccess.GetLevelLoader();
            var levelData = levelLoader.Load(levelToEdit);
            var map = new TileWrapper(levelData.TileMap, Content);
            var config = ConfigurationAccess.GetCurrentConfig();
            config.Screen.TargetHeight = config.Screen.ScreenHeight;
            var editorCameraFocus = new EditorInfoForCameraExtractor(map.GetLevelPosition(levelData.StartCheckpoint.PlayerPosition));
            var levelCoordinatesMaster = new CameraObjectGlass(editorCameraFocus, config.Screen, config.Camera);

            this.screenCoordinatesMaster = new ScreenCoordinatesMaster(config.Screen, levelCoordinatesMaster);
            var coordinatesConveter = new CoordinatesConverter(screenCoordinatesMaster, map);

            this.mapDisplayer = new TiledMapDisplayer(map, this, screenCoordinatesMaster);
            this.gridDisplayer = new GridDisplayer(map, coordinatesConveter, Content);
            this.editor = new Editor(levelData, Content, coordinatesConveter);
            editor.LevelChanged += LevelChanged;

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            var elapsedSeconds = (Single)gameTime.ElapsedGameTime.TotalSeconds;
            InputProcessor.Instance.Update(elapsedSeconds);
            screenCoordinatesMaster.Update(elapsedSeconds);
            mapDisplayer.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            mapDisplayer.Draw();
            editor.Draw(spriteBatch);
            gridDisplayer.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LevelChanged(Object sender, LevelChangedEventArgs e)
        {
            LevelDataAccess.GetLevelLoader().Save(levelToEdit, e.UpdatedLevel);
        }
    }
}
