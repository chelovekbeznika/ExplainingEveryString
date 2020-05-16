using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class EnemyPositionEditorMode : EditorMode<EnemyPositionInEditor>
    {
        private Int32 wave;
        private List<List<IEditorMode>> enemiesParametersEditorsModes;
        private Func<EnemyPositionInEditor, List<IEditorMode>> createEditorModesForEnemy;

        public override String ModeName => $"{wave} wave enemies position";

        public override List<IEditorMode> ParentModes { get; }

        public override List<IEditorMode> CurrentDerivativeModes => Editables.Count > 0 && SelectedEditableIndex.HasValue
            ? enemiesParametersEditorsModes[SelectedEditableIndex.Value]
            : null;

        public EnemyPositionEditorMode(LevelData levelData, List<IEditorMode> levelEditorModes, List<IEditorMode> enemiesEditorModes, 
            CoordinatesConverter coordinatesConverter, BlueprintDisplayer blueprintDisplayer, RectangleCornersDisplayer cornersDisplayer, 
            IBlueprintsLoader blueprintsLoader, Int32 wave)
            : base(levelData, coordinatesConverter, blueprintDisplayer, blueprintsLoader)
        {
            this.ParentModes = levelEditorModes;
            this.wave = wave;
            Editables = GetEditables();
            this.createEditorModesForEnemy = enemy => new List<IEditorMode>
                {
                    new TrajectoryParametersEditorMode(enemiesEditorModes, enemy, LevelData,
                        coordinatesConverter, cornersDisplayer, blueprintDisplayer)
                };
            enemiesParametersEditorsModes = Editables.Select(createEditorModesForEnemy).ToList();
        }

        public override LevelData SaveChanges()
        {
            LevelData.EnemyWaves[wave].Enemies = Editables.Select(enemyPosition => enemyPosition.ActorStartInfo).ToArray();
            return LevelData;
        }

        protected override EnemyPositionInEditor Create(String editableType, PositionOnTileMap positionOnTileMap)
        {
            var enemyInEditor = new EnemyPositionInEditor
            {
                ActorStartInfo = new ActorStartInfo
                {
                    BlueprintType = editableType,
                    TilePosition = positionOnTileMap
                }
            };
            enemiesParametersEditorsModes.Add(createEditorModesForEnemy(enemyInEditor));
            return enemyInEditor;
        }

        public override void DeleteCurrentlySelected()
        {
            if (SelectedEditableIndex != null)
                enemiesParametersEditorsModes.RemoveAt(SelectedEditableIndex.Value);
            base.DeleteCurrentlySelected();
        }

        protected override List<EnemyPositionInEditor> GetEditables()
        {
            return LevelData.EnemyWaves[wave].Enemies.Select(asi => new EnemyPositionInEditor { ActorStartInfo = asi }).ToList();
        }

        protected override String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader)
        {
            return blueprintsLoader.GetBlueprints()
                .Where(pair => pair.Value is EnemyBlueprint)
                .Select(pair => pair.Key).ToArray();
        }
    }

    internal class EnemyPositionInEditor : IEditable
    {
        public ActorStartInfo ActorStartInfo { get; set; }

        public PositionOnTileMap PositionTileMap { get => ActorStartInfo.TilePosition; set => ActorStartInfo.TilePosition = value; }

        public String GetEditableType()
        {
            return ActorStartInfo.BlueprintType;
        }
    }
}
