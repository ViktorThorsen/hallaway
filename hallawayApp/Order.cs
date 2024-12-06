namespace hallawayApp;

using System.Diagnostics;

public class Order
{
    private string orderName;
    private Party party;
    private HotelManager _hotelManager;
    private int admin_id;
    private Hotel hotel;
    private AddonManager _addonManager;
    private List<Addon> addonList;
    private DatePicker _datePicker;
    private DateTime start_date;
    private DateTime end_date;
    private double totalPrice = 0;
    
    private DatabaseActions _databaseActions;

    public Order(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
    }

    public async Task CreateOrder(int admin)
{
    party = new Party(_databaseActions);
    _hotelManager = new HotelManager(_databaseActions);
    _datePicker = new DatePicker();
    _addonManager = new AddonManager(_databaseActions);
    addonList = new List<Addon>();
    admin_id = admin;

    bool running = true;

    while (running)
    {
        Console.Clear();

        // Determine status messages
        string partymessage = party._persons.Count >= 1 ? "(Done)" : "(NOT done)";
        string hotelmessage = (hotel != null && hotel.hotelID != null) ? "(Done)" : "(NOT done)";
        string addonsMessage = addonList.Any() ? "(Done)" : "(NOT done)";
        string datemessage = (start_date != DateTime.MinValue && end_date != DateTime.MinValue) ? "(Done)" : "(NOT done)";

        // Main menu
        Console.WriteLine("Menu> OrderMenu");
        Console.WriteLine("---------------------------");
        Console.WriteLine($"1) Manage party {partymessage}");
        Console.WriteLine($"2) Select destination {hotelmessage}");
        Console.WriteLine($"3) Add Extra Addons {addonsMessage}");
        Console.WriteLine($"4) Set date {datemessage}");
        Console.WriteLine($"5) View details");
        Console.WriteLine($"6) Done");
        Console.WriteLine($"0) Quit");

        Console.WriteLine("\nEnter your choice: ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Invalid input. Please enter a number between 0 and 6.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            continue;
        }

        switch (choice)
        {
            case 1: // Step 1: Manage Party
                await party.PartyMenu();
                break;

            case 2: // Step 2: Select Destination (Only available if party is managed)
                if (party._persons.Count < 1)
                {
                    Console.WriteLine("You must manage the party before selecting a destination.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    hotel = await _hotelManager.FindHotelMenu();
                }
                break;
            case 4: // Step 4: Set Date (Only available if destination is selected)
                if (hotel == null || hotel.hotelID == null)
                {
                    Console.WriteLine("You must select a destination before setting the date.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    var (startDate, endDate) = _datePicker.PickDateRange();
                    start_date = startDate;
                    end_date = endDate;
                }
                break;

            case 5: // View Details
                await ShowOrderDetailsMenu();
                break;

            case 6: // Final Step: Complete Order (Only available if all steps are done)
                if (party._persons.Count < 1 || hotel == null || hotel.hotelID == null || start_date == DateTime.MinValue || end_date == DateTime.MinValue)
                {
                    Console.WriteLine("You must complete all previous steps before finalizing the order.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    int orderID = await _databaseActions.AddOrder(party.partyID, admin_id, hotel.hotelID, start_date, totalPrice, end_date);
                    await _databaseActions.AddtoAddonXOrder(addonList, orderID);
                    Console.WriteLine("Order completed successfully!");
                    Console.WriteLine("Press Enter to exit...");
                    Console.ReadLine();
                    running = false; // Exit the menu
                }
                break;

            case 0: // Quit
                running = false;
                Console.WriteLine("Goodbye!");
                break;

            default:
                Console.WriteLine("Invalid option. Please choose a valid menu option.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                break;
        }
    }
}

    public async Task ShowOrderDetailsMenu()
    {
        Console.Clear();
        Console.WriteLine("Menu> OrderMenu");
        Console.WriteLine("---------------------------");
        Console.WriteLine("Persons in Party:");
        foreach (Person person in party._persons)
        {
            Console.WriteLine($"{person.name}");
        }

        Console.WriteLine($"Start Date: {start_date}");
        Console.WriteLine($"End Date: {end_date}");
        Console.WriteLine($"Destination: {hotel?.hotelName ?? "No destination selected"}");
        Console.WriteLine("Selected Addons:");
        foreach (var addon in addonList)
        {
            Console.WriteLine($"- {addon.name} (${addon.price})");
        }
    }

    public void AddCostToTotal(double price)
    {
        totalPrice += price;
    }
}