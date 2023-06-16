using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using System;

namespace ExplainingEveryString.Core.GameState
{
    internal class GameStateManager
    {
        private enum GameState { BetweenLevels, CutsceneBefore, LevelTitle, InGame, Paused, LevelEnding, CutsceneAfter }

        private ComponentsManager componentsManager;
        private LevelSequenceSpecification levelSequenceSpecification;
        private LevelSequence levelSequence;
        private GameProgress gameProgress;

        private GameState currentState = GameState.BetweenLevels;
        private Int32? saveProfileNumber = null;

        internal Boolean IsPaused => currentState == GameState.Paused;

        internal GameStateManager(LevelSequenceSpecification levelSequenceSpecification, 
            ComponentsManager componentsManager, Int32 currentProfile)
        {
            this.componentsManager = componentsManager;
            this.levelSequenceSpecification = levelSequenceSpecification;

            SwitchSaveProfile(currentProfile);
        }

        internal void InitComponents()
        {
            componentsManager.InitComponents();
        }

        internal void Update()
        {
            switch (currentState)
            {
                case GameState.InGame:
                    if (componentsManager.CurrentGameplay.Lost)
                    {
                        StartCurrentLevel(false);
                    }
                    if (componentsManager.CurrentGameplay.Won)
                    {
                        if (componentsManager.CutsceneAfterLevel != null)
                            SwitchToCutsceneAfterState();
                        else
                        {
                            if (levelSequence.ShowEndingTitle)
                                SwitchToLevelEndingState();
                            else
                                SwitchToNextLevel();
                        }
                    }
                    break;
                case GameState.CutsceneBefore:
                    if (componentsManager.CutsceneBeforeLevel.Closed)
                        SwitchToTitleState();
                    break;
                case GameState.LevelTitle:
                    if (componentsManager.CurrentLevelTitle.Closed)
                        SwitchToInGameState();
                    break;
                case GameState.LevelEnding:
                    if (componentsManager.CurrentLevelEnding.Closed)
                        SwitchToNextLevel();
                    break;
                case GameState.CutsceneAfter:
                    if (componentsManager.CutsceneAfterLevel.Closed)
                    {
                        if (levelSequence.ShowEndingTitle)
                            SwitchToLevelEndingState();
                        else
                            SwitchToNextLevel();
                    }
                    break;
            }
        }

        internal void SwitchSaveProfile(Int32 newProfile)
        {
            if (saveProfileNumber != null)
                GameProgressAccess.Save(gameProgress, saveProfileNumber.Value);
            if (currentState != GameState.BetweenLevels)
                SwitchToBetweenLevelsState();

            this.gameProgress = GameProgressAccess.Load(newProfile);
            if (gameProgress != null)
                this.levelSequence = new LevelSequence(levelSequenceSpecification,
                    gameProgress.CurrentLevelFileName, gameProgress.MaxAchievedLevelName);
            else
            {
                this.levelSequence = new LevelSequence(levelSequenceSpecification, null, null);
                ProgressToLevelStart();
            }
            saveProfileNumber = newProfile;
        }

        internal void StartNewGame()
        {
            levelSequence.Reset();
            ProgressToLevelStart();
            GameProgressAccess.Save(gameProgress, saveProfileNumber.Value);
            StartCurrentLevel(true);
        }

        internal void ContinueCurrentGame()
        {
            StartCurrentLevel(true);
        }

        internal void ContinueFrom(String levelName)
        {
            gameProgress = new GameProgress
            {
                CurrentLevelFileName = levelName,
                MaxAchievedLevelName = levelSequence.GetMaxAchievedLevelFile(),
                LevelProgress = new LevelProgress
                {
                    CurrentCheckPoint = CheckpointSpecification.StartCheckpointName,
                    GameTime = 0
                }
            };
            GameProgressAccess.Save(gameProgress, saveProfileNumber.Value);
            StartCurrentLevel(true);
        }

        internal Boolean LevelAvailable(String levelFileName)
        {
            return levelSequence.LevelIsAvailable(levelFileName);
        }

        internal void NotableProgressMaid(Object sender, CheckpointReachedEventArgs eventArgs)
        {
            gameProgress.LevelProgress = eventArgs.LevelProgress;
            GameProgressAccess.Save(gameProgress, saveProfileNumber.Value);
        }

        private void StartCurrentLevel(Boolean showTitle)
        {
            levelSequence.MarkLevelAsCurrentContinuePoint(gameProgress.CurrentLevelFileName);
            componentsManager.DeleteCurrentLevelRelatedComponents();
            componentsManager.InitNewLevelRelatedComponents(gameProgress, levelSequence);
            if (showTitle)
            {
                if (componentsManager.CutsceneBeforeLevel != null)
                    SwitchToCutsceneBeforeState();
                else
                    SwitchToTitleState();
            }
            else
                SwitchToInGameState();
        }

        private void SwitchToNextLevel()
        {
            levelSequence.MarkLevelComplete();
            if (!levelSequence.GameCompleted)
            {
                ProgressToLevelStart();
                StartCurrentLevel(true);
                GameProgressAccess.Save(gameProgress, saveProfileNumber.Value);
            }
            else
            {
                SwitchToBetweenLevelsState();
                componentsManager.Menu.ReturnMenuToDefaultStateAtPause();
            }
        }

        internal void TryPauseSwitch()
        {
            switch (currentState)
            {
                case GameState.InGame:
                    SwitchToPausedState();
                    break;
                case GameState.Paused:
                    SwitchToInGameState();
                    break;
            }
        }

        internal void TryPause()
        {
            if (currentState == GameState.InGame)
                SwitchToPausedState();
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

        private void SwitchToTitleState()
        {
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchMenuRelatedComponents(false);
            componentsManager.SwitchCutsceneAfterLevel(false);
            componentsManager.SwitchCutsceneBeforeLevel(false);
            componentsManager.SwitchLevelEndingRelatedComponents(false);
            componentsManager.SwitchLevelTitleRelatedComponents(true);
            currentState = GameState.LevelTitle;
        }

        private void SwitchToInGameState()
        {
            componentsManager.SwitchMenuRelatedComponents(false);
            componentsManager.SwitchCutsceneAfterLevel(false);
            componentsManager.SwitchCutsceneBeforeLevel(false);
            componentsManager.SwitchLevelTitleRelatedComponents(false);
            componentsManager.SwitchLevelEndingRelatedComponents(false);
            componentsManager.SwitchGameplayRelatedComponents(true);
            currentState = GameState.InGame;
        }

        private void SwitchToPausedState()
        {
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchLevelTitleRelatedComponents(false);
            componentsManager.SwitchLevelEndingRelatedComponents(false);
            componentsManager.SwitchCutsceneAfterLevel(false);
            componentsManager.SwitchCutsceneBeforeLevel(false);
            componentsManager.SwitchMenuRelatedComponents(true);
            componentsManager.Menu.ReturnMenuToDefaultStateAtPause();
            currentState = GameState.Paused;
        }

        private void SwitchToLevelEndingState()
        {
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchMenuRelatedComponents(false);
            componentsManager.SwitchLevelTitleRelatedComponents(false);
            componentsManager.SwitchCutsceneAfterLevel(false);
            componentsManager.SwitchCutsceneBeforeLevel(false);
            componentsManager.SwitchLevelEndingRelatedComponents(true);
            currentState = GameState.LevelEnding;
        }

        private void SwitchToBetweenLevelsState()
        {
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchLevelTitleRelatedComponents(false);
            componentsManager.SwitchCutsceneAfterLevel(false);
            componentsManager.SwitchCutsceneBeforeLevel(false);
            componentsManager.SwitchLevelEndingRelatedComponents(false);
            componentsManager.SwitchMenuRelatedComponents(true);
            componentsManager.DeleteCurrentLevelRelatedComponents();
            currentState = GameState.BetweenLevels;
        }

        private void SwitchToCutsceneBeforeState()
        {
            componentsManager.SwitchMenuRelatedComponents(false);
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchLevelTitleRelatedComponents(false);
            componentsManager.SwitchCutsceneAfterLevel(false);
            componentsManager.SwitchLevelEndingRelatedComponents(false);
            componentsManager.SwitchCutsceneBeforeLevel(true);
            currentState = GameState.CutsceneBefore;
        }

        private void SwitchToCutsceneAfterState()
        {
            componentsManager.SwitchMenuRelatedComponents(false);
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchLevelTitleRelatedComponents(false);
            componentsManager.SwitchCutsceneBeforeLevel(false);
            componentsManager.SwitchLevelEndingRelatedComponents(false);
            componentsManager.SwitchCutsceneAfterLevel(true);
            currentState = GameState.CutsceneAfter;
        }

        private void ProgressToLevelStart()
        {
            this.gameProgress = new GameProgress
            {
                CurrentLevelFileName = this.levelSequence.GetCurrentLevelFile(),
                MaxAchievedLevelName = this.levelSequence.GetMaxAchievedLevelFile(),
                LevelProgress = new LevelProgress
                {
                    CurrentCheckPoint = CheckpointSpecification.StartCheckpointName,
                    GameTime = 0
                }
            };
        }
    }
}
