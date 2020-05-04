using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Displaying.FogOfWar;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Editor
{
    internal interface IEditorMode
    {
        void Load(LevelData levelData);
        void Draw(SpriteBatch spriteBatch);
    }

    internal abstract class EditorMode<T> : IEditorMode where T : IEditable
    {
        private IScreenCoordinatesMaster screenCoordinatesMaster;
        private IEditableDisplayer editableDisplayer;
        private TileWrapper tileWrapper;
        private List<T> editables;

        protected EditorMode(IScreenCoordinatesMaster screenCoordinatesMaster, TileWrapper tileWrapper, IEditableDisplayer editableDisplayer)
        {
            this.screenCoordinatesMaster = screenCoordinatesMaster;
            this.tileWrapper = tileWrapper;
            this.editableDisplayer = editableDisplayer;
        }

        public void Load(LevelData levelData)
        {
            editables = GetEditables(levelData);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var editable in editables)
                editableDisplayer.Draw(spriteBatch, editable.GetEditableType(), GetScreenPosition(editable.PositionTileMap));
        }

        private PositionOnTileMap GetLevelPosition(Vector2 screenPosition)
        {
            var levelPosition = screenCoordinatesMaster.ConvertToLevelPosition(screenPosition);
            return tileWrapper.GetTilePosition(levelPosition);
        }

        private Vector2 GetScreenPosition(PositionOnTileMap tileMapPosition)
        {
            var levelPosition = tileWrapper.GetLevelPosition(tileMapPosition);
            return screenCoordinatesMaster.ConvertToScreenPosition(levelPosition);
        }

        protected abstract List<T> GetEditables(LevelData levelData);
        //protected abstract List<T> SaveEditables(LevelData levelData);
        //protected abstract List<String> GetEditableTypes();
    }
}
