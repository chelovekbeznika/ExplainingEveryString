using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplainingEveryString.Core.Menu
{
    interface IMenuBuilder
    {
        MenuItemsContainer BuildMenu(MenuVisiblePart menuVisiblePart);
    }
}
