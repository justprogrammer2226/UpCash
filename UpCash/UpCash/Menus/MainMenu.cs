using System;
using System.Collections.Generic;

namespace UpCash.Menus
{
    /// <summary> Главное меню, служит для навигации по остальным меню. </summary>
    internal class MainMenu : AMenu
    {
        public MainMenu(string title = null)
        {
            Title = title;

            Options = new List<Option>()
            {
                new Option("Счета", new AccountsMenu().Show),
                new Option("Операции", new OperationsMenu().Show),
                new Option("Посмотреть валюты", new CurrenciesMenu().Show),
                new Option("Статьи", new ItemsMenu().Show),
                new Option("Выход", () => MenuAction = MenuActions.Back)
            };
        }
    }
}
