using System;
using System.Collections.Generic;
using System.Data;

namespace UpCash.Menus
{
    /// <summary> Меню счетов, служит для просмотрав счетов. </summary>
    internal class AccountsMenu : AMenu
    {
        public AccountsMenu(string title = null)
        {
            Title = title;

            Options = new List<Option>()
            {
                new Option("Показать счета", ShowAccounts),
                new Option("Добавить счёт", ShowAddAccountMenu),
                new Option("Удалить счёт", ShowDeleteAccountMenu),
                new Option("Главное меню", () => MenuAction = MenuActions.Back)
            };
        }

        /// <summary> Показывает список аккаунтов. </summary>
        private void ShowAccounts()
        {
            Console.Clear();

            DataTable accounts = MyDataBase.GetDB().GetTable("SELECT name, balance, currency FROM Account;");

            for(int i = 0; i < accounts.Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts.Rows[i]["name"].ToString()} - {accounts.Rows[i]["balance"].ToString()} - {accounts.Rows[i]["currency"].ToString()}");
            }

            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
            Console.ReadKey();
        }

        /// <summary> Показывает меню для добавления аккаунта. </summary>
        private void ShowAddAccountMenu()
        {
            Console.Clear();

            string accountName = Console.ReadLine();
            double accountBalance = double.Parse(Console.ReadLine());
            string accountCurrency = Console.ReadLine();

            MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"INSERT INTO Account VALUES ('{accountName}', '{accountBalance}', '{accountCurrency}');");

            Console.WriteLine("Аккаунт успешно добавлен.");
            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
            Console.ReadKey();
        }

        /// <summary> Показывает меню для удаления аккаунта. </summary>
        private void ShowDeleteAccountMenu()
        {
            Console.Clear();

            DataTable accounts = MyDataBase.GetDB().GetTable("SELECT name, balance, currency FROM Account;");

            for (int i = 0; i < accounts.Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts.Rows[i]["name"].ToString()} - {accounts.Rows[i]["balance"].ToString()} - {accounts.Rows[i]["currency"].ToString()}");
            }

            Console.WriteLine("Введите имя счёта, который нужно удалить.");
            string accountName = Console.ReadLine();

            MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"DELETE FROM Account WHERE name = '{accountName}';");

            Console.WriteLine("Аккаунт успешно удалён.");
            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
            Console.ReadKey();
        }
    }
}
