﻿using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Configuration;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameState
{
    internal class GameStateManager
    {
        private enum GameState { BetweenLevels, LevelTitle, InGame, Paused }

        private GameState currentState = GameState.BetweenLevels;
        private ComponentsManager componentsManager;
        private LevelSequence levelSequence;
        private GameProgress gameProgress;

        internal Boolean IsPaused => currentState == GameState.Paused;

        internal GameStateManager(LevelSequnceSpecification levelSequenceSpecification, 
            ComponentsManager componentsManager)
        {
            this.componentsManager = componentsManager;
            this.gameProgress = GameProgressAccess.Load();
            
            if (gameProgress != null)
                this.levelSequence = new LevelSequence(levelSequenceSpecification, 
                    gameProgress.CurrentLevelFileName, gameProgress.MaxAchievedLevelName);
            else
            {
                this.levelSequence = new LevelSequence(levelSequenceSpecification, null, null);
                ProgressToLevelStart();
            }
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
                        SwitchToNextLevel();
                    break;
                case GameState.LevelTitle:
                    if (componentsManager.CurrentLevelTitle.Closed)
                        SwitchToInGameState();
                    break;
            }
        }

        internal void StartNewGame()
        {
            levelSequence.Reset();
            ProgressToLevelStart();
            GameProgressAccess.Save(gameProgress);
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
            GameProgressAccess.Save(gameProgress);
            StartCurrentLevel(true);
        }

        internal Boolean LevelAvailable(String levelFileName)
        {
            return levelSequence.LevelIsAvailable(levelFileName);
        }

        internal void NotableProgressMaid(Object sender, CheckpointReachedEventArgs eventArgs)
        {
            gameProgress.LevelProgress = eventArgs.LevelProgress;
            GameProgressAccess.Save(gameProgress);
        }

        private void StartCurrentLevel(Boolean showTitle)
        {
            levelSequence.MarkLevelAsCurrentContinuePoint(gameProgress.CurrentLevelFileName);
            componentsManager.DeleteCurrentGameplayRelatedComponents();
            componentsManager.InitNewGameplayRelatedComponents(gameProgress);
            if (showTitle)
                SwitchToTitleState();
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
                GameProgressAccess.Save(gameProgress);
            }
            else
            {
                SwitchToBetweenLevelsState();
                componentsManager.Menu.ReturnMenuToDefaultStateAtPause();
            }
        }

        internal void TryPauseSwitch()
        {
            if (currentState == GameState.InGame)
            {
                SwitchToPausedState();
                componentsManager.Menu.ReturnMenuToDefaultStateAtPause();
                return;
            }
            if (currentState == GameState.Paused)
            {
                SwitchToInGameState();
                return;
            }
        }

        internal void ConfigChanged(Configuration newConfig)
        {
            componentsManager.MenuMusic.Volume = newConfig.MusicVolume;
            componentsManager.GameMusic.Volume = newConfig.MusicVolume;
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
            componentsManager.SwitchLevelTitleRelatedComponents(true);
            currentState = GameState.LevelTitle;
        }

        private void SwitchToInGameState()
        {
            componentsManager.SwitchGameplayRelatedComponents(true);
            componentsManager.SwitchMenuRelatedComponents(false);
            componentsManager.SwitchLevelTitleRelatedComponents(false);
            currentState = GameState.InGame;
        }

        private void SwitchToPausedState()
        {
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchMenuRelatedComponents(true);
            componentsManager.SwitchLevelTitleRelatedComponents(false);
            currentState = GameState.Paused;
        }

        private void SwitchToBetweenLevelsState()
        {
            componentsManager.SwitchMenuRelatedComponents(true);
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchLevelTitleRelatedComponents(false);
            componentsManager.DeleteCurrentGameplayRelatedComponents();
            currentState = GameState.BetweenLevels;
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
