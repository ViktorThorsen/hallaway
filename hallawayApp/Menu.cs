using System.Diagnostics;
using System.Net.Mime;

namespace hallawayApp;

public class Menu
{
    private DatabaseActions _databaseActions;
    
    public Menu(DatabaseActions databaseActions)
    {
        this._databaseActions = databaseActions;
    }

    public async Task ShowMainMenu(int adminId)
    {
        Console.Clear();
        Console.WriteLine(
            $"Menu" +
            $"\n--------------------" +
            $"\n1) Create new Order \n2)" +
            $" Edit Registered Persons \n3) Edit Hotels \n0 Quit");
        Console.WriteLine("\nEnter your choice: ");
        int input = Int32.Parse(Console.ReadLine());
        Debug.Assert(input != null);

        switch (input)
        {
            case 1:
                Order order = new Order(_databaseActions);
                await order.CreateOrder(adminId);
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

    public async Task<int> ShowAdminMenu()
    {
        List<Admin> admins = await _databaseActions.GetAllAdmin();

        int halfCount = (admins.Count + 1) / 2;
        for (int i = 0; i < halfCount; i++)
        {
            string firstColumn = $"{i + 1}) {admins[i].name}";
            string secondColumn = (i + halfCount < admins.Count)
                ? $"{i + 1 + halfCount}) {admins[i + halfCount].name}"
                : ""; // Handle the case when the number of persons is odd
            Console.WriteLine($"{firstColumn,-30} {secondColumn}");

        }

        Console.WriteLine("0) Cancel");

        while (true)
        {
            Console.Write($"\nChoose your admin profile: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                continue;
            }

            if (choice == 0)
            {
                Console.WriteLine("Selection canceled.");
                return 0; // Exit function on cancel
            }

            if (choice < 1 || choice > admins.Count)
            {
                Console.WriteLine(
                    $"Invalid choice. Please select a number between 1 and {admins.Count}, or 0 to cancel.");
                continue;
            }

            // Calculate the corresponding user_id
            int adminId = choice; // Since the list is ordered by user_id and 1-based indexing is used
            Admin selectedAdmin = admins[choice - 1];

            Console.WriteLine($"You selected: {selectedAdmin.name}");
            return choice;
            break; // Exit the loop after a valid choice

        }
    }
}
   
    