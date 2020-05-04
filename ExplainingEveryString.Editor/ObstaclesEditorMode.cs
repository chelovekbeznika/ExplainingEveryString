using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Editor
{
    internal class ObstaclesEditorMode : EditorMode<ObstacleInEditor>
    {
        public ObstaclesEditorMode(IScreenCoordinatesMaster screenCoordinatesMaster, TileWrapper tileWrapper, 
            BlueprintDisplayer editableDisplayer, IBlueprintsLoader blueprintsLoader) 
            : base(screenCoordinatesMaster, tileWrapper, editableDisplayer, blueprintsLoader)
        {
        }

        public override String ModeName => "Obstacles";

        protected override List<ObstacleInEditor> GetEditables(LevelData levelData)
        {
            return levelData.ObstaclesTilePositions
                .SelectMany(pair => pair.Value.Select(position => new ObstacleInEditor { ObstacleType = pair.Key, PositionTileMap = position }))
                .ToList();
        }

        protected override String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader)
        {
            return blueprintsLoader.GetBlueprints()
                .Where(pair => pair.Value is ObstacleBlueprint)
                .Select(pair => pair.Key).ToArray();
        }
    }

    internal class ObstacleInEditor : IEditable
    {
        public PositionOnTileMap PositionTileMap { get; set; }
        public String GetEditableType() => ObstacleType;
        internal String ObstacleType { get; set; }
    }
}
