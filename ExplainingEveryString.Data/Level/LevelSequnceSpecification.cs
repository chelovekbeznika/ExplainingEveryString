using System;

namespace ExplainingEveryString.Data.Level
{
    public class LevelSequenceSpecification
    {
        public String TutorialCutsceneName { get; set; } 
        public LevelsBlockSpecification[] LevelsBlocks { get; set; }
        public LevelSpecification[] Levels { get; set; }
    }
}
