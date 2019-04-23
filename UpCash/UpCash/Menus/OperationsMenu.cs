using ConsoleHelper;
using System;
using System.Collections.Generic;
using System.Data;
using DateTimeH;
using System.Data.SQLite;

namespace UpCash.Menus
{
    /// <summary> Меню операций, служит для просмотра и произведения различных денежных операций. </summary>
    internal class OperationsMenu : AMenu
    {
        private static OperationsMenu _instance;

        private OperationsMenu()
        {
            Options = new List<Option>()
            {
                new Option("Просмотреть все операции", ShowListOfOperations),
                new Option("Произвести операцию", AddOperation),
                new Option("Удалить операцию", DeleteOperation),
                new Option("Главное меню", () => MenuAction = MenuActions.Back)
            };
        }

        public static OperationsMenu GetMenu()
        {
            if (_instance == null) _instance = new OperationsMenu();
            return _instance;
        }

        /// <summary> Показывает меню списка операций. </summary>
        private void ShowListOfOperations()
        {
            Console.Clear();
            Console.WriteLine("Список операций:");
            ShowAllOperations();
            ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
        }

        /// <summary> Выводит все операции. </summary>
        private void ShowAllOperations()
        {
            DataTable operations = MyDataBase.GetDB().GetTable("SELECT id, accountName, value, item, sub_item, date FROM Operation WHERE item IS NOT NULL OR sub_item IS NOT NULL;");

            for (int i = 0; i < operations.Rows.Count; i++)
            {
                if(operations.Rows[i]["item"] != null)
                {
                    Console.WriteLine($"{i + 1}. {operations.Rows[i]["id"].ToString()} | {operations.Rows[i]["accountName"].ToString()} | {operations.Rows[i]["value"].ToString()} | {operations.Rows[i]["item"].ToString()} | {operations.Rows[i]["date"].ToString()}");
                }
                else if (operations.Rows[i]["item"] == null && operations.Rows[i]["sub_item"] != null)
                {
                    Console.WriteLine($"{i + 1}. {operations.Rows[i]["id"].ToString()} | {operations.Rows[i]["accountName"].ToString()} | {operations.Rows[i]["value"].ToString()} | {operations.Rows[i]["sub_item"].ToString()} | {operations.Rows[i]["date"].ToString()}");
                }
            }
        }

        /// <summary> Добавляет операцию. </summary>
        private void AddOperation()
        {
            Console.Clear();

            string accountName = GetAccountName();
            double value = GetOperationValue();
            string item = GetItemName();
            string typeItem = null;
            bool isSubItem = false;

            // Проверяем, есть ли эта статья в списке главных статей
            // Если пунктов нет, то вернёт объект, с 0 строк, а не null.
            DataTable items = MyDataBase.GetDB().GetTable($"SELECT name_item, type_item FROM Item;");
            for (int i = 0; i < items.Rows.Count; i++)
            {
                if (items.Rows[i]["name_item"].ToString() == item)
                {
                    typeItem = items.Rows[i]["type_item"].ToString();
                }
            }

            // Если статья не была найдена в списке главных статей, то ищем её в вложеных статьях
            if(typeItem == null)
            {
                // Если подпунктов нет, то вернёт объект, с 0 строк, а не null.
                DataTable subItems = MyDataBase.GetDB().GetTable($"SELECT name_sub_item, name_item FROM SubItem;");
                for (int i = 0; i < subItems.Rows.Count; i++)
                {
                    if (subItems.Rows[i]["name_item"].ToString() == item)
                    {
                        isSubItem = true;
                        typeItem = MyDataBase.GetDB().ExecuteQueryWithAnswer($"SELECT type_item FROM Item = WHERE name_item = '{item}'");
                    }
                }
            }

            try
            {
                if (typeItem == null) throw new NullReferenceException("Тип статьи должен иметь значение.");
                else
                {
                    if(isSubItem)
                    {
                        MyDataBase.GetDB().ExecuteQueryWithoutAnswer("INSERT INTO Operation (accountName, value, sub_item, date) VALUES" +
                            $"('{accountName}', {value}, '{item}', '{DateTimeHelper.GetDateNow('/')}');");
                    }
                    else
                    {
                        MyDataBase.GetDB().ExecuteQueryWithoutAnswer("INSERT INTO Operation (accountName, value, item, date) VALUES" +
                            $"('{accountName}', {value}, '{item}', '{DateTimeHelper.GetDateNow('/')}');");
                    }

                    double balance = double.Parse(MyDataBase.GetDB().ExecuteQueryWithAnswer($"SELECT balance FROM Account WHERE name = '{accountName}';"));
                    double newBalance = typeItem == "Доход" ? balance + value : balance - value;

                    MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"UPDATE Account SET balance = {newBalance} WHERE name = '{accountName}';");

                    Console.WriteLine("Операция успешно произведена.");
                    ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Операция не была произведена. Вероятно вы ввели имя не существующего статьи.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Операция не была произведена. Вероятно вы ввели имя не существующего счёта.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            }
        }

        /// <summary> Удаляет операцию. </summary>
        private void DeleteOperation()
        {
            Console.Clear();
            MyDataBase.GetDB().ExecuteQueryWithoutAnswer($"DELETE FROM Operation WHERE id = '{GetIdOperation()}';");
            Console.WriteLine("Операция успешно удалена.");
            ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
        }

        /// <summary> Возращает имя счёта, введённого пользователем. </summary>
        private string GetAccountName()
        {
            return ConsoleInput.GetInput("Введите имя счёта.", returnValidInput: true, action: () =>
            {
                Console.WriteLine("Список счётов:");
                AccountsMenu.GetMenu().ShowAccounts();
                Console.WriteLine();
            }, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидное имя счёта");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }

        /// <summary> Возращает количество денег для операции, введённого пользователем. </summary>
        private double GetOperationValue()
        {
            return ConsoleInput.GetInput("Введите количество денег, на которое хотите изменить счёт.", null, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидное количество денег (больше 0).");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }

        /// <summary> Возращает имя счёта, введённого пользователем. </summary>
        private string GetItemName()
        {
            return ConsoleInput.GetInput("Введите имя статьи дохода или расхода, по которой будет производиться операция..", returnValidInput: true, action: () =>
            {
                Console.WriteLine($"Статьи типа доход:");
                ItemsMenu.GetMenu().ShowItems("Доход");
                Console.WriteLine($"Статьи типа расход:");
                ItemsMenu.GetMenu().ShowItems("Расход");
                Console.WriteLine();
            }, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидное название статьи.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }

        /// <summary> Возращает баланс счёта, введённого пользователем. </summary>
        private int GetIdOperation()
        {
            return (int)ConsoleInput.GetInput("Введите id операции, которую нужно удалить.", action: () =>
            {
                Console.WriteLine("Список операций:");
                ShowAllOperations();
            }, errorNotification: () =>
            {
                Console.WriteLine("Вы должны ввести валидный id операции.");
                ConsoleOutput.PressAnyKeyToContinue("Нажмите любую клавишу, что б закрыть это меню.");
            });
        }
    }
}
