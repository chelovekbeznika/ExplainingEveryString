using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal interface IEditorMode
    {
        void Load(LevelData levelData);
        void Draw(SpriteBatch spriteBatch);
        void EditableTypeChange(Int32 typesSwitched);
        void Add(Vector2 screenPosition);
        void Unselect();
        void SelectedEditableChange(Int32 editablesSwitched);
        void DeleteCurrentlySelected();
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
        private Int32? selectedEditableIndex = null;
        private String[] editableTypes;
        private Int32 selectedEditableTypeIndex = 0;

        public String CurrentEditableType => editableTypes[selectedEditableTypeIndex];

        public abstract String ModeName { get; }

        protected EditorMode(IScreenCoordinatesMaster screenCoordinatesMaster, TileWrapper tileWrapper, 
            IEditableDisplayer editableDisplayer, IBlueprintsLoader blueprintsLoader)
        {
            this.screenCoordinatesMaster = screenCoordinatesMaster;
            this.tileWrapper = tileWrapper;
            this.editableDisplayer = editableDisplayer;
            this.editableTypes = GetEditableTypes(blueprintsLoader);
        }

        public void EditableTypeChange(Int32 typesSwitched)
        {
            selectedEditableTypeIndex += typesSwitched;
            if (selectedEditableTypeIndex < 0)
                selectedEditableTypeIndex += editableTypes.Length;
            if (selectedEditableTypeIndex >= editableTypes.Length)
                selectedEditableTypeIndex -= editableTypes.Length;
        }

        public void Unselect()
        {
            selectedEditableIndex = null;
        }

        public void SelectedEditableChange(Int32 editablesSwitched)
        {
            if (Editables.Count == 0)
                return;

            if (selectedEditableIndex == null)
            {
                if (editablesSwitched > 0)
                    selectedEditableIndex = -1;
                else
                    selectedEditableIndex = Editables.Count;
            }

            selectedEditableIndex += editablesSwitched;
            if (selectedEditableIndex < 0)
                selectedEditableIndex += Editables.Count;
            if (selectedEditableIndex >= Editables.Count)
                selectedEditableIndex -= Editables.Count;
        }

        public void Load(LevelData levelData)
        {
            Editables = GetEditables(levelData);
        }

        public void DeleteCurrentlySelected()
        {
            if (selectedEditableIndex == null)
                return;

            Editables.RemoveAt(selectedEditableIndex.Value);
            if (selectedEditableIndex >= Editables.Count)
                selectedEditableIndex = 0;
            if (Editables.Count == 0)
                selectedEditableIndex = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var (editable, index) in Editables.Select((editable, index) => (editable, index)))
                editableDisplayer.Draw(spriteBatch, editable.GetEditableType(), GetScreenPosition(editable.PositionTileMap), index == selectedEditableIndex);
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
