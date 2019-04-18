using System;

namespace UpCash.Menus
{
    /// <summary> Опция меню. </summary>
    internal struct Option
    {
        /// <summary> Имя опции. </summary>
        public string Name { get; }

        /// <summary> Действие, которое будет выполняться, при выборе данной опции. </summary>
        public Action Action { get; }

        public Option(string optionName, Action action)
        {
            Name = optionName;
            Action = action;
        }
    }
}
