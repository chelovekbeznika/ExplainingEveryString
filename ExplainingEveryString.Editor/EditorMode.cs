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
        Int32? SelectedEditableIndex { get; }
        void MoveSelected(Vector2 screenPosition);
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
        private String[] editableTypes;
        private Int32 selectedEditableTypeIndex = 0;

        public String CurrentEditableType => editableTypes[selectedEditableTypeIndex];

        public abstract String ModeName { get; }

        public Int32? SelectedEditableIndex { get; private set; }

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
            SelectedEditableIndex = null;
        }

        public void SelectedEditableChange(Int32 editablesSwitched)
        {
            if (Editables.Count == 0)
                return;

            if (SelectedEditableIndex == null)
            {
                if (editablesSwitched > 0)
                    SelectedEditableIndex = -1;
                else
                    SelectedEditableIndex = Editables.Count;
            }

            SelectedEditableIndex += editablesSwitched;
            if (SelectedEditableIndex < 0)
                SelectedEditableIndex += Editables.Count;
            if (SelectedEditableIndex >= Editables.Count)
                SelectedEditableIndex -= Editables.Count;
        }

        public void Load(LevelData levelData)
        {
            Editables = GetEditables(levelData);
        }

        public void DeleteCurrentlySelected()
        {
            if (SelectedEditableIndex == null)
                return;

            Editables.RemoveAt(SelectedEditableIndex.Value);
            if (SelectedEditableIndex >= Editables.Count)
                SelectedEditableIndex = Editables.Count - 1;
            if (Editables.Count == 0)
                SelectedEditableIndex = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var (editable, index) in Editables.Select((editable, index) => (editable, index)))
                editableDisplayer.Draw(spriteBatch, editable.GetEditableType(), GetScreenPosition(editable.PositionTileMap), index == SelectedEditableIndex);
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

        public void MoveSelected(Vector2 screenPosition)
        {
            if (SelectedEditableIndex == null)
                return;

            var tilePosition = GetLevelPosition(screenPosition);
            MoveSelected(Editables[SelectedEditableIndex.Value], tilePosition);
        }

        protected abstract void MoveSelected(T editable, PositionOnTileMap positionOnTileMap);
    }
}
