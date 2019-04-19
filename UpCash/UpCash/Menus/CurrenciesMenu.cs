using System;
using System.Collections.Generic;
using System.Data;

namespace UpCash.Menus
{
    /// <summary> Меню валют, служит для настройки информации о валютах. </summary>
    internal class CurrenciesMenu : AMenu
    {
        private static CurrenciesMenu _instance;

        private CurrenciesMenu()
        {
            Options = new List<Option>()
            {
                new Option("Вывести все валюты", ShowCurrencies),
                new Option("Добавить валюту", ShowAddCurrencyMenu),
                new Option("Удалить валюту", () => MenuAction = MenuActions.Back),
                new Option("Настроить курс валют", () => MenuAction = MenuActions.Back),
                new Option("Главное меню", () => MenuAction = MenuActions.Back)
            };
        }

        public static CurrenciesMenu GetMenu()
        {
            if (_instance == null) _instance = new CurrenciesMenu();
            return _instance;
        }

        private void ShowCurrencies()
        {
            Console.Clear();
            Console.WriteLine("Список валют:");

            DataTable currencies = MyDataBase.GetDB().GetTable("SELECT code, name FROM Currency;");

            for(int i = 0; i < currencies.Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {currencies.Rows[i]["code"].ToString()} - {currencies.Rows[i]["name"].ToString()}");
            }

            Console.WriteLine("Нажмите любую клавишу, что б вернуться.");
            Console.ReadKey();
        }

        private void ShowAddCurrencyMenu()
        {
            Console.Clear();
            Console.WriteLine("Список валют:");

            DataTable currencies = MyDataBase.GetDB().GetTable("SELECT code, name FROM Currency;");

            for (int i = 0; i < currencies.Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {currencies.Rows[i]["code"].ToString()} - {currencies.Rows[i]["name"].ToString()}");
            }

            // TODO: Реализуй добавление валюты

            Console.WriteLine("Нажмите любую клавишу, что б вернуться.");
            Console.ReadKey();
        }
    }
}
