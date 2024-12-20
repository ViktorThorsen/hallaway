namespace hallawayApp;

public class AdminControl
{
    private DatabaseActions _databaseActions;

    public AdminControl(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
    }
    public async Task EditOrderMenu()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine(
                $"Menu> RemoveOrder" +
                $"\n---------------------------" +
                $"\n1) Delete Order \n2) Delete Person From Order \n0) Done");
            Console.WriteLine("\nEnter your choice: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 1.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                continue;
            }

            switch (choice)
            {
                case 1: 
                    Console.WriteLine("Please enter an order ID to remove order:");
                    if (int.TryParse(Console.ReadLine(), out int orderID))
                    {
                        await _databaseActions.RemoveOrder(orderID); 
                        

                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                    } 
                    else 
                    {
                        Console.WriteLine("Invalid order ID. Please enter a valid number."); 
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine(); 
                    } 
                    break;
                case 2:
                    Console.WriteLine("Please enter an order ID to edit party in order:");
                    if (int.TryParse(Console.ReadLine(), out int order))
                    {

                        await _databaseActions.EditPartyByOrder(order);
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();
                    } 
                    else 
                    {
                        Console.WriteLine("Invalid order ID. Please enter a valid number."); 
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine(); 
                    } 
                    break;
                case 0:
                    running = false;
                    break;

                default:
                    Console.WriteLine("Invalid option. Please choose a valid menu option.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }
    
}