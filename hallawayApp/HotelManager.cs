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
    private string sortOption = "default";

    public HotelManager(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
    }

    public async Task<Hotel> FindHotelMenu()
    {
        bool running = true;
        hotelList = await _databaseActions.GetAllHotels();
        Hotel selectedHotel = null;

        while (running)
        {
            Console.Clear();
            Console.WriteLine(
                $"Menu> OrderMenu> DestinationMenu" +
                $"\n---------------------------" +
                $"\n1) Show all hotels, Sorting Order:({sortOption}) \n2) Filter on city \n3) Filter on distance to beach" +
                $"\n4) Filter on rating \n5) Filter on pool availability" +
                $"\n6) Clear filters \n7) Change sort order \n0) Return");
            Console.WriteLine("\nEnter your choice: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 7.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    selectedHotel = ShowHotels();
                    if (selectedHotel != null)
                    {
                        return selectedHotel;
                    }
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
                case 7:
                    ChangeSortOrder();
                    break;
                case 0:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a valid menu option.");
                    break;
            }
        }
        return null;
    }

   private Hotel ShowHotels()
{
    Console.Clear();

    var filteredHotels = hotelList.Where(hotel =>
        (string.IsNullOrEmpty(city) || hotel.address.City.Equals(city, StringComparison.OrdinalIgnoreCase)) &&
        (!distancetobeach.HasValue || hotel.distanceBeach <= distancetobeach.Value) &&
        (!rating.HasValue || (int)hotel.ratingEnum >= rating.Value) &&
        (!hasPool.HasValue || hotel.pool == hasPool.Value)
    ).ToList();
    
    filteredHotels = sortOption switch
    {
        "name" => filteredHotels.OrderBy(h => h.hotelName).ToList(),
        "rating" => filteredHotels.OrderByDescending(h => h.ratingEnum).ToList(),
        "distanceBeach" => filteredHotels.OrderBy(h => h.distanceBeach).ToList(),
        "distanceCityCenter" => filteredHotels.OrderBy(h => h.distanceCityCenter).ToList(),
        _ => filteredHotels
    };

    if (!filteredHotels.Any())
    {
        Console.WriteLine("No hotels match the current filters.");
        return null;
    }
    
    Console.WriteLine("Hotels:");
    Console.WriteLine(new string('-', 100));
    Console.WriteLine("{0,-5} | {1,-25} | {2,-15} | {3,-15} | {4,-12} | {5,-12} | {6,-5}",
                      "ID", "Name", "City", "Rating", "Beach (m)", "Center (m)", "Pool");
    Console.WriteLine(new string('-', 100));
    
    foreach (var hotel in filteredHotels)
    {
        Console.WriteLine("{0,-5} | {1,-25} | {2,-15} | {3,-15} | {4,-12} | {5,-12} | {6,-5}",
                          hotel.hotelID,
                          hotel.hotelName,
                          hotel.address.City,
                          hotel.ratingEnum.ToString().Replace("_", " "),
                          hotel.distanceBeach,
                          hotel.distanceCityCenter,
                          hotel.pool ? "Yes" : "No");
    }

    Console.WriteLine(new string('-', 100));
    Console.WriteLine("\nEnter hotel id or leave empty to return: ");
    string input = Console.ReadLine();
    if (!int.TryParse(input, out int id))
    {
        Console.WriteLine("Invalid input. Please enter a valid number.");
        return null;
    }

    var selectedHotel = filteredHotels.FirstOrDefault(hotel => hotel.hotelID == id);
    if (selectedHotel == null)
    {
        Console.WriteLine("No hotel found with the given ID.");
    }
    return selectedHotel;
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
    
    private void ChangeSortOrder()
    {
        Console.Clear();
        Console.WriteLine("Choose sorting option:");
        Console.WriteLine("1) Default (No specific order)");
        Console.WriteLine("2) Order by name");
        Console.WriteLine("3) Order by rating");
        Console.WriteLine("4) Order by distance to beach");
        Console.WriteLine("5) Order by distance to city center");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                sortOption = "default";
                break;
            case "2":
                sortOption = "name";
                break;
            case "3":
                sortOption = "rating";
                break;
            case "4":
                sortOption = "distanceBeach";
                break;
            case "5":
                sortOption = "distanceCityCenter";
                break;
            default:
                Console.WriteLine("Invalid option. Keeping current sort order.");
                break;
        }

        Console.WriteLine($"Sorting order set to: {sortOption}");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}