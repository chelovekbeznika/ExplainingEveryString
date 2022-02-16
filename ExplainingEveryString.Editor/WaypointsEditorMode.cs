using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplainingEveryString.Editor
{
    internal class WaypointsEditorMode : EditorMode<WaypointInEditor>
    {
        public WaypointsEditorMode(LevelData levelData, EditableDisplayingCenter edc) 
            : base(levelData, edc.CoordinatesConverter, edc.WaypointsDisplayer, edc.BlueprintsLoader)
        {
            Editables = GetEditables();
        }

        public override string ModeName => "Waypoints";

        public override List<IEditorMode> ParentModes => null;

        public override List<IEditorMode> CurrentDerivativeModes => null;

        public override LevelData SaveChanges()
        {
            LevelData.Waypoints = Editables.GroupBy(w => w.RoomName).ToDictionary(
                keySelector: g => g.Key,
                elementSelector: g => new Room { Waypoints = g.Select(waypointInEditor => waypointInEditor.PositionTileMap).ToList() });
            return LevelData;
        }

        protected override WaypointInEditor Create(string editableType, PositionOnTileMap positionOnTileMap) =>
        new WaypointInEditor
        {
            PositionTileMap = positionOnTileMap,
            RoomName = editableType
        };

        protected override List<WaypointInEditor> GetEditables()
        {
            if (LevelData.Waypoints == null)
                LevelData.Waypoints = new Dictionary<String, Room>();
            return LevelData.Waypoints.SelectMany(pair => pair.Value.Waypoints.Select(w => new WaypointInEditor
            {
                PositionTileMap = w,
                RoomName = pair.Key
            })).ToList();
        }

        protected override string[] GetEditableTypes(IBlueprintsLoader blueprintsLoader) => new string[]
        {
            "Alpha",
            "Beta",
            "Gamma",
            "Delta",
            "Epsilon",
            "Dzeta",
            "Eta",
            "Teta",
            "Yota",
            "Kappa",
            "Lambda",
            "Mu",
            "Nu",
            "Xi",
            "Omicron",
            "Pi",
            "Ro",
            "Sigma",
            "Tau",
            "Ypsilon",
            "Fi",
            "Hi",
            "Psi",
            "Omega"
        };
    }

    internal class WaypointInEditor : IEditable
    {
        public String RoomName { get; set; }

        public PositionOnTileMap PositionTileMap { get ; set; }

        public string GetEditableType() => RoomName;
    }
}
