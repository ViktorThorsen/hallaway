using System.Security.Authentication;

namespace hallawayApp;

public class HotelManager
{
    private DatabaseActions _databaseActions;
    private List<Hotel> hotelList;
    private string city;
    private int? distancetobeach;
    private int? rating;
    private bool? hasPool;

    public HotelManager(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
    }

    public async Task FindHotelMenu()
    {
        bool running = true;
        hotelList = await _databaseActions.GetAllHotels();

        while (running)
        {
            Console.Clear();
            Console.WriteLine(
                $"Menu> OrderMenu> DestinationMenu" +
                $"\n---------------------------" +
                $"\n1) Show all hotels \n2) Filter on city \n3) Filter on distance to beach" +
                $"\n4) Filter on rating \n5) Filter on pool availability \n6) Clear filters \n0) Return");
            Console.WriteLine("\nEnter your choice: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 6.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ShowHotels();
                    break;
                case 2:
                    city = FilterOnCity();
                    break;
                case 3:
                    distancetobeach = FilterOnDistanceToBeach();
                    break;
                case 4:
                    rating = FilterOnRating();
                    break;
                case 5:
                    hasPool = FilterOnPool();
                    break;
                case 6:
                    ClearFilters();
                    break;
                case 0:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a valid menu option.");
                    break;
            }
        }
    }

    private void ShowHotels()
    {
        Console.Clear();

        var filteredHotels = hotelList.Where(hotel =>
            (string.IsNullOrEmpty(city) || hotel.address.City.Equals(city, StringComparison.OrdinalIgnoreCase)) &&
            (!distancetobeach.HasValue || hotel.distanceBeach <= distancetobeach.Value) &&
            (!rating.HasValue || (int)hotel.ratingEnum >= rating.Value) &&
            (!hasPool.HasValue || hotel.pool == hasPool.Value)
        ).ToList();

        if (!filteredHotels.Any())
        {
            Console.WriteLine("No hotels match the current filters.");
        }
        else
        {
            foreach (var hotel in filteredHotels)
            {
                Console.WriteLine($"{hotel.hotelID}) {hotel.hotelName} - City: {hotel.address.City}, Distance to beach: {hotel.distanceBeach} meters, Rating: {hotel.ratingEnum}, Pool: {hotel.pool}");
            }
        }

        Console.WriteLine("\nPress enter to continue");
        Console.ReadLine();
    }

    public string FilterOnCity()
    {
        Console.Clear();
        Console.WriteLine("Enter city (leave empty to clear filter): ");
        string input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("City filter cleared.");
            return null;
        }

        Console.WriteLine($"City filter set to: {input}");
        return input;
    }

    public int? FilterOnDistanceToBeach()
    {
        Console.Clear();
        Console.WriteLine("Enter maximum distance to beach in meters (leave empty to clear filter): ");
        string input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Distance to beach filter cleared.");
            return null;
        }

        if (int.TryParse(input, out int distance))
        {
            Console.WriteLine($"Distance to beach filter set to: {distance} meters.");
            return distance;
        }

        Console.WriteLine("Invalid input. Distance filter not set.");
        return null;
    }

    public int? FilterOnRating()
    {
        Console.Clear();
        Console.WriteLine("Enter minimum rating (1-5, leave empty to clear filter): ");
        string input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Rating filter cleared.");
            return null;
        }

        if (int.TryParse(input, out int minRating) && Enum.IsDefined(typeof(Rating), minRating))
        {
            Console.WriteLine($"Rating filter set to: {minRating}");
            return minRating;
        }

        Console.WriteLine("Invalid input. Rating filter not set.");
        return null;
    }

    public bool? FilterOnPool()
    {
        Console.Clear();
        Console.WriteLine("Do you want a pool? (yes/no, leave empty to clear filter): ");
        string input = Console.ReadLine()?.ToLower();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Pool filter cleared.");
            return null;
        }

        if (input == "yes")
        {
            Console.WriteLine("Pool filter set to: Yes");
            return true;
        }

        if (input == "no")
        {
            Console.WriteLine("Pool filter set to: No");
            return false;
        }

        Console.WriteLine("Invalid input. Pool filter not set.");
        return null;
    }

    private void ClearFilters()
    {
        city = null;
        distancetobeach = null;
        rating = null;
        hasPool = null;

        Console.Clear();
        Console.WriteLine("All filters have been cleared.");
        Console.WriteLine("Press enter to continue.");
        Console.ReadLine();
    }
}