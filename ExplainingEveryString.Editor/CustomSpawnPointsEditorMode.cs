using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class SpawnPointsEditorMode : EditorMode<SpawnSpecificationInEditor>
    {
        internal event EventHandler SpawnSpecsChanged;

        private EnemyPositionInEditor enemyPositionInEditor;
        private IEditableDisplayer enemyEditorDisplayer;
        private ActorStartInfo EditedEnemy => enemyPositionInEditor.ActorStartInfo;
        internal Vector2 EnemyPosition => CoordinatesConverter.TileToLevel(enemyPositionInEditor.PositionTileMap);
        internal List<SpawnSpecificationInEditor> SpawnSpecsList => Editables;

        public override String ModeName => $"custom spawn points for {EditedEnemy.BlueprintType} at {EditedEnemy.TilePosition}";

        public override List<IEditorMode> ParentModes { get; }

        public override List<IEditorMode> CurrentDerivativeModes => null;

        public SpawnPointsEditorMode(List<IEditorMode> parentModes, EnemyPositionInEditor enemyPositionInEditor, LevelData levelData,
            EditableDisplayingCenter editableDisplayingCenter)
            : base(levelData, editableDisplayingCenter.CoordinatesConverter, editableDisplayingCenter.SpawnPoint, null)
        {
            this.ParentModes = parentModes;
            this.enemyPositionInEditor = enemyPositionInEditor;
            this.enemyEditorDisplayer = editableDisplayingCenter.Blueprint;
            this.Editables = GetEditables();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var enemyScreenPosition = CoordinatesConverter.TileToScreen(EditedEnemy.TilePosition);
            enemyEditorDisplayer.Draw(spriteBatch, EditedEnemy.BlueprintType, enemyScreenPosition, false);
            base.Draw(spriteBatch);
        }

        public override LevelData SaveChanges()
        {
            EditedEnemy.CustomSpawns = Editables.Select(editable => editable.SpawnSpecification).ToArray();
            return LevelData;
        }

        protected override SpawnSpecificationInEditor Create(String editableType, PositionOnTileMap positionOnTileMap)
        {
            SpawnSpecsChanged?.Invoke(this, EventArgs.Empty);
            return new SpawnSpecificationInEditor(this, CoordinatesConverter, new SpawnSpecification
            {
                Angle = 0,
                TrajectoryParameters = null,
                SpawnPoint = CoordinatesConverter.TileToLevel(positionOnTileMap) - EnemyPosition
            });
        }

        protected override List<SpawnSpecificationInEditor> GetEditables()
        {
            return EditedEnemy.CustomSpawns?
                .Select(spawnSpecification => new SpawnSpecificationInEditor(this, CoordinatesConverter, spawnSpecification))
                .ToList() ?? new List<SpawnSpecificationInEditor>();
        }

        protected override String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader)
        {
            return new[] { "Spawn Point" };
        }
    }

    internal class SpawnSpecificationInEditor : IEditable
    {
        private SpawnPointsEditorMode editor;
        private CoordinatesConverter coordinatesConverter;
        internal SpawnSpecification SpawnSpecification { get; private set; }

        internal Single Angle { get => SpawnSpecification.Angle; set => SpawnSpecification.Angle = value; }

        internal SpawnSpecificationInEditor(SpawnPointsEditorMode editor, CoordinatesConverter coordinatesConverter, SpawnSpecification spawnSpecification)
        {
            this.editor = editor;
            this.coordinatesConverter = coordinatesConverter;
            this.SpawnSpecification = spawnSpecification;
        }

        public PositionOnTileMap PositionTileMap 
        {
            get => coordinatesConverter.LevelToTile(editor.EnemyPosition + SpawnSpecification.SpawnPoint);
            set => SpawnSpecification.SpawnPoint = coordinatesConverter.TileToLevel(value) - editor.EnemyPosition;
        }

        public String GetEditableType() => "Spawn Point";
    }
}
