using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Editor
{
    internal class StartRegionEditorMode : IEditorMode
    {
        private LevelData levelData;
        private IEditableDisplayer displayer;
        private CoordinatesConverter coordinatesConverter;

        private PositionOnTileMap upperLeftCorner;
        private PositionOnTileMap rightBottomCorner;
        private Boolean upperLeftCornerSelected = true;
        private Int32 enemyWaveNumber;

        public Int32? SelectedEditableIndex => upperLeftCornerSelected ? 0 : 1;

        public String CurrentEditableType => upperLeftCornerSelected ? "Upper left corner" : "Bottom right corner";

        public String ModeName => $"Start region of {enemyWaveNumber} enemy wave";

        public List<IEditorMode> ParentModes { get; private set; }

        public List<IEditorMode> CurrentDerivativeModes => null;

        internal StartRegionEditorMode(LevelData levelData, List<IEditorMode> levelEditorModes, 
            CoordinatesConverter coordinatesConverter, IEditableDisplayer displayer, Int32 wave)
        {
            this.levelData = levelData;
            this.ParentModes = levelEditorModes;
            this.coordinatesConverter = coordinatesConverter;
            this.displayer = displayer;
            this.enemyWaveNumber = wave;
            Load(levelData);
        }

        private void Load(LevelData levelData)
        {
            var startRegion = levelData.EnemyWaves[enemyWaveNumber].StartRegion;
            upperLeftCorner = new PositionOnTileMap { X = startRegion.X, Y = startRegion.Y };
            rightBottomCorner = new PositionOnTileMap { X = startRegion.X + startRegion.Width, Y = startRegion.Y + startRegion.Height };
        }

        public void Add(Vector2 screenPosition)
        {
        }

        public void DeleteCurrentlySelected()
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            displayer.Draw(spriteBatch, "Upper left corner", coordinatesConverter.GetScreenPosition(upperLeftCorner), upperLeftCornerSelected);
            displayer.Draw(spriteBatch, "Bottom right corner", coordinatesConverter.GetScreenPosition(rightBottomCorner), !upperLeftCornerSelected);
        }

        public void EditableTypeChange(Int32 typesSwitched)
        {
            SelectedCornerChange(typesSwitched);
        }

        public void MoveSelected(Vector2 screenPosition)
        {
            var levelPosition = coordinatesConverter.GetLevelPosition(screenPosition);
            if (upperLeftCornerSelected)
                upperLeftCorner = new PositionOnTileMap { X = levelPosition.X, Y = levelPosition.Y };
            else
                rightBottomCorner = new PositionOnTileMap { X = levelPosition.X, Y = levelPosition.Y };

            AdjustCornersToTheirNames();
        }

        private void AdjustCornersToTheirNames()
        {
            var newUpperLeftCorner = new PositionOnTileMap
            {
                X = upperLeftCorner.X < rightBottomCorner.X ? upperLeftCorner.X : rightBottomCorner.X,
                Y = upperLeftCorner.Y < rightBottomCorner.Y ? upperLeftCorner.Y : rightBottomCorner.Y
            };
            var newRightBottomCorner = new PositionOnTileMap
            {
                X = upperLeftCorner.X > rightBottomCorner.X ? upperLeftCorner.X : rightBottomCorner.X,
                Y = upperLeftCorner.Y > rightBottomCorner.Y ? upperLeftCorner.Y : rightBottomCorner.Y
            };
            upperLeftCorner = newUpperLeftCorner;
            rightBottomCorner = newRightBottomCorner;
        }

        public LevelData SaveChanges()
        {
            var newRegion = new Rectangle(
                x: upperLeftCorner.X,
                y: upperLeftCorner.Y,
                width: rightBottomCorner.X - rightBottomCorner.Y,
                height: rightBottomCorner.Y - rightBottomCorner.Y);
            levelData.EnemyWaves[enemyWaveNumber].StartRegion = newRegion;
            return levelData;
        }

        public void SelectedEditableChange(Int32 editablesSwitched)
        {
            SelectedCornerChange(editablesSwitched);
        }

        public void Unselect()
        {
        }

        private void SelectedCornerChange(Int32 changeMeasure)
        {
            if (changeMeasure % 2 == 0)
                return;
            else
                upperLeftCornerSelected = !upperLeftCornerSelected;
        }
    }
}
