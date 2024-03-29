﻿using ExplainingEveryString.Core.GameModel;
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

        private enum GameMode { GameNotStarted, Story, OneLevelRun, WholeGameRun }
        private enum GameState 
        { 
            BetweenLevels, CutsceneBefore, LevelTitle, InGame, Paused, 
            TimeRecordsTable, LevelEnding, CutsceneAfter, MenuCutscene 
        }

        private readonly ComponentsManager componentsManager;
        private readonly LevelSequenceSpecification levelSequenceSpecification;
        private LevelSequence levelSequence;
        private GameProgress gameProgress;
        private Boolean needToRestartGameMusic = true;

        private GameState currentState = GameState.BetweenLevels;
        private GameMode currentMode = GameMode.GameNotStarted;

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
            GameTimeState = new GameTimeStateManager(componentsManager, () => gameProgress, levelSequenceSpecification);
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
                { GameState.MenuCutscene, componentsManager.SwitchMenuCutsceneRelatedComponents }
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
                        needToRestartGameMusic = false;
                        if (currentMode == GameMode.Story)
                            StartCurrentStoryLevel(false);
                        else
                            StartCurrentLevelTimeAttack();
                    }
                    if (componentsManager.CurrentGameplay.Won)
                    {
                        needToRestartGameMusic = true;
                        if (currentMode == GameMode.Story)
                        {
                            if (componentsManager.CutsceneAfterLevel != null)
                                SwitchToNewState(GameState.CutsceneAfter);
                            else
                            {
                                if (levelSequence.ShowEndingTitle)
                                    SwitchToNewState(GameState.LevelEnding);
                                else
                                    SwitchToNextStoryLevel();
                            }
                        }
                        else if (currentMode == GameMode.OneLevelRun)
                        {
                            GameTimeState.UpdateLevelRecord();
                            SaveGame();
                            SwitchToNewState(GameState.TimeRecordsTable);
                        }
                        else if (currentMode == GameMode.WholeGameRun)
                        {
                            GameTimeState.UpdateLevelRecord();
                            GameTimeState.ToNextLevel();
                            if (GameTimeState.RunFinished)
                                GameTimeState.UpdateGameRecord();
                            SaveGame();
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
                        SwitchToNextStoryLevel();
                    break;
                case GameState.CutsceneAfter:
                    if (componentsManager.CutsceneAfterLevel.Closed)
                    {
                        if (levelSequence.ShowEndingTitle)
                            SwitchToNewState(GameState.LevelEnding);
                        else
                            SwitchToNextStoryLevel();
                    }
                    break;
                case GameState.TimeRecordsTable:
                    if (componentsManager.TimeAttackResultsComponent.Closed)
                        if (currentMode == GameMode.OneLevelRun)
                            SwitchToNewState(GameState.BetweenLevels);
                        else if (currentMode == GameMode.WholeGameRun)
                        {
                            if (GameTimeState.RunFinished)
                                SwitchToNewState(GameState.BetweenLevels);
                            else
                                StartCurrentLevelTimeAttack();
                        }
                    break;
                case GameState.MenuCutscene:
                    if (componentsManager.MenuCutscene.Closed)
                        if (currentMode == GameMode.GameNotStarted)
                            SwitchToNewState(GameState.BetweenLevels);
                        else
                            SwitchToNewState(GameState.Paused);
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
                gameProgress = new GameProgress() 
                { 
                    LevelRecords = new Dictionary<String, Single>(),
                    TimeAttackModeOpened = false
                };
                ProgressToLevelStart();
            }
            SaveProfileNumber = newProfile;
        }

        internal void StartNewGame()
        {
            currentMode = GameMode.Story;
            needToRestartGameMusic = true;
            levelSequence.Reset();
            ProgressToLevelStart();
            SaveGame();
            StartCurrentStoryLevel();
        }

        internal void ContinueCurrentGame()
        {
            currentMode = GameMode.Story;
            needToRestartGameMusic = true;
            StartCurrentStoryLevel();
        }

        internal void ContinueFrom(String levelName)
        {
            currentMode = GameMode.Story;
            needToRestartGameMusic = true;
            gameProgress.CurrentLevelFileName = levelName;
            gameProgress.MaxAchievedLevelName = levelSequence.GetMaxAchievedLevelFile();
            gameProgress.LevelProgress = new LevelProgress
            {
                CurrentCheckPoint = CheckpointSpecification.StartCheckpointName
            };
            SaveGame();
            StartCurrentStoryLevel();
        }

        internal void ShowTutorial()
        {
            componentsManager.DeleteMenuCutscene();
            componentsManager.InitTutorialInMenu(levelSequenceSpecification.TutorialCutsceneName);
            SwitchToNewState(GameState.MenuCutscene);
        }

        internal void ShowTimeTableFromMenu()
        {
            componentsManager.DeleteMenuCutscene();
            componentsManager.InitTimeTableInMenu(levelSequenceSpecification);
            SwitchToNewState(GameState.MenuCutscene);
        }

        internal void StartWholeGameRun()
        {
            currentMode = GameMode.WholeGameRun;
            needToRestartGameMusic = true;
            GameTimeState.StartWholeGameRun();
            StartCurrentLevelTimeAttack();
        }

        internal void StartOneLevelRun(String levelName)
        {
            currentMode = GameMode.OneLevelRun;
            needToRestartGameMusic = true;
            GameTimeState.StartOneLevelRun(levelName);
            StartCurrentLevelTimeAttack();
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
                SaveGame();
            }
            else
            {
                GameTimeState.LevelProgress = eventArgs.LevelProgress;
            }
        }

        private void StartCurrentStoryLevel(Boolean showTitle = true)
        {
            levelSequence.MarkLevelAsCurrentContinuePoint(gameProgress.CurrentLevelFileName);
            GameTimeState.StartStoryGame();
            StartLevel(gameProgress.CurrentLevelFileName, gameProgress.LevelProgress, !showTitle);
        }

        private void StartCurrentLevelTimeAttack()
        {
            StartLevel(GameTimeState.LevelName, GameTimeState.LevelProgress, true);
        }

        private void StartLevel(String levelName, LevelProgress levelProgress, Boolean startWithGameplay)
        {
            componentsManager.DeleteCurrentLevelRelatedComponents();
            componentsManager.InitNewLevelRelatedComponents(levelName, levelProgress, levelSequence);

            if (startWithGameplay)
                SwitchToNewState(GameState.InGame);
            else
            {
                if (componentsManager.CutsceneBeforeLevel != null)
                    SwitchToNewState(GameState.CutsceneBefore);
                else
                    SwitchToNewState(GameState.LevelTitle);
            }
        }

        private void SwitchToNextStoryLevel()
        {
            levelSequence.MarkLevelComplete();
            if (!levelSequence.GameCompleted)
            {
                ProgressToLevelStart();
                StartCurrentStoryLevel();
                SaveGame();
            }
            else
            {
                gameProgress.TimeAttackModeOpened = true;
                SaveGame();
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
            componentsManager.GameMusic.PlaySong(songName, needToRestartGameMusic);
        }

        internal void SongInMenuSelected(String songName)
        {
            componentsManager.MenuMusic.PlaySong(songName, true);
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
            {
                componentsManager.DeleteCurrentLevelRelatedComponents();
                currentMode = GameMode.GameNotStarted;
            }
            if (newState == GameState.Paused)
                componentsManager.Menu.ReturnMenuToDefaultStateAtPause();
            if (newState == GameState.TimeRecordsTable)
                componentsManager.TimeAttackResultsComponent?.UpdateGameProgress(gameProgress, GameTimeState.Splits);
            if (newState == GameState.MenuCutscene)
                (componentsManager.MenuCutscene as TimeAttackResultsComponent)?.UpdateGameProgress(gameProgress, GameTimeState.Splits);
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

        private void SaveGame()
        {
            GameProgressAccess.Save(gameProgress, SaveProfileNumber);
        }
    }
}
