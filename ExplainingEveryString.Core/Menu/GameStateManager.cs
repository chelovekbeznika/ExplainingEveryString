using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Menu
{
    internal class GameStateManager
    {
        private enum GameState { Gameplay, Menu }

        private GameState CurrentState = GameState.Menu;
        private Game game;
        private ComponentsManager componentsManager;

        internal GameStateManager(Game game, ComponentsManager componentsManager)
        {
            this.game = game;
            this.componentsManager = componentsManager;
        }

        internal void InitMenuInput(MenuInputProcessor menuInputProcessor)
        {
            menuInputProcessor.Exit.ButtonPressed += (sender, e) => game.Exit();
            menuInputProcessor.Pause.ButtonPressed += (sender, e) => TryPauseGame();
            menuInputProcessor.Down.ButtonPressed += (sender, e) => componentsManager.Menu.SelectedItemIndex += 1;
            menuInputProcessor.Up.ButtonPressed += (sender, e) => componentsManager.Menu.SelectedItemIndex -= 1;
            menuInputProcessor.Accept.ButtonPressed += (sender, e) => componentsManager.Menu.Accept();
        }

        internal void InitComponents()
        {
            componentsManager.InitComponents();
        }

        internal void TryPauseGame()
        {
            GameState newState = CurrentState == GameState.Gameplay ? GameState.Menu : GameState.Gameplay;
            CurrentState = newState;
            switch (newState)
            {
                case GameState.Menu:
                    componentsManager.SwitchGameplayRelatedComponents(false);
                    componentsManager.SwitchMenuRelatedComponents(true);
                    break;
                case GameState.Gameplay:
                    componentsManager.SwitchGameplayRelatedComponents(true);
                    componentsManager.SwitchMenuRelatedComponents(false);
                    break;
            }
        }
    }
}
