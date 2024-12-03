namespace hallawayApp;

public class HotelManager
{
    private DatabaseActions _databaseActions;
    private List<Hotel> hotelList;
    private List<Hotel> filteredHotels;
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
                $"Menu> OrderMenu> PartyMenu" +
                $"\n---------------------------" +
                $"\n1) Show all hotels \n2) Filter on city \n3) Filter on distance to the beach\n4) Filter on room size\n5) Filter on price\n6) Filter on availability\n7) Filter on kids club\n\n\n\n\n\n\n \n0) Quit");
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
                    SelectHotel();
                    break;
                case 2:
                    FilterOnCity();
                    break;
                case 3:
                    FilterOnDistanceBeach();
                    break;
                case 4:
                    FilterOnRoomSize();
                    break;
                case 5:
                    FilterOnPrice();
                    break;
                case 6:
                    FilterOnIsAvailable();
                    break;
                case 7:
                    FilterOnKidsClub();
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
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
    private void FilterOnCity()
    {
        Console.Clear();
        Console.WriteLine($"Please enter a city to search in: ");
        string city = Console.ReadLine();
        if (hotelList.Any(hotel => hotel.address.City == city))
        {
            Console.Clear();
            Console.WriteLine($"Hotels found in {city}:");
            foreach (var hotel in hotelList)
            {
                if (hotel.address.City == city)
                {
                    Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName}" +
                                      $"\n\taddress: {hotel.address.Street}");
                }
            }
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine($"Could not find any hotels in {city}");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnDistanceBeach()
    {
        Console.Clear();
        Console.WriteLine($"Please enter a max distance to the beach: ");
        int distance = Int32.Parse(Console.ReadLine());
        if (hotelList.Any(hotel => hotel.distanceBeach <= distance))
        {
            Console.Clear();
            Console.WriteLine($"Hotels within {distance}m:");
            foreach (var hotel in hotelList)
            {
                if (hotel.distanceBeach <= distance)
                {
                    Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                      $"\n\taddress: {hotel.address.Street}" +
                                      $"\n\tdistance to the beach: {hotel.distanceBeach}m");
                }
            }
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine($"Could not find any hotels within {distance}m");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnRoomSize()
    {
        int desiredSize = Int32.Parse(Console.ReadLine());
        if (hotelList.Any(hotel => hotel.roomList.Any(room => room._size <= desiredSize)))
        {
            Console.Clear();
            Console.WriteLine($"Hotels within {desiredSize}m2:");
            foreach (var hotel in hotelList)
            {
                foreach (var room in hotel.roomList)
                {
                    if (room._size <= desiredSize)
                    {
                        Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                          $"\n\taddress: {hotel.address.Street} " +
                                          $"\n\troom size: {room._size}");
                    }
                }
            }
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine($"Could not find any hotels within {desiredSize}m");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnPrice()
    {
        int priceMax = Int32.Parse(Console.ReadLine());
        if (hotelList.Any(hotel => hotel.roomList.Any(room => room._price <= priceMax)))
        {
            Console.Clear();
            Console.WriteLine($"Hotels with rooms under {priceMax}kr:");
            foreach (var hotel in hotelList)
            {
                foreach (var room in hotel.roomList)
                {
                    if (room._size <= priceMax)
                    {
                        Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                          $"\n\taddress: {hotel.address.Street} " +
                                          $"\n\tprice: {room._price}kr");
                    }
                }
            }
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine($"Could not find any hotels under {priceMax}kr");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnIsAvailable()
    {
        if (hotelList.Any(hotel => hotel.roomList.Any(room => room._isAvailable)))
        {
            Console.Clear();
            Console.WriteLine($"Hotels with available rooms: ");
            foreach (var hotel in hotelList)
            {
                foreach (var room in hotel.roomList)
                {
                    if (room._isAvailable)
                    {
                        Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                          $"\n\taddress: {hotel.address.Street} " +
                                          $"\nAvailable: {room._isAvailable}");
                    }
                }
            }
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine($"Could not find any hotels with available rooms");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnKidsClub()
    {
        if (hotelList.Any(hotel => hotel.roomList.Any(room => room._isAvailable)))
        {
            Console.Clear();
            Console.WriteLine($"Hotels with kids club: ");
            foreach (var hotel in hotelList)
            {
                if (hotel.kidsClub)
                {
                    Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                      $"\n\taddress: {hotel.address.Street} " +
                                      $"\n\trating: {hotel.ratingEnum} stars");

                }
            }
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine($"Could not find any hotels with available rooms");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }

    private void FilterOnPool()
    {
            Console.Clear();
            Console.WriteLine($"Hotels with a pool: ");
            foreach (var hotel in hotelList)
            {
                if (hotel.pool)
                {
                    Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                      $"\n\taddress: {hotel.address.Street} " +
                                      $"\n\tpool: {hotel.pool} ");
                    Console.Write("Press enter to continue");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"Could not find any hotels with pools");
                    Console.Write("Press enter to continue");
                    Console.ReadLine();
                }
            }
    }

    private void FilterOnResturant()
    {
        Console.Clear();
        Console.WriteLine($"Hotels with available rooms: ");
        foreach (var hotel in hotelList)
        {
            if (hotel.restaurante)
            {
                Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                  $"\n\taddress: {hotel.address.Street} " +
                                  $"\n\tpool: {hotel.restaurante} ");
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine($"Could not find any hotels with restaurants");
                Console.Write("Press enter to continue");
                Console.ReadLine();
            }
        }
    }
    private void FilterOnRating()
    {
        int input = Int32.Parse(Console.ReadLine());
       
        {
            Console.Clear();
            Console.WriteLine($"Hotels with {input} stars: ");
            foreach (var hotel in hotelList)
            {
                foreach (var room in hotel.roomList)
                {
                    if ((int)hotel.ratingEnum == input)
                    {
                        Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                          $"\n\taddress: {hotel.address.Street} " +
                                          $"\nAvailable: {room._isAvailable}");
                    }
                }
            }
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnDistanceCityCenter()
    {
        Console.Clear();
        Console.WriteLine($"Please enter a max distance to the city center: ");
        int distance = Int32.Parse(Console.ReadLine());
        if (hotelList.Any(hotel => hotel.distanceBeach <= distance))
        {
            Console.Clear();
            Console.WriteLine($"Hotels within {distance}m:");
            foreach (var hotel in hotelList)
            {
                if (hotel.distanceCityCenter <= distance)
                {
                    Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                      $"\n\taddress: {hotel.address.Street}" +
                                      $"\n\tdistance to the beach: {hotel.distanceCityCenter}m");
                }
            }
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine($"Could not find any hotels within {distance}m");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }

    private Hotel SelectHotel()
    {
        Console.WriteLine("Enter a hotel id to select a hotel");
        int index = Int32.Parse(Console.ReadLine());
        foreach (var hotel in hotelList)
        {
            if (hotel.hotelID == index)
            {
                return hotel;
            }
            else
            {
                Console.WriteLine($"No hotel found at {index}");
            }
        }
        return null;
    }
}