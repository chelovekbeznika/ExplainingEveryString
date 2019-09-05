using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using Microsoft.Xna.Framework;
using System;

namespace ExplainingEveryString.Core.GameState
{
    internal class ComponentsManager
    {
        private EesGame game;
        private readonly IBlueprintsLoader blueprintsLoader;

        internal InterfaceComponent Interface { get; private set; }
        internal MenuComponent Menu { get; private set; }
        internal GameplayComponent CurrentGameplay { get; private set; }

        internal ComponentsManager(EesGame game, IBlueprintsLoader blueprintsLoader)
        {
            this.game = game;
            this.blueprintsLoader = blueprintsLoader;
            Interface = new InterfaceComponent(game);
            Menu = new MenuComponent(game);
        }

        internal void InitNewGameplayComponent(GameProgress gameProgress)
        {
            CurrentGameplay = new GameplayComponent(
                game, blueprintsLoader, gameProgress.LevelName, gameProgress.LevelProgress);
            Interface.SetGameplayComponentToDraw(CurrentGameplay);
            game.Components.Insert(ComponentsOrder.Gameplay, CurrentGameplay);
        }

        internal void DeleteCurrentGameplayComponent()
        {
            if (CurrentGameplay != null)
            {
                game.Components.Remove(CurrentGameplay);
                Interface.SetGameplayComponentToDraw(null);
                CurrentGameplay = null;
            }
        }

        internal void InitComponents()
        {
            GameComponentCollection components = game.Components;
            TimersComponent.Init(game);
            components.Add(Interface);
            components.Add(Menu);
            SwitchMenuRelatedComponents(true);
        }

        internal void SwitchGameplayRelatedComponents(Boolean active)
        {     
            Interface.Enabled = active; 
            Interface.Visible = active;
            CurrentGameplay.Enabled = active;
            CurrentGameplay.Visible = active;
        }

        internal void SwitchMenuRelatedComponents(Boolean active)
        {
            Menu.Enabled = active;
            Menu.Visible = active;
        }
    }
}
