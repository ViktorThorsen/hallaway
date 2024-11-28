using System.Diagnostics;

namespace hallawayApp;

public class Menu
{
    public void CallMainMenu()
    {
        Console.Clear();
        Console.WriteLine($"=========================== " +
                          $"Welcome to Hallaway!" +
                          $" =========================== " +
                          $"\n1) Add a party \n2) Pick a date \n3) Select destination \n4) View order details \n5) Done \n0) Quit");
        int input = Int32.Parse(Console.ReadLine());
        Debug.Assert(input != null);

        switch (input)
        {
            case 1:
                Party party = new Party();
                break;
            case 2:
                // Pick a date
                break;
            case 3:
                // Select destination
                break;
            case 4:
                // Order details
                break;
            case 5:
                // Confirm order
                break;
            case 0:
                Console.WriteLine("Goodbye!"); // Quit!
                break;
            default:
                break;
        }
    }
}