namespace hallawayApp;

public class HotelManager
{
    private DatabaseActions _databaseActions;
    private List<Hotel> hotelList;
    public HotelManager(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
    }
    public async Task FindHotelMenu()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine(
                $"Menu> OrderMenu> PartyMenu" +
                $"\n---------------------------" +
                $"\n1) Show all hotels \n2) Add Already Registered Person to party \n3) Delete person from party\n4) Done \n10) Sort hotels by pool \n0) Quit");
            Console.WriteLine("\nEnter your choice: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 4.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    hotelList = await _databaseActions.GetAllHotels();
                    ShowHotels();
                    break;
                case 2:
                
                    break;
                case 3:
                
                    break;
                case 4:
               
                    running = false; // Exit the menu loop
                    break;
                case 10:
                    SelectHotels(1);
                    
                    break;
                case 11:
                    // hotelList = await SortHotelsByRestaurant(); 
                    ShowHotels();
                    break;
                case 12: 
                    // hotelList = await SortHotelsByRating(); 
                    ShowHotels();
                    break;
                case 13:
                    // hotelList = await SortHotelsByDistanceCityCenter(); 
                    ShowHotels();
                    break;
                case 14:
                    // hotelList = await SortHotelsByEveningEntertainment(); 
                    ShowHotels();
                    break;
                    
                case 0:
                    Console.WriteLine("Goodbye!");
                 
                    running = false; // Exit the menu loop
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a valid menu option.");
                    break;
            }
        }
    }

    private void ShowHotels()
    {
        foreach (Hotel hotel in hotelList)
        {
            Console.WriteLine($"{hotel.hotelName}");
        }

        Console.ReadLine();
    }

    private void SelectHotels(int index)
    {
        foreach (var hotel in hotelList)
        {
            if ( == index)
            {
                // return hotel;
                
                Console.WriteLine($"{hotel.hotelName}");
            }
            else
            {
                Console.WriteLine("No available hotels");
            }
        }
    }
}
        
        

       
    
    
