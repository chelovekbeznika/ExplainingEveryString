using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class ObstaclesEditorMode : EditorMode<ObstacleInEditor>
    {
        public ObstaclesEditorMode(LevelData levelData, CoordinatesConverter coordinatesConverter, 
            BlueprintDisplayer editableDisplayer, IBlueprintsLoader blueprintsLoader) 
            : base(levelData, coordinatesConverter, editableDisplayer, blueprintsLoader)
        {
            Editables = GetEditables();
        }

        public override String ModeName => "Obstacles";

        public override List<IEditorMode> ParentModes => null;

        public override List<IEditorMode> CurrentDerivativeModes => null;

        public override LevelData SaveChanges()
        {
            var newObstacles = new Dictionary<String, List<PositionOnTileMap>>();
            foreach (var obstacle in Editables)
            {
                if (!newObstacles.ContainsKey(obstacle.ObstacleType))
                    newObstacles.Add(obstacle.ObstacleType, new List<PositionOnTileMap>());
                newObstacles[obstacle.ObstacleType].Add(obstacle.PositionTileMap);
            }
            LevelData.ObstaclesTilePositions = newObstacles.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
            return LevelData;
        }

        protected override List<ObstacleInEditor> GetEditables()
        {
            return LevelData.ObstaclesTilePositions
                .SelectMany(pair => pair.Value.Select(position => new ObstacleInEditor { ObstacleType = pair.Key, PositionTileMap = position }))
                .ToList();
        }

        protected override String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader)
        {
            return blueprintsLoader.GetBlueprints()
                .Where(pair => pair.Value is ObstacleBlueprint)
                .Select(pair => pair.Key).ToArray();
        }

        protected override ObstacleInEditor Create(String editableType, PositionOnTileMap positionOnTileMap)
        {
            return new ObstacleInEditor
            {
                ObstacleType = editableType,
                PositionTileMap = positionOnTileMap
            };
        }
    }

    internal class ObstacleInEditor : IEditable
    {
        public PositionOnTileMap PositionTileMap { get; set; }
        public String GetEditableType() => ObstacleType;
        internal String ObstacleType { get; set; }
    }
}
