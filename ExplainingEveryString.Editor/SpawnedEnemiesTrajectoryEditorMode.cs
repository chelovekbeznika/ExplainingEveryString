using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class SpawnedEnemiesTrajectoryEditorMode : IEditorMode
    {
        private SpawnPointsEditorMode spawnPointsEditor;
        private CoordinatesConverter coordinatesConverter;
        private IEditableDisplayer editableDisplayer;
        private Int32 selectedSpawnSpecificationIndex;

        public String ModeName => $"trajectory points for {spawnPointsEditor.ModeName}";

        public List<IEditorMode> ParentModes => spawnPointsEditor.ParentModes;

        public List<IEditorMode> CurrentDerivativeModes => null;

        public Int32? SelectedEditableIndex { get; private set; }

        public String CurrentEditableType => selectedSpawnSpecificationIndex.ToString();

        private SpawnSpecificationInEditor SelectedSpawnSpecification => spawnPointsEditor.SpawnSpecsList[selectedSpawnSpecificationIndex];
        private List<Vector2> CurrentTrajectory
        {
            get => SelectedSpawnSpecification.SpawnSpecification.TrajectoryParameters;
            set => SelectedSpawnSpecification.SpawnSpecification.TrajectoryParameters = value;
        }

        internal SpawnedEnemiesTrajectoryEditorMode(SpawnPointsEditorMode editor, EditableDisplayingCenter editableDisplayingCenter)
        {
            this.spawnPointsEditor = editor;
            this.editableDisplayer = editableDisplayingCenter.SpawnedTrajectory;
            this.coordinatesConverter = editableDisplayingCenter.CoordinatesConverter;
        }

        public void Add(Vector2 screenPosition)
        {
            var spawnPoint = coordinatesConverter.TileToLevel(SelectedSpawnSpecification.PositionTileMap);
            var newTrajectoryPoint = coordinatesConverter.ScreenToLevel(screenPosition) - spawnPoint;
            if (CurrentTrajectory == null)
                CurrentTrajectory = new List<Vector2> { newTrajectoryPoint };
            else
                CurrentTrajectory.Add(newTrajectoryPoint);
            SelectedEditableIndex = null;
        }

        public void DeleteCurrentlySelected()
        {
            if (SelectedEditableIndex != null)
                CurrentTrajectory.RemoveAt(SelectedEditableIndex.Value);
            SelectedEditableIndex = null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentTrajectory != null)
                foreach (var (pointIndex, trajectoryPoint) in CurrentTrajectory.Select((point, index) => (index, point)))
                {
                    var selected = pointIndex == SelectedEditableIndex;
                    var levelPosition = coordinatesConverter.TileToLevel(SelectedSpawnSpecification.PositionTileMap) + trajectoryPoint;
                    var screenPosition = coordinatesConverter.LevelToScreen(levelPosition);
                    editableDisplayer.Draw(spriteBatch, selectedSpawnSpecificationIndex.ToString(), screenPosition, selected);
                }
        }

        public void EditableTypeChange(Int32 typesSwitched)
        {
            selectedSpawnSpecificationIndex += typesSwitched;
            while (selectedSpawnSpecificationIndex < 0)
                selectedSpawnSpecificationIndex += spawnPointsEditor.SpawnSpecsList.Count;
            while (selectedSpawnSpecificationIndex >= spawnPointsEditor.SpawnSpecsList.Count)
                selectedSpawnSpecificationIndex -= spawnPointsEditor.SpawnSpecsList.Count;
            SelectedEditableIndex = null;
        }

        public void MoveSelected(Vector2 screenPosition)
        {
            if (SelectedEditableIndex != null)
            {
                var spawnPoint = coordinatesConverter.TileToLevel(SelectedSpawnSpecification.PositionTileMap);
                var newTrajectoryPoint = coordinatesConverter.ScreenToLevel(screenPosition) - spawnPoint;
                CurrentTrajectory[SelectedEditableIndex.Value] = newTrajectoryPoint;
            }
        }

        public LevelData SaveChanges() => spawnPointsEditor.SaveChanges();

        public void SelectedEditableChange(Int32 editablesSwitched)
        {
            if (CurrentTrajectory == null || CurrentTrajectory.Count == 0)
                return;

            if (SelectedEditableIndex == null)
                SelectedEditableIndex = editablesSwitched > 0 ? editablesSwitched - 1 : CurrentTrajectory.Count + editablesSwitched;
            else
                SelectedEditableIndex += editablesSwitched;

            while (SelectedEditableIndex < 0)
                SelectedEditableIndex += CurrentTrajectory.Count;
            while (SelectedEditableIndex >= CurrentTrajectory.Count)
                SelectedEditableIndex -= CurrentTrajectory.Count;
        }

        public void Unselect()
        {
            SelectedEditableIndex = null;
        }
    }
}
