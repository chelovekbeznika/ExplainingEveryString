using System;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemsContainer
    {
        private Int32 defaultButton;

        internal MenuItem[] Items { get; private set; }
        internal Int32 SelectedIndex { get; private set; }

        internal MenuItemsContainer(MenuItem[] items, Int32 defaultButton)
        {
            this.Items = items;
            this.defaultButton = defaultButton;
            SelectDefaultButton();
        }

        internal void SelectDefaultButton()
        {
            if (IsValidButtonIndex(SelectedIndex))
                Items[SelectedIndex].Selected = false;
            this.SelectedIndex = defaultButton - 1;
            this.SelectedIndex = FindVisibleButton(+1);
            this.Items[SelectedIndex].Selected = true;
        }

        internal void SelectNextButton()
        {
            Items[SelectedIndex].Selected = false;
            SelectedIndex = FindVisibleButton(+1);
            Items[SelectedIndex].Selected = true;
        }

        internal void SelectPreviousButton()
        {
            if (SelectedIndex > 0)
            {
                Items[SelectedIndex].Selected = false;
                SelectedIndex = FindVisibleButton(-1);
                Items[SelectedIndex].Selected = true;
            }
        }

        private Int32 FindVisibleButton(Int32 step)
        {
            Int32 currentButton = SelectedIndex + step;
            while (IsValidButtonIndex(currentButton) && !Items[currentButton].IsVisible())
            {
                currentButton += step;
            }
            if (currentButton >= 0 && currentButton < Items.Length)
                return currentButton;
            else
                return SelectedIndex;
        }

        private Boolean IsValidButtonIndex(Int32 index)
        {
            return index >= 0 && index < Items.Length;
        }

        internal void RequestSelectedCommandExecution()
        {
            Items[SelectedIndex].RequestCommandExecution();
        }
    }
}
