using ExplainingEveryString.Core.GameModel;
using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameState
{
    internal class GameStateManager
    {
        private enum GameState { BetweenLevels, InGame, Paused }

        private GameState CurrentState = GameState.BetweenLevels;
        private Game game;
        private ComponentsManager componentsManager;
        private LevelSequence levelSequence;
        private GameProgress gameProgress;

        internal GameStateManager(Game game, ComponentsManager componentsManager)
        {
            this.game = game;
            this.componentsManager = componentsManager;
            this.gameProgress = GameProgressAccess.Load();
            String[] rawLevelSequence = LevelSequenceAccess.LoadLevelSequence();
            if (gameProgress != null)
                this.levelSequence = new LevelSequence(rawLevelSequence, gameProgress.LevelName);
            else
            {
                this.levelSequence = new LevelSequence(rawLevelSequence, null);
                ProgressToLevelStart();
            }
        }

        internal void InitMenuInput(MenuInputProcessor menuInputProcessor)
        {
            menuInputProcessor.Pause.ButtonPressed += (sender, e) => TryPauseGame();
            menuInputProcessor.Down.ButtonPressed += (sender, e) => componentsManager.Menu.SelectedItemIndex += 1;
            menuInputProcessor.Up.ButtonPressed += (sender, e) => componentsManager.Menu.SelectedItemIndex -= 1;
            menuInputProcessor.Accept.ButtonPressed += (sender, e) => componentsManager.Menu.Accept();
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
            }
        }

        private void TryPauseGame()
        {
            if (CurrentState == GameState.InGame)
            {
                SwitchToPausedState();
                return;
            }
            if (CurrentState == GameState.Paused)
            {
                SwitchToInGameState();
                return;
            }
        }

        private void SwitchToInGameState()
        {
            componentsManager.SwitchGameplayRelatedComponents(true);
            componentsManager.SwitchMenuRelatedComponents(false);
            CurrentState = GameState.InGame;
        }

        private void SwitchToPausedState()
        {
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.SwitchMenuRelatedComponents(true);
            CurrentState = GameState.Paused;
        }

        private void SwitchToBetweenLevelsState()
        {
            componentsManager.SwitchMenuRelatedComponents(true);
            componentsManager.SwitchGameplayRelatedComponents(false);
            componentsManager.DeleteCurrentGameplayComponent();
            CurrentState = GameState.BetweenLevels;
        }

        private void ProgressToLevelStart()
        {
            this.gameProgress = new GameProgress
            {
                LevelName = this.levelSequence.CurrentLevelName,
                LevelProgress = new LevelProgress
                {
                    CurrentCheckPoint = CheckpointsManager.StartCheckpointName,
                    GameTime = 0
                }
            };
        }
    }
}
