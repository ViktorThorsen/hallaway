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
        bool running = true;
        admin_id = admin;

     running = true;

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
        int input = Int32.Parse(Console.ReadLine());
        Debug.Assert(input != null);

        switch (input)
        {
            case 1:
                await party.PartyMenu();
                break;
            case 2:
                var (startDate, endDate) = _datePicker.PickDateRange();
                start_date = startDate;
                end_date = endDate;
                break;
            case 3:
                hotel = await _hotelManager.FindHotelMenu();
                break;
            case 4:
                var newAddons = await _addonManager.FindAddonMenu(hotel.hotelID, addonList);
                if (newAddons != null && newAddons.Any())
                {
                    // Add new addons to the existing list, avoiding duplicates
                    foreach (var addon in newAddons)
                    {
                        if (!addonList.Contains(addon))
                        {
                            addonList.Add(addon);
                        }
                    }
                }
                break;
                break;
            case 5 :
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
        bool viewingDetails = true;

        while (viewingDetails)
        {
            Console.Clear();
            Console.WriteLine($"Menu> OrderMenu \n---------------------------");
            Console.WriteLine($"Persons in Party: ");
            foreach (Person person in party._persons)
            {
                Console.WriteLine($"{person.name}");
            }
            Console.WriteLine($"Start Date: {start_date}");
            Console.WriteLine($"Start Date: {end_date}");
            Console.WriteLine($"Destination: {hotel.hotelName}");
            Console.WriteLine("\n0) Back");

            Console.WriteLine("\nEnter 0 to back: ");
            string input = Console.ReadLine();

            if (input == "0")
            {
                viewingDetails = false; // Exit the details menu
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter '0' to go back.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    public void AddCostToTotal(double price)
    {
        totalPrice += price;
    }
}