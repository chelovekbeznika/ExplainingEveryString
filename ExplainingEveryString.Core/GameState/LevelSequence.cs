using ExplainingEveryString.Data.Level;
using System;
using System.Linq;

namespace ExplainingEveryString.Core.GameState
{
    internal class LevelSequence
    {
        private String[] fileNamesSequence;
        private Int32 levelsCompleted = 0;
        internal LevelSequnceSpecification Specification { get; private set; }

        internal LevelSequence(LevelSequnceSpecification specification, String startLevel)
        {
            this.fileNamesSequence = specification.Levels.Select(level => level.LevelData).ToArray();
            foreach (var (index, level) in fileNamesSequence.Select((level, index) => (index, level)))
                if (level == startLevel)
                    this.levelsCompleted = index;
        }

        internal Boolean GameCompleted => levelsCompleted >= fileNamesSequence.Length;
        internal String GetCurrentLevelFile() => fileNamesSequence[levelsCompleted];
        internal void MarkLevelComplete() => levelsCompleted += 1;
        internal void Reset() => levelsCompleted = 0;
        internal String CurrentLevelName => fileNamesSequence[levelsCompleted];
    }
}
