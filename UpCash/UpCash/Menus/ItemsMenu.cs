using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using ConsoleHelper;

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
                new Option("Посмотреть статьи доходов", () => ShowListOfItems("Доход")),
                new Option("Посмотреть статьи расходов", () => ShowListOfItems("Расход")),
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
        private void ShowListOfItems(string type)
        {
            Console.Clear();
            Console.WriteLine($"Статьи типа {type.ToLower()}:");
            ShowItems(type);
            ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
        }

        /// <summary> Показывает все статьи типа type. </summary>
        /// <param name="type"> Тип статей, которые нужно отобразить. </param>
        public void ShowItems(string type)
        {
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
        }

        /// <summary> Открывает меню добавление элемента в статью типа type. </summary>
        /// <param name="type"> Тип статьи, в которую нужно добавить элемент. </param>
        private void ShowAddItemMenu(string type)
        {
            Console.Clear();
            Console.WriteLine($"Введите имя статьи типа {type.ToLower()} которую хотите добавить.");

            string itemName = GetItemName();

            Console.WriteLine("Если вы хотите, что б эта статья была вложеной, введите имя главной статьи, иначе просто нажмите Enter.");

            string mainItemName = Console.ReadLine();

            if (string.IsNullOrEmpty(mainItemName))
            {
                try
                {
                    MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"INSERT INTO Item VALUES ('{itemName}', '{type}');");
                    Console.WriteLine("Статья успешно добавлена.");
                    ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
                    return;
                }
                catch (SQLiteException)
                {
                    Console.WriteLine("Статья не была добавлена. Вероятно вы ввели имя существующей статьи.");
                    ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
                }
            }
            else
            {
                string typeOfMainItem = MyDataBase.GetDB().ExecuteQueryWithAnswer($"SELECT type_item FROM Item WHERE name_item = '{mainItemName}'");

                if (typeOfMainItem == type)
                {
                    try
                    {
                        MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"INSERT INTO SubItem VALUES ('{itemName}', '{mainItemName}');");
                        Console.WriteLine("Статья успешно добавлена.");
                        ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
                        return;
                    }
                    catch (SQLiteException)
                    {
                        Console.WriteLine("Статья не была добавлена. Вероятно вы ввели имя существующей статьи.");
                        ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
                    }
                }
                else if (typeOfMainItem == null)
                {
                    Console.WriteLine("Данной главной статьи не существует, введите статью которая существует.");
                    ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
                    return;
                }
                else
                {
                    Console.WriteLine($"Вы собираетесь добавить статью типа {type.ToUpper()}, но в качестве главного элемента указали статью другого типа.");
                    ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
                    return;
                }
            }
        }

        /// <summary> Поазывает меню удаления удаления статьи. </summary>
        private void ShowDeleteItemMenu()
        {
            Console.Clear();
            Console.WriteLine($"Введите имя статьи которую хотите удалить.");

            string itemName = GetItemName();

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
                    ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
                    return;
                }
            }

            Console.WriteLine($"Статья {itemName} успешно удалена.");
            ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
        }

        private string GetItemName()
        {
            return ConsoleInput.GetInput("Введите имя статьи.", returnValidInput: true, action: () =>
            {
                Console.WriteLine($"Статьи типа доход:");
                ShowItems("Доход");
                Console.WriteLine($"Статьи типа расход:");
                ShowItems("Расход");
                Console.WriteLine();
            }, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидное имя статьи.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }
    }
}
