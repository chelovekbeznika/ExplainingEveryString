using System.Linq;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemWithContainer : MenuItemButton
    {
        private readonly MenuItemsContainer container;
        private readonly MenuVisiblePart menuVisiblePart;

        internal override MenuItemsContainer ParentContainer
        {
            get => base.ParentContainer;
            set
            {
                base.ParentContainer = value;
                container.ParentContainer = value;
            }
        }

        internal MenuItemWithContainer(IMenuItemDisplayble displayble, MenuItemsContainer container, MenuVisiblePart menuVisiblePart)
            : base(displayble)
        {
            this.container = container;
            this.menuVisiblePart = menuVisiblePart;
            this.IsVisible = () => container.Items.Any(item => item.IsVisible());
        }

        internal void SetParentContainer(MenuItemsContainer parentContainer)
        {
            container.ParentContainer = parentContainer;
        }

        internal override void RequestCommandExecution()
        {
            base.RequestCommandExecution();
            menuVisiblePart.CurrentButtonsContainer = container;
        }
    }
}
