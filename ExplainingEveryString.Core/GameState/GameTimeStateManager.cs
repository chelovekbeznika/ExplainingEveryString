using ExplainingEveryString.Core.Displaying;
using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameState
{
    internal class GameTimeStateManager : IUpdateable
    {
        private class RunInfo
        {
            internal Dictionary<String, Single> Splits = new Dictionary<String, Single>();
            internal Single SplitsSum = 0;
            internal Int32 LevelsPassed = 0;
        }

        private readonly Dictionary<String, MenuItem> levelTimeAttackButtons = new Dictionary<String, MenuItem>();
        private readonly ComponentsManager componentsManager;
        private readonly LevelSequenceSpecification levelSequenceSpecification;
        private readonly Func<GameProgress> gameProfileGetter;

        private RunInfo currentRun = null;

        internal Single? LevelTime { get; private set; } = null;
        internal Single? RunTime => currentRun != null ? currentRun.SplitsSum + LevelTime : null;
        internal String LevelName { get; private set; }
        internal LevelProgress LevelProgress { get; set; }
        internal Single? LevelRecord => (gameProfileGetter()?.LevelRecords?.ContainsKey(LevelName) ?? false)
            ? gameProfileGetter().LevelRecords[LevelName] : null as Single?;
        internal Single? PersonalBest => gameProfileGetter()?.PersonalBest;
        internal Boolean RunFinished => currentRun?.LevelsPassed >= levelSequenceSpecification.Levels.Length;

        internal GameTimeStateManager(ComponentsManager componentsManager, Func<GameProgress> gameProfileGetter, 
            LevelSequenceSpecification levelSequenceSpecification)
        {
            this.componentsManager = componentsManager;
            this.gameProfileGetter = gameProfileGetter;
            this.levelSequenceSpecification = levelSequenceSpecification;
        }

        public void Update(Single elapsedSeconds)
        {
            KeepInSyncLevelRecordsInMainMenu();
            if (componentsManager.CurrentGameplay?.TimerIsOn ?? false && LevelTime.HasValue)
                LevelTime += elapsedSeconds;
        }

        internal void StartStoryGame()
        {
            currentRun = null;
            LevelName = null;
            LevelTime = null;
        }

        internal void StartOneLevelRun(String levelName)
        {
            currentRun = null;
            LevelName = levelName;
            LevelTime = 0;
            LevelProgress = new LevelProgress()
            {
                CurrentCheckPoint = CheckpointSpecification.StartCheckpointName
            };
        }

        internal void StartWholeGameRun()
        {
            LevelName = levelSequenceSpecification.Levels.First().LevelData;
            LevelTime = 0;
            LevelProgress = new LevelProgress()
            {
                CurrentCheckPoint = CheckpointSpecification.StartCheckpointName
            };
            currentRun = new RunInfo();
        }

        internal void ToNextLevel()
        {
            currentRun.LevelsPassed += 1;
            currentRun.SplitsSum += LevelTime.Value;
            currentRun.Splits.Add(LevelName, LevelTime.Value);
            if (RunFinished)
            {
                LevelTime = null;
                LevelName = null;
                LevelProgress = null;
                return;
            }
            else
            {
                LevelName = levelSequenceSpecification.Levels[currentRun.LevelsPassed].LevelData;
                LevelProgress = new LevelProgress()
                {
                    CurrentCheckPoint = CheckpointSpecification.StartCheckpointName
                };
                LevelTime = 0;
            }
        }

        internal void UpdateLevelRecord()
        {
            var gameProgress = gameProfileGetter();
            if (gameProgress.LevelRecords.ContainsKey(LevelName))
            {
                if (gameProgress.LevelRecords[LevelName] > LevelTime)
                {
                    gameProgress.LevelRecords[LevelName] = LevelTime.Value;
                    componentsManager.TimeAttackResultsComponent?.NotifyNewLevelRecord(LevelName);
                }
            }
            else
            {
                gameProgress.LevelRecords.Add(LevelName, LevelTime.Value);
                componentsManager.TimeAttackResultsComponent?.NotifyNewLevelRecord(LevelName);
            }
        }

        internal void UpdateGameRecord()
        {
            var gameProgress = gameProfileGetter();
            if (gameProgress.PersonalBest == null || RunTime < gameProgress.PersonalBest)
            {
                gameProgress.PersonalBest = RunTime;
                gameProgress.PersonalBestSplits = currentRun.Splits;
                componentsManager.TimeAttackResultsComponent?.NotifyNewGameRecord();
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
