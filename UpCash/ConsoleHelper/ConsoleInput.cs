using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelper
{
    public static class ConsoleInput
    {
        public static string GetInput(string title, bool returnValidInput = true, Action action = null, Action errorNotification = null)
        {
            string input;
            do
            {
                // Очищаем консоль и выполняем action, если он есть.
                Console.Clear();
                action?.Invoke();

                // Выводим заголовок, если он есть и принимаем ввод пользователя.
                if(title != null) Console.WriteLine(title);
                input = Console.ReadLine();

                // Если мы должны вернуть валидные данные...
                if (returnValidInput)
                {
                    // ...мы проверяем строку на валидность.
                    // Если строка валидная, прекращаем ввод, иначе выводим сообщение о ошибке.
                    if (IsValidInput(input)) break;
                    else errorNotification?.Invoke();
                }
            } while (returnValidInput);
            return input; 
        }

        /// <summary> Проверяет строку input на валидность. </summary>
        /// <param name="input"> Строка, которую нужно проверить на валидность. </param>
        /// <returns> Возращает true, если строка input валидная, иначе false. </returns>
        public static bool IsValidInput(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        //private static string GetNameCurrency()
        //{
        //    string nameCurrency;
        //    while (true)
        //    {
        //        Console.Clear();
        //        Console.WriteLine("Список валют:");
        //        ShowCurrencies();

        //        Console.WriteLine("Введите имя валюты.");
        //        nameCurrency = Console.ReadLine();

        //        if (IsValidNameCurrency(nameCurrency)) break;
        //        else
        //        {
        //            Console.WriteLine("Вы должны ввести валидное имя валюты.");
        //            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
        //            Console.ReadKey();
        //        }
        //    }
        //    return nameCurrency;
        //}

        //private double GetAccounBalance()
        //{
        //    string accountBalance;
        //    double balance;
        //    while (true)
        //    {
        //        Console.Clear();

        //        Console.WriteLine("Список аккаунтов:");
        //        ShowAccounts();
        //        Console.WriteLine();

        //        Console.WriteLine("Введите баланс счёта.");
        //        accountBalance = Console.ReadLine();

        //        if (IsValidAccountBalance(accountBalance, out balance)) break;
        //        else
        //        {
        //            Console.WriteLine("Вы ввели некорректный баланс счёта.");
        //            Console.WriteLine("Нажмите любую клавишу, что б закрыть это меню.");
        //            Console.ReadKey();
        //        }
        //    }
        //    return balance;
        //}

        //private static bool IsValidCodeCurrency(string codeCurrency)
        //{
        //    return !string.IsNullOrEmpty(codeCurrency);
        //}

        //private bool IsValidAccountBalance(string accountBalance, out double balance)
        //{
        //    return double.TryParse(accountBalance, out balance);
        //}
    }
}
