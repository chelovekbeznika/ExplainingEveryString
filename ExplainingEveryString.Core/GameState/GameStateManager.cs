using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExplainingEveryString.Core.GameState
{
    internal class GameStateManager : IUpdateable
    {
        private delegate void ComponentsSwitch(Boolean active);
        private readonly Dictionary<GameState, ComponentsSwitch> componentSwitches;

        private enum GameMode { Story, OneLevelRun, WholeGameRun }
        private enum GameState { BetweenLevels, CutsceneBefore, LevelTitle, InGame, Paused, TimeRecordsTable, LevelEnding, CutsceneAfter }

        private readonly ComponentsManager componentsManager;
        private readonly LevelSequenceSpecification levelSequenceSpecification;
        private LevelSequence levelSequence;
        private GameProgress gameProgress;

        private GameState currentState = GameState.BetweenLevels;
        private GameMode currentMode = GameMode.Story;
        private Int32 SaveProfileNumber
        {
            get => ConfigurationAccess.GetCurrentConfig().SaveProfile;
            set
            {
                ConfigurationAccess.GetCurrentConfig().SaveProfile = value;
                ConfigurationAccess.SaveCurrentConfig();
            }
        }

        internal Boolean IsPaused => currentState == GameState.Paused;
        internal GameTimeStateManager GameTimeState { get; private set; }

        internal GameStateManager(LevelSequenceSpecification levelSequenceSpecification, 
            ComponentsManager componentsManager, Int32 currentProfile)
        {
            this.componentsManager = componentsManager;
            GameTimeState = new GameTimeStateManager(componentsManager, () => gameProgress);
            this.levelSequenceSpecification = levelSequenceSpecification;
            this.componentSwitches = new Dictionary<GameState, ComponentsSwitch>
            {
                { GameState.BetweenLevels, componentsManager.SwitchMenuRelatedComponents },
                { GameState.Paused, componentsManager.SwitchMenuRelatedComponents },
                { GameState.CutsceneBefore, componentsManager.SwitchCutsceneBeforeLevel },
                { GameState.LevelTitle, componentsManager.SwitchLevelTitleRelatedComponents },
                { GameState.InGame, componentsManager.SwitchGameplayRelatedComponents },
                { GameState.CutsceneAfter, componentsManager.SwitchCutsceneAfterLevel },
                { GameState.LevelEnding, componentsManager.SwitchLevelEndingRelatedComponents },
                { GameState.TimeRecordsTable, componentsManager.SwitchTimeAttackResultsComponents },
            };

            SwitchSaveProfile(currentProfile);
        }

        internal void InitComponents()
        {
            componentsManager.InitComponents();
        }

        public void Update(Single elapsedSeconds)
        {
            GameTimeState.Update(elapsedSeconds);
            switch (currentState)
            {
                case GameState.InGame:
                    if (componentsManager.CurrentGameplay.Lost)
                    {
                        var currentGameTime = GameTimeState.LevelTime;
                        StartCurrentLevel(false, currentGameTime);
                    }
                    if (componentsManager.CurrentGameplay.Won)
                    {
                        if (currentMode == GameMode.Story)
                        {
                            if (componentsManager.CutsceneAfterLevel != null)
                                SwitchToNewState(GameState.CutsceneAfter);
                            else
                            {
                                if (levelSequence.ShowEndingTitle)
                                    SwitchToNewState(GameState.LevelEnding);
                                else
                                    SwitchToNextLevel();
                            }
                        }
                        else
                        {
                            GameTimeState.UpdateLevelRecord();
                            GameProgressAccess.Save(gameProgress, SaveProfileNumber);
                            SwitchToNewState(GameState.TimeRecordsTable);
                        }
                    }
                    break;
                case GameState.CutsceneBefore:
                    if (componentsManager.CutsceneBeforeLevel.Closed)
                        SwitchToNewState(GameState.LevelTitle);
                    break;
                case GameState.LevelTitle:
                    if (componentsManager.CurrentLevelTitle.Closed)
                        SwitchToNewState(GameState.InGame);
                    break;
                case GameState.LevelEnding:
                    if (componentsManager.CurrentLevelEnding.Closed)
                        SwitchToNextLevel();
                    break;
                case GameState.CutsceneAfter:
                    if (componentsManager.CutsceneAfterLevel.Closed)
                    {
                        if (levelSequence.ShowEndingTitle)
                            SwitchToNewState(GameState.LevelEnding);
                        else
                            SwitchToNextLevel();
                    }
                    break;
                case GameState.TimeRecordsTable:
                    if (componentsManager.TimeAttackResultsComponent.Closed)
                        SwitchToNewState(GameState.BetweenLevels);
                    break;
            }
        }

        internal void SwitchSaveProfile(Int32 newProfile)
        {
            if (newProfile == SaveProfileNumber && gameProgress != null)
                return;
            if (currentState != GameState.BetweenLevels)
                SwitchToNewState(GameState.BetweenLevels);

            gameProgress = GameProgressAccess.Load(newProfile);
            if (gameProgress != null)
                levelSequence = new LevelSequence(levelSequenceSpecification,
                    gameProgress.CurrentLevelFileName, gameProgress.MaxAchievedLevelName);
            else
            {
                levelSequence = new LevelSequence(levelSequenceSpecification, null, null);
                ProgressToLevelStart();
            }
            SaveProfileNumber = newProfile;
        }

        internal void StartNewGame()
        {
            currentMode = GameMode.Story;
            levelSequence.Reset();
            ProgressToLevelStart();
            GameProgressAccess.Save(gameProgress, SaveProfileNumber);
            StartCurrentLevel(true);
        }

        internal void ContinueCurrentGame()
        {
            currentMode = GameMode.Story;
            StartCurrentLevel(true);
        }

        internal void ContinueFrom(String levelName)
        {
            currentMode = GameMode.Story;
            gameProgress.CurrentLevelFileName = levelName;
            gameProgress.MaxAchievedLevelName = levelSequence.GetMaxAchievedLevelFile();
            gameProgress.LevelProgress = new LevelProgress
            {
                CurrentCheckPoint = CheckpointSpecification.StartCheckpointName
            };
            GameProgressAccess.Save(gameProgress, SaveProfileNumber);
            StartCurrentLevel(true);
        }

        internal void StartOneLevelRun(String levelName)
        {
            currentMode = GameMode.OneLevelRun;
            gameProgress.CurrentLevelFileName = levelName;
            gameProgress.MaxAchievedLevelName = levelSequence.GetMaxAchievedLevelFile();
            gameProgress.LevelProgress = new LevelProgress
            {
                CurrentCheckPoint = CheckpointSpecification.StartCheckpointName
            };
            StartCurrentLevel(false, 0);
        }

        internal Boolean LevelAvailable(String levelFileName)
        {
            return levelSequence.LevelIsAvailable(levelFileName);
        }

        internal void NotableProgressMaid(Object sender, CheckpointReachedEventArgs eventArgs)
        {
            if (currentMode == GameMode.Story)
            {
                gameProgress.LevelProgress = eventArgs.LevelProgress;
                GameProgressAccess.Save(gameProgress, SaveProfileNumber);
            }
        }

        private void StartCurrentLevel(Boolean showTitle, Single? gameTime = null)
        {
            levelSequence.MarkLevelAsCurrentContinuePoint(gameProgress.CurrentLevelFileName);
            componentsManager.DeleteCurrentLevelRelatedComponents();
            componentsManager.InitNewLevelRelatedComponents(gameProgress, levelSequence);

            if (gameTime is null)
                GameTimeState.StartStoryGame();
            else
                GameTimeState.StartOneLevelRun(gameProgress.CurrentLevelFileName, gameTime.Value);

            if (showTitle)
            {
                if (currentMode == GameMode.Story && componentsManager.CutsceneBeforeLevel != null)
                    SwitchToNewState(GameState.CutsceneBefore);
                else
                    SwitchToNewState(GameState.LevelTitle);
            }
            else
                SwitchToNewState(GameState.InGame);
        }

        private void SwitchToNextLevel()
        {
            levelSequence.MarkLevelComplete();
            if (!levelSequence.GameCompleted)
            {
                ProgressToLevelStart();
                StartCurrentLevel(true);
                GameProgressAccess.Save(gameProgress, SaveProfileNumber);
            }
            else
            {
                SwitchToNewState(GameState.BetweenLevels);
                componentsManager.Menu.ReturnMenuToDefaultStateAtPause();
            }
        }

        internal void TryPauseSwitch()
        {
            switch (currentState)
            {
                case GameState.InGame:
                    SwitchToNewState(GameState.Paused);
                    break;
                case GameState.Paused:
                    SwitchToNewState(GameState.InGame);
                    break;
            }
        }

        internal void TryPause()
        {
            if (currentState == GameState.InGame)
                SwitchToNewState(GameState.Paused);
        }

        internal void ConfigChanged(Configuration newConfig)
        {
            componentsManager.MenuMusic.Volume = newConfig.MusicVolume;
            componentsManager.GameMusic.Volume = newConfig.MusicVolume;
            componentsManager.CutsceneMusic.Volume = newConfig.MusicVolume;
        }

        internal void StartMusicInGame(String songName)
        {
            componentsManager.GameMusic.PlaySong(songName);
        }

        internal void SongInMenuSelected(String songName)
        {
            componentsManager.MenuMusic.PlaySong(songName);
        }

        internal void StopMusicInMenu()
        {
            componentsManager.MenuMusic.Stop();
        }

        internal void SendGlobalNotification(String type)
        {
            componentsManager.Notifications.SendNotification(type);
        }

        private void SwitchToNewState(GameState newState)
        {
            var otherGameStates = Enum.GetValues(typeof(GameState)).OfType<GameState>().Where(state => state != newState);
            foreach (var state in otherGameStates)
                componentSwitches[state](false);
            if (newState == GameState.BetweenLevels)
                componentsManager.DeleteCurrentLevelRelatedComponents();
            if (newState == GameState.Paused)
                componentsManager.Menu.ReturnMenuToDefaultStateAtPause();
            componentSwitches[newState](true);
            currentState = newState;
        }

        private void ProgressToLevelStart()
        {
            gameProgress.CurrentLevelFileName = levelSequence.GetCurrentLevelFile();
            gameProgress.MaxAchievedLevelName = levelSequence.GetMaxAchievedLevelFile();
            gameProgress.LevelProgress = new LevelProgress
            {
                CurrentCheckPoint = CheckpointSpecification.StartCheckpointName
            };
        }
    }
}
