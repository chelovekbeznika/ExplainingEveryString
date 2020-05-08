using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.Tiles;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public override LevelData SaveChanges(LevelData levelData)
        {
            var newObstacles = new Dictionary<String, List<PositionOnTileMap>>();
            foreach (var obstacle in Editables)
            {
                if (!newObstacles.ContainsKey(obstacle.ObstacleType))
                    newObstacles.Add(obstacle.ObstacleType, new List<PositionOnTileMap>());
                newObstacles[obstacle.ObstacleType].Add(obstacle.PositionTileMap);
            }
            levelData.ObstaclesTilePositions = newObstacles.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray());
            return levelData;
        }

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
