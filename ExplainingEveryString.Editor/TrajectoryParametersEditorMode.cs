using ExplainingEveryString.Core.GameModel.Enemies;
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
    internal class TrajectoryParametersEditorMode : EditorMode<TrajectoryPointInEditor>
    {
        private EnemyPositionInEditor enemyPositionInEditor;
        private Vector2 BasePoint => TileWrapper.GetLevelPosition(EditedEnemy.TilePosition);
        private ActorStartInfo EditedEnemy => enemyPositionInEditor.ActorStartInfo;
        private IEditableDisplayer enemyEditorDisplayer;

        private TileWrapper TileWrapper => CoordinatesConverter.TileWrapper;

        public TrajectoryParametersEditorMode(List<IEditorMode> parentModes, EnemyPositionInEditor enemyPositionInEditor, LevelData levelData, 
            CoordinatesConverter coordinatesConverter, RectangleCornersDisplayer editableDisplayer, BlueprintDisplayer enemyEditorDisplayer) 
            : base(levelData, coordinatesConverter, editableDisplayer, null)
        {
            this.ParentModes = parentModes;
            this.enemyPositionInEditor = enemyPositionInEditor;
            this.enemyEditorDisplayer = enemyEditorDisplayer;
            this.Editables = GetEditables();
        }

        public override String ModeName => $"trajectory parameters of {EditedEnemy.BlueprintType} at {EditedEnemy.TilePosition}";

        public override List<IEditorMode> ParentModes { get; }

        public override List<IEditorMode> CurrentDerivativeModes => null;

        public override LevelData SaveChanges()
        {
            var newTrajectoryParameters = Editables
                .Select(editable => TileWrapper.GetLevelPosition(editable.PositionTileMap))
                .Select(levelPosition => levelPosition - BasePoint)
                .ToArray();
            EditedEnemy.TrajectoryParameters = newTrajectoryParameters;
            return LevelData;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var enemyScreenPosition = CoordinatesConverter.GetScreenPosition(EditedEnemy.TilePosition);
            enemyEditorDisplayer.Draw(spriteBatch, EditedEnemy.BlueprintType, enemyScreenPosition, false);
            base.Draw(spriteBatch);
        }

        protected override TrajectoryPointInEditor Create(String editableType, PositionOnTileMap positionOnTileMap)
        {
            return new TrajectoryPointInEditor { PositionTileMap = positionOnTileMap };
        }

        protected override List<TrajectoryPointInEditor> GetEditables()
        {
            var trajectoryParameters = EditedEnemy.TrajectoryParameters ?? Array.Empty<Vector2>();
            return trajectoryParameters
                .Select(point => new TrajectoryPointInEditor { PositionTileMap = TileWrapper.GetTilePosition(point + BasePoint) })
                .ToList();
        }

        protected override String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader) => new[] { "Trajectory point" };
    }

    internal class TrajectoryPointInEditor : IEditable
    {
        public PositionOnTileMap PositionTileMap { get; set; }

        public String GetEditableType() => "Trajectory point";
    }
}
