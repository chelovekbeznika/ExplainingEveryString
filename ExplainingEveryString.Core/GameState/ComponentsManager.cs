using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Menu;
using System;

namespace ExplainingEveryString.Core.GameState
{
    internal class ComponentsManager
    {
        private EesGame game;

        internal InterfaceComponent Interface { get; private set; }
        internal MenuComponent Menu { get; private set; }
        internal GameplayComponent CurrentGameplay { get; private set; }
        internal MusicComponent MenuMusic { get; private set; }
        internal MusicComponent GameMusic { get; private set; }
        internal NotificationsComponent Notifications { get; private set; }

        internal ComponentsManager(EesGame game, LevelSequnceSpecification levelSequenceSpecification,
            MusicTestButtonSpecification[] musicTestSpecification)
        {
            this.game = game;
            Interface = new InterfaceComponent(game);
            Menu = new MenuComponent(game, levelSequenceSpecification, musicTestSpecification);
            MenuMusic = new MusicComponent(game);
            GameMusic = new MusicComponent(game);
            Notifications = new NotificationsComponent(game);
        }

        internal void InitNewGameplayComponent(GameProgress gameProgress)
        {
            CurrentGameplay = new GameplayComponent(
                game, gameProgress.CurrentLevelFileName, gameProgress.LevelProgress);
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
            var components = game.Components;
            TimersComponent.Init(game);
            components.Add(Interface);
            components.Add(Menu);
            components.Add(MenuMusic);
            components.Add(GameMusic);
            components.Add(Notifications);
            SwitchMenuRelatedComponents(true);
        }

        internal void SwitchGameplayRelatedComponents(Boolean active)
        {     
            Interface.Enabled = active; 
            Interface.Visible = active;
            CurrentGameplay.Enabled = active;
            CurrentGameplay.Visible = active;
            GameMusic.Enabled = active;
            TimersComponent.Instance.Enabled = active;
        }

        internal void SwitchMenuRelatedComponents(Boolean active)
        {
            Menu.Enabled = active;
            Menu.Visible = active;
            MenuMusic.Enabled = active;
        }
    }
}
