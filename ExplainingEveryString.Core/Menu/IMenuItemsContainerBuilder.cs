namespace ExplainingEveryString.Core.Menu
{
    interface IMenuBuilder
    {
        MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart);
    }
}
