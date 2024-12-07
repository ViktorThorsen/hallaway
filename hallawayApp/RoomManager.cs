namespace hallawayApp;

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
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine($"Menu> Hotel: {hotel.hotelName}> Room Menu");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine($"1) Select Dates: {(startDate.HasValue && endDate.HasValue ? $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}" : "Not Selected")}");
            Console.WriteLine("2) Show All Rooms with Availability");
            Console.WriteLine("3) Selected Room: " + (selectedRoom != null ? $"Room ID {selectedRoom.RoomId}, Price: {selectedRoom.Price:C}" : "None"));
            Console.WriteLine("4) Done");
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
                case 1: // Select Dates
                    (startDate, endDate) = PickDateRange();
                    break;

                case 2: // Show All Rooms
                    selectedRoom = await ShowAndSelectRoom(hotel, startDate, endDate);
                    break;

                case 3: // View Selected Room
                    Console.WriteLine(selectedRoom != null
                        ? $"Selected Room Details:\n  Room ID: {selectedRoom.RoomId}, Price: {selectedRoom.Price:C}, Size: {selectedRoom.Size} sqm"
                        : "No room selected.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;

                case 4: // Done
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

                case 0: // Return to Previous Menu
                    running = false;
                    Console.WriteLine("Returning to the previous menu...");
                    return null;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    Console.ReadLine();
                    break;
            }
        }

        return null; // Default return if the loop exits
    }

    private async Task<Room?> ShowAndSelectRoom(Hotel hotel, DateTime? startDate, DateTime? endDate)
    {
        Console.Clear();
        Console.WriteLine($"Rooms for Hotel: {hotel.hotelName}");
        Console.WriteLine("-----------------------------------");

        if (!startDate.HasValue || !endDate.HasValue)
        {
            Console.WriteLine("You must select dates first to check availability.");
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
            return null;
        }

        var rooms = await _databaseActions.GetRoomsByHotelId(hotel.hotelID);
        var reservations = await _databaseActions.GetReservationsForHotel(hotel.hotelID);

        if (rooms.Count == 0)
        {
            Console.WriteLine($"No rooms found for hotel '{hotel.hotelName}'.");
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
            return null;
        }

        // Display all rooms with booking details
        for (int i = 0; i < rooms.Count; i++)
        {
            var room = rooms[i];
            Console.WriteLine($"{i + 1}) Room ID: {room.RoomId}, Price: {room.Price:C}, Size: {room.Size} sqm");

            var roomReservations = reservations.Where(r => r.RoomId == room.RoomId).ToList();
            if (roomReservations.Count > 0)
            {
                Console.WriteLine("   Booked Dates:");
                foreach (var reservation in roomReservations)
                {
                    Console.WriteLine($"     - From: {reservation.StartDate:yyyy-MM-dd} To: {reservation.EndDate:yyyy-MM-dd}");
                }
            }
            else
            {
                Console.WriteLine("   No bookings for this room.");
            }

            Console.WriteLine("-----------------------------------");
        }

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

        // Check if the selected room is available for the given dates
        var overlappingReservations = reservations.Where(r =>
            r.RoomId == selectedRoom.RoomId &&
            (startDate < r.EndDate && endDate > r.StartDate)).ToList();

        if (overlappingReservations.Any())
        {
            Console.WriteLine("The selected room is not available for the chosen dates.");
            Console.WriteLine("Press Enter to return to the menu...");
            Console.ReadLine();
            return null;
        }

        Console.WriteLine($"You selected Room ID {selectedRoom.RoomId}, Price: {selectedRoom.Price:C}");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
        return selectedRoom;
    }

    private (DateTime startDate, DateTime endDate) PickDateRange()
{
    // Correct date range
    DateTime validStartRange = DateTime.Parse("2024-12-01");
    DateTime validEndRange = DateTime.Parse("2025-02-28"); // Updated to 28th February

    DateTime startDate, endDate;

    while (true)
    {
        Console.Clear();
        Console.WriteLine($"Select a date range between {validStartRange:yyyy-MM-dd} and {validEndRange:yyyy-MM-dd}.");

        Console.WriteLine("Enter the start date (yyyy-MM-dd):");
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

        Console.WriteLine("Enter the end date (yyyy-MM-dd):");
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

        // Valid range and dates
        break;
    }

    return (startDate, endDate);
}
}