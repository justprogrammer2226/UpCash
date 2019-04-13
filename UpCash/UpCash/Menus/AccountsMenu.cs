using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                new Option("Главное меню", () => menuAction = MenuActions.Back)
            };

            menuAction = MenuActions.Show;
        }

        /// <summary> Показывает меню. </summary>
        public override void Show()
        {
            while (true)
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

        /// <summary> Показывает список аккаунтов. </summary>
        private void ShowAccounts()
        {
            Console.Clear();

            DataTable accounts = MyDataBase.GetTable("SELECT * FROM Account;");

            for(int i = 0; i < accounts.Rows.Count; i++)
                Console.WriteLine($"{i + 1}. {accounts.Rows[i][0].ToString()} - {accounts.Rows[i][1].ToString()} - {accounts.Rows[i][2].ToString()}");

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

            MyDataBase.ExecuteQueryWithoutAnswer($"INSERT INTO Account VALUES ('{accountName}', '{accountBalance}', '{accountCurrency}');");

            Console.WriteLine("Аккаунт успешно добавлен.");
            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
            Console.ReadKey();
        }

        /// <summary> Показывает меню для удаления аккаунта. </summary>
        private void ShowDeleteAccountMenu()
        {
            Console.Clear();
          
            DataTable accounts = MyDataBase.GetTable("SELECT * FROM Account;");

            for (int i = 0; i < accounts.Rows.Count; i++)
                Console.WriteLine($"{i + 1}. {accounts.Rows[i][0].ToString()} - {accounts.Rows[i][1].ToString()} - {accounts.Rows[i][2].ToString()}");

            Console.WriteLine("Введите имя счёта, который нужно удалить.");
            string accountName = Console.ReadLine();

            MyDataBase.ExecuteQueryWithoutAnswer($"DELETE FROM Account WHERE name = '{accountName}';");

            Console.WriteLine("Аккаунт успешно удалён.");
            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
            Console.ReadKey();
        }
    }
}
