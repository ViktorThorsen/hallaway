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
                    hotelList = await SortHotelsByPool();
                    ShowHotels();
                    break;
                case 11:
                    hotelList = await SortHotelsByRestaurant();
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
                    hotelList = await SortHotelsByEveningEntertainment();
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
    
    private async Task<List<Hotel>> SortHotelsByPool() 
    { 
        var allHotels = await _databaseActions.GetAllHotels(); 
        var hotelsWithPool = new List<Hotel>();

        foreach (var hotel in allHotels)
        {
            if (hotel.pool)
            {
                hotelsWithPool.Add(hotel);
            }
        } 
        return hotelsWithPool; 
    }

    private async Task<List<Hotel>>  SortHotelsByRestaurant()
    {
        var allHotels = await _databaseActions.GetAllHotels();
        var hotelsWithRestaurant = new List<Hotel>();

        foreach (var hotel in allHotels)
        {
            if (hotel.restaurante)
            {
                hotelsWithRestaurant.Add(hotel);
            }
            return hotelsWithRestaurant;
        }

        return null;
    }

    

  
    private async Task<List<Hotel>> SortHotelsByEveningEntertainment()
    {
        var allHotels = await _databaseActions.GetAllHotels();
        var hotelsWithEveningEntertainment = new List<Hotel>();

        foreach (var hotel in allHotels)
        {
            if (hotel.eveningEntertainment)
            {
                hotelsWithEveningEntertainment.Add(hotel);
            }
            return hotelsWithEveningEntertainment;  
        }
        return null;
    }

}
        
        

       
    
    
