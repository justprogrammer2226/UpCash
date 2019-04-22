using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

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
                new Option("Вывести все валюты", ShowCurrenciesOutputMenu),
                new Option("Добавить валюту", ShowAddCurrencyMenu),
                new Option("Удалить валюту", ShowDeleteCurrencyMenu),
                new Option("Главное меню", () => MenuAction = MenuActions.Back)
            };
        }

        public static CurrenciesMenu GetMenu()
        {
            if (_instance == null) _instance = new CurrenciesMenu();
            return _instance;
        }

        private void ShowCurrenciesOutputMenu()
        {
            Console.Clear();
            Console.WriteLine("Список валют:");
            ShowCurrencies();
            Console.WriteLine("Нажмите любую клавишу, что б вернуться.");
            Console.ReadKey();
        }

        private void ShowCurrencies()
        {
            DataTable currencies = MyDataBase.GetDB().GetTable("SELECT code, name FROM Currency;");
            for(int i = 0; i < currencies.Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {currencies.Rows[i]["code"].ToString()} - {currencies.Rows[i]["name"].ToString()}");
            }
        }

        private void ShowAddCurrencyMenu()
        {
            Console.Clear();
            Console.WriteLine("Список валют:");
            ShowCurrencies();

            string codeCurrency = GetCodeCurrency();
            string nameCurrency = GetNameCurrency();

            try
            {
                MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"INSERT INTO Currency VALUES ('{codeCurrency}', '{nameCurrency}');");
                Console.WriteLine("Валюта успешно добавлена.");
                Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                Console.ReadKey();
            }
            catch (SQLiteException)
            {
                Console.WriteLine("Валюта не была добавлена. Вероятно вы ввели имя существующеей валюты");
                Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                Console.ReadKey();
            }
        }

        private void ShowDeleteCurrencyMenu()
        {
            try
            {
                Console.Clear();
                MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"DELETE FROM Currency WHERE code = '{GetCodeCurrency()}';");
                Console.WriteLine("Счёт успешно удалён.");
                Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                Console.ReadKey();
            }
            catch (SQLiteException)
            {
                Console.WriteLine("Валюта не была удалена. Вероятно, есть счета, которые ссылаются на эту валюту.");
                Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                Console.ReadKey();
            }
        }

        private string GetCodeCurrency()
        {
            string codeCurrency;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Список валют:");
                ShowCurrencies();

                Console.WriteLine("Введите код валюты.");
                codeCurrency = Console.ReadLine();

                if (IsValidCodeCurrency(codeCurrency)) break;
                else
                {
                    Console.WriteLine("Вы должны ввести валидный код валюты.");
                    Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                    Console.ReadKey();
                }
            }
            return codeCurrency;
        }

        private string GetNameCurrency()
        {
            string nameCurrency;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Список валют:");
                ShowCurrencies();

                Console.WriteLine("Введите имя валюты.");
                nameCurrency = Console.ReadLine();

                if (IsValidNameCurrency(nameCurrency)) break;
                else
                {
                    Console.WriteLine("Вы должны ввести валидное имя валюты.");
                    Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                    Console.ReadKey();
                }
            }
            return nameCurrency;
        }

        private bool IsValidCodeCurrency(string codeCurrency)
        {
            return !string.IsNullOrEmpty(codeCurrency);
        }

        private bool IsValidNameCurrency(string nameCurrency)
        {
            return !string.IsNullOrEmpty(nameCurrency);
        }
    }
}
