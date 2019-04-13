using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
