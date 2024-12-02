﻿namespace hallawayApp;
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
            Console.Clear();
        Console.WriteLine(
                          $"Menu> OrderMenu" +
                          $"\n---------------------------" + 
                          $"\n1) Manage party " +
                          $"\n2) Set date " +
                          $"\n3) Select destination " +
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
                // Pick a date
                break;
            case 3:
                
                break;
            case 4:
                ShowOrderDetailsMenu();
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

    public async Task ShowOrderDetailsMenu()
    {
        bool viewingDetails = true;

        while (viewingDetails)
        {
            Console.Clear();
            Console.WriteLine($"Menu> OrderMenu \n---------------------------");
            foreach (Person person in party._persons)
            {
                Console.WriteLine($"{person.name}");
            }
            Console.WriteLine($"Start Date: ");
            Console.WriteLine($"Destination: ");
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
    //  Method that produces a list of hotels
    /*
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
    }*/
}