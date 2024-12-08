namespace hallawayApp;
using System.Globalization;
public class RoomManager
{
    private readonly DatabaseActions _databaseActions;

    public RoomManager(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
    }

    public async Task<Reservation?> RoomMenu(Hotel hotel)
{
    if (hotel == null || hotel.hotelID == null)
    {
        Console.WriteLine("Invalid hotel. Returning to the previous menu.");
        return null;
    }

    Room? selectedRoom = null;
    DateTime? startDate = null;
    DateTime? endDate = null;
    string sortOption = "price";
    bool running = true;

    while (running)
    {
        Console.Clear();
        Console.WriteLine($"Menu> Hotel: {hotel.hotelName}> Room Menu");
        Console.WriteLine("-----------------------------------");
        Console.WriteLine($"1) Select Dates: {(startDate.HasValue && endDate.HasValue ? $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}" : "Not Selected")}");
        Console.WriteLine($"2) Sort Rooms by: {(sortOption == "price" ? "Price" : "Size")}");
        Console.WriteLine("3) Show All Rooms");
        Console.WriteLine("4) Selected Room: " + (selectedRoom != null ? $"Room ID {selectedRoom.RoomId}, Price: {selectedRoom.Price.ToString("C", new CultureInfo("sv-SE"))}" : "None"));
        Console.WriteLine("5) Done");
        Console.WriteLine("0) Return to Previous Menu");
        Console.WriteLine("\nEnter your choice: ");

        string input = Console.ReadLine();

        if (!int.TryParse(input, out int choice))
        {
            Console.WriteLine("Invalid input. Please enter a number.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            continue;
        }

        switch (choice)
        {
            case 1:
                (startDate, endDate) = PickDateRange();
                break;

            case 2:
                Console.Clear();
                Console.WriteLine("Choose sorting option:");
                Console.WriteLine("1) Price");
                Console.WriteLine("2) Size");
                Console.WriteLine("Enter your choice:");
                string sortInput = Console.ReadLine();
                if (sortInput == "1")
                {
                    sortOption = "price";
                    Console.WriteLine("Sorting by Price.");
                }
                else if (sortInput == "2")
                {
                    sortOption = "size";
                    Console.WriteLine("Sorting by Size.");
                }
                else
                {
                    Console.WriteLine("Invalid choice. Sorting option unchanged.");
                }
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                break;

            case 3:
                selectedRoom = await ShowAndSelectRoom(hotel, startDate, endDate, sortOption);
                break;

            case 4:
                Console.WriteLine(selectedRoom != null
                    ? $"Selected Room Details:\n  Room ID: {selectedRoom.RoomId}, Price: {selectedRoom.Price.ToString("C", new CultureInfo("sv-SE"))}, Size: {selectedRoom.Size} sqm"
                    : "No room selected.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                break;

            case 5:
                if (selectedRoom != null && startDate.HasValue && endDate.HasValue)
                {
                    running = false;
                    return new Reservation
                    {
                        RoomId = selectedRoom.RoomId,
                        StartDate = startDate.Value,
                        EndDate = endDate.Value
                    };
                }
                else
                {
                    Console.WriteLine("You must select dates and a room before proceeding.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                break;

            case 0:
                running = false;
                Console.WriteLine("Returning to the previous menu...");
                return null;

            default:
                Console.WriteLine("Invalid choice. Please select a valid option.");
                Console.ReadLine();
                break;
        }
    }

    return null;
}

   private async Task<Room?> ShowAndSelectRoom(Hotel hotel, DateTime? startDate, DateTime? endDate, string sortOption)
{
    Console.Clear();
    Console.WriteLine($"Rooms for Hotel: {hotel.hotelName} (Sorted by {(sortOption == "price" ? "Price" : "Size")})");
    Console.WriteLine(new string('-', 80));

    if (!startDate.HasValue || !endDate.HasValue)
    {
        Console.WriteLine("You must select dates first to check availability.");
        Console.WriteLine("Press Enter to return to the menu...");
        Console.ReadLine();
        return null;
    }

    var rooms = await _databaseActions.GetRoomsByHotelId(hotel.hotelID);
    var reservations = await _databaseActions.GetReservationsForHotel(hotel.hotelID);
    
    rooms = sortOption == "price"
        ? rooms.OrderBy(r => r.Price).ToList()
        : rooms.OrderBy(r => r.Size).ToList();

    if (rooms.Count == 0)
    {
        Console.WriteLine($"No rooms found for hotel '{hotel.hotelName}'.");
        Console.WriteLine("Press Enter to return to the menu...");
        Console.ReadLine();
        return null;
    }
    
    Console.WriteLine($"{"#",-4} {"Room Name",-15} {"Price",-15} {"Size",-10} {"Reservations",-35}");
    Console.WriteLine(new string('-', 80));

    for (int i = 0; i < rooms.Count; i++)
    {
        var room = rooms[i];
        var roomReservations = reservations.Where(r => r.RoomId == room.RoomId).ToList();

        string availability = roomReservations.Count > 0
            ? string.Join(", ", roomReservations.Select(r => $"{r.StartDate:yyyy-MM-dd} to {r.EndDate:yyyy-MM-dd}"))
            : "No reservations";

        Console.WriteLine($"{i + 1,-4} {room.room_name,-15} {room.Price.ToString("C", new CultureInfo("sv-SE")),-15:C} {room.Size,-10} {availability,-35}");
    }

    Console.WriteLine(new string('-', 80));
    Console.WriteLine("Enter the number of the room to select (or 0 to cancel):");

    string input = Console.ReadLine();

    if (!int.TryParse(input, out int selectedIndex) || selectedIndex < 0 || selectedIndex > rooms.Count)
    {
        Console.WriteLine("Invalid input. Press Enter to return to the menu...");
        Console.ReadLine();
        return null;
    }

    if (selectedIndex == 0)
    {
        Console.WriteLine("No room selected. Press Enter to return to the menu...");
        Console.ReadLine();
        return null;
    }

    var selectedRoom = rooms[selectedIndex - 1];
    
    var overlappingReservations = reservations.Where(r =>
        r.RoomId == selectedRoom.RoomId &&
        (startDate < r.EndDate && endDate > r.StartDate)).ToList();

    if (overlappingReservations.Any())
    {
        Console.WriteLine($"The selected room ({selectedRoom.room_name}) is not available for the chosen dates.");
        Console.WriteLine("Press Enter to return to the menu...");
        Console.ReadLine();
        return null;
    }

    Console.WriteLine($"You selected Room: {selectedRoom.room_name}, Price: {selectedRoom.Price.ToString("C", new CultureInfo("sv-SE"))}");
    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
    return selectedRoom;
}

    private (DateTime startDate, DateTime endDate) PickDateRange()
{
    DateTime validStartRange = DateTime.Parse("2024-12-01");
    DateTime validEndRange = DateTime.Parse("2025-02-28");

    DateTime startDate, endDate;

    while (true)
    {
        Console.Clear();
        Console.WriteLine($"Select a date range between {validStartRange:yyyy-MM-dd} and {validEndRange:yyyy-MM-dd}.");

        Console.WriteLine("Enter the Check-In date (yyyy-MM-dd):");
        if (!DateTime.TryParse(Console.ReadLine(), out startDate))
        {
            Console.WriteLine("Invalid date format. Please try again.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            continue;
        }

        if (startDate < validStartRange || startDate > validEndRange)
        {
            Console.WriteLine($"Start date must be between {validStartRange:yyyy-MM-dd} and {validEndRange:yyyy-MM-dd}.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            continue;
        }

        Console.WriteLine("Enter the Check-out date (yyyy-MM-dd):");
        if (!DateTime.TryParse(Console.ReadLine(), out endDate))
        {
            Console.WriteLine("Invalid date format. Please try again.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            continue;
        }

        if (endDate < validStartRange || endDate > validEndRange)
        {
            Console.WriteLine($"End date must be between {validStartRange:yyyy-MM-dd} and {validEndRange:yyyy-MM-dd}.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            continue;
        }

        if (startDate >= endDate)
        {
            Console.WriteLine("The end date must be after the start date. Try again.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            continue;
        }
        
        break;
    }

    return (startDate, endDate);
}
}