using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class EnemyPositionEditorMode : EditorMode<EnemyPositionInEditor>
    {
        public EnemyPositionEditorMode(IScreenCoordinatesMaster screenCoordinatesMaster, 
            TileWrapper tileWrapper, BlueprintDisplayer blueprintDisplayer, IBlueprintsLoader blueprintsLoader) 
            : base(screenCoordinatesMaster, tileWrapper, blueprintDisplayer, blueprintsLoader)
        {
        }

        public override String ModeName => "1 enemy wave";

        public override LevelData SaveChanges(LevelData levelData)
        {
            levelData.EnemyWaves[0].Enemies = Editables.Select(enemyPosition => enemyPosition.ActorStartInfo).ToArray();
            return levelData;
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
            return levelData.EnemyWaves[0].Enemies.Select(asi => new EnemyPositionInEditor { ActorStartInfo = asi }).ToList();
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
