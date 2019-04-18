using System;
using System.Collections.Generic;

namespace UpCash.Menus
{
    /// <summary> Меню операций, служит для просмотра и произведения различных денежных операций. </summary>
    internal class OperationsMenu : AMenu
    {
        public OperationsMenu(string title = null)
        {
            Title = title;

            Options = new List<Option>()
            {
                new Option("Главное меню", () => MenuAction = MenuActions.Back)
            };
        }
    }
}
