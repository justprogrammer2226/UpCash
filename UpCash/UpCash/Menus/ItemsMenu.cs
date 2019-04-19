using System;
using System.Collections.Generic;
using System.Data;

namespace UpCash.Menus
{
    /// <summary> Меню статей, служит для настройки статей расхода и дохода. </summary>
    internal class ItemsMenu : AMenu
    {
        private static ItemsMenu _instance;

        private ItemsMenu()
        {
            Options = new List<Option>()
            {
                new Option("Посмотреть статьи доходов", () => ShowItems("Доход")),
                new Option("Посмотреть статьи расходов", () => ShowItems("Расход")),
                new Option("Добавить статью дохода", () => ShowAddItemMenu("Доход")),
                new Option("Добавить статью расхода", () => ShowAddItemMenu("Расход")),
                new Option("Удалить статью", ShowDeleteItemMenu),
                new Option("Главное меню", () => MenuAction = MenuActions.Back)
            };
        }

        public static ItemsMenu GetMenu()
        {
            if (_instance == null) _instance = new ItemsMenu();
            return _instance;
        }

        /// <summary> Показывает статьи типа type. </summary>
        /// <param name="type"> Тип статьи, которую нужно показать. </param>
        private void ShowItems(string type)
        {
            Console.Clear();
            Console.WriteLine($"Статьи типа {type.ToLower()}:");

            // Если пунктов нет, то вернёт объект, с 0 строк, а не null.
            DataTable items = MyDataBase.GetDB().GetTable($"SELECT name_item FROM Item WHERE type_item = '{type}';");

            for (int i = 0; i < items.Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {items.Rows[i]["name_item"].ToString()}");

                // Если подпунктов нет, то вернёт объект, с 0 строк, а не null.
                DataTable subItems = MyDataBase.GetDB().GetTable($"SELECT name_sub_item FROM SubItem WHERE name_item = '{items.Rows[i]["name_item"].ToString()}';");

                for (int j = 0; j < subItems.Rows.Count; j++)
                {
                    Console.WriteLine($"  {i + 1}.{j + 1}. {subItems.Rows[j]["name_sub_item"].ToString()}");
                }
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
                MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"INSERT INTO Item VALUES ('{itemName}', '{type}');");
            }
            else
            {
                string typeOfMainItem = MyDataBase.GetDB().ExecuteQueryWithAnswer($"SELECT type_item FROM Item WHERE name_item = '{mainItemName}'");

                if (MyDataBase.GetDB().ExecuteQueryWithAnswer($"SELECT type_item FROM Item WHERE name_item = '{mainItemName}'") == type)
                {
                    MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"INSERT INTO SubItem VALUES ('{itemName}', '{mainItemName}');");
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

            bool isItemExist = MyDataBase.GetDB().ExecuteQueryWithAnswer($"SELECT name_item FROM Item WHERE name_item = '{itemName}';") != null ? true : false;

            if (isItemExist)
            {
                Console.WriteLine($"Данная статья являеться главной, если она имеет подпункты, они удаляться вместе с ней. Если вы согласны, напишите +, иначе напишите Enter.");

                string input = Console.ReadLine();

                if (input == "+")
                {
                    MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"DELETE FROM SubItem WHERE name_item = '{itemName}'");
                    MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"DELETE FROM Item WHERE name_item = '{itemName}'");
                }
                else
                {
                    return;
                }
            }
            else
            {
                bool isSubItemExist = MyDataBase.GetDB().ExecuteQueryWithAnswer($"SELECT name_sub_item FROM SubItem WHERE name_sub_item = '{itemName}';") != null ? true : false;

                if (isSubItemExist)
                {
                    MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"DELETE FROM SubItem WHERE name_item = '{itemName}'");
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
