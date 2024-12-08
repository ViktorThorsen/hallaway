﻿namespace hallawayApp;

using System.Diagnostics;

public class Order
{
    private string orderName;
    private Party party;
    private HotelManager _hotelManager;
    private int admin_id;
    private Hotel hotel;
    private Reservation _reservation;
    private AddonManager _addonManager;
    private List<Addon> addonList;
    private RoomManager _roomManager;
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
    _roomManager = new RoomManager(_databaseActions);
    _addonManager = new AddonManager(_databaseActions);
    addonList = new List<Addon>();
    _reservation = new Reservation();
    admin_id = admin;

    bool running = true;

    while (running)
    {
        Console.Clear();
        
        string partymessage = party._persons.Count >= 1 ? "(Done)" : "(NOT done)";
        string hotelmessage = (hotel != null && hotel.hotelID != null) ? "(Done)" : "(NOT done)";
        string addonsMessage = addonList.Any() ? "(Done)" : "(NOT done)";
        string datemessage = (_reservation.StartDate != DateTime.MinValue && _reservation.EndDate != DateTime.MinValue) ? "(Done)" : "(NOT done)";
        
        Console.WriteLine("Menu> OrderMenu");
        Console.WriteLine("---------------------------");
        Console.WriteLine($"1) Manage party {partymessage}");
        Console.WriteLine($"2) Select destination {hotelmessage}");
        Console.WriteLine($"3) Add Extra Addons {addonsMessage}");
        Console.WriteLine($"4) Select Room And Date {datemessage}");
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
            case 1:
                await party.PartyMenu();
                break;

            case 2:
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

            case 3:
                if (hotel == null || hotel.hotelID == null)
                {
                    Console.WriteLine("You must select a destination before adding addons.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    var newAddons = await _addonManager.FindAddonMenu(hotel.hotelID, addonList);
                    if (newAddons != null && newAddons.Any())
                    {
                        foreach (var addon in newAddons)
                        {
                            if (!addonList.Contains(addon))
                            {
                                addonList.Add(addon);
                            }
                        }
                    }

                    foreach (var addon in addonList)
                    {
                       totalPrice = AddToTotal(addon.price);
                    }
                }
                break;

            case 4:
                if (hotel == null || hotel.hotelID == null)
                {
                    Console.WriteLine("You must select a destination before selecting room and date.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    _reservation = await _roomManager.RoomMenu(hotel);
                    TimeSpan days = _reservation.EndDate - _reservation.StartDate;
                    double price = await _databaseActions.GetRoomPrice(_reservation.RoomId);
                    AddToTotal(price * days.Days);
                }
                break;

            case 5:
                await ShowOrderDetailsMenu();
                break;

            case 6:
                if (party._persons.Count < 1 || hotel == null || hotel.hotelID == null || _reservation.RoomId == 0)
                {
                    Console.WriteLine("You must complete all previous steps before finalizing the order.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    int reservation_id = await _databaseActions.AddReservation(_reservation);
                    int orderID = await _databaseActions.AddOrder(party.partyID, admin_id, hotel.hotelID, totalPrice, reservation_id);
                    await _databaseActions.AddtoAddonXOrder(addonList, orderID);
                    Console.WriteLine("Order completed successfully!");
                    Console.WriteLine("Press Enter to exit...");
                    Console.ReadLine();
                    running = false;
                }
                break;

            case 0:
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
        Console.WriteLine($"Start Date: {_reservation.StartDate}");
        Console.WriteLine($"End Date: {_reservation.EndDate}");
        Console.WriteLine($"Destination: {hotel?.hotelName ?? "No destination selected"}");
        Console.WriteLine("Selected Addons:");
        foreach (var addon in addonList)
        {
            Console.WriteLine($"- {addon.name} (${addon.price})");
        }

        Console.WriteLine("Total price: " + totalPrice);

        Console.WriteLine("\nPress Enter to return...");
        Console.ReadLine();
    }
    public double AddToTotal(double price)
    {
        double newPrice = totalPrice += price;
        return newPrice;
    }
}
