using System.Data;

namespace hallawayApp;
using System.Diagnostics;
public class Order
{
    private string orderName;
    private Party party;
    private Admin admin;
    private Hotel hotel;
    private DateTime date;
    private double totalPrice;
    private List<Addon> addonList;
    private DatabaseActions _databaseActions;

    public Order(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
    }

    public async Task CreateOrder()
    {
        party = new Party(_databaseActions);
        bool running = true;

        while (running){
        Console.WriteLine(
                          $"===========================" +
                          $"===========================" + 
                          $"\n1) Manage party " +
                          $"\n2) Set date " +
                          $"\n3) Select destination " +
                          $"\n4) View details " +
                          $"\n5) Done " +
                          $"\n0) Quit");
        int input = Int32.Parse(Console.ReadLine());
        Debug.Assert(input != null);

        switch (input)
        {
            case 1:
                await party.PartyMenu();
                break;
            case 2:
                // Pick a date
                break;
            case 3:
                Console.Clear();
                DestinationMenu();
                break;
            case 4:
                ShowOrderDetails();
                break;
            case 5:
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

    public void ShowOrderDetails()
    {
        Console.WriteLine($"===========================");
        foreach (Person person in party._persons)
        {
            Console.WriteLine($"{person.name}");
        }
        Console.WriteLine($"Start Date: ");
        Console.WriteLine($"Destination: ");
    }
    //  Method that produces a list of hotels
    public void ShowAllHotels()
    {
        // Get list of hotels from the database
        List<Hotel> hotelList = new List<Hotel>();

        // Checking if the list hotelList is empty
        if (hotelList.Count == 0) 
        {
            // If no hotels are found
            Console.WriteLine("No hotels available.");
        }
        else
        {
            // Showing available hotels
            Console.WriteLine("Available hotels:");
            foreach (var hotel in hotelList)
            {
                // Showing hotel details 
                Console.WriteLine($"Name: {hotel.hotelName},Address: {hotel.address}," +
                                  $"Pool: {hotel.pool}," +
                                  $"KidsClub: {hotel.kidsClub}, Distance to beach: {hotel.distanceBeach}" +
                                  $"Distance to city: {hotel.distanceCityCenter}," +
                                  $"Evening entertainment: {hotel.eveningEntertainment}");
            }
        }
        Console.WriteLine();
    }
    private void DestinationMenu()
    {
        Console.WriteLine(
            $"===========================" + 
            $"===========================" + 
            $"\n1) View cities" +
            $"\n2) Search Hotel" +
            $"\n0) Cancel");
        int input = Int32.Parse(Console.ReadLine());

        switch (input)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 0:
                break;
        }
    }

    private async void ListCities()
    {
        List<string> cities = await _databaseActions.GetCities();
        for (int i = 0; i < cities.Count(); i++)
        {
            Console.WriteLine($"{i}) {cities[i]}");
        }
    }

}