using ConsoleHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace UpCash.Menus
{
    /// <summary> Меню счётов, служит для просмотра счётов и управления ними. </summary>
    internal class AccountsMenu : AMenu
    {
        private static AccountsMenu _instance;

        private AccountsMenu()
        {
            Options = new List<Option>()
            {
                new Option("Показать счёта", ShowListOfAccounts),
                new Option("Добавить счёт", ShowAddAccountMenu),
                new Option("Удалить счёт", ShowDeleteAccountMenu),
                new Option("Главное меню", () => MenuAction = MenuActions.Back)
            };
        }

        /// <summary> Возращает меню. </summary>
        public static AccountsMenu GetMenu()
        {
            if (_instance == null) _instance = new AccountsMenu();
            return _instance;
        }

        #region Implementing menu options
        /// <summary> Показывает меню списка счётов. </summary>
        private void ShowListOfAccounts()
        {
            Console.Clear();
            Console.WriteLine("Список счетов:");
            ShowAccounts();
            ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
        }

        /// <summary> Показывает меню для добавления счёта. </summary>
        private void ShowAddAccountMenu()
        {
            Console.Clear();
            Console.WriteLine("Список счетов:");
            ShowAccounts();

            string accountName = GetAccountName();
            double accountBalance = GetAccounBalance();
            string accountCurrency = GetAccounCurrency();

            try
            {
                MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"INSERT INTO Account VALUES ('{accountName}', '{accountBalance}', '{accountCurrency}');");
                Console.WriteLine("Счёта успешно добавлен.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
            catch (SQLiteException)
            {
                Console.WriteLine("Счёт не был добавлен. Вероятно вы ввели имя существующего счёта или указали несуществующий код валюты.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
        }

        /// <summary> Показывает меню для удаления счётов. </summary>
        private void ShowDeleteAccountMenu()
        {
            try
            {
                Console.Clear();
                MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"DELETE FROM Account WHERE name = '{GetAccountName()}';");
                Console.WriteLine("Счёт успешно удалён.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
            catch (SQLiteException)
            {
                Console.WriteLine("Счёт не был удален. Вероятно, есть операции, которые ссылаются на эту валюту.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
        }
        #endregion

        /// <summary> Выводит список всех счётов. </summary>
        private void ShowAccounts()
        {
            DataTable accounts = MyDataBase.GetDB().GetTable("SELECT name, balance, currency FROM Account;");

            for (int i = 0; i < accounts.Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts.Rows[i]["name"].ToString()} - {accounts.Rows[i]["balance"].ToString()} - {accounts.Rows[i]["currency"].ToString()}");
            }
        }

        /// <summary> Возращает имя счёта, введённого пользователем. </summary>
        private string GetAccountName()
        {
            return ConsoleInput.GetInput("Введите имя счёта.", returnValidInput: true, action: () =>
            {
                Console.WriteLine("Список счётов:");
                ShowAccounts();
                Console.WriteLine();
            }, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидное имя счёта");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }

        /// <summary> Возращает баланс счёта, введённого пользователем. </summary>
        private double GetAccounBalance()
        {
            return ConsoleInput.GetInput("Введите баланс счёта.", action: () =>
            {
                Console.WriteLine("Список счётов:");
                ShowAccounts();
                Console.WriteLine();
            }, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидное баланс счёта");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }

        /// <summary> Возращает валюту счёта, введённого пользователем. </summary>
        /// <param name="action"> Действие, которое будет выполнено, после отчистки. </param>
        private string GetAccounCurrency()
        {
            return ConsoleInput.GetInput("Введите валюту счёта.", returnValidInput: true, action: () =>
            {
                Console.WriteLine("Список счётов:");
                ShowAccounts();
                Console.WriteLine();
            }, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидную валюту счёта.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }
    }
}
