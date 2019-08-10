using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.GameState
{
    internal class LevelSequence
    {
        private String[] fileNamesSequence;
        private Int32 levelsCompleted = 0;

        internal LevelSequence(String[] fileNamesSequence)
        {
            this.fileNamesSequence = fileNamesSequence;
        }

        internal Boolean GameCompleted => levelsCompleted >= fileNamesSequence.Length;
        internal String GetCurrentLevelFile() => fileNamesSequence[levelsCompleted];
        internal void MarkLevelComplete() => levelsCompleted += 1;
        internal void Reset() => levelsCompleted = 0;
    }
}
