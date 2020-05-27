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
    internal class CustomSpawnPointsEditorMode : EditorMode<SpawnSpecificationInEditor>
    {
        private EnemyPositionInEditor enemyPositionInEditor;
        private IEditableDisplayer enemyEditorDisplayer;
        private ActorStartInfo EditedEnemy => enemyPositionInEditor.ActorStartInfo;
        internal TileWrapper TileWrapper => CoordinatesConverter.TileWrapper;
        internal Vector2 EnemyPosition => TileWrapper.GetLevelPosition(enemyPositionInEditor.PositionTileMap);


        public override String ModeName => $"custom spawn points for {EditedEnemy.BlueprintType} at {EditedEnemy.TilePosition}";

        public override List<IEditorMode> ParentModes { get; }

        public override List<IEditorMode> CurrentDerivativeModes => null;

        public CustomSpawnPointsEditorMode(List<IEditorMode> parentModes, EnemyPositionInEditor enemyPositionInEditor, LevelData levelData,
            CoordinatesConverter coordinatesConverter, EditableDisplayingCenter editableDisplayingCenter)
            : base(levelData, coordinatesConverter, editableDisplayingCenter.SpawnPoint, null)
        {
            this.ParentModes = parentModes;
            this.enemyPositionInEditor = enemyPositionInEditor;
            this.enemyEditorDisplayer = editableDisplayingCenter.Blueprint;
            this.Editables = GetEditables();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var enemyScreenPosition = CoordinatesConverter.GetScreenPosition(EditedEnemy.TilePosition);
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
            return new SpawnSpecificationInEditor(this, new SpawnSpecification
            {
                Angle = 0,
                TrajectoryParameters = null,
                SpawnPoint = TileWrapper.GetLevelPosition(positionOnTileMap) - EnemyPosition
            });
        }

        protected override List<SpawnSpecificationInEditor> GetEditables()
        {
            return EditedEnemy.CustomSpawns?
                .Select(spawnSpecification => new SpawnSpecificationInEditor(this, spawnSpecification))
                .ToList() ?? new List<SpawnSpecificationInEditor>();
        }

        protected override String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader)
        {
            return new[] { "Spawn Point" };
        }
    }

    internal class SpawnSpecificationInEditor : IEditable
    {
        private CustomSpawnPointsEditorMode editor;
        internal SpawnSpecification SpawnSpecification { get; private set; }

        internal Single Angle { get => SpawnSpecification.Angle; set => SpawnSpecification.Angle = value; }

        internal SpawnSpecificationInEditor(CustomSpawnPointsEditorMode editor, SpawnSpecification spawnSpecification)
        {
            this.editor = editor;
            this.SpawnSpecification = spawnSpecification;
        }

        public PositionOnTileMap PositionTileMap 
        {
            get => editor.TileWrapper.GetTilePosition(editor.EnemyPosition + SpawnSpecification.SpawnPoint);
            set => SpawnSpecification.SpawnPoint = editor.TileWrapper.GetLevelPosition(value) - editor.EnemyPosition;
        }

        public String GetEditableType() => "Spawn Point";
    }
}
