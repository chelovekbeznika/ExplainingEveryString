using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameState
{
    internal class LevelSequence
    {
        private Int32 levelsCompleted = 0;
        private Int32 currentLevel = 0;
        internal LevelSequnceSpecification Specification { get; private set; }
        private Dictionary<String, Int32> fileNameToNumberMapping = new Dictionary<String, Int32>();

        internal LevelSequence(LevelSequnceSpecification specification, String startLevelName, String maxLevelName)
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
        internal String GetCurrentLevelFile() => Specification.Levels[currentLevel].LevelData;
        internal String GetMaxAchievedLevelFile() => !GameCompleted 
            ? Specification.Levels[levelsCompleted].LevelData
            : Specification.Levels[Specification.Levels.Length - 1].LevelData;

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
