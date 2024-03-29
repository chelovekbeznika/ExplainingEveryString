﻿using System;

namespace ExplainingEveryString.Core.Menu
{
    internal class MenuItemsContainer
    {
        private readonly Int32 defaultButton;

        internal MenuItem[] Items { get; private set; }
        internal Int32 SelectedIndex { get; private set; }
        internal MenuItemsContainer ParentContainer { get; set; }
        internal event EventHandler ContainerAppearedOnScreen;

        internal MenuItemsContainer(MenuItem[] items, Int32 defaultButton = 0)
        {
            this.Items = items;
            foreach (var item in Items)
                item.ParentContainer = this;
            this.defaultButton = defaultButton;
            SelectDefaultButton();
        }

        internal void SelectDefaultButton()
        {
            if (IsValidButtonIndex(SelectedIndex))
                Items[SelectedIndex].Selected = false;
            this.SelectedIndex = defaultButton - 1;
            this.SelectedIndex = FindVisibleButton(+1);
            if (IsValidButtonIndex(SelectedIndex))
                this.Items[SelectedIndex].Selected = true;
        }

        internal void ProcessScreenAppearance()
        {
            ContainerAppearedOnScreen?.Invoke(this, EventArgs.Empty);
        }

        internal void SelectNextButton()
        {
            if (SelectedIndex < Items.Length - 1)
            {
                Items[SelectedIndex].Selected = false;
                SelectedIndex = FindVisibleButton(+1);
                Items[SelectedIndex].Selected = true;
            }
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
            var currentButton = SelectedIndex + step;
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

        internal void Increment()
        {
            Items[SelectedIndex].Increment();
        }

        internal void Decrement()
        {
            Items[SelectedIndex].Decrement();
        }
    }
}
