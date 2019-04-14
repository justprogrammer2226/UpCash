using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpCash.Menus
{
    /// <summary> Меню статей, служит для настройки статей расхода и дохода. </summary>
    internal class ItemsMenu : AMenu
    {
        public ItemsMenu(string title = null)
        {
            Title = title;

            Options = new List<Option>()
            {
                new Option("Посмотреть статьи доходов", () => ShowItems("Доход")),
                new Option("Посмотреть статьи расходов", () => ShowItems("Расход")),
                new Option("Добавить статью дохода", () => ShowAddItemMenu("Доход")),
                new Option("Добавить статью расхода", () => ShowAddItemMenu("Расход")),
                new Option("Удалить статью", ShowDeleteItemMenu),
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

        /// <summary> Показывает статьи типа type. </summary>
        /// <param name="type"> Тип статьи, которую нужно показать. </param>
        private void ShowItems(string type)
        {
            Console.Clear();
            Console.WriteLine($"Статьи типа {type.ToLower()}:");

            // Если пунктов нет, то вернёт объект, с 0 строк, а не null.
            DataTable items = MyDataBase.GetTable($"SELECT * FROM Item WHERE type_item = '{type}';");

            for (int i = 0; i < items.Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {items.Rows[i][0].ToString()}");

                // Если подпунктов нет, то вернёт объект, с 0 строк, а не null.
                DataTable subItems = MyDataBase.GetTable($"SELECT * FROM SubItem WHERE name_item = '{items.Rows[i][0].ToString()}';");

                for (int j = 0; j < subItems.Rows.Count; j++)
                    Console.WriteLine($"  {i + 1}.{j + 1}. {subItems.Rows[j][0].ToString()}");
            }

            Console.WriteLine("Нажмите любую клавишу, что б вернуться.");
            Console.ReadKey();
        }

        /// <summary> Открывает меню добавление элемента в статью типа type. </summary>
        /// <param name="type"> Тип статьи, в которую нужно добавить элемент. </param>
        private void ShowAddItemMenu(string type)
        {
            Console.Clear();
            Console.WriteLine($"Введите имя статьи типа {type.ToLower()} которую хотите добавить.");

            string itemName = Console.ReadLine();

            Console.WriteLine("Если вы хотите, что б эта статья была вложеной, введите имя главной статьи, иначе просто нажмите Enter.");

            string mainItemName = Console.ReadLine();

            if (string.IsNullOrEmpty(mainItemName))
            {
                MyDataBase.ExecuteQueryWithoutAnswer($"INSERT INTO Item VALUES ('{itemName}', '{type}');");
            }
            else
            {
                string typeOfMainItem = MyDataBase.ExecuteQueryWithAnswer($"SELECT type_item FROM Item WHERE name_item = '{mainItemName}'");

                if (MyDataBase.ExecuteQueryWithAnswer($"SELECT type_item FROM Item WHERE name_item = '{mainItemName}'") == type)
                {
                    MyDataBase.ExecuteQueryWithoutAnswer($"INSERT INTO SubItem VALUES ('{itemName}', '{mainItemName}');");
                }
                else if (typeOfMainItem == null)
                {
                    Console.WriteLine("Данной главной статьи не существует, введите статью которая существует.");
                    Console.WriteLine("Нажмите любую клавишу, что б вернуться в предыдущее меню..");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine($"Вы собираетесь добавить статью типа {type.ToUpper()}, но в качестве главного элемента указали статью другого типа.");
                    Console.WriteLine("Нажмите любую клавишу, что б вернуться в предыдущее меню..");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine($"Статья типа {type.ToLower()} успешно добавлена.");
            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
            Console.ReadKey();
        }

        /// <summary> Поазывает меню удаления удаления статьи. </summary>
        private void ShowDeleteItemMenu()
        {
            Console.Clear();
            Console.WriteLine($"Введите имя статьи которую хотите удалить.");

            string itemName = Console.ReadLine();

            bool isItemExist = MyDataBase.ExecuteQueryWithAnswer($"SELECT name_item FROM Item WHERE name_item = '{itemName}';") != null ? true : false;

            if (isItemExist)
            {
                Console.WriteLine($"Данная статья являеться главной, если она имеет подпункты, они удаляться вместе с ней. Если вы согласны, напишите +, иначе напишите Enter.");

                string input = Console.ReadLine();

                if (input == "+")
                {
                    MyDataBase.ExecuteQueryWithoutAnswer($"DELETE FROM SubItem WHERE name_item = '{itemName}'");
                    MyDataBase.ExecuteQueryWithoutAnswer($"DELETE FROM Item WHERE name_item = '{itemName}'");
                }
                else
                {
                    return;
                }
            }
            else
            {
                bool isSubItemExist = MyDataBase.ExecuteQueryWithAnswer($"SELECT name_sub_item FROM SubItem WHERE name_sub_item = '{itemName}';") != null ? true : false;

                if (isSubItemExist)
                {
                    MyDataBase.ExecuteQueryWithoutAnswer($"DELETE FROM SubItem WHERE name_item = '{itemName}'");
                }
                else
                {
                    Console.WriteLine("Данной статьи не существует.");
                    Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine($"Статья {itemName} успешно удалена.");
            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
            Console.ReadKey();
        }
    }
}
