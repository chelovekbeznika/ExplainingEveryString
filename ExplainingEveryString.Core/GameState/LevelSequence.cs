using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameState
{
    internal class LevelSequence
    {
        private Int32 levelsCompleted = 0;
        private Int32 currentLevel = 0;
        internal LevelSequenceSpecification Specification { get; private set; }
        private Dictionary<String, Int32> fileNameToNumberMapping = new Dictionary<String, Int32>();

        internal LevelSequence(LevelSequenceSpecification specification, String startLevelName, String maxLevelName)
        {
            this.Specification = specification;
            this.fileNameToNumberMapping = specification.Levels.Select((level, index) => new { level.LevelData, index })
                .ToDictionary(pair => pair.LevelData, pair => pair.index);
            if (startLevelName != null)
                this.currentLevel = fileNameToNumberMapping[startLevelName];
            if (maxLevelName != null)
                this.levelsCompleted = fileNameToNumberMapping[maxLevelName];
        }

        internal Boolean GameCompleted => levelsCompleted >= Specification.Levels.Length;
        internal Boolean ShowEndingTitle => Specification.Levels[currentLevel].ShowEndingTitle;
        internal String GetCurrentLevelFile() => Specification.Levels[currentLevel].LevelData;
        internal String GetCurrentLevelTitle() => Specification.Levels[currentLevel].TitleSprite;
        internal (String, String) GetCurrentLevelCutscenes() => 
            (Specification.Levels[currentLevel].CutsceneBefore, Specification.Levels[currentLevel].CutsceneAfter);
        internal List<String> GetCurrentLevelEndingLayers() => Specification.Levels
            .Take(currentLevel + 1 + 1) //All passed levels and next one
            .Select(level => level.LevelsBlockId).Distinct()
            .Select(blockId => Specification.LevelsBlocks.Single(block => block.Id == blockId))
            .Select(block => block.LevelEndingLayerSprite).ToList();
        internal Vector2? GetNextLevelMapMark() => currentLevel < Specification.Levels.Length - 1
            ? Specification.Levels[currentLevel + 1].MapMark : null as Vector2?;
        internal List<Vector2> GetPath() => Specification.Levels.Take(currentLevel + 1).Select(l => l.MapMark).ToList();
        internal String GetMaxAchievedLevelFile() => !GameCompleted 
            ? Specification.Levels[levelsCompleted].LevelData
            : Specification.Levels[^1].LevelData;

        internal Boolean LevelIsAvailable(String levelName)
        {
            return levelsCompleted >= fileNameToNumberMapping[levelName];
        }

        internal void MarkLevelAsCurrentContinuePoint(String levelName)
        {
            currentLevel = fileNameToNumberMapping[levelName];
        }

        internal void MarkLevelComplete()
        {
            currentLevel += 1;
            if (levelsCompleted < currentLevel)
            {
                levelsCompleted = currentLevel;
                if (GameCompleted)
                    currentLevel -= 1;
            }   
        }

        internal void Reset() => currentLevel = 0;
    }
}
