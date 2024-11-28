using System.Diagnostics;

namespace hallawayApp;

public class Menu
{
    public void CallMainMenu(DatabaseActions databaseActions)
    {
        Console.WriteLine($"===========================" +
                          $"Welcome to Hallaway!" +
                          $"===========================" +
                          $"\n1) Create new Order \n2) Edit Registered Persons \n3) Edit Hotels \n0 Quit");
        int input = Int32.Parse(Console.ReadLine());
        Debug.Assert(input != null);

        switch (input)
        {
            case 1:
                Order order = new Order();
                break;
            case 2:
                
                break;
            case 3:
                
                break;
            case 0:
                Console.WriteLine("Goodbye!"); // Quit!
                break;
            default:
                break;
        }
    }
}