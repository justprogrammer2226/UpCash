using System;
using System.Collections.Generic;

namespace UpCash.Menus
{
    /// <summary> Меню операций, служит для просмотра и произведения различных денежных операций. </summary>
    internal class OperationsMenu : AMenu
    {
        private static OperationsMenu _instance;

        private OperationsMenu()
        {
            Options = new List<Option>()
            {
                new Option("Главное меню", () => MenuAction = MenuActions.Back)
            };
        }

        public static OperationsMenu GetMenu()
        {
            if (_instance == null) _instance = new OperationsMenu();
            return _instance;
        }
    }
}
