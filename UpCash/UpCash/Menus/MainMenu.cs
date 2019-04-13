using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                new Option("Счета", () => ShowMenu(new AccountsMenu())),
                new Option("Операции", () => ShowMenu(new OperationsMenu())),
                new Option("Посмотреть валюты", () => ShowMenu(new CurrenciesMenu())),
                new Option("Посмотреть статьи", () => ShowMenu(new ItemsMenu())),
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

        private void ShowMenu(AMenu menu) => menu.Show();
    }
}
