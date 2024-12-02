using System.Diagnostics;

namespace hallawayApp;

public class Menu
{
    private DatabaseActions _databaseActions;
    public async Task CallMainMenu(DatabaseActions databaseActions)
    {
        this._databaseActions = databaseActions;
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
                string hotelName = "Lugnets Retreat";
                Hotel hotelTest = await databaseActions.GetHotel(hotelName);
                
                Console.WriteLine(hotelTest.address.Street.ToString());
                break;
            case 0:
                Console.WriteLine("Goodbye!"); // Quit!
                break;
            default:
                break;
        }
    }
    
    }
   
    