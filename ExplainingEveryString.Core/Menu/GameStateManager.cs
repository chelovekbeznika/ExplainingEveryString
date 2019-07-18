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
        private GameplayComponent gameplayComponent;
        private InterfaceComponent interfaceComponent;
        private MenuComponent menuComponent;

        internal GameStateManager(Game game, 
            GameplayComponent gameplay, InterfaceComponent @interface, MenuComponent menu)
        {
            this.game = game;
            this.gameplayComponent = gameplay;
            this.interfaceComponent = @interface;
            this.menuComponent = menu;
        }

        internal void InitMenuInput(MenuInputProcessor menuInputProcessor)
        {
            menuInputProcessor.Exit.ButtonPressed += (sender, e) => game.Exit();
            menuInputProcessor.Pause.ButtonPressed += (sender, e) => SwitchGameState();
            menuInputProcessor.Down.ButtonPressed += (sender, e) => menuComponent.SelectedItemIndex += 1;
            menuInputProcessor.Up.ButtonPressed += (sender, e) => menuComponent.SelectedItemIndex -= 1;
        }

        internal void InitComponents()
        {
            GameComponentCollection components = game.Components;
            components.Add(gameplayComponent);
            components.Add(interfaceComponent);
            components.Add(menuComponent);
            SwitchMenuRelatedComponents(true);
            SwitchGameplayRelatedComponents(false);
        }

        internal void SwitchGameState()
        {
            GameState newState = CurrentState == GameState.Gameplay ? GameState.Menu : GameState.Gameplay;
            CurrentState = newState;
            switch (newState)
            {
                case GameState.Menu:
                    SwitchGameplayRelatedComponents(false);
                    SwitchMenuRelatedComponents(true);
                    break;
                case GameState.Gameplay:
                    SwitchGameplayRelatedComponents(true);
                    SwitchMenuRelatedComponents(false);
                    break;
            }
        }

        private void SwitchGameplayRelatedComponents(Boolean active)
        {
            gameplayComponent.Enabled = active;
            interfaceComponent.Enabled = active;
            gameplayComponent.Visible = active;
            interfaceComponent.Visible = active;
        }

        private void SwitchMenuRelatedComponents(Boolean active)
        {
            menuComponent.Enabled = active;
            menuComponent.Visible = active;
        }
    }
}
