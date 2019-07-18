using ExplainingEveryString.Core.Menu;
using ExplainingEveryString.Data.Blueprints;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core
{
    internal class ComponentsManager
    {
        private EesGame game;

        internal InterfaceComponent Interface { get; private set; }
        internal MenuComponent Menu { get; private set; }
        internal GameplayComponent CurrentGameplay { get; private set; }

        internal ComponentsManager(EesGame game)
        {
            this.game = game;
            Interface = new InterfaceComponent(game);
            Menu = new MenuComponent(game);
        }

        internal void ConstructGameplayComponent(IBlueprintsLoader blueprintsLoader, String levelFile)
        {
            CurrentGameplay = new GameplayComponent(game, blueprintsLoader, levelFile);
        }

        internal void InitComponents()
        {
            GameComponentCollection components = game.Components;
            components.Add(CurrentGameplay);
            components.Add(Interface);
            components.Add(Menu);
            SwitchMenuRelatedComponents(true);
            SwitchGameplayRelatedComponents(false);
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
