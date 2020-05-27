using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class EnemyPositionEditorMode : EditorMode<EnemyPositionInEditor>, ICustomParameterEditor
    {
        private Int32 wave;
        private List<List<IEditorMode>> enemiesParametersEditorsModes;
        private Func<EnemyPositionInEditor, List<IEditorMode>> createEditorModesForEnemy;

        public override String ModeName => $"{wave} wave enemies position";

        public override List<IEditorMode> ParentModes { get; }

        public override List<IEditorMode> CurrentDerivativeModes => Editables.Count > 0 && SelectedEditableIndex.HasValue
            ? enemiesParametersEditorsModes[SelectedEditableIndex.Value]
            : null;

        public String CurrentParameterValue => CurrentEditable.ActorStartInfo.Angle.ToString();

        public String ParameterName => "Angle";

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
                        coordinatesConverter, cornersDisplayer, blueprintDisplayer),
                    new CustomSpawnPointsEditorMode(enemiesEditorModes, enemy, LevelData,
                        coordinatesConverter, cornersDisplayer, blueprintDisplayer)
                };
            enemiesParametersEditorsModes = Editables.Select(createEditorModesForEnemy).ToList();
        }

        public override LevelData SaveChanges()
        {
            LevelData.EnemyWaves[wave].Enemies = Editables
                .Where(enemyPosition => !enemyPosition.IsBoss)
                .Select(enemyPosition => enemyPosition.ActorStartInfo).ToArray();
            LevelData.EnemyWaves[wave].Bosses = Editables
                .Where(enemyPosition => enemyPosition.IsBoss)
                .Select(enemyPosition => enemyPosition.ActorStartInfo).ToArray();
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
            var currentWave = LevelData.EnemyWaves[wave];
            return currentWave.Enemies.Select(asi => new EnemyPositionInEditor { ActorStartInfo = asi, IsBoss = false })
                .Concat(currentWave.Bosses?.Select(asi => new EnemyPositionInEditor { ActorStartInfo = asi, IsBoss = true }) 
                    ?? new List<EnemyPositionInEditor>())
                .ToList();
        }

        protected override String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader)
        {
            return blueprintsLoader.GetBlueprints()
                .Where(pair => pair.Value is EnemyBlueprint)
                .Select(pair => pair.Key).ToArray();
        }

        public void ToNextValue()
        {
            if (CurrentEditable == null)
                return;
            CurrentEditable.ActorStartInfo.Angle += 5;
            NormalizeAngle();
        }

        public void ToPreviousValue()
        {
            if (CurrentEditable == null)
                return;
            CurrentEditable.ActorStartInfo.Angle -= 5;
            NormalizeAngle();
        }

        private void NormalizeAngle()
        {
            while (CurrentEditable.ActorStartInfo.Angle < 0)
                CurrentEditable.ActorStartInfo.Angle += 360;
            while (CurrentEditable.ActorStartInfo.Angle >= 360)
                CurrentEditable.ActorStartInfo.Angle -= 360;
        }
    }

    internal class EnemyPositionInEditor : IEditable
    {
        internal ActorStartInfo ActorStartInfo { get; set; }

        internal Boolean IsBoss { get; set; }

        public PositionOnTileMap PositionTileMap { get => ActorStartInfo.TilePosition; set => ActorStartInfo.TilePosition = value; }

        public String GetEditableType()
        {
            return ActorStartInfo.BlueprintType;
        }
    }
}
