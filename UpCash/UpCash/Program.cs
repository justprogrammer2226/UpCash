using UpCash.Menus;

namespace UpCash
{
    class Program
    {
        static void Main(string[] args)
        {
            AMenu mainMenu = MainMenu.GetMenu();
            mainMenu.Show();
        }
    }
}
