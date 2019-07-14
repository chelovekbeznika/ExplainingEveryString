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
        private GameComponentCollection components;
        private GameplayComponent gameplayComponent;
        private InterfaceComponent interfaceComponent;
        private MenuComponent menuComponent;

        internal GameStateManager(GameComponentCollection components, 
            GameplayComponent gameplay, InterfaceComponent @interface, MenuComponent menu)
        {
            this.components = components;
            this.gameplayComponent = gameplay;
            this.interfaceComponent = @interface;
            this.menuComponent = menu;
        }

        internal void InitComponents()
        {
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
