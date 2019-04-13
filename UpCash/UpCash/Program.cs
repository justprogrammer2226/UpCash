using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpCash.Menus;

namespace UpCash
{
    class Program
    {
        static void Main(string[] args)
        {
            AMenu mainMenu = new MainMenu();
            mainMenu.Show();
        }
    }
}
