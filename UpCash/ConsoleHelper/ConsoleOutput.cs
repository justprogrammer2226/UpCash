using System;

namespace ConsoleHelper
{
    public static class ConsoleOutput
    {
        public static void PressAnyKeyToContinue(string title = "Нажмите любую клавишу, что б продолжить.")
        {
            if(title != null) Console.WriteLine(title);
            Console.ReadKey();
        }
    }
}
