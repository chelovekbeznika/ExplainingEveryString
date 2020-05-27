using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Editor
{
    internal class DoorsEditorMode : EditorMode<DoorInEditor>, ICustomParameterEditor
    {
        private Int32 waveNumber;

        public DoorsEditorMode(Int32 waveNumber, LevelData levelData, List<IEditorMode> levelEditorModes, 
            CoordinatesConverter coordinatesConverter, EditableDisplayingCenter editableDisplayingCenter) 
            : base(levelData, coordinatesConverter, editableDisplayingCenter.Blueprint, editableDisplayingCenter.BlueprintsLoader)
        {
            this.waveNumber = waveNumber;
            this.ParentModes = levelEditorModes;
            Editables = GetEditables();
        }

        public override String ModeName => $"{waveNumber} wave doors";

        public override List<IEditorMode> ParentModes { get; }

        public override List<IEditorMode> CurrentDerivativeModes => null;

        public String CurrentParameterValue => CurrentEditable.DoorStartInfo.ClosesAt.HasValue 
            ? CurrentEditable.DoorStartInfo.ClosesAt.ToString()
            : "null, cause door closed at start";

        public String ParameterName => "Wave to close door ";

        public override LevelData SaveChanges()
        {
            LevelData.EnemyWaves[waveNumber].Doors = Editables.Select(editable => editable.DoorStartInfo).ToArray();
            return LevelData;
        }

        public void ToNextValue()
        {
            var doorInfo = CurrentEditable.DoorStartInfo;
            if (doorInfo.ClosesAt == null)
                doorInfo.ClosesAt = 0;
            else
                doorInfo.ClosesAt += 1;
        }

        public void ToPreviousValue()
        {
            var doorInfo = CurrentEditable.DoorStartInfo;
            if (doorInfo.ClosesAt == 0)
                doorInfo.ClosesAt = null;
            else if (doorInfo.ClosesAt > 0)
                doorInfo.ClosesAt -= 1;
        }

        protected override DoorInEditor Create(String editableType, PositionOnTileMap positionOnTileMap)
        {
            return new DoorInEditor(new DoorStartInfo { BlueprintType = editableType, TilePosition = positionOnTileMap });
        }

        protected override List<DoorInEditor> GetEditables()
        {
            return LevelData.EnemyWaves[waveNumber].Doors?.Select(dsi => new DoorInEditor(dsi)).ToList() ?? new List<DoorInEditor>();
        }

        protected override String[] GetEditableTypes(IBlueprintsLoader blueprintsLoader)
        {
            return blueprintsLoader.GetBlueprints()
                .Where(pair => pair.Value is DoorBlueprint)
                .Select(pair => pair.Key).ToArray();
        }
    }

    internal class DoorInEditor : IEditable
    {
        internal DoorStartInfo DoorStartInfo { get; private set; }

        internal DoorInEditor(DoorStartInfo doorStartInfo)
        {
            this.DoorStartInfo = doorStartInfo;
        }

        public PositionOnTileMap PositionTileMap { get => DoorStartInfo.TilePosition; set => DoorStartInfo.TilePosition = value; }

        public String GetEditableType() => DoorStartInfo.BlueprintType;
    }
}
