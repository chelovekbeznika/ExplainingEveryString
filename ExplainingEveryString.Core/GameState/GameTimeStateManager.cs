using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;

namespace ExplainingEveryString.Core.GameState
{
    internal class GameTimeStateManager : IUpdateable
    {
        private readonly Dictionary<String, MenuItem> levelTimeAttackButtons = new Dictionary<String, MenuItem>();
        private readonly ComponentsManager componentsManager;
        private Func<GameProgress> gameProfileGetter;
        internal Single? LevelTime { get; private set; } = null;
        internal String LevelName { get; private set; }
        internal LevelProgress LevelProgress { get; set; }
        internal Single? CurrentLevelRecord => (gameProfileGetter()?.LevelRecords?.ContainsKey(LevelName) ?? false)
            ? gameProfileGetter().LevelRecords[LevelName] : null as Single?;

        internal GameTimeStateManager(ComponentsManager componentsManager, Func<GameProgress> gameProfileGetter)
        {
            this.componentsManager = componentsManager;
            this.gameProfileGetter = gameProfileGetter;
        }

        public void Update(Single elapsedSeconds)
        {
            KeepInSyncLevelRecordsInMainMenu();
            if (componentsManager.CurrentGameplay?.TimerIsOn ?? false && LevelTime.HasValue)
                LevelTime += elapsedSeconds;
        }

        internal void StartOneLevelRun(String levelName, Single startTime)
        {
            LevelName = levelName;
            LevelTime = startTime;
            LevelProgress = new LevelProgress()
            {
                CurrentCheckPoint = CheckpointSpecification.StartCheckpointName
            };
        }

        internal void StartStoryGame()
        {
            LevelName = null;
            LevelTime = null;
        }

        internal void UpdateLevelRecord()
        {
            var gameProgress = gameProfileGetter();
            var level = gameProgress.CurrentLevelFileName;
            if (gameProgress.LevelRecords.ContainsKey(level))
            {
                if (gameProgress.LevelRecords[level] > LevelTime)
                {
                    gameProgress.LevelRecords[level] = LevelTime.Value;
                    componentsManager.TimeAttackResultsComponent?.NotifyNewRecord(level);
                }
            }
            else
            {
                gameProgress.LevelRecords.Add(level, LevelTime.Value);
                componentsManager.TimeAttackResultsComponent?.NotifyNewRecord(level);
            }
        }

        internal void RegisterLevelTimeButton(String levelName, MenuItem levelButton)
        {
            levelTimeAttackButtons.Add(levelName, levelButton);
        }

        private void KeepInSyncLevelRecordsInMainMenu()
        {
            var currentLevelResults = gameProfileGetter().LevelRecords;
            foreach (var (level, button) in levelTimeAttackButtons)
            {
                if (currentLevelResults.ContainsKey(level))
                    button.Text = GameTimeHelper.ToTimeString(currentLevelResults[level]);
                else
                    button.Text = null;
            }
        }
    }
}
