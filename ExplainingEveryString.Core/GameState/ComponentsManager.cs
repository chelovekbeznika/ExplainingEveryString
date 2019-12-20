using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.Blueprints;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Menu;
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
        internal MusicComponent Music { get; private set; }

        internal ComponentsManager(EesGame game, LevelSequnceSpecification levelSequenceSpecification,
            MusicTestButtonSpecification[] musicTestSpecification, IBlueprintsLoader blueprintsLoader)
        {
            this.game = game;
            this.blueprintsLoader = blueprintsLoader;
            Interface = new InterfaceComponent(game);
            Menu = new MenuComponent(game, levelSequenceSpecification, musicTestSpecification);
            Music = new MusicComponent(game);
        }

        internal void InitNewGameplayComponent(GameProgress gameProgress)
        {
            CurrentGameplay = new GameplayComponent(
                game, blueprintsLoader, gameProgress.CurrentLevelFileName, gameProgress.LevelProgress);
            Interface.SetGameplayComponentToDraw(CurrentGameplay);
            game.Components.Add(CurrentGameplay);
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
            components.Add(Music);
            SwitchMenuRelatedComponents(true);
        }

        internal void SwitchGameplayRelatedComponents(Boolean active)
        {     
            Interface.Enabled = active; 
            Interface.Visible = active;
            CurrentGameplay.Enabled = active;
            CurrentGameplay.Visible = active;
            TimersComponent.Instance.Enabled = active;
        }

        internal void SwitchMenuRelatedComponents(Boolean active)
        {
            Menu.Enabled = active;
            Menu.Visible = active;
            Music.Enabled = active;
        }
    }
}
