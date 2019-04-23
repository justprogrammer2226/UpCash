using System.Collections.Generic;

namespace UpCash.Menus
{
    /// <summary> Главное меню, служит для навигации по остальным меню. </summary>
    internal class MainMenu : AMenu
    {
        private static MainMenu _instance;

        private MainMenu()
        {
            Options = new List<Option>()
            {
                new Option("Счета", AccountsMenu.GetMenu().Show),
                new Option("Операции", OperationsMenu.GetMenu().Show),
                new Option("Посмотреть валюты", CurrenciesMenu.GetMenu().Show),
                new Option("Статьи", ItemsMenu.GetMenu().Show),
                new Option("Выход", () => MenuAction = MenuActions.Back)
            };
        }

        public static MainMenu GetMenu()
        {
            if (_instance == null) _instance = new MainMenu();
            return _instance;
        }
    }
}
