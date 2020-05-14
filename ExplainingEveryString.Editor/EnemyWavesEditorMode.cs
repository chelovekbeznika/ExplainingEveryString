using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Editor
{
    internal class EnemyWavesEditorMode : IEditorMode
    {
        private Int32 waves;
        private List<List<IEditorMode>> wavesEditorModes;

        private BlueprintDisplayer blueprintDisplayer;
        private IBlueprintsLoader blueprintsLoader;
        private RectangleCornersDisplayer cornersDisplayer;
        private CoordinatesConverter coordinatesConverter;
        private List<IEditorMode> levelEditorModes;
        private LevelData levelData;

        public Int32? SelectedEditableIndex { get; private set; } = null;

        public String CurrentEditableType => "Enemy wave";

        public String ModeName => "Enemy waves";

        public List<IEditorMode> ParentModes => null;

        public List<IEditorMode> CurrentDerivativeModes => SelectedEditableIndex != null ? wavesEditorModes[SelectedEditableIndex.Value] : null;

        public EnemyWavesEditorMode(LevelData levelData, List<IEditorMode> levelEditorModes, CoordinatesConverter coordinatesConverter, 
            RectangleCornersDisplayer cornersDisplayer, BlueprintDisplayer blueprintDisplayer, IBlueprintsLoader blueprintsLoader)
        {
            this.levelData = levelData;
            this.levelEditorModes = levelEditorModes;
            this.coordinatesConverter = coordinatesConverter;
            this.cornersDisplayer = cornersDisplayer;
            this.blueprintDisplayer = blueprintDisplayer;
            this.blueprintsLoader = blueprintsLoader;
            Load(levelData);
        }

        private void Load(LevelData levelData)
        {
            waves = levelData.EnemyWaves.Count;
            wavesEditorModes = new List<List<IEditorMode>>();
            foreach (var waveNumber in Enumerable.Range(0, waves))
            {
                wavesEditorModes.Add(EditorModesForWave(waveNumber));
            }
        }

        public void Add(Vector2 screenPosition)
        {
            waves += 1;
            SelectedEditableIndex = waves - 1;
            levelData.EnemyWaves.Add(new EnemyWave
            {
                Enemies = Array.Empty<Data.Level.ActorStartInfo>(),
                MaxEnemiesAtOnce = Int32.MaxValue,
                StartRegion = default
            });
            wavesEditorModes.Add(EditorModesForWave(waves - 1));
        }

        public void DeleteCurrentlySelected()
        {
            //Nah... This will introduce too many complications.
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        }

        public void EditableTypeChange(Int32 typesSwitched)
        {
        }

        public void MoveSelected(Vector2 screenPosition)
        {
        }

        public LevelData SaveChanges()
        {
            return levelData;
        }

        public void SelectedEditableChange(Int32 editablesSwitched)
        {
            SelectedEditableIndex = EditingHelper.SelectedEditableChange(editablesSwitched, waves, SelectedEditableIndex);
        }

        public void Unselect()
        {
            SelectedEditableIndex = null;
        }

        private List<IEditorMode> EditorModesForWave(Int32 waveNumber)
        {
            var result = new List<IEditorMode>
            {
                new StartRegionEditorMode(levelData, levelEditorModes, coordinatesConverter, cornersDisplayer, waveNumber),
                new EnemyPositionEditorMode(levelData, levelEditorModes, coordinatesConverter, blueprintDisplayer, blueprintsLoader, waveNumber)
            };
            return result;
        }
    }
}
