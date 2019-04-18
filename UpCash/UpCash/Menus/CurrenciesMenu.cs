using System;
using System.Collections.Generic;
using System.Data;

namespace UpCash.Menus
{
    /// <summary> Меню валют, служит для настройки информации о валютах. </summary>
    internal class CurrenciesMenu : AMenu
    {
        public CurrenciesMenu(string title = null)
        {
            Title = title;

            Options = new List<Option>()
            {
                new Option("Вывести все валюты", ShowCurrencies),
                new Option("Добавить валюту", ShowAddCurrencyMenu),
                new Option("Удалить валюту", () => menuAction = MenuActions.Back),
                new Option("Настроить курс валют", () => menuAction = MenuActions.Back),
                new Option("Главное меню", () => menuAction = MenuActions.Back)
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
                else if (menuAction == MenuActions.Back) break;
            }
        }

        private void ShowCurrencies()
        {
            Console.Clear();
            Console.WriteLine("Список валют:");

            DataTable currencies = MyDataBase.GetDB().GetTable("SELECT * FROM Currency;");

            for(int i = 0; i < currencies.Rows.Count; i++)
                Console.WriteLine($"{i + 1}. {currencies.Rows[i][0].ToString()} - {currencies.Rows[i][1].ToString()}");

            Console.WriteLine("Нажмите любую клавишу, что б вернуться.");
            Console.ReadKey();
        }

        private void ShowAddCurrencyMenu()
        {
            Console.Clear();
            Console.WriteLine("Список валют:");

            DataTable currencies = MyDataBase.GetDB().GetTable("SELECT * FROM Currency;");

            for (int i = 0; i < currencies.Rows.Count; i++)
                Console.WriteLine($"{i + 1}. {currencies.Rows[i][0].ToString()} - {currencies.Rows[i][1].ToString()}");

            Console.WriteLine("Нажмите любую клавишу, что б вернуться.");
            Console.ReadKey();
        }
    }
}
