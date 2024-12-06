namespace hallawayApp;
using System.Diagnostics;
public class Order
{
    private string orderName;
    private Party party;
    private HotelManager _hotelManager;
    private int admin_id;
    private Hotel hotel;
    private DatePicker _datePicker;
    private DateTime start_date;
    private DateTime end_date;
    private double totalPrice = 100;
    private List<Addon> addonList;
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
        bool running = true;
        admin_id = admin;

        while (running)
        {
            string partymessage = "(NOT done)";
            string hotelmessage = "(NOT done)";
            string datemessage = "(NOT done)";
            if (party._persons.Count >= 1)
            {
                partymessage = "(Done)";
            }

            if (start_date != DateTime.MinValue && end_date != DateTime.MinValue)
            {
                datemessage = "(Done)";
            }
            if (hotel != null && hotel.hotelID != null)
            {
                hotelmessage = "(Done)";
            }
            
            Console.Clear();
        Console.WriteLine(
                          $"Menu> OrderMenu" +
                          $"\n---------------------------" + 
                          $"\n1) Manage party {partymessage}" +
                          $"\n2) Set date {datemessage}" +
                          $"\n3) Select destination {hotelmessage}" +
                          $"\n4) View details " +
                          $"\n5) Done " +
                          $"\n0) Quit");
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
                await ShowOrderDetailsMenu();
                break;
            case 5:
                await _databaseActions.AddOrder(party.partyID, admin_id, hotel.hotelID, start_date, totalPrice, end_date);
                running = false;
                break;
            case 0:
                running = false;
                Console.WriteLine("Goodbye!"); // Quit!
                break;
            default:
                break;
        }}
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
}