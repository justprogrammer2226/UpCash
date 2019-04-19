using System;
using System.Collections.Generic;

namespace UpCash.Menus
{
    internal abstract class AMenu
    {
        /// <summary> Заголовок меню. </summary>
        /// <remarks> Заголовок меню будет отображаться при показе меню. </remarks>
        public string Title { get; protected set; }

        /// <summary> Список опций меню. </summary>
        public List<Option> Options { get; protected set; }

        /// <summary> Действие, которое должно выполнить меню сейчас. </summary>
        public MenuActions MenuAction { get; protected set; }

        /// <summary> Показать меню. </summary>
        public virtual void Show()
        {
            MenuAction = MenuActions.Show;
            while (true)
            {
                if (MenuAction == MenuActions.Show)
                {
                    Console.Clear();

                    if (!string.IsNullOrEmpty(Title))
                    {
                        Console.WriteLine(Title);
                    }

                    for (int i = 0; i < Options.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Options[i].Name}");
                    }

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
                else if (MenuAction == MenuActions.Back)
                {
                    break;
                }
            }
        }

        /// <summary> Устанавливает заголовок для меню. </summary>
        /// <param name="title"> Заголовок меню. </param>
        public void SetTitle(string title)
        {
            Title = title;
        }
    }
}
