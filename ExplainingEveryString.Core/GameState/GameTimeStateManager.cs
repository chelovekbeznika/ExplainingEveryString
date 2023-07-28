using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameState
{
    internal class GameTimeStateManager
    {
        private readonly Dictionary<String, MenuItem> levelTimeAttackButtons = new Dictionary<String, MenuItem>();

        public void Update()
        {
            var profile = ConfigurationAccess.GetCurrentConfig().SaveProfile;
            var currentLevelResults = GameProgressAccess.Load(profile).LevelRecords;
            foreach (var (level, button) in levelTimeAttackButtons)
            {
                if (currentLevelResults.ContainsKey(level))
                    button.Text = GameTimeHelper.ToTimeString(currentLevelResults[level]);
                else
                    button.Text = null;
            }
        }

        internal void RegisterLevelTimeButton(String levelName, MenuItem levelButton)
        {
            levelTimeAttackButtons.Add(levelName, levelButton);
        }
    }
}
