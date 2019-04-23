using ConsoleHelper;
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

        /// <summary> Показывает меню вывода валют. </summary>
        private void ShowCurrenciesOutputMenu()
        {
            Console.Clear();
            Console.WriteLine("Список валют:");
            ShowCurrencies();
            ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
        }

        /// <summary> Выводит список валют. </summary>
        private void ShowCurrencies()
        {
            DataTable currencies = MyDataBase.GetDB().GetTable("SELECT code, name FROM Currency;");
            for(int i = 0; i < currencies.Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {currencies.Rows[i]["code"].ToString()} - {currencies.Rows[i]["name"].ToString()}");
            }
        }

        /// <summary> Выводит меню добавления валюты. </summary>
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
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
            catch (SQLiteException)
            {
                Console.WriteLine("Валюта не была добавлена. Вероятно вы ввели имя существующеей валюты");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
        }

        /// <summary> Выводит меню удаления валюты. </summary>
        private void ShowDeleteCurrencyMenu()
        {
            try
            {
                Console.Clear();
                MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"DELETE FROM Currency WHERE code = '{GetCodeCurrency()}';");
                Console.WriteLine("Валюта успешно удалена.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
            catch (SQLiteException)
            {
                Console.WriteLine("Валюта не была удалена. Вероятно, есть счета, которые ссылаются на эту валюту.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
        }

        /// <summary> Возращает код валюты, введённой пользователем. </summary>
        private string GetCodeCurrency()
        {
            return ConsoleInput.GetInput("Введите код валюты.", returnValidInput: true, action: () =>
            {
                Console.Clear();
                Console.WriteLine("Список валют:");
                ShowCurrencies();
                Console.WriteLine();
            }, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидный код валюты.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }

        /// <summary> Возращает имя валюты, введённой пользователем. </summary>
        private string GetNameCurrency()
        {
            return ConsoleInput.GetInput("Введите имя валюты.", returnValidInput: true, action: () =>
            {
                Console.Clear();
                Console.WriteLine("Список валют:");
                ShowCurrencies();
                Console.WriteLine();
            }, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидное имя валюты.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }
    }
}
