using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.Level;
using ExplainingEveryString.Data.Menu;
using System;

namespace ExplainingEveryString.Core.GameState
{
    internal class ComponentsManager
    {
        private readonly EesGame game;

        internal InterfaceComponent Interface { get; private set; }
        internal MenuComponent Menu { get; private set; }
        internal GameplayComponent CurrentGameplay { get; private set; }
        internal LevelTitleComponent CurrentLevelTitle { get; private set; }
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

        internal void InitNewGameplayRelatedComponents(GameProgress gameProgress)
        {
            CurrentGameplay = new GameplayComponent(
                game, gameProgress.CurrentLevelFileName, gameProgress.LevelProgress);
            Interface.SetGameplayComponentToDraw(CurrentGameplay);
            CurrentLevelTitle = new LevelTitleComponent(
                game, gameProgress.CurrentLevelFileName);
            game.Components.Add(CurrentLevelTitle);
            game.Components.Add(CurrentGameplay);
        }

        internal void DeleteCurrentGameplayRelatedComponents()
        {
            if (CurrentGameplay != null)
            {
                game.Components.Remove(CurrentGameplay);
                Interface.SetGameplayComponentToDraw(null);
                CurrentGameplay = null;
            }
            if (CurrentLevelTitle != null)
            {
                game.Components.Remove(CurrentLevelTitle);
                CurrentLevelTitle = null;
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

        internal void SwitchLevelTitleRelatedComponents(Boolean active)
        {
            if (CurrentLevelTitle != null)
            {
                CurrentLevelTitle.Enabled = active;
                CurrentLevelTitle.Visible = active;
            }
        }
    }
}
