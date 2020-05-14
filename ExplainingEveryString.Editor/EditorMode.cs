﻿using ExplainingEveryString.Core.GameModel;
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
        void Draw(SpriteBatch spriteBatch);
        void EditableTypeChange(Int32 typesSwitched);
        void Add(Vector2 screenPosition);
        void Unselect();
        Int32? SelectedEditableIndex { get; }
        void MoveSelected(Vector2 screenPosition);
        void SelectedEditableChange(Int32 editablesSwitched);
        void DeleteCurrentlySelected();
        LevelData SaveChanges();
        String CurrentEditableType { get; }
        String ModeName { get; }

        List<IEditorMode> ParentModes { get; }
        List<IEditorMode> CurrentDerivativeModes { get; }
    }

    internal abstract class EditorMode<T> : IEditorMode where T : IEditable
    {
        private IEditableDisplayer editableDisplayer;
        private CoordinatesConverter coordinatesConverter;       
        private String[] editableTypes;
        private Int32 selectedEditableTypeIndex = 0;

        protected List<T> Editables { get; set; }
        protected LevelData LevelData { get; private set; }

        public String CurrentEditableType => editableTypes[selectedEditableTypeIndex];

        public abstract String ModeName { get; }

        public Int32? SelectedEditableIndex { get; private set; }

        public abstract List<IEditorMode> ParentModes { get; }

        public abstract List<IEditorMode> CurrentDerivativeModes { get; }

        protected EditorMode(LevelData levelData, CoordinatesConverter coordinatesConverter,
            IEditableDisplayer editableDisplayer, IBlueprintsLoader blueprintsLoader)
        {
            this.LevelData = levelData;
            this.coordinatesConverter = coordinatesConverter;
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
            SelectedEditableIndex = EditingHelper.SelectedEditableChange(editablesSwitched, Editables.Count, SelectedEditableIndex);
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
                editableDisplayer.Draw(spriteBatch, editable.GetEditableType(), 
                    coordinatesConverter.GetScreenPosition(editable.PositionTileMap), index == SelectedEditableIndex);
        }

        protected abstract List<T> GetEditables(LevelData levelData);
        public abstract LevelData SaveChanges();
        protected abstract String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader);

        public void Add(Vector2 screenPosition)
        {
            var newEditable = Create(CurrentEditableType, coordinatesConverter.GetLevelPosition(screenPosition));
            Editables.Add(newEditable);
        }

        protected abstract T Create(String editableType, PositionOnTileMap positionOnTileMap);

        public void MoveSelected(Vector2 screenPosition)
        {
            if (SelectedEditableIndex == null)
                return;

            var tilePosition = coordinatesConverter.GetLevelPosition(screenPosition);
            Editables[SelectedEditableIndex.Value].PositionTileMap = tilePosition;
        }
    }
}
