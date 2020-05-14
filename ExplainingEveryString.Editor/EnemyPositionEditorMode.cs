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

        public override String ModeName => $"{wave} wave enemies position";

        public override List<IEditorMode> ParentModes { get; }

        public override List<IEditorMode> CurrentDerivativeModes => null;

        public EnemyPositionEditorMode(LevelData levelData, List<IEditorMode> levelEditorModes, CoordinatesConverter coordinatesConverter,
            BlueprintDisplayer blueprintDisplayer, IBlueprintsLoader blueprintsLoader, Int32 wave)
            : base(levelData, coordinatesConverter, blueprintDisplayer, blueprintsLoader)
        {
            this.ParentModes = levelEditorModes;
            this.wave = wave;
            Editables = GetEditables(levelData);
        }

        public override LevelData SaveChanges()
        {
            LevelData.EnemyWaves[wave].Enemies = Editables.Select(enemyPosition => enemyPosition.ActorStartInfo).ToArray();
            return LevelData;
        }

        protected override EnemyPositionInEditor Create(String editableType, PositionOnTileMap positionOnTileMap)
        {
            return new EnemyPositionInEditor
            {
                ActorStartInfo = new ActorStartInfo
                {
                    BlueprintType = editableType,
                    TilePosition = positionOnTileMap
                }
            };
        }

        protected override List<EnemyPositionInEditor> GetEditables(LevelData levelData)
        {
            return levelData.EnemyWaves[wave].Enemies.Select(asi => new EnemyPositionInEditor { ActorStartInfo = asi }).ToList();
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
