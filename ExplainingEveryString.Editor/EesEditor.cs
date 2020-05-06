using ExplainingEveryString.Core;
using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
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
        private Editor editor;

        public EesEditor(string levelToEdit)
        {
            PreInit();
            this.levelToEdit = levelToEdit;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            var levelLoader = LevelDataAccess.GetLevelLoader();
            var levelData = levelLoader.Load(levelToEdit);
            var map = new TileWrapper(Content.Load<TiledMap>(levelData.TileMap));
            var config = ConfigurationAccess.GetCurrentConfig().Camera;
            var editorCameraFocus = new EditorInfoForCameraExtractor(map.GetLevelPosition(levelData.PlayerPosition.TilePosition));
            var levelCoordinatesMaster = new CameraObjectGlass(editorCameraFocus, GraphicsDevice.Viewport, config);

            this.screenCoordinatesMaster = new ScreenCoordinatesMaster(GraphicsDevice.Viewport, levelCoordinatesMaster);
            this.mapDisplayer = new TiledMapDisplayer(map, this, screenCoordinatesMaster);
            this.editor = new Editor(levelData, Content, screenCoordinatesMaster, map);
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
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void LevelChanged(Object sender, LevelChangedEventArgs e)
        {
            LevelDataAccess.GetLevelLoader().Save(levelToEdit, e.UpdatedLevel);
        }
    }
}
