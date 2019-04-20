using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace UpCash.Menus
{
    /// <summary> Меню счётов, служит для просмотрав счётов. </summary>
    internal class AccountsMenu : AMenu
    {
        private static AccountsMenu _instance;

        private AccountsMenu()
        {
            Options = new List<Option>()
            {
                new Option("Показать счёта", ShowAccountsOutputMenu),
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

        /// <summary> Показывает меню списка счётов. </summary>
        private void ShowAccountsOutputMenu()
        {
            Console.Clear();

            ShowAccounts();

            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
            Console.ReadKey();
        }

        /// <summary> Показывает меню для добавления счёта. </summary>
        private void ShowAddAccountMenu()
        {
            while(true)
            {
                Console.Clear();

                string accountName = GetAccountName();
                double accountBalance = GetAccounBalance();
                string accountCurrency = GetAccounCurrency();

                try
                {
                    MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"INSERT INTO Account VALUES ('{accountName}', '{accountBalance}', '{accountCurrency}');");
                    Console.WriteLine("Счёта успешно добавлен.");
                    Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                    Console.ReadKey();
                    break;
                }
                catch (SQLiteException)
                {
                    Console.WriteLine("Счёт не был добавлен. Вероятно вы ввели имя существующего баланса или ввели неправильное имя счёта.");
                    Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                    Console.ReadKey();
                }
            }
        }

        /// <summary> Показывает меню для удаления счётов. </summary>
        private void ShowDeleteAccountMenu()
        {
            Console.Clear();
            string accountName = GetAccountName();

            MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"DELETE FROM Account WHERE name = '{accountName}';");

            Console.WriteLine("Счёт успешно удалён.");
            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
            Console.ReadKey();
        }

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
            string accountName;
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Список аккаунтов:");
                ShowAccounts();
                Console.WriteLine();

                Console.WriteLine("Введите имя счёта.");
                accountName = Console.ReadLine();

                if (IsValidAccountName(accountName)) break;
                else
                {
                    Console.WriteLine("Вы должны ввести валидное имя счёта");
                    Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                    Console.ReadKey();
                }
            }
            return accountName;
        }

        /// <summary> Проверяет валидное ли имя для счёта. </summary>
        /// <param name="accountName"> Имя счёта. </param>
        /// <returns> Возращает true, если имя счёта валидное, иначе false. </returns>
        private bool IsValidAccountName(string accountName)
        {
            return !string.IsNullOrEmpty(accountName);
        }

        /// <summary> Возращает баланс счёта, введённого пользователем. </summary>
        private double GetAccounBalance()
        {
            string accountBalance;
            double balance;
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Список аккаунтов:");
                ShowAccounts();
                Console.WriteLine();

                Console.WriteLine("Введите баланс счёта.");
                accountBalance = Console.ReadLine();

                if (IsValidAccountBalance(accountBalance, out balance)) break;
                else
                {
                    Console.WriteLine("Вы ввели некорректный баланс счёта.");
                    Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                    Console.ReadKey();
                }
            }
            return balance;
        }

        /// <summary> Проверяет валидный ли балан для счёта. </summary>
        /// <param name="accountBalance"> Баланс аккаунта. </param>
        /// <param name="balance"> Баланс аккаунта типа double, если строка с балансом валидная. </param>
        /// <returns> Возращает true, если баланс счёта валидный, иначе false. </returns>
        private bool IsValidAccountBalance(string accountBalance, out double balance)
        {
            return double.TryParse(accountBalance, out balance);
        }

        /// <summary> Возращает валюту счёта, введённого пользователем. </summary>
        /// <param name="action"> Действие, которое будет выполнено, после отчистки. </param>
        private string GetAccounCurrency()
        {
            string accountCurrency;
            while (true)
            {
                Console.Clear();

                Console.WriteLine("Список аккаунтов:");
                ShowAccounts();
                Console.WriteLine();

                Console.WriteLine("Введите валюту счёта.");
                accountCurrency = Console.ReadLine();

                if (IsValidAccountCurrency(accountCurrency)) break;
                else
                {
                    Console.WriteLine("Вы должны ввести имя счёта, а не просто нажать Enter.");
                    Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                    Console.ReadKey();
                }
            }
            return accountCurrency;
        }

        /// <summary> Проверяет валидная ли валюта для счёта. </summary>
        /// <param name="accountCurrency"> Валюта счёта. </param>
        /// <returns> Возращает true, если валюта счёта валидное, иначе false. </returns>
        private bool IsValidAccountCurrency(string accountCurrency)
        {
            return !string.IsNullOrEmpty(accountCurrency);
        }
    }
}
