namespace hallawayApp;

public class HotelManager
{
    private DatabaseActions _databaseActions;
    private List<Hotel> hotelList;
    private List<Hotel> filteredHotels = new List<Hotel>();
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
                $"\n1) Show all hotels \n2) Filter on city\n0) Return");
            Console.WriteLine("\nEnter your choice: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 2.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    ShowHotels();
                    break;
                case 2: 
                    SelectCity();
                    break;
                case 3:
                    break;
                case 0:
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
        Console.Clear();
        foreach (Hotel hotel in hotelList)
        {
            Console.WriteLine($"{hotel.hotelID}) {hotel.hotelName}");
        }
        Console.WriteLine("Press enter to continue");
        Console.ReadLine();
    }
    
    private void FilterOnCity(string city)
    {
        GetFilteredHotels();
        foreach (var hotel in filteredHotels)
        {
            Console.WriteLine($"{hotel.hotelName}");
        }
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
            SelectFilter();
        }
    }
    private void FilterOnDistanceBeach()
    {
        Console.Clear();
        Console.WriteLine($"Please enter a max distance to the beach: ");
        int distance = Int32.Parse(Console.ReadLine());
        if (filteredHotels.Any(hotel => hotel.distanceBeach >= distance))
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
        if (filteredHotels.Any(hotel => hotel.roomList.Any(room => room._size >= desiredSize)))
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
    
    /*
    // Bubblesort for hotels rating
    private void SortHotelRatingAsc()
    {
        int first = 0;
        int lastHotel = filteredHotels.Count() - 1;
        for (int i = 0; i < lastHotel; i++)
        {
            int hotelsLeft = lastHotel - i;
            int lastRoom = filteredHotels[i].roomList[i].Count() - 1;

            for (int j = 0; j < hotelsLeft; j++)
            {
                int roomsLeft = 
                for (int k = 0; k < )
            }

            foreach (var hotel in filteredHotels)
            {
                foreach (var room in hotel.roomList)
                {
                    if (room._price < hotel.roomList.IndexOf(room) + 1)
                    {
                        
                    }
                }
            }
        }
    }
    
    // Bubblesort for hotels rating
    private void SortHotelRatingDesc()
    {
        int first = 0;
        int last = filteredHotels.Count() - 1;
        for (int i = 0; i < last; i++)
        {
            int toSort = last - i;

            for (int j = 0; j < toSort; j++)
            {
                if (filteredHotels[j].ratingEnum < filteredHotels[j + 1].ratingEnum)
                {
                    Hotel temp = filteredHotels[j];
                    filteredHotels[j] = filteredHotels[j + 1];
                    filteredHotels[j + 1] = temp;
                }
            }
        }
    }*/
    
    // Bubblesort for hotels rating
    private void SortHotelPriceAsc()
    {
        int first = 0;
        int last = filteredHotels.Count() - 1;
        for (int i = 0; i < last; i++)
        {
            int toSort = last - i;

            for (int j = 0; j < toSort; j++)
            {
                if (filteredHotels[j].ratingEnum > filteredHotels[j + 1].ratingEnum)
                {
                    Hotel temp = filteredHotels[j];
                    filteredHotels[j] = filteredHotels[j + 1];
                    filteredHotels[j + 1] = temp;
                }
            }
        }
    }
    
    // Bubblesort for hotels rating
    private void SortHotelPriceDesc()
    {
        int first = 0;
        int last = filteredHotels.Count() - 1;
        for (int i = 0; i < last; i++)
        {
            int toSort = last - i;

            for (int j = 0; j < toSort; j++)
            {
                if (filteredHotels[j].ratingEnum < filteredHotels[j + 1].ratingEnum)
                {
                    Hotel temp = filteredHotels[j];
                    filteredHotels[j] = filteredHotels[j + 1];
                    filteredHotels[j + 1] = temp;
                }
            }
        }
    }
    private void SortRatingMenu()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine($"Sort hotels on ratings: \n1)Ascending \n2)Descending \n0) Cancel");
            int input = Int32.Parse(Console.ReadLine());

            switch (input)
            {
                case 1:
                    SortHotelPriceAsc();
                    break;
                case 2:
                    SortHotelPriceDesc();
                    break;
                case 0:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Please enter a valid input");
                    break;
            }
        }
    }
    
    private void GetFilteredHotels()
    {
        foreach (Hotel hotel in hotelList)
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
        Console.Clear();
        if (filteredHotels.Count() != null)
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
        else
        {
            Console.WriteLine("There are no hotels matching your search :(");
        }
    }
    

    private void SelectFilter()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine(
                $"Filter Hotels" +
                $"\n1) Distance beach" +
                $"\n2) Room size" +
                $"\n3) City" +
                $"\n4) Price" +
                $"\n5) Available" +
                $"\n6) Kids club" +
                $"\n7) Pool" +
                $"\n8) Restaurant" +
                $"\n9) Rating" +
                $"\n10) Distance city center" +
                $"\n11) Evening entertainment" +
                $"\n\t20) Print filtered hotels" +
                $"\n\t21) Remove filters" +
                $"\n\t0) Exit");

            int choice = Int32.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    FilterOnDistanceBeach();
                    break;
                case 2:
                    FilterOnRoomSize();
                    break;
                case 3:
                    FilterOnPrice();
                    break;
                case 4:
                    FilterOnIsAvailable();
                    break;
                case 5:
                    FilterOnKidsClub();                
                    break;
                case 6:
                    FilterOnKidsClub();
                    break;
                case 7:
                    FilterOnPool();
                    break;
                case 8:
                    FilterOnRestaurant();
                    break;
                case 9:
                    SortRatingMenu();
                    break;
                case 10: 
                    FilterOnDistanceCityCenter();
                    break;
                case 11:
                    FilterOnEveningEntertainment();
                    break;
                case 20:
                    ShowFilteredHotels();
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

    private void SelectCity()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine($"Select a city to search for hotels" +
                              $"\n1) Falkenberg" +
                              $"\n2) Halmstad" +
                              $"\n3) Hyltebruk" +
                              $"\n4) Laholm" +
                              $"\n5) Kungsbacka" +
                              $"\n6) Varberg" +
                              $"\n0) Quit");
            int input = Int32.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    FilterOnCity("Falkenberg");
                    break;
                case 2:
                    FilterOnCity("Halmstad");
                    break;
                case 3:
                    FilterOnCity("Hyltebruk");
                    break;
                case 4:
                    FilterOnCity("Laholm");
                    break;
                case 5:
                    FilterOnCity("Kungsbacka");
                    break;
                case 6:
                    FilterOnCity("Varberg");
                    break;
                case 0:
                    running = false;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Please enter a valid input \nPress enter to continue");
                    Console.ReadLine();
                    break;
            }
        }
    }
}