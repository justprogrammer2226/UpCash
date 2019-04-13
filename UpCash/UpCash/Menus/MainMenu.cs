﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpCash.Menus
{
    internal class MainMenu : AMenu
    {
        public MainMenu(string title = null)
        {
            Title = title;

            Options = new List<Option>()
            {
                new Option("Посмотреть счета", ShowAccountsMenu),
                new Option("Операции", ShowOperationsMenu),
                new Option("Посмотреть валюты", ShowCurrenciesMenu),
                new Option("Посмотреть статьи", ShowItemsMenu),
                new Option("Выход", () => menuAction = MenuActions.Back)
            };

            menuAction = MenuActions.Show;
        }

        /// <summary> Показывает меню. </summary>
        public override void Show()
        {
            while(true)
            {
                if (menuAction == MenuActions.Show)
                {
                    Console.Clear();

                    if (Title != null) Console.WriteLine(Title);

                    for (int i = 0; i < Options.Count; i++)
                        Console.WriteLine($"{i + 1}. {Options[i].Name}");

                    if (int.TryParse(Console.ReadLine(), out int indexSelectedOption) && indexSelectedOption >= 1 && indexSelectedOption <= Options.Count)
                    {
                        Options[indexSelectedOption - 1].Action();
                    }
                    else
                    {
                        Console.WriteLine("Данной опции не существует.");
                        Console.ReadKey();
                    }
                }
                else if (menuAction == MenuActions.Back) Environment.Exit(0);
            }
        }

        private void ShowAccountsMenu()
        {
            AMenu accountsMenu = new AccountsMenu();
            accountsMenu.Show();
        }

        private void ShowOperationsMenu()
        {
            AMenu operationsMenu = new OperationsMenu();
            operationsMenu.Show();
        }

        private void ShowCurrenciesMenu()
        {
            AMenu currenciesMenu = new CurrenciesMenu();
            currenciesMenu.Show();
        }

        private void ShowItemsMenu()
        {
            AMenu itemsMenu = new ItemsMenu();
            itemsMenu.Show();
        }
    }
}
