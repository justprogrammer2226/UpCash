using System;

namespace ConsoleHelper
{
    public static class ConsoleInput
    {
        /// <summary> Обрабатывает ввод пользователя. </summary>
        /// <param name="title"> Заголовок, который будет отображено перед запросом на ввод. </param>
        /// <param name="returnValidInput"> Если true, то метод будет выполняться до тех пор, пока не будет введено валидное значение.
        /// Если false, метод приймет только 1 ввод пользователя, не зависимо от того, коректный он или нет. </param>
        /// <param name="action"> Действие, которое нужно выполнить, до отображение заголовка. </param>
        /// <param name="errorNotification"> Действие, которое нужно выполнить, при вводе невалидных данных. </param>
        /// <returns></returns>
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

        /// <summary> Обрабатывает ввод пользователя. </summary>
        /// <param name="title"> Заголовок, который будет отображено перед запросом на ввод. </param>
        /// <param name="action"> Действие, которое нужно выполнить, до отображение заголовка. </param>
        /// <param name="errorNotification"> Действие, которое нужно выполнить, при вводе невалидных данных. </param>
        /// <returns></returns>
        public static double GetInput(string title, Action action = null, Action errorNotification = null)
        {
            double result;
            string input;
            while(true)
            {
                // Очищаем консоль и выполняем action, если он есть.
                Console.Clear();
                action?.Invoke();

                // Выводим заголовок, если он есть и принимаем ввод пользователя.
                if (title != null) Console.WriteLine(title);
                input = Console.ReadLine();

                // Мы проверяем строку на валидность.
                // Если строка валидная, прекращаем ввод, иначе выводим сообщение о ошибке.
                if (IsValidInput(input, out result)) break;
                else errorNotification?.Invoke();
            }
            return result;
        }

        /// <summary> Проверяет строку input на валидность и, если строка валидная, число с неё будет в result. </summary>
        /// <param name="input"> Строка, которую нужно проверить на валидность. </param>
        /// <param name="result"> Если строка будет валидной, число из неё запишиться в эту переменную. </param>
        /// <returns> Возращает true, если строка input конвертируеться в double. </returns>
        public static bool IsValidInput(string input, out double result)
        {
            return double.TryParse(input, out result);
        }
    }
}
