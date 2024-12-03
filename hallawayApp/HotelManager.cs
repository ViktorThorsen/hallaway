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
        hotelList = await _databaseActions.GetAllHotels();
        while (running)
        {
            Console.Clear();
            Console.WriteLine(
                $"Menu> OrderMenu> PartyMenu" +
                $"\n---------------------------" +
                $"\n1) Show all hotels \n2) Filter on city \n3) Filter on distance to the beach\n4) Done \n0) Quit");
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
                    ShowHotels();
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
                    Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                      $"\n\taddress: {hotel.address.Street} " +
                                      $"\n\tpool: {hotel.pool} " +
                                      $"\n\trestaurant: {hotel.restaurante}" +
                                      $"\n\tkid's club: {hotel.kidsClub}" +
                                      $"\n\trating: {hotel.ratingEnum} stars" +
                                      $"\n\tdistance to the beach: {hotel.distanceBeach}m" +
                                      $"\n\tdistance to the city center: {hotel.distanceCityCenter}m" +
                                      $"\n\tevening entertainment: {hotel.eveningEntertainment}");
                    foreach (var room in hotel.roomList)
                    {
                        
                        Console.WriteLine($"\nsize: {room._size}m2" +
                                          $"\nAvailable: {room._isAvailable}" +
                                          $"\n\n\tprice: {room._price}kr");
                    }
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
                                      $"\n\taddress: {hotel.address.Street} " +
                                      $"\n\tpool: {hotel.pool} " +
                                      $"\n\trestaurant: {hotel.restaurante}" +
                                      $"\n\tkid's club: {hotel.kidsClub}" +
                                      $"\n\trating: {hotel.ratingEnum} stars" +
                                      $"\n\tdistance to the beach: {hotel.distanceBeach}m" +
                                      $"\n\tdistance to the city center: {hotel.distanceCityCenter}m" +
                                      $"\n\tevening entertainment: {hotel.eveningEntertainment}");
                }
                foreach (var room in hotel.roomList)
                {
                    Console.WriteLine($"\nsize: {room._size}m2" +
                                      $"\nAvailable: {room._isAvailable}" +
                                      $"\n\n\tprice: {room._price}kr");
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
                                          $"\n\tpool: {hotel.pool} " +
                                          $"\n\trestaurant: {hotel.restaurante}" +
                                          $"\n\tkid's club: {hotel.kidsClub}" +
                                          $"\n\trating: {hotel.ratingEnum} stars" +
                                          $"\n\tdistance to the beach: {hotel.distanceBeach}m" +
                                          $"\n\tdistance to the city center: {hotel.distanceCityCenter}m" +
                                          $"\n\tevening entertainment: {hotel.eveningEntertainment}" +
                                          $"\n\troom size: {room._size}" +
                                          $"\nsize: {room._size}m2" +
                                          $"\nAvailable: {room._isAvailable}" +
                                          $"\n\n\tprice: {room._price}kr");
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
                                          $"\n\tpool: {hotel.pool} " +
                                          $"\n\trestaurant: {hotel.restaurante}" +
                                          $"\n\tkid's club: {hotel.kidsClub}" +
                                          $"\n\trating: {hotel.ratingEnum} stars" +
                                          $"\n\tdistance to the beach: {hotel.distanceBeach}m" +
                                          $"\n\tdistance to the city center: {hotel.distanceCityCenter}m" +
                                          $"\n\tevening entertainment: {hotel.eveningEntertainment}" +
                                          $"\n\troom size: {room._size}" +
                                          $"\nsize: {room._size}m2" +
                                          $"\nAvailable: {room._isAvailable}" +
                                          $"\n\n\tprice: {room._price}kr");
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
                                          $"\n\tpool: {hotel.pool} " +
                                          $"\n\trestaurant: {hotel.restaurante}" +
                                          $"\n\tkid's club: {hotel.kidsClub}" +
                                          $"\n\trating: {hotel.ratingEnum} stars" +
                                          $"\n\tdistance to the beach: {hotel.distanceBeach}m" +
                                          $"\n\tdistance to the city center: {hotel.distanceCityCenter}m" +
                                          $"\n\tevening entertainment: {hotel.eveningEntertainment}" +
                                          $"\n\troom size: {room._size}" +
                                          $"\nsize: {room._size}m2" +
                                          $"\nAvailable: {room._isAvailable}" +
                                          $"\n\n\tprice: {room._price}kr");
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
            Console.WriteLine($"Hotels with available rooms: ");
            foreach (var hotel in hotelList)
            {
                if (hotel.kidsClub)
                {
                    Console.WriteLine($"\n{hotel.hotelID}) {hotel.hotelName} " +
                                      $"\n\taddress: {hotel.address.Street} " +
                                      $"\n\tpool: {hotel.pool} " +
                                      $"\n\trestaurant: {hotel.restaurante}" +
                                      $"\n\tkid's club: {hotel.kidsClub}" +
                                      $"\n\trating: {hotel.ratingEnum} stars" +
                                      $"\n\tdistance to the beach: {hotel.distanceBeach}m" +
                                      $"\n\tdistance to the city center: {hotel.distanceCityCenter}m" +
                                      $"\n\tevening entertainment: {hotel.eveningEntertainment}");

                }
                foreach (var room in hotel.roomList)
                { 
                    Console.WriteLine($"\n\troom size: {room._size}" +
                                      $"\nsize: {room._size}m2" +
                                      $"\nAvailable: {room._isAvailable}" +
                                      $"\n\n\tprice: {room._price}kr");
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
}