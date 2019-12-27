using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameState
{
    internal class GameStateManager
    {
        private enum GameState { BetweenLevels, InGame, Paused }

        private GameState currentState = GameState.BetweenLevels;
        private readonly Game game;
        private ComponentsManager componentsManager;
        private LevelSequence levelSequence;
        private GameProgress gameProgress;

        internal Boolean IsPaused => currentState == GameState.Paused;

        internal GameStateManager(Game game, LevelSequnceSpecification levelSequenceSpecification, 
            ComponentsManager componentsManager)
        {
            this.game = game;
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
            if (componentsManager.CurrentGameplay != null)
            {
                if (componentsManager.CurrentGameplay.Lost)
                {
                    StartCurrentLevel();
                }
                if (componentsManager.CurrentGameplay.Won)
                    SwitchToNextLevel();
            }
        }

        internal void StartNewGame()
        {
            levelSequence.Reset();
            ProgressToLevelStart();
            GameProgressAccess.Save(gameProgress);
            StartCurrentLevel();
        }

        internal void ContinueCurrentGame()
        {
            StartCurrentLevel();
        }

        internal void ContinueFrom(String levelName)
        {
            gameProgress = new GameProgress
            {
                CurrentLevelFileName = levelName,
                MaxAchievedLevelName = levelSequence.GetMaxAchievedLevelFile(),
                LevelProgress = new LevelProgress
                {
                    CurrentCheckPoint = CheckpointsManager.StartCheckpointName,
                    GameTime = 0
                }
            };
            GameProgressAccess.Save(gameProgress);
            StartCurrentLevel();
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

        private void StartCurrentLevel()
        {
            componentsManager.DeleteCurrentGameplayComponent();
            componentsManager.InitNewGameplayComponent(gameProgress);
            SwitchToInGameState();
        }

        private void SwitchToNextLevel()
        {
            levelSequence.MarkLevelComplete();
            if (!levelSequence.GameCompleted)
            {
                ProgressToLevelStart();
                StartCurrentLevel();
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

        private void SwitchToInGameState()
        {
            componentsManager.SwitchGameplayRelatedComponents(true);
            componentsManager.SwitchMenuRelatedComponents(false);
            currentState = GameState.InGame;
        }

        private void SwitchToPausedState()
        {
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchMenuRelatedComponents(true);
            currentState = GameState.Paused;
        }

        private void SwitchToBetweenLevelsState()
        {
            componentsManager.SwitchMenuRelatedComponents(true);
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.DeleteCurrentGameplayComponent();
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
                    CurrentCheckPoint = CheckpointsManager.StartCheckpointName,
                    GameTime = 0
                }
            };
        }
    }
}
