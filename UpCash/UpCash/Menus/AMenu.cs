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
        public MenuActions menuAction { get; protected set; }

        /// <summary> Показать меню. </summary>
        public abstract void Show();
    }
}
