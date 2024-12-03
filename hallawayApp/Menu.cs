using System.Diagnostics;

namespace hallawayApp;

public class Menu
{
    private DatabaseActions _databaseActions;
    
    public Menu(DatabaseActions databaseActions)
    {
        this._databaseActions = databaseActions;
        ShowAdminMenu();
        //ShowMainMenu();

    }

    public async Task ShowMainMenu()
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
                await order.CreateOrder();
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

    public async Task ShowAdminMenu()
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
        Console.ReadLine();
    }
    }
   
    