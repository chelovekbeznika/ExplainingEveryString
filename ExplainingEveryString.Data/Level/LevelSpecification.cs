using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;

namespace ExplainingEveryString.Data.Level
{
    public class LevelSpecification
    {
        public String LevelData { get; set; }
        [DefaultValue(null)]
        public String CutsceneBefore { get; set; }
        [DefaultValue(null)]
        public String CutsceneAfter { get; set; }
        public String ButtonSprite { get; set; }
        public String TitleSprite { get; set; }
        public Vector2 MapMark { get; set; }
        public String LevelsBlockId { get; set; }
        [DefaultValue(true)]
        public Boolean ShowEndingTitle { get; set; }
    }
}
