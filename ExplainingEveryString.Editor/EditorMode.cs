using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Editor
{
    internal interface IEditorMode
    {
        void Load(LevelData levelData);
        void Draw(SpriteBatch spriteBatch);
        void EditableTypeChange(Int32 typesSwitched);
        void Add(Vector2 screenPosition);
        LevelData SaveChanges(LevelData levelData);
        String CurrentEditableType { get; }
        String ModeName { get; }
    }

    internal abstract class EditorMode<T> : IEditorMode where T : IEditable
    {
        private IScreenCoordinatesMaster screenCoordinatesMaster;
        private IEditableDisplayer editableDisplayer;
        private TileWrapper tileWrapper;
        protected List<T> Editables { get; private set; }
        private String[] editableTypes;
        private Int32 selectedEditableIndex;

        public String CurrentEditableType => editableTypes[selectedEditableIndex];

        public abstract String ModeName { get; }

        protected EditorMode(IScreenCoordinatesMaster screenCoordinatesMaster, TileWrapper tileWrapper, 
            IEditableDisplayer editableDisplayer, IBlueprintsLoader blueprintsLoader)
        {
            this.screenCoordinatesMaster = screenCoordinatesMaster;
            this.tileWrapper = tileWrapper;
            this.editableDisplayer = editableDisplayer;
            this.editableTypes = GetEditableTypes(blueprintsLoader);
            this.selectedEditableIndex = 0;
        }

        public void EditableTypeChange(Int32 typesSwitched)
        {
            selectedEditableIndex += typesSwitched;
            if (selectedEditableIndex < 0)
                selectedEditableIndex += editableTypes.Length;
            if (selectedEditableIndex >= editableTypes.Length)
                selectedEditableIndex -= editableTypes.Length;
        }

        public void Load(LevelData levelData)
        {
            Editables = GetEditables(levelData);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var editable in Editables)
                editableDisplayer.Draw(spriteBatch, editable.GetEditableType(), GetScreenPosition(editable.PositionTileMap));
        }

        protected PositionOnTileMap GetLevelPosition(Vector2 screenPosition)
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
        public abstract LevelData SaveChanges(LevelData levelData);
        protected abstract String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader);

        public abstract void Add(Vector2 screenPosition);
    }
}
