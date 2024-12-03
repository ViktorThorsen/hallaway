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
                $"\n1) Show all hotels \n2) Filter on city \n4) Done \n0) Quit");
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
                    SelectFilter();
                    break;
                case 3: 
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
    /*private void FilterOnCity()
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
    }*/
    
    private void FilterOnCity()
    {
        Console.Clear();
        Console.WriteLine($"Please enter a city to search in: ");
        string city = Console.ReadLine();
        if (filteredHotels.Any(hotel => hotel.address.City == city))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                if (hotel.address.City != city)
                {
                    hotelsToRemove.Add(hotel);
                }
            }
            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel); 
                
            }
        }
    }
    private void FilterOnDistanceBeach()
    {
        Console.Clear();
        Console.WriteLine($"Please enter a max distance to the beach: ");
        int distance = Int32.Parse(Console.ReadLine());
        if (filteredHotels.Any(hotel => hotel.distanceBeach <= distance))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                if (hotel.distanceBeach >= distance)
                {
                    hotelsToRemove.Add(hotel);
                }
            }
            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
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
        if (filteredHotels.Any(hotel => hotel.roomList.Any(room => room._size <= desiredSize)))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                foreach (var room in hotel.roomList)
                {
                    if (room._size >= desiredSize)
                    {
                        hotelsToRemove.Add(hotel);
                    }
                }
            }

            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
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
        if (filteredHotels.Any(hotel => hotel.roomList.Any(room => room._price <= priceMax)))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                foreach (var room in hotel.roomList)
                {
                    if (room._price >= priceMax)
                    {
                        hotelsToRemove.Add(hotel);
                    }
                }
            }
            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
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
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                foreach (var room in hotel.roomList)
                {
                    if (!room._isAvailable)
                    {
                        hotelsToRemove.Add(hotel);
                    }
                }
            }
            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
        }
        else
        {
            Console.WriteLine($"Could not find any rooms that were available");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnKidsClub()
    {
        if (filteredHotels.Any(hotel => hotel.kidsClub))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                if (hotel.kidsClub)
                {
                    hotelsToRemove.Add(hotel);
                }
            }

            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
        }
        else
        {
            Console.WriteLine($"Could not find any hotels");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnPool()
    {
        if (filteredHotels.Any(hotel => hotel.pool))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                if (hotel.pool)
                {
                    hotelsToRemove.Add(hotel);
                }
            }

            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
        }
        else
        {
            Console.WriteLine($"Could not find any hotels");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnRestaurant()
    {
        if (filteredHotels.Any(hotel => hotel.restaurante))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                if (hotel.restaurante)
                {
                    hotelsToRemove.Add(hotel);
                }
            }

            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
        }
        else
        {
            Console.WriteLine($"Could not find any hotels");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnRating()
    {
        Console.Clear();
        Console.WriteLine($"Please enter desired rating: ");
        int desiredRating = Int32.Parse(Console.ReadLine());
        if (filteredHotels.Any(hotel => hotel.kidsClub))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                if ((int)hotel.ratingEnum == desiredRating)
                {
                    hotelsToRemove.Add(hotel);
                }
            }

            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
        }
        else
        {
            Console.WriteLine($"Could not find any hotels");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnDistanceCityCenter()
    {
        Console.Clear();
        Console.WriteLine($"Please enter a max distance to the beach: ");
        int distance = Int32.Parse(Console.ReadLine());
        if (filteredHotels.Any(hotel => hotel.distanceCityCenter <= distance))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                if (hotel.distanceCityCenter >= distance)
                {
                    hotelsToRemove.Add(hotel);
                }
            }
            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
        }
        else
        {
            Console.WriteLine($"Could not find any hotels within {distance}m");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    private void FilterOnEveningEntertainment()
    {
        if (filteredHotels.Any(hotel => hotel.eveningEntertainment))
        {
            var hotelsToRemove = new List<Hotel>();
            foreach (var hotel in filteredHotels)
            {
                if (hotel.eveningEntertainment)
                {
                    hotelsToRemove.Add(hotel);
                }
            }

            foreach (var hotel in hotelsToRemove)
            {
                filteredHotels.Remove(hotel);
            }
        }
        else
        {
            Console.WriteLine($"Could not find any hotels");
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }
    }
    
    private void GetFilteredHotels()
    {
        foreach (var hotel in hotelList)
        {
            filteredHotels.Add(hotel);
        }
    }

    private void ClearFilteredHotels()
    {
        var hotelsToRemove = new List<Hotel>();
        foreach (var hotel in filteredHotels)
        {
            hotelsToRemove.Add(hotel);
        }
        foreach (var hotel in hotelsToRemove)
        {
            filteredHotels.Remove(hotel);
        }
    }

    private void ShowFilteredHotels()
    {
        foreach (var hotel in filteredHotels)
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
        Console.Write("Press enter to continue");
        Console.ReadLine();
    }
    

    private void SelectFilter()
    {
        GetFilteredHotels();
        bool running = true;
        while (running)
        {
            Console.WriteLine(
                $"\n1) City" +
                $"\n2) Distance beach" +
                $"\n3) Room size" +
                $"\n4) City" +
                $"\n5) Price" +
                $"\n6) Available" +
                $"\n7) Kids club" +
                $"\n8) Pool" +
                $"\n9) Restaurant" +
                $"\n10) Rating" +
                $"\n11) Distance city center" +
                $"\n12) Evening entertainment" +
                $"\n\t20) Print filtered hotels" +
                $"\n\t21) Remove filters" +
                $"\n\t0) Exit");

            int choice = Int32.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    FilterOnCity();
                    break;
                case 2:
                    FilterOnDistanceBeach();
                    break;
                case 3:
                    FilterOnRoomSize();
                    break;
                case 4:
                    FilterOnPrice();
                    break;
                case 5:
                    FilterOnIsAvailable();
                    break;
                case 6:
                    FilterOnKidsClub();                
                    break;
                case 7:
                    FilterOnKidsClub();
                    break;
                case 8:
                    FilterOnPool();
                    break;
                case 9:
                    FilterOnRestaurant();
                    break;
                case 10:
                    FilterOnRating();
                    break;
                case 11: 
                    FilterOnDistanceCityCenter();
                    break;
                case 12:
                    FilterOnEveningEntertainment();
                    break;
                case 20:
                    GetFilteredHotels();
                    break;
                case 21:
                    ClearFilteredHotels();
                    break;
                case 0:
                    running = false;
                    break;
            }
        }
    }
}